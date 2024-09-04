using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Nominatim.API.Interfaces;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Route("business")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IBusinessUserService _businessUserService;
        private readonly IBusinessWorkingInfoService _businessWorkingInfoService;
        private readonly IBusinessGalleryService _businessGalleryService;
        private readonly IBusinessPropertiesService _businessPropertiesService;
        private readonly INominatimWebInterface _nominatimWebInterface;
        private readonly IServicesService _servicesService;
        private readonly ICommentService _commentService;
        private readonly IMemoryCache _memoryCache;
        private readonly IFileHandler _fileHandler;

        public BusinessController(
            IBusinessService businessService,
            IBusinessUserService businessUserService,
            IBusinessWorkingInfoService businessWorkingInfoService,
            IBusinessGalleryService businessGalleryService,
            IBusinessPropertiesService businessPropertiesService,
            INominatimWebInterface nominatimWebInterface,
            IServicesService servicesService,
            ICommentService commentService,
            IMemoryCache memoryCache,
            IFileHandler fileHandler)
        {
            _businessService = businessService;
            _businessUserService = businessUserService;
            _businessWorkingInfoService = businessWorkingInfoService;
            _businessGalleryService = businessGalleryService;
            _businessPropertiesService = businessPropertiesService;
            _nominatimWebInterface = nominatimWebInterface;
            _servicesService = servicesService;
            _memoryCache = memoryCache;
            _fileHandler = fileHandler;
            _commentService = commentService;
        }


        /// <summary>
        /// Get Business Info By Id (Brings only its own information)
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        /// "00000000-0000-0000-0000-000000000000"
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getinfo")]
        [Authorize]
        public async Task<IActionResult> GetInfoById([FromBody] string id)
        {
            ResponseModel<Business> response = new ResponseModel<Business>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessService.GetBusinessByIdAsync(id.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessNotFound;
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Business By Id (It also brings relationship table information)
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        /// "00000000-0000-0000-0000-000000000000"
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getbyid")]
        [Authorize]
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            ResponseModel<Business> response = new ResponseModel<Business>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessService.GetBusinessAllByIdAsync(id.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessNotFound;
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Session Business
        /// </summary>
        /// <returns></returns>
        [HttpPost("get")]
        [Authorize(Roles = "BusinessAdmin,BusinessWorker,Business")]
        public async Task<IActionResult> Get()
        {
            ResponseModel<Business> response = new ResponseModel<Business>();

            var business = await HelperMethods.GetSessionBusiness(Request, _businessService);

            if (business == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessNotFound;
                return Ok(response);
            }

            response.Data = business;

            return Ok(response);
        }

        /// <summary>
        /// Get Business List No Cache
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbusinesslistnocache")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBusinessListNoCache()
        {
            ResponseModel<IList<BusinessListResponseModel>> response = new ResponseModel<IList<BusinessListResponseModel>>();
            response.Data = await _businessService.GetBusinessListForCache(false);

            return Ok(response);
        }


        /// <summary>
        /// Get Business Details By Id (Can be used for business detail page.)
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        /// "00000000-0000-0000-0000-000000000000"
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getdetails")]
        public async Task<IActionResult> GetBusinessDetailsById([FromBody] string id)
        {
            var culture = Request.Headers["Language"].ToString().IsNull("en");
            ResponseModel<BusinessDetailResponseModel> response = new ResponseModel<BusinessDetailResponseModel>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = $"{Resource.Resource.ErrorMessage} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            var businessDetail = await _businessService.GetBusinessDetailByIdAsync(id.ToGuid());

            if(businessDetail == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessNotFound;
                return Ok(response);
            }

            businessDetail.isOpen = HelperMethods.GetBusinessOpen(businessDetail.businessWorkingInfo, businessDetail.officialDayAvailable);
            businessDetail.averageRating = Math.Round(businessDetail.averageRating, 1);
            
            if (!culture.Equals("tr"))
                businessDetail.description = businessDetail.descriptionEn.IsNull(businessDetail.description);

            var pointList = await _commentService.GetCommentPointListForCache(businessId: id.ToGuid());

            if (!pointList.IsNullOrEmpty())
            {
                businessDetail.workers = businessDetail.workers
                    .Select(x =>
                    {
                        var points = pointList.Where(p => p.workerIds.Contains(x.id));

                        if(!points.IsNullOrEmpty())
                        {
                            x.countRating = points.Count();
                            x.point = points.Average(x => x.point);
                        }

                        return x;
                    })
                    .ToList();
            }

            var services = new List<Services>();

            if (_memoryCache.TryGetValue("services", out object list))
            {
                services = (List<Services>)list;
            }
            else
            {
                services = await _servicesService.GetServicesAsync();
                _memoryCache.Set("services", services, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal
                });
            }

            var discountMultiplier = 1.0;
            var activeDiscounts = businessDetail.discounts?
                .Where(x => x.type == DiscountType.AllDay
                            || (x.type == DiscountType.WeekDay &&
                                DateTime.Today.DayOfWeek >= DayOfWeek.Monday &&
                                DateTime.Today.DayOfWeek <= DayOfWeek.Friday)
                            || (x.type == DiscountType.WeekEnd &&
                                (DateTime.Today.DayOfWeek == DayOfWeek.Saturday ||
                                  DateTime.Today.DayOfWeek == DayOfWeek.Sunday)))
                .OrderBy(x => x.rate)
                .ToList();

            var popularServices = businessDetail.businessServices.Where(x => x.isPopular);

            if (popularServices.Any())
            {
                BusinessServicesInfoResponseModel businessServiceInfo = new BusinessServicesInfoResponseModel();
                businessServiceInfo.serviceName = Resource.Resource.PopularServices;
                businessServiceInfo.className = "popular";

                foreach (var item in popularServices)
                {
                    var activeDiscount = (activeDiscounts?
                        .Where(x => x.serviceIds.Contains(item.serviceId.Value.ToString()) || x.serviceIds.IsNullOrEmpty()))
                        .MaxBy(x => x.rate);

                    setDiscountTitle(activeDiscount);
                    
                    item.discountRate = activeDiscount?.rate??0; 
                    item.discountPrice = item.price * (1 - (item.discountRate / 100));
                    
                    if (!culture.Equals("tr")) item.name = item.nameEn.IsNull(item.name);
                    
                    businessServiceInfo.businessServices.Add(item);
                }

                businessDetail.businessServicesInfos.Add(businessServiceInfo);
            }

            foreach (var items in businessDetail.businessServices.GroupBy(x => x.serviceId))
            {
                BusinessServicesInfoResponseModel businessServiceInfo = new BusinessServicesInfoResponseModel();
                var service = services.FirstOrDefault(x => x.id == items.Key.Value);
                businessServiceInfo.serviceName = service != null ? (culture == "en" ? service.nameEn : service.name) : string.Empty;
                businessServiceInfo.className = service != null ? service.className : string.Empty;

                foreach (var item in items)
                {
                    var activeDiscount = (activeDiscounts?
                            .Where(x =>x.serviceIds.Contains(item.serviceId.Value.ToString()) || x.serviceIds.IsNullOrEmpty()))
                        .MaxBy(x => x.rate);

                    setDiscountTitle(activeDiscount);
                    
                    item.discountRate = activeDiscount?.rate??0; 
                    item.discountPrice = item.price * (1 - (item.discountRate / 100));

                    if (!culture.Equals("tr")) item.name = item.nameEn.IsNull(item.name);
                    
                    businessServiceInfo.businessServices.Add(item);
                }

                businessDetail.businessServicesInfos.Add(businessServiceInfo);
            }

            response.Data = businessDetail;

            return Ok(response);
        }

        /// <summary>
        /// Get Business Detail By NameforUrl (Can be used for fast appointment page.)
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        /// "tolga-beauty-house"
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getdetailbynameforurl")]
        public async Task<IActionResult> GetBusinessDetailByNameForUrl([FromBody] string nameForUrl)
        {
            var culture = Request.Headers["Language"].ToString().IsNull("en");
            ResponseModel<BusinessDetailResponseModel> response = new ResponseModel<BusinessDetailResponseModel>();

            if (nameForUrl.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("nameForUrl", Resource.Resource.NotEmpty));
                response.Message = $"{Resource.Resource.ErrorMessage} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            var businessDetail = await _businessService.GetBusinessDetailByNameForUrlAsync(nameForUrl);

            if (businessDetail == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("errorInfo", Resource.Resource.BusinessNotFound));
                response.Message = $"{Resource.Resource.ErrorMessage} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            businessDetail.isOpen = HelperMethods.GetBusinessOpen(businessDetail.businessWorkingInfo, businessDetail.officialDayAvailable);
            businessDetail.averageRating = Math.Round(businessDetail.averageRating, 1);
           
            if (!culture.Equals("tr"))
                businessDetail.description = businessDetail.descriptionEn.IsNull(businessDetail.description);
            
            var services = new List<Services>();

            if (_memoryCache.TryGetValue("services", out object list))
            {
                services = (List<Services>)list;
            }
            else
            {
                services = await _servicesService.GetServicesAsync();
                _memoryCache.Set("services", services, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal
                });
            }

            var discountMultiplier = 1.0;
            var activeDiscounts = businessDetail.discounts?
                .Where(x => x.type == DiscountType.AllDay
                            || (x.type == DiscountType.WeekDay &&
                                DateTime.Today.DayOfWeek >= DayOfWeek.Monday &&
                                DateTime.Today.DayOfWeek <= DayOfWeek.Friday)
                            || (x.type == DiscountType.WeekEnd &&
                                (DateTime.Today.DayOfWeek == DayOfWeek.Saturday ||
                                  DateTime.Today.DayOfWeek == DayOfWeek.Sunday)))
                .OrderBy(x => x.rate)
                .ToList();

            var popularServices = businessDetail.businessServices.Where(x => x.isPopular);

            if (popularServices.Any())
            {
                BusinessServicesInfoResponseModel businessServiceInfo = new BusinessServicesInfoResponseModel();
                businessServiceInfo.serviceName = Resource.Resource.PopularServices;
                businessServiceInfo.className = "popular";

                foreach (var item in popularServices)
                {
                    var activeDiscount = (activeDiscounts?
                        .Where(x => x.serviceIds.Contains(item.serviceId.Value.ToString()) || x.serviceIds.IsNullOrEmpty()))
                        .MaxBy(x => x.rate);

                    setDiscountTitle(activeDiscount);

                    item.discountRate = activeDiscount?.rate ?? 0;
                    item.discountPrice = item.price * (1 - (item.discountRate / 100));
                    
                    if (!culture.Equals("tr")) item.name = item.nameEn.IsNull(item.name);

                    businessServiceInfo.businessServices.Add(item);
                }

                businessDetail.businessServicesInfos.Add(businessServiceInfo);
            }

            foreach (var items in businessDetail.businessServices.GroupBy(x => x.serviceId))
            {
                BusinessServicesInfoResponseModel businessServiceInfo = new BusinessServicesInfoResponseModel();
                var service = services.FirstOrDefault(x => x.id == items.Key.Value);
                businessServiceInfo.serviceName = service != null ? (culture == "tr" ? service.name : service.nameEn) : string.Empty;
                businessServiceInfo.className = service != null ? service.className : string.Empty;

                foreach (var item in items)
                {
                    var activeDiscount = (activeDiscounts?
                        .Where(x => x.serviceIds.Contains(item.serviceId.Value.ToString()) || x.serviceIds.IsNullOrEmpty()))
                        .MaxBy(x => x.rate);

                    setDiscountTitle(activeDiscount);

                    item.discountRate = activeDiscount?.rate ?? 0;
                    item.discountPrice = item.price * (1 - (item.discountRate / 100));
                    
                    if (!culture.Equals("tr")) item.name = item.nameEn.IsNull(item.name);

                    businessServiceInfo.businessServices.Add(item);
                }

                businessDetail.businessServicesInfos.Add(businessServiceInfo);
            }

            response.Data = businessDetail;

            return Ok(response);
        }

        /// <summary>
        /// Get Business Names
        /// </summary>
        /// <returns></returns>
        [HttpPost("getnames")]
        [Authorize]
        public async Task<IActionResult> GetBusinessNames()
        {
            ResponseModel<List<Tuple<string, string>>> response = new ResponseModel<List<Tuple<string, string>>>();

            var businessSelectListDictionary = await _businessService.GetBusinessSelectListAsync();

            if (businessSelectListDictionary == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.RecordNotFound;
                return Ok(response);
            }

            response.Data = businessSelectListDictionary;

            return Ok(response);
        }

        /// <summary>
        /// Set Profile Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost("setprofilephoto")]
        [Authorize(Roles = "BusinessAdmin,Business,Admin")]
        public async Task<IActionResult> SetProfilePhoto([FromForm] BusinessFileInfoRequestModel businessFileInfoModel)
        {
            ResponseModel<BusinessGallery> response = new ResponseModel<BusinessGallery>();

            if (businessFileInfoModel.file == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("files", Resource.Resource.NotEmpty));
                response.Message = $"{Resource.Resource.ImageNotUploaded} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            var business = await HelperMethods.GetSessionBusiness(Request, _businessService);

            if (business == null && businessFileInfoModel.businessId.HasValue)
            {
                business = await _businessService.GetBusinessByIdAsync(businessFileInfoModel.businessId.Value);
            }

            if (business == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("errorInfo", Resource.Resource.BusinessNotFound));
                response.Message = $"{Resource.Resource.ImageNotUploaded} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            List<BusinessGallery> businessGalleries = new List<BusinessGallery>();

            string fileName = $"{businessFileInfoModel.file.FileName.Split(".").FirstOrDefault()}-{DateTime.Now.ToString("ddMMhhmmss")}.{businessFileInfoModel.file.FileName.Split(".").LastOrDefault()}";
            string businessName = business.name.ToLower().TurkishChrToEnglishChr().Replace(" ", "-");
            await _fileHandler.UploadFile(businessFileInfoModel.file, $"BusinessImages/{businessName}", fileName);

            string imageUrl = await _fileHandler.UploadFreeImageServer(businessFileInfoModel.file);

            if (imageUrl.IsNullOrEmpty())
                imageUrl = string.Format("{0}://{1}/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, $"StaticFiles/UploadedFiles/BusinessImages/{businessName}/{fileName}");

            BusinessGallery businessGallery = new BusinessGallery
            {
                imageUrl = imageUrl,
                businessId = business.id,
                size = null,
                isProfilePhoto = true,
            };

            response.Message = Resource.Resource.ImageUploaded;
            response.Data = await _businessGalleryService.SaveBusinessGalleryAsync(businessGallery);

            BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(business.id));

            return Ok(response);
        }

        /// <summary>
        /// Save Business By Admin
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "name" : "Mert Kuaför",
        ///        "address" : "Merkez, Eski Büyükdere Cd. 37/B, 34416 Kağıthane/İstanbul",
        ///        "website": "https://caregarden.app/",
        ///        "mobileOrOnlineServiceOnly": false,
        ///        "serviceIds": "00000000-0000-0000-0000-000000000000;00000000-0000-0000-0000-000000000001"
        ///        "workerSizeType": 2
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        [Authorize(Roles = "BusinessAdmin")]
        public async Task<IActionResult> Save([FromBody] BusinessAdminSaveBusinessRequestModel requestModel)
        {
            ResponseModel<Business> response = new ResponseModel<Business>();

            if (requestModel.name.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.NotEmpty));
            }

            if (!requestModel.name.IsNullOrEmpty() && !requestModel.name.IsValidFullName())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.ValidNameMessage));
            }

            if (requestModel.address.IsNullOrEmpty() && !requestModel.mobileOrOnlineServiceOnly)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("address", Resource.Resource.NotEmpty));
            }

            if (requestModel.serviceIds.IsNullOrEmpty() || requestModel.serviceIds?.Length < 32)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("serviceIds", Resource.Resource.NotEmpty));
            }

            if (requestModel.workerSizeType == WorkerSizeType.Unspecified)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("workerSizeType", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }

            var businessUser = await HelperMethods.GetSessionBusinessUser(Request, _businessUserService);

            var isExistsBusiness = await _businessService.GetBusinessExistsByEmailAsync(businessUser.email);

            if (isExistsBusiness)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessFoundEnteredMail;
                return Ok(response);
            }

            isExistsBusiness = await _businessService.GetBusinessExistsByTelephoneNumberAsync(businessUser.telephone);

            if (isExistsBusiness)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessFoundEnteredTelephone;
                return Ok(response);
            }

            Business business = new Business { 
                name = requestModel.name,
                address = requestModel.address,
                mobileOrOnlineServiceOnly = requestModel.mobileOrOnlineServiceOnly,
                serviceIds = requestModel.serviceIds,
                workingSizeType = requestModel.workerSizeType,
                email = businessUser.email,
                telephone = businessUser.telephone
            };

            business.nameForUrl = await _businessService.GetNameForUrl(business);
            await HelperMethods.SetBusinessLocationInfoByAddress(business, _nominatimWebInterface);

            if (business.latitude > 0 && business.longitude > 0)
            {
                var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);
                business.location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(business.latitude, business.longitude));
            }

            business = await _businessService.SaveBusinessAsync(business);

            businessUser.businessId = business.id;
            await _businessUserService.UpdateBusinessUserAsync(businessUser);

            BackgroundJob.Enqueue(() => _businessPropertiesService.SaveStaticBusinessPropertiesAsync(business.id, requestModel.website));

            response.Data = business;
            response.Message = Resource.Resource.RegistrationSuccess;

            return Ok(response);
        }

        /// <summary>
        /// Update Business
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "name" : "Mert Kuaför",
        ///        "address" : "Merkez, Eski Büyükdere Cd. 37/B, 34416 Kağıthane/İstanbul",
        ///        "telephone" : "+905467335892",
        ///        "latitude": "41.08611",
        ///        "longitude": "29.00717",
        ///        "city": "İstanbul",
        ///        "province": "Kağıthane",
        ///        "district": "Çeliktepe Mah.",
        ///        "description": "Tesettür saç kesim, bakım, boya, kalıcı makyaj, microblading, manikür, pedikür, kaynak saç.",
        ///        "descriptionEn": "Hijab haircut, care, coloring, permanent makeup, microblading, manicure, pedicure, hair welding.",
        ///        "isFeatured": true,
        ///        "logoUrl": "safdsf.jpg",
        ///        "hasPromotion": true
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize(Roles = "BusinessAdmin,Business,Admin")]
        public async Task<IActionResult> Update(Business updateBusiness)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var business = await HelperMethods.GetSessionBusiness(Request, _businessService)
                               ?? await _businessService.GetBusinessByIdAsync(updateBusiness.id);

            if (business == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("errorInfo", Resource.Resource.BusinessNotFound));
                response.Message = $"{Resource.Resource.RecordNotUpdated} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            business.name = updateBusiness.name.IsNull(business.name);
            business.address = updateBusiness.address.IsNull(business.address);
            business.telephone = updateBusiness.telephone.IsNull(business.telephone);
            business.latitude = updateBusiness.latitude.IsNull(business.latitude);
            business.longitude = updateBusiness.longitude.IsNull(business.longitude);
            business.city = updateBusiness.city.IsNull(business.city);
            business.province = updateBusiness.province.IsNull(business.province);
            business.district = updateBusiness.district.IsNull(business.district);
            business.description = updateBusiness.description.IsNull(business.description);
            business.descriptionEn = updateBusiness.descriptionEn.IsNull(business.descriptionEn);
            business.logoUrl = updateBusiness.logoUrl.IsNull(business.logoUrl);
            business.isFeatured = updateBusiness.isFeatured;
            business.hasPromotion = updateBusiness.hasPromotion;
            //business.nameForUrl = await _businessService.GetNameForUrl(business);

            if (updateBusiness.latitude > 0 && updateBusiness.longitude > 0)
            {
                var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);
                business.location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(updateBusiness.latitude, updateBusiness.longitude));
            }

            var sessionUserRole = HelperMethods.GetClaimInfo(Request, ClaimTypes.Role);

            if (sessionUserRole.IsNotNullOrEmpty() && sessionUserRole.Equals("Admin"))
            {
                business.isActive = updateBusiness.isActive;
                business.verified = updateBusiness.verified;
            }

            await _businessService.UpdateBusinessAsync(business);
            response.Message = Resource.Resource.RecordUpdated;
            response.Data = true;

            BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(business.id));

            return Ok(response);
        }


        /// <summary>
        /// Save Business Working Info
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "businessWorkingInfo" : {
        ///             "mondayWorkHours" : "09:00-21:00",
        ///             "tuesdayWorkHours" : "09:00-21:00",
        ///             "wednesdayWorkHours" : "09:00-21:00",
        ///             "thursdayWorkHours" : "09:00-21:00",
        ///             "fridayWorkHours" : "09:00-21:00",
        ///             "saturdayWorkHours" : "09:00-13:00",
        ///             "sundayWorkHours" : null
        ///        },
        ///        "appointmentTimeInterval" : 30,
        ///        "appointmentPeopleCount" : 5,
        ///        "officialHolidayAvailable" : true,
        ///        "workingGenderType" : 0
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("saveworkinginfo")]
        [Authorize(Roles = "BusinessAdmin,Business,Admin")]
        public async Task<IActionResult> SaveWorkingInfo(BusinessWorkInfoRequestModel businessWorkInfoModel)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (businessWorkInfoModel.businessWorkingInfo == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessWorkingInfo", Resource.Resource.NotEmpty));
            }
            
            if (businessWorkInfoModel.appointmentTimeInterval == 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("appointmentTimeInterval", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = $"{Resource.Resource.RegistrationFailed} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            var business = await HelperMethods.GetSessionBusiness(Request, _businessService)
                           ?? await _businessService.GetBusinessByIdAsync(businessWorkInfoModel.businessId);

            if (business == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("errorInfo", Resource.Resource.BusinessNotFound));
                response.Message = $"{Resource.Resource.RecordNotUpdated} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            business.officialHolidayAvailable = businessWorkInfoModel.officialHolidayAvailable;
            business.appointmentPeopleCount = businessWorkInfoModel.appointmentPeopleCount;
            business.appointmentTimeInterval = businessWorkInfoModel.appointmentTimeInterval;
            business.workingGenderType = businessWorkInfoModel.workingGenderType;

            await _businessService.UpdateBusinessAsync(business);

            businessWorkInfoModel.businessWorkingInfo.businessId = business.id;
            businessWorkInfoModel.businessWorkingInfo.officialHolidayAvailable = business.officialHolidayAvailable;
            await _businessWorkingInfoService.DeleteBusinessWorkingInfoByBusinessIdAsync(business.id);
            await _businessWorkingInfoService.SaveBusinessWorkingInfoAsync(businessWorkInfoModel.businessWorkingInfo);

            response.Message = Resource.Resource.RegistrationSuccess;
            response.Data = true;

            BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(business.id));

            return Ok(response);
        }

        /// <summary>
        /// Add Gallery Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost("addgalleryphoto")]
        [Authorize(Roles = "BusinessAdmin,Business,Admin")]
        public async Task<IActionResult> AddGalleryPhoto([FromForm] BusinessFileInfoRequestModel businessFileInfoModel)
        {
            ResponseModel<BusinessGallery> response = new ResponseModel<BusinessGallery>();

            if (businessFileInfoModel.file == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("file", Resource.Resource.NotEmpty));
                response.Message = $"{Resource.Resource.ImageNotUploaded} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            var business = await HelperMethods.GetSessionBusiness(Request, _businessService);

            if (business == null && businessFileInfoModel.businessId.HasValue)
            {
                business = await _businessService.GetBusinessByIdAsync(businessFileInfoModel.businessId.Value);
            }

            if (business == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("errorInfo", Resource.Resource.BusinessNotFound));
                response.Message = $"{Resource.Resource.ImageNotUploaded} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            string fileName = $"{businessFileInfoModel.file.FileName.Split(".").FirstOrDefault()}-{DateTime.Now.ToString("ddMMhhmmss")}.{businessFileInfoModel.file.FileName.Split(".").LastOrDefault()}";
            string businessName = business.name.ToLower().TurkishChrToEnglishChr().Replace(" ", "-");
            await _fileHandler.UploadFile(businessFileInfoModel.file, $"BusinessImages/{businessName}", fileName);

            string imageUrl = await _fileHandler.UploadFreeImageServer(businessFileInfoModel.file);

            if (imageUrl.IsNullOrEmpty())
                imageUrl = string.Format("{0}://{1}/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, $"StaticFiles/UploadedFiles/BusinessImages/{businessName}/{fileName}");

            var businessGallery = new BusinessGallery
            {
                imageUrl = imageUrl,
                businessId = business.id,
                isProfilePhoto = businessFileInfoModel.isProfilePhoto,
                isSliderPhoto = businessFileInfoModel.isSliderPhoto,
                sortOrder = businessFileInfoModel.sortOrder,
                size = null
            };

            response.Message = Resource.Resource.ImageUploaded;
            response.Data = await _businessGalleryService.SaveBusinessGalleryAsync(businessGallery);

            if(businessGallery.isProfilePhoto)
            {
                BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(business.id));
            }

            return Ok(response);
        }

        /// <summary>
        /// Update Gallery Photo
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
        ///        "isSliderPhoto" : true,
        ///        "isProfilePhoto" : true,
        ///        "sortOrder" : 1
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("updategalleryphoto")]
        [Authorize(Roles = "BusinessAdmin,Business,Admin")]
        public async Task<IActionResult> UpdateGalleryPhoto([FromBody] BusinessGallery updateBusinessGallery)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var businessGallery = await _businessGalleryService.GetBusinessGalleryByIdAsync(updateBusinessGallery.id);

            if (businessGallery == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("errorInfo", Resource.Resource.RecordNotFound));
                response.Message = $"{Resource.Resource.RecordNotUpdated} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            businessGallery.isSliderPhoto = updateBusinessGallery.isSliderPhoto;
            businessGallery.isProfilePhoto = updateBusinessGallery.isProfilePhoto;
            businessGallery.sortOrder = updateBusinessGallery.sortOrder;

            response.Data = await _businessGalleryService.UpdateBusinessGalleryAsync(businessGallery);
            response.Message = Resource.Resource.RecordUpdated;

            return Ok(response);
        }

        /// <summary>
        /// Delete Gallery Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost("deletegalleryphoto")]
        [Authorize(Roles = "BusinessAdmin,Business,Admin")]
        public async Task<IActionResult> DeleteGalleryPhoto([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = $"{Resource.Resource.RecordNotDeleted} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            if (response.HasError)
                return Ok(response);

            await _businessGalleryService.DeleteBusinessGalleryByIdAsync(id.ToGuid());
            response.Message = Resource.Resource.RecordDeleted;
            response.Data = true;

            return Ok(response);
        }


        /// <summary>
        /// Delete Business
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        [Authorize(Roles = "BusinessAdmin,Business,Admin")]
        public async Task<IActionResult> Delete([FromBody] Business business)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            response.Data = await _businessService.DeleteBusinessAsync(business);
            response.Message = Resource.Resource.RecordDeleted;

            return Ok(response);
        }

        private void setDiscountTitle(Discount? discount)
        {
            if(discount == null) return;
            
            var typeText = discount.type == DiscountType.WeekDay
                ? Resource.Resource.Weekday
                : discount.type == DiscountType.WeekEnd
                    ? Resource.Resource.Weekend
                    : string.Empty;

            discount.title = $"{string.Format(Resource.Resource.Sale, discount.rate)} {typeText}";
        }
    }
}
