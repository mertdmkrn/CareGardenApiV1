using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
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
        [Authorize]
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            ResponseModel<UserResponseModel> response = new ResponseModel<UserResponseModel>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _userService.GetUserResponseModelById(id.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = $"{id} id {Resource.Resource.UserNotFound}";
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Session User
        /// </summary>
        /// <returns></returns>
        [HttpPost("get")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            ResponseModel<UserResponseModel> response = new ResponseModel<UserResponseModel>();
            var user = await HelperMethods.GetSessionUserResponseModel(Request, _userService);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFound;
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
        [Authorize]
        public async Task<IActionResult> SendFeedBack([FromForm] MailRequest email)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (email.Subject.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("subject", Resource.Resource.NotEmpty));
            }

            if (email.Body.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("body", Resource.Resource.ValidEmailMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.FeedbackNotSend;
                return Ok(response);
            }

            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            var userName = HelperMethods.GetClaimInfo(Request, ClaimTypes.Name);
            var userEmail = HelperMethods.GetClaimInfo(Request, ClaimTypes.Email);

            email.Body = $"<p>{email.Body}</p><p>Gönderen: {userName} - {userEmail}</p>";
            email.ToEmailList = await _userService.GetAdminEmailListAsync();

            await _mailHandler.SendEmailAsync(email);

            response.Message = Resource.Resource.FeedbackSend;
            response.Data = true;

            return Ok(response);
        }

        /// <summary>
        /// User Set Profile Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost("setprofilephoto")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> SetProfilePhoto(IFormFile file)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            if (file == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("file", Resource.Resource.NotEmpty));
                response.Message = Resource.Resource.ErrorMessage;
                return Ok(response);
            }

            var user = await HelperMethods.GetSessionUser(Request, _userService);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFound;
                return Ok(response);
            }

            string fileName = $"{user?.fullName.ToLower().TurkishChrToEnglishChr().Replace(" ", "-")} - {DateTime.Now.ToString("ddMMhhmmss")}.{file.FileName.Split(".").LastOrDefault()}";
            await _fileHandler.UploadFile(file, "UserImages", fileName);
            user.imageUrl = await _fileHandler.UploadFreeImageServer(file);

            if (user.imageUrl.IsNullOrEmpty())
                user.imageUrl = string.Format("{0}://{1}/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, $"StaticFiles/UploadedFiles/UserImages/{fileName}");

            await _userService.UpdateUserAsync(user);

            response.Message = Resource.Resource.ImageUploaded;
            response.Data = user.imageUrl;

            return Ok(response);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RecordNotDeleted;
                return Ok(response);
            }

            User user = await _userService.GetUserById(id.ToGuid());

            if (user == null)
            {
                response.HasError = true;
                response.Message = $"{id} id {Resource.Resource.UserNotFound}";
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
        public async Task<IActionResult> GetPopularBusiness([FromBody] BusinessSearchRequestModel businessSearchModel)
        {
            ResponseModel<IList<BusinessListResponseModel>> response = new ResponseModel<IList<BusinessListResponseModel>>();

            bool isSendLocationInfo = businessSearchModel.latitude.HasValue && businessSearchModel.latitude > 0 &&
                                      businessSearchModel.longitude.HasValue && businessSearchModel.longitude > 0;

            string userCityInfo = HelperMethods.GetClaimInfo(Request, ClaimTypes.Locality);


            if (businessSearchModel.city.IsNullOrEmpty())
            {
                if (isSendLocationInfo && userCityInfo.IsNullOrEmpty())
                {
                    businessSearchModel.city = HelperMethods.findNearestCity(businessSearchModel.longitude.Value, businessSearchModel.latitude.Value);
                }
                else
                {
                    businessSearchModel.city = userCityInfo.IsNull("İstanbul");
                }
            }

            response.Data = await _businessService.GetBusinessByPopularAsync(businessSearchModel);

            if (!isSendLocationInfo && response.Data.IsNullOrEmpty() && businessSearchModel.city != "İstanbul")
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
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetFavoriteBusiness([FromBody] BusinessSearchRequestModel businessSearchModel)
        {
            ResponseModel<IList<BusinessListResponseModel>> response = new ResponseModel<IList<BusinessListResponseModel>>();

            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            businessSearchModel.favoriteBusinessIds = await _userService.GetUserFavoriteBusinessIds(userId.ToGuid());

            if (!businessSearchModel.favoriteBusinessIds.IsNullOrEmpty())
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
        public async Task<IActionResult> GetNearByBusiness([FromBody] BusinessSearchRequestModel businessSearchModel)
        {
            ResponseModel<IList<BusinessListResponseModel>> response = new ResponseModel<IList<BusinessListResponseModel>>();

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
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Update([FromBody] User updateUser)
        {
            ResponseModel<UserResponseModel> response = new ResponseModel<UserResponseModel>();

            if (updateUser.fullName.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("firstname", Resource.Resource.NotEmpty));
            }

            if (updateUser.city.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("city", Resource.Resource.NotEmpty));
            }

            if (!updateUser.birthDate.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("birthDate", Resource.Resource.NotEmpty));
            }

            if (!updateUser.fullName.IsValidFullName())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.ValidNameMessage));
            }

            if (updateUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!updateUser.email.IsNullOrEmpty() && !updateUser.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RecordNotUpdated;
                return Ok(response);
            }

            var user = await HelperMethods.GetSessionUser(Request, _userService);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFound;
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
            response.Message = Resource.Resource.RegistrationSuccess;

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
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeRequestModel updateUser)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (updateUser.currentPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("currentPassword", Resource.Resource.NotEmpty));
            }

            if (updateUser.newPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("newPassword", Resource.Resource.NotEmpty));
            }

            if (updateUser.newRetryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("newRetryPassword", Resource.Resource.NotEmpty));
            }

            if (updateUser.currentPassword.IsNotNullOrEmpty() && !updateUser.currentPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("currentPassword", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (updateUser.newPassword.IsNotNullOrEmpty() && !updateUser.newPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("newPassword", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (updateUser.newRetryPassword.IsNotNullOrEmpty() && !updateUser.newRetryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("newRetryPassword", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (!updateUser.newPassword.Equals(updateUser.newRetryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("newPassword", Resource.Resource.PasswordsMustBeEqual));
                response.ValidationErrors.Add(new ValidationError("newRetryPassword", Resource.Resource.PasswordsMustBeEqual));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.ResetPasswordFailed;
                return Ok(response);
            }

            var user = await HelperMethods.GetSessionUser(Request, _userService);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFound;
                return Ok(response);
            }

            if (user.password != updateUser.currentPassword.HashString())
            {
                response.HasError = true;
                response.Message = Resource.Resource.CurrentPasswordNotCorrectly;
                return Ok(response);
            }

            user.password = updateUser.newPassword;
            await _userService.UpdateUserAsync(user, true);

            response.Data = true;
            response.Message = Resource.Resource.ResetPasswordSuccess;

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
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateLocation([FromBody] User updateUser)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (!updateUser.latitude.HasValue || updateUser.latitude <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("latitude", Resource.Resource.NotEmpty));
            }

            if (!updateUser.longitude.HasValue || updateUser.longitude <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("longitude", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = $"{Resource.Resource.LocationNotUpdated} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            var user = await HelperMethods.GetSessionUser(Request, _userService);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFound;
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
            response.Message = Resource.Resource.LocationUpdated;

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
