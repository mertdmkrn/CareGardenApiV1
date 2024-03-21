using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBusinessService _businessService;
        private readonly IFaqService _faqService;
        private readonly IFileHandler _fileHandler;
        private readonly IMemoryCache _memoryCache;
        private readonly IMailHandler _mailHandler;

        public UserController(
            IUserService userService,
            IBusinessService businessService,
            IFaqService faqService,
            IFileHandler fileHandler,
            IMemoryCache memoryCache,
            IMailHandler mailHandler)
        {
            _userService = userService;
            _businessService = businessService;
            _faqService = faqService;
            _fileHandler = fileHandler;
            _memoryCache = memoryCache;
            _mailHandler = mailHandler;
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbyid")]
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            ResponseModel<UserResponseModel> response = new ResponseModel<UserResponseModel>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message += Resource.Resource.IdParametreHatasi;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _userService.GetUserResponseModelById(id.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message += $"{id} id {Resource.Resource.KullaniciBulunamadi}";
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Session User
        /// </summary>
        /// <returns></returns>
        [HttpPost("get")]
        public async Task<IActionResult> Get()
        {
            ResponseModel<UserResponseModel> response = new ResponseModel<UserResponseModel>();
            var user = await HelperMethods.GetSessionUserResponseModel(Request, _userService);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            response.Data = user;

            return Ok(response);
        }

        /// <summary>
        /// User Send FeedBack
        /// </summary>
        /// <returns></returns>
        [HttpPost("sendfeedback")]
        public async Task<IActionResult> SendFeedBack([FromForm] MailRequest email)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (email.Subject.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("subject", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (email.Body.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("body", Resource.Resource.GecerliMailMesaji));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.GeriBildirimGonderilemedi;
                return Ok(response);
            }

            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            if (userId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.GirdiginizMaileAitKullaniciBulunamadi;
                return Ok(response);
            }

            var userName = HelperMethods.GetClaimInfo(Request, ClaimTypes.Name);
            var userEmail = HelperMethods.GetClaimInfo(Request, ClaimTypes.Email);

            email.Body = $"<p>{email.Body}</p><p>Gönderen: {userName} - {userEmail}</p>";
            email.ToEmailList = await _userService.GetAdminEmailListAsync();

            await _mailHandler.SendEmailAsync(email);

            response.Message = Resource.Resource.GeriBildirimGonderildi;
            response.Data = true;

            return Ok(response);
        }

        /// <summary>
        /// User Set Profile Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost("setprofilephoto")]
        public async Task<IActionResult> SetProfilePhoto(IFormFile file)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            if (file == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("file", Resource.Resource.BuAlaniBosBirakmayiniz));
                response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
                return Ok(response);
            }

            var user = await HelperMethods.GetSessionUser(Request, _userService);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            string fileName = $"{user?.fullName.ToLower().TurkishChrToEnglishChr().Replace(" ", "-")} - {DateTime.Now.ToString("ddMMhhmmss")}.{file.FileName.Split(".").LastOrDefault()}";
            await _fileHandler.UploadFile(file, "UserImages", fileName);
            user.imageUrl = await _fileHandler.UploadFreeImageServer(file);

            if (user.imageUrl.IsNullOrEmpty())
                user.imageUrl = string.Format("{0}://{1}/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, $"StaticFiles/UploadedFiles/UserImages/{fileName}");

            await _userService.UpdateUserAsync(user);

            response.Message = Resource.Resource.ResimYuklemeBasarili;
            response.Data = user.imageUrl;

            return Ok(response);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitSilinemedi;
                return Ok(response);
            }

            User user = await _userService.GetUserById(id.ToGuid());

            if (user == null)
            {
                response.HasError = true;
                response.Message = $"{id} id {Resource.Resource.KullaniciBulunamadi}";
                return Ok(response);
            }

            response.Data = await _userService.DeleteUserAsync(user);

            return Ok(response);
        }

        /// <summary>
        /// User Get Popular Business
        /// </summary>
        /// <returns></returns>
        [HttpPost("getpopularbusiness")]
        public async Task<IActionResult> GetPopularBusiness([FromBody] BusinessSearchModel businessSearchModel)
        {
            ResponseModel<IList<BusinessListModel>> response = new ResponseModel<IList<BusinessListModel>>();
            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            if (userId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            bool isSendNoLocationInfo = (!businessSearchModel.latitude.HasValue || businessSearchModel.latitude == 0)
                                        && (!businessSearchModel.longitude.HasValue || businessSearchModel.longitude == 0)
                                        && businessSearchModel.city.IsNullOrEmpty();

            if(isSendNoLocationInfo)
            {
                businessSearchModel.city = HelperMethods.GetClaimInfo(Request, ClaimTypes.Locality).IsNull("İstanbul");
            }
                
            response.Data = await _businessService.GetBusinessByPopularAsync(businessSearchModel);

            if(isSendNoLocationInfo && response.Data.IsNullOrEmpty() && businessSearchModel.city != "İstanbul")
            {
                businessSearchModel.city = "İstanbul";
                response.Data = await _businessService.GetBusinessByPopularAsync(businessSearchModel);
            }

            return Ok(response);
        }

        /// <summary>
        /// User Get Favorite Business
        /// </summary>
        /// <returns></returns>
        [HttpPost("getfavoritebusiness")]
        public async Task<IActionResult> GetFavoriteBusiness([FromBody] BusinessSearchModel businessSearchModel)
        {
            ResponseModel<IList<BusinessListModel>> response = new ResponseModel<IList<BusinessListModel>>();

            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            if (userId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            businessSearchModel.favoriteBusinessIds = await _userService.GetUserFavoriteBusinessIds(userId.ToGuid());

            businessSearchModel.favoriteBusinessIds.Add(new Guid("2d2e027e-6e9d-44eb-9871-d4ea169e1cbc"));

            if(!businessSearchModel.favoriteBusinessIds.IsNullOrEmpty())
            {
                response.Data = await _businessService.GetBusinessByUserFavorites(businessSearchModel);
            }

            return Ok(response);
        }


        /// <summary>
        /// User Get Near By Business
        /// </summary>
        /// <returns></returns>
        [HttpPost("getnearbybusiness")]
        public async Task<IActionResult> GetNearByBusiness([FromBody] BusinessSearchModel businessSearchModel)
        {
            ResponseModel<IList<BusinessListModel>> response = new ResponseModel<IList<BusinessListModel>>();
            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            if (userId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            if (businessSearchModel.longitude == null || businessSearchModel.latitude == null)
            {
                return Ok(response);
            }

            response.Data = await _businessService.GetBusinessNearByDistanceAsync(businessSearchModel);

            return Ok(response);
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "fullName" : "Mert DEMİRKIRAN",
        ///        "email" : "mertdmkrn37@gmail.com",
        ///        "birthDate": "1998-08-01",
        ///        "gender": 1,
        ///        "city": "İstanbul",
        ///        "services": "Make Up;Beauty Saloon"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] User updateUser)
        {
            ResponseModel<UserResponseModel> response = new ResponseModel<UserResponseModel>();

            if (updateUser.fullName.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("firstname", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateUser.city.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("city", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!updateUser.birthDate.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("birthDate", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!updateUser.fullName.IsValidFullName())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.GecerliBirIsimGiriniz));
            }

            if (updateUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!updateUser.email.IsNullOrEmpty() && !updateUser.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.GecerliMailMesaji));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.GuncellemeYapilamadi;
                return Ok(response);
            }

            var user = await HelperMethods.GetSessionUser(Request, _userService);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            user.email = updateUser.email.IsNull(user.email);
            user.fullName = updateUser.fullName;
            user.birthDate = updateUser.birthDate;
            user.gender = updateUser.gender;
            user.city = updateUser.city;
            user.services = updateUser.services?.TrimEnd(';');

            await _userService.UpdateUserAsync(user);
            response.Data = await _userService.GetUserResponseModelById(user.id);
            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
        }

        /// <summary>
        /// User Change Password
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "currentPassword" : "12345678",
        ///        "newPassword" : "87654321",
        ///        "newRetryPassword": "87654321"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeModel updateUser)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (updateUser.currentPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("currentPassword", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateUser.newPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("newPassword", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateUser.newRetryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("newRetryPassword", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateUser.currentPassword.IsNotNullOrEmpty() && !updateUser.currentPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("currentPassword", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (updateUser.newPassword.IsNotNullOrEmpty() && !updateUser.newPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("newPassword", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (updateUser.newRetryPassword.IsNotNullOrEmpty() && !updateUser.newRetryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("newRetryPassword", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (!updateUser.newPassword.Equals(updateUser.newRetryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("newPassword", Resource.Resource.SifrelerEsitOlmali));
                response.ValidationErrors.Add(new ValidationError("newRetryPassword", Resource.Resource.SifrelerEsitOlmali));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.GuncellemeYapilamadi;
                return Ok(response);
            }

            var user = await HelperMethods.GetSessionUser(Request, _userService);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            if (user.password != updateUser.currentPassword.HashString())
            {
                response.HasError = true;
                response.Message = Resource.Resource.GuncelSifreniziDogruGiriniz;
                return Ok(response);
            }

            user.password = updateUser.newPassword;
            await _userService.UpdateUserAsync(user, true);

            response.Data = true;
            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
        }

        /// <summary>
        /// Update User Location
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "latitude" : 32,
        ///        "longitude": 31
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("updatelocation")]
        public async Task<IActionResult> UpdateLocation([FromBody] User updateUser)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (!updateUser.latitude.HasValue || updateUser.latitude <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("latitude", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!updateUser.longitude.HasValue || updateUser.longitude <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("longitude", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.GuncellemeYapilamadi;
                return Ok(response);
            }

            var user = await HelperMethods.GetSessionUser(Request, _userService);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            user.latitude = updateUser.latitude.HasValue ? updateUser.latitude : user.latitude;
            user.longitude = updateUser.longitude.HasValue ? updateUser.longitude : user.latitude;

            if (updateUser.latitude > 0 && updateUser.longitude > 0)
            {
                var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);
                user.location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(updateUser.longitude.Value, updateUser.longitude.Value));
            }

            await _userService.UpdateUserAsync(user);
            response.Data = true;
            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
        }


        /// <summary>
        /// Get Faqs
        /// </summary>
        /// <returns></returns>
        [HttpPost("getfaqs")]
        public async Task<IActionResult> GetFaqs()
        {
            var culture = Request.Headers["Language"].ToString().IsNull("en");
            ResponseModel<FaqResponseModel> response = new ResponseModel<FaqResponseModel>();

            var fagResponseModel = new FaqResponseModel();

            if (_memoryCache.TryGetValue("faqs", out object list))
            {
                fagResponseModel.faqs = (List<Faq>)list;
            }
            else
            {
                fagResponseModel.faqs = await _faqService.GetFaqsAsync();
                _memoryCache.Set("faqs", fagResponseModel.faqs, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal
                });
            }

            fagResponseModel.categories = fagResponseModel.faqs.Select(x => culture == "en" ? x.categoryEn : x.category).Distinct().ToList();

            response.Data = fagResponseModel;

            return Ok(response);
        }
    }
}
