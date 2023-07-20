using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    public class BusinessController : ControllerBase
    {
        private IBusinessService _businessService;
        private IBusinessWorkingInfoService _businessWorkingInfoService;
        private IBusinessGalleryService _businessGalleryService;
        private IFileHandler _fileHandler;

        public BusinessController()
        {
            _businessService = new BusinessService();
            _businessGalleryService = new BusinessGalleryService();
            _businessWorkingInfoService = new BusinessWorkingInfoService();
            _fileHandler = new FileHandler();
        }

        /// <summary>
        /// Get Business By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("business/getbyid")]
        public async Task<IActionResult> GetBusinessById([FromBody] string id)
        {
            ResponseModel<Business> response = new ResponseModel<Business>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
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
                    response.Message += id + " id " + Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                response.Data.password = null;
                response.Data.retryPassword = null;

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }

        }

        /// <summary>
        /// Get Session Business
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("business/get")]
        public async Task<IActionResult> GetBusiness()
        {
            ResponseModel<Business> response = new ResponseModel<Business>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                var business = await HelperMethods.GetSessionBusiness(Request, _businessService);
                
                if (business == null)
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                business.password = null;
                response.Data = business;

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Get Business Select Box
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("business/selectlist")]
        public async Task<IActionResult> GetBusinessSelectList()
        {
            ResponseModel<List<Tuple<string, string>>> response = new ResponseModel<List<Tuple<string, string>>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
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
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Set Profile Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("business/setprofilephoto")]
        public async Task<IActionResult> SetProfilePhoto(IFormFile file)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (file == null)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("files", Resource.Resource.BuAlaniBosBirakmayiniz));
                    response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
                    return Ok(response);
                }

                var business = await HelperMethods.GetSessionBusiness(Request, _businessService);

                if (business == null)
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.SirketBulunamadi;
                    return Ok(response);
                }

                List<BusinessGallery> businessGalleries = new List<BusinessGallery>();

                string fileName = file.FileName.Split(".").FirstOrDefault() + "-" + DateTime.Now.ToString("ddMMhhmmss") + "." + file.FileName.Split(".").LastOrDefault();
                string businessName = business.name.ToLower().TurkishChrToEnglishChr().Replace(" ", "-");
                await _fileHandler.UploadFile(file, "BusinessImages/" + businessName, fileName);

                string imageUrl = await _fileHandler.UploadFreeImageServer(file);

                if(imageUrl.IsNullOrEmpty())
                    imageUrl = string.Format("{0}://{1}/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, "UploadedFiles/BusinessImages/" + businessName + "/" + fileName);

                BusinessGallery businessGallery = new BusinessGallery
                {
                    imageUrl = imageUrl,
                    businessId = business.id,
                    size = file.GetImageSize(),
                    isProfilePhoto = true,
                };

                await _businessGalleryService.SaveBusinessGalleryAsync(businessGallery);

                response.Message = Resource.Resource.ResimYuklemeBasarili;
                response.Data = true;

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
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
        ///        "discountRate": 20,
        ///        "isFeatured": true,
        ///        "hasPromotion": true
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("business/update")]
        public async Task<IActionResult> Update(Business updateBusiness)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                var business = await HelperMethods.GetSessionBusiness(Request, _businessService);

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
                business.discountRate = updateBusiness.discountRate.IsNull(business.discountRate);
                business.isFeatured = updateBusiness.isFeatured;
                business.hasPromotion = updateBusiness.hasPromotion;

                if(updateBusiness.latitude > 0 && updateBusiness.longitude > 0)
                {
                    var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);
                    business.location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(updateBusiness.latitude, updateBusiness.longitude));
                }

                await _businessService.UpdateBusinessAsync(business);
                response.Message = Resource.Resource.KayitBasarili;
                response.Data = true;

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
        }


        /// <summary>
        /// Save Business Working Info
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "workingGenderType" : 1,
        ///        "workingDayType" : 2,
        ///        "workingStartHour": "09:00",
        ///        "workingEndHour": "21:00",
        ///        "appointmentTimeInterval": 30,
        ///        "appointmentPeopleCount": 5,
        ///        "officialHolidayAvailable": true
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("business/saveworkinginfo")]
        public async Task<IActionResult> SaveWorkingInfo(Business updateBusiness)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (updateBusiness == null)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("updateBusiness", Resource.Resource.BuAlaniBosBirakmayiniz));
                    response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
                    return Ok(response);
                }

                var business = await HelperMethods.GetSessionBusiness(Request, _businessService);

                if (business == null)
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.SirketBulunamadi;
                    return Ok(response);
                }
                
                business.workingGenderType = updateBusiness.workingGenderType;
                business.workingDayType = updateBusiness.workingDayType;
                business.workingStartHour = updateBusiness.workingStartHour;
                business.workingEndHour = updateBusiness.workingEndHour;
                business.appointmentTimeInterval = updateBusiness.appointmentTimeInterval;
                business.appointmentPeopleCount = updateBusiness.appointmentPeopleCount;
                business.officialHolidayAvailable = updateBusiness.officialHolidayAvailable;

                await _businessService.UpdateBusinessAsync(business);
                List<BusinessWorkingInfo> businessWorkingInfos = new List<BusinessWorkingInfo>();

                for (int i = 0; i < 15; i++)
                {
                    BusinessWorkingInfo businessWorkingInfo = new BusinessWorkingInfo();

                    DateTime date = DateTime.Today.AddDays(i);

                    businessWorkingInfo.businessId = business.id;
                    businessWorkingInfo.appointmentPeopleCount = business.appointmentPeopleCount;
                    businessWorkingInfo.appointmentTimeInterval = business.appointmentTimeInterval;
                    businessWorkingInfo.startHour = business.workingStartHour;
                    businessWorkingInfo.endHour = business.workingEndHour;
                    businessWorkingInfo.date = date;

                    if (business.workingDayType == Enums.WorkingDayType.MondayFriday && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
                    {
                        businessWorkingInfo.isOffDay = true;
                    }
                    else if (business.workingDayType == Enums.WorkingDayType.MondaySaturday && date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        businessWorkingInfo.isOffDay = true;
                    }

                    if (business.officialHolidayAvailable && Constants.OfficialDays.Any(x => x.date.Equals(date)))
                    {
                        businessWorkingInfo.isOffDay = true;
                    }

                    businessWorkingInfos.Add(businessWorkingInfo);
                }

                await _businessWorkingInfoService.DeleteBusinessWorkingInfoByBusinessIdAsync(business.id);
                await _businessWorkingInfoService.SaveBusinessWorkingInfosAsync(businessWorkingInfos);

                response.Message = Resource.Resource.KayitBasarili;
                response.Data = true;

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Add Gallery Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("business/addgalleryphoto")]
        public async Task<IActionResult> AddGalleryPhoto(List<IFormFile> files)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (files == null || files.Count == 0)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("files", Resource.Resource.BuAlaniBosBirakmayiniz));
                    response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
                    return Ok(response);
                }

                var business = await HelperMethods.GetSessionBusiness(Request, _businessService);

                if (business == null)
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.SirketBulunamadi;
                    return Ok(response);
                }

                List<BusinessGallery> businessGalleries = new List<BusinessGallery>();

                foreach (var file in files)
                {
                    string fileName = file.FileName.Split(".").FirstOrDefault() + "-" + DateTime.Now.ToString("ddMMhhmmss") + "." + file.FileName.Split(".").LastOrDefault();
                    string businessName = business.name.ToLower().TurkishChrToEnglishChr().Replace(" ", "-");
                    await _fileHandler.UploadFile(file, "BusinessImages/" + businessName, fileName);

                    string imageUrl = await _fileHandler.UploadFreeImageServer(file);

                    if (imageUrl.IsNullOrEmpty())
                        imageUrl = string.Format("{0}://{1}/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, "UploadedFiles/BusinessImages/" + businessName + "/" + fileName);

                    businessGalleries.Add(new BusinessGallery
                    {
                        imageUrl = imageUrl,
                        businessId = business.id,
                        size = file.GetImageSize()
                    });
                }
       
                await _businessGalleryService.SaveBusinessGalleriesAsync(businessGalleries);

                response.Message = Resource.Resource.ResimYuklemeBasarili;
                response.Data = true;

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
        }

        ///// <summary>
        ///// Delete User
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("user/delete")]
        //public async Task<IActionResult> Delete([FromBody] string id)
        //{
        ///    ResponseModel<bool> response = new ResponseModel<bool>();
        ///    Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

        ///    try
        ///    {
        ///        if (!id.IsGuid())
        ///        {
        ///            response.HasError = true;
        ///            response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
        ///        }

        ///        if (response.HasError)
        ///        {
        ///            response.Message = Resource.Resource.KayitSilinemedi;
        ///            return Ok(response);
        ///        }

        ///        User user = await _userService.GetUserById(id.ToGuid());

        ///        if (user == null)
        ///        {
        ///            response.HasError = true;
        ///            response.Message = id + " id " + Resource.Resource.KullaniciBulunamadi;
        ///            return Ok(response);
        ///        }

        ///        response.Data = await _userService.DeleteUserAsync(user);

        ///        return Ok(response);
        ///    }
        ///    catch (Exception ex)
        ///    {
        ///        response.HasError = true;
        ///        response.Message = Resource.Resource.KayitSilinemedi + " Exception => " + ex.Message;
        ///        return Ok(response);
        ///    }
        //}


        ///// <summary>
        ///// Update User
        ///// </summary>
        ///// <remarks>
        ///// **Sample request body:**
        /////
        /////     { 
        /////        "fullName" : "Mert DEMİRKIRAN",
        /////        "birthDate": "1998-08-01",
        /////        "gender": 1,
        /////        "city": "İstanbul",
        /////        "services": "Make Up;Beauty Saloon"
        /////     }
        /////
        ///// </remarks>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("user/update")]
        //public async Task<IActionResult> Update([FromBody] User updateUser)
        //{
        ///    ResponseModel<User> response = new ResponseModel<User>();
        ///    Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

        ///    try
        ///    {
        ///        if (updateUser.fullName.IsNullOrEmpty())
        ///        {
        ///            response.HasError = true;
        ///            response.ValidationErrors.Add(new ValidationError("firstname", Resource.Resource.BuAlaniBosBirakmayiniz));
        ///        }

        ///        if (!updateUser.birthDate.HasValue)
        ///        {
        ///            response.HasError = true;
        ///            response.ValidationErrors.Add(new ValidationError("birthDate", Resource.Resource.BuAlaniBosBirakmayiniz));
        ///        }

        ///        if (!updateUser.fullName.IsValidFullName())
        ///        {
        ///            response.HasError = true;
        ///            response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.GecerliBirIsimGiriniz));
        ///        }

        ///        if (response.HasError)
        ///        {
        ///            response.Message = Resource.Resource.GuncellemeYapilamadi;
        ///            return Ok(response);
        ///        }

        ///        var user = await HelperMethods.GetSessionUser(Request, _userService);

        ///        if (user == null)
        ///        {
        ///            response.HasError = true;
        ///            response.Message = Resource.Resource.KullaniciBulunamadi;
        ///            return Ok(response);
        ///        }

        ///        user.fullName = updateUser.fullName;
        ///        user.birthDate = updateUser.birthDate;
        ///        user.gender = updateUser.gender;
        ///        user.city = updateUser.city;
        ///        user.services = updateUser.services.TrimEnd(';');

        ///        response.Data = await _userService.UpdateUserAsync(user);

        ///        return Ok(response);
        ///    }
        ///    catch (Exception ex)
        ///    {
        ///        response.HasError = true;
        ///        response.Message = Resource.Resource.GuncellemeYapilamadi + " Exception => " + ex.Message;
        ///        return Ok(response);
        ///    }
        //}
    }
}
