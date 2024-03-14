using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("business")]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IBusinessWorkingInfoService _businessWorkingInfoService;
        private readonly IBusinessGalleryService _businessGalleryService;
        private readonly IServicesService _servicesService;
        private readonly IMemoryCache _memoryCache;
        private readonly IFileHandler _fileHandler;
        private readonly IElasticHandler _elasticHandler;

        public BusinessController(
            IBusinessService businessService,
            IBusinessWorkingInfoService businessWorkingInfoService,
            IBusinessGalleryService businessGalleryService,
            IServicesService servicesService,
            IMemoryCache memoryCache,
            IFileHandler fileHandler,
            IElasticHandler elasticHandler)
        {
            _businessService = businessService;
            _businessWorkingInfoService = businessWorkingInfoService;
            _businessGalleryService = businessGalleryService;
            _servicesService = servicesService;
            _memoryCache = memoryCache;
            _fileHandler = fileHandler;
            _elasticHandler = elasticHandler;
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
        public async Task<IActionResult> GetInfoById([FromBody] string id)
        {
            ResponseModel<Business> response = new ResponseModel<Business>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message += Resource.Resource.IdParametreHatasi;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessService.GetBusinessByIdAsync(id.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = $"{id} id {Resource.Resource.SirketBulunamadi}";
                return Ok(response);
            }

            response.Data.password = null;
            response.Data.retryPassword = null;

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
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            ResponseModel<Business> response = new ResponseModel<Business>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message += Resource.Resource.IdParametreHatasi;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessService.GetBusinessAllByIdAsync(id.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = $"{id} id {Resource.Resource.SirketBulunamadi}";
                return Ok(response);
            }

            response.Data.password = null;
            response.Data.retryPassword = null;

            return Ok(response);
        }

        /// <summary>
        /// Get Session Business
        /// </summary>
        /// <returns></returns>
        [HttpPost("get")]
        public async Task<IActionResult> Get()
        {
            ResponseModel<Business> response = new ResponseModel<Business>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            var business = await HelperMethods.GetSessionBusiness(Request, _businessService);

            if (business == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.SirketBulunamadi;
                return Ok(response);
            }

            business.password = null;
            response.Data = business;

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
            ResponseModel<BusinessDetailModel> response = new ResponseModel<BusinessDetailModel>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message += Resource.Resource.IdParametreHatasi;
                return Ok(response);
            }

            var businessDetail = await _businessService.GetBusinessDetailByIdAsync(id.ToGuid());
            businessDetail.isOpen = HelperMethods.GetBusinessOpen(businessDetail.businessWorkingInfo, businessDetail.officialDayAvailable);
            businessDetail.averageRating = Math.Round(businessDetail.averageRating, 1);

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
            Discount activeDiscount = null;

            foreach (var item in businessDetail.discounts.OrderBy(x => x.rate))
            {
                if (item.type == DiscountType.AllDay)
                {
                    discountMultiplier = 1 - (item.rate / 100);
                    activeDiscount = item;
                }
                else if (item.type == DiscountType.WeekDay && DateTime.Today.DayOfWeek >= DayOfWeek.Monday && DateTime.Today.DayOfWeek <= DayOfWeek.Friday)
                {
                    discountMultiplier = 1 - (item.rate / 100);
                    activeDiscount = item;
                }
                else if (item.type == DiscountType.WeekEnd && (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday))
                {
                    discountMultiplier = 1 - (item.rate / 100);
                    activeDiscount = item;
                }

                var typeText = item.type == DiscountType.WeekDay
                                 ? Resource.Resource.HaftaIci
                                 : item.type == DiscountType.WeekEnd
                                   ? Resource.Resource.HaftaSonu
                                   : string.Empty;

                item.title = $"{string.Format(Resource.Resource.Indirim, item.rate)} {typeText}";
            }

            var popularServices = businessDetail.businessServices.Where(x => x.isPopular);

            if (popularServices.Any())
            {
                BusinessServicesInfo businessServiceInfo = new BusinessServicesInfo();
                businessServiceInfo.serviceName = Resource.Resource.PopulerServisler;
                businessServiceInfo.className = "popular";

                foreach (var item in popularServices)
                {
                    bool isDiscountAvailable = activeDiscount != null && (activeDiscount.serviceIds.IsNullOrEmpty() || activeDiscount.serviceIds.Contains(item.serviceId.Value.ToString()));

                    item.discountPrice = isDiscountAvailable
                                         ? item.price * discountMultiplier
                                         : item.price;

                    businessServiceInfo.businessServices.Add(item);
                }

                businessDetail.businessServicesInfos.Add(businessServiceInfo);
            }

            foreach (var items in businessDetail.businessServices.GroupBy(x => x.serviceId))
            {
                BusinessServicesInfo businessServiceInfo = new BusinessServicesInfo();
                var service = services.FirstOrDefault(x => x.id == items.Key.Value);
                businessServiceInfo.serviceName = service != null ? (culture == "en" ? service.nameEn : service.name) : string.Empty;
                businessServiceInfo.className = service != null ? service.className : string.Empty;

                foreach (var item in items)
                {
                    bool isDiscountAvailable = activeDiscount != null && (activeDiscount.serviceIds.IsNullOrEmpty() || activeDiscount.serviceIds.Contains(item.serviceId.Value.ToString()));

                    item.discountPrice = isDiscountAvailable
                                         ? item.price * discountMultiplier
                                         : item.price;

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
        public async Task<IActionResult> GetBusinessNames()
        {
            ResponseModel<List<Tuple<string, string>>> response = new ResponseModel<List<Tuple<string, string>>>();

            var businessSelectListDictionary = await _businessService.GetBusinessSelectListAsync();

            if (businessSelectListDictionary == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitBulunamadi;
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
        public async Task<IActionResult> SetProfilePhoto([FromForm] BusinessFileInfoModel businessFileInfoModel)
        {
            ResponseModel<BusinessGallery> response = new ResponseModel<BusinessGallery>();

            if (businessFileInfoModel.file == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("files", Resource.Resource.BuAlaniBosBirakmayiniz));
                response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
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
                response.Message = Resource.Resource.SirketBulunamadi;
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

            response.Message = Resource.Resource.ResimYuklemeBasarili;
            response.Data = await _businessGalleryService.SaveBusinessGalleryAsync(businessGallery);
            BackgroundJob.Enqueue(() => _elasticHandler.UpdateOrCreateIndexBusiness(business.id));

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
        public async Task<IActionResult> Update(Business updateBusiness)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var business = await HelperMethods.GetSessionBusiness(Request, _businessService)
                               ?? await _businessService.GetBusinessByIdAsync(updateBusiness.id);

            if (business == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.SirketBulunamadi;
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
            BackgroundJob.Enqueue(() => _elasticHandler.UpdateOrCreateIndexBusiness(business.id));
            response.Message = Resource.Resource.KayitBasarili;
            response.Data = true;

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
        public async Task<IActionResult> SaveWorkingInfo(BusinessWorkInfoModel businessWorkInfoModel)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (businessWorkInfoModel.businessWorkingInfo == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessWorkingInfo", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (businessWorkInfoModel.appointmentPeopleCount == 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("appointmentPeopleCount", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (businessWorkInfoModel.appointmentTimeInterval == 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("appointmentTimeInterval", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitSilinemedi;
                return Ok(response);
            }

            var business = await HelperMethods.GetSessionBusiness(Request, _businessService)
                           ?? await _businessService.GetBusinessByIdAsync(businessWorkInfoModel.businessId);

            if (business == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.SirketBulunamadi;
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
            BackgroundJob.Enqueue(() => _elasticHandler.UpdateOrCreateIndexBusiness(business.id));

            response.Message = Resource.Resource.KayitBasarili;
            response.Data = true;

            return Ok(response);
        }

        /// <summary>
        /// Add Gallery Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost("addgalleryphoto")]
        public async Task<IActionResult> AddGalleryPhoto([FromForm] BusinessFileInfoModel businessFileInfoModel)
        {
            ResponseModel<BusinessGallery> response = new ResponseModel<BusinessGallery>();

            if (businessFileInfoModel.file == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("file", Resource.Resource.BuAlaniBosBirakmayiniz));
                response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
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
                response.Message = Resource.Resource.SirketBulunamadi;
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

            response.Message = Resource.Resource.ResimYuklemeBasarili;
            response.Data = await _businessGalleryService.SaveBusinessGalleryAsync(businessGallery);
            BackgroundJob.Enqueue(() => _elasticHandler.UpdateOrCreateIndexBusiness(business.id));

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
        public async Task<IActionResult> UpdateGalleryPhoto([FromBody] BusinessGallery updateBusinessGallery)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var businessGallery = await _businessGalleryService.GetBusinessGalleryByIdAsync(updateBusinessGallery.id);

            if (businessGallery == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            businessGallery.isSliderPhoto = updateBusinessGallery.isSliderPhoto;
            businessGallery.isProfilePhoto = updateBusinessGallery.isProfilePhoto;
            businessGallery.sortOrder = updateBusinessGallery.sortOrder;

            response.Data = await _businessGalleryService.UpdateBusinessGalleryAsync(businessGallery);

            if (businessGallery.businessId.HasValue)
            {
                BackgroundJob.Enqueue(() => _elasticHandler.UpdateOrCreateIndexBusiness(businessGallery.businessId.Value));
            }

            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
        }

        /// <summary>
        /// Delete Gallery Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost("deletegalleryphoto")]
        public async Task<IActionResult> DeleteGalleryPhoto([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message += Resource.Resource.IdParametreHatasi;
            }

            if (response.HasError)
                return Ok(response);

            await _businessGalleryService.DeleteBusinessGalleryByIdAsync(id.ToGuid());
            response.Message = Resource.Resource.KayitSilindi;
            response.Data = true;

            return Ok(response);
        }


        /// <summary>
        /// Delete Business
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Business business)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            response.Data = await _businessService.DeleteBusinessAsync(business);
            return Ok(response);
        }
    }
}
