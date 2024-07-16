using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Service.Abstract;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Route("businessuser")]
    public class BusinessUserController : ControllerBase
    {
        private readonly IBusinessUserService _businessUserService;
        private readonly IFaqService _faqService;
        private readonly IResetLinkService _resetLinkService;
        private readonly IConfirmationService _confirmationService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IFileHandler _fileHandler;
        private readonly IMemoryCache _memoryCache;
        private readonly IMailHandler _mailHandler;

        public BusinessUserController(
            IBusinessUserService businessUserService,
            IFaqService faqService,
            IResetLinkService resetLinkService,
            IConfirmationService confirmationService,
            ITokenHandler tokenHandler,
            IFileHandler fileHandler,
            IMemoryCache memoryCache,
            IMailHandler mailHandler)
        {
            _businessUserService = businessUserService;
            _faqService = faqService;
            _resetLinkService = resetLinkService;
            _confirmationService = confirmationService;
            _tokenHandler = tokenHandler;
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
            ResponseModel<BusinessUserResponseModel> response = new ResponseModel<BusinessUserResponseModel>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessUserService.GetBusinessUserResponseModelById(id.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = $"{id} id {Resource.Resource.UserNotFound}";
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Session Business User
        /// </summary>
        /// <returns></returns>
        [HttpPost("get")]
        [Authorize(Roles = "BusinessAdmin,BusinessWorker,Business")]
        public async Task<IActionResult> Get()
        {
            ResponseModel<BusinessUserResponseModel> response = new ResponseModel<BusinessUserResponseModel>();
            var businessUser = await HelperMethods.GetSessionBusinessUserResponseModel(Request, _businessUserService);

            if (businessUser == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFound;
                return Ok(response);
            }

            response.Data = businessUser;

            return Ok(response);
        }

        /// <summary>
        /// Send Telephone Confirmation Code
        /// </summary>
        /// <returns></returns>
        [HttpPost("sendtelephoneconfirmationcode")]
        public async Task<IActionResult> SendTelephoneConfirmationCode([FromBody] string telephoneNumber)
        {
            ResponseModel<int> response = new ResponseModel<int>();
            int confirmationCode = new Random().Next(1000, 9999);

            if (telephoneNumber.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.NotEmpty));
            }

            if (!telephoneNumber.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.ValidTelephoneMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.ConfirmationCodeNotSend;
                response.HasError = true;
                return Ok(response);
            }

            var isExistUser = await _businessUserService.GetBusinessUserExistsByTelephoneNumberAsync(telephoneNumber);

            if (isExistUser)
            {
                response.HasError = true;
                response.Message = $"{Resource.Resource.UserFoundEnteredTelephone} {Resource.Resource.ResetPasswordMessage}";
                return Ok(response);
            }

            var systemConfirmationInfo = await _confirmationService.GetConfirmationInfo(telephoneNumber);

            if (systemConfirmationInfo != null && systemConfirmationInfo.createDate.DifferenceBetweenDates(DateTime.Now, DateType.Minute) < 1)
            {
                response.Message = Resource.Resource.ConfirmationCodeMessageInOneMinute;
                response.HasError = true;
                return Ok(response);
            }

            string smsMessage = string.Format(Resource.Resource.ConfirmationMessage, confirmationCode);

            //var sendSms = await _smsHandler.SendSmsAsync(smsMessage, telephoneNumber);

            await _confirmationService.SaveConfirmationInfoAsync(telephoneNumber, confirmationCode.ToString());

            response.Message = $"{Resource.Resource.ConfirmationCodeSend} {smsMessage}";
            response.Data = confirmationCode;

            return Ok(response);
        }


        /// <summary>
        /// User Set Profile Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost("setprofilephoto")]
        [Authorize(Roles = "BusinessAdmin,BusinessWorker,Business")]
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

            var businessUser = await HelperMethods.GetSessionBusinessUser(Request, _businessUserService);

            if (businessUser == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFound;
                return Ok(response);
            }

            string fileName = $"{businessUser?.fullName.ToLower().TurkishChrToEnglishChr().Replace(" ", "-")} - {DateTime.Now.ToString("ddMMhhmmss")}.{file.FileName.Split(".").LastOrDefault()}";
            await _fileHandler.UploadFile(file, "BusinessUserImages", fileName);
            businessUser.imageUrl = await _fileHandler.UploadFreeImageServer(file);

            if (businessUser.imageUrl.IsNullOrEmpty())
                businessUser.imageUrl = string.Format("{0}://{1}/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, $"StaticFiles/UploadedFiles/UserImages/{fileName}");

            await _businessUserService.UpdateBusinessUserAsync(businessUser);

            response.Message = Resource.Resource.ImageUploaded;
            response.Data = businessUser.imageUrl;

            return Ok(response);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        [Authorize(Roles = "BusinessAdmin,Business,Admin")]
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

            BusinessUser businessUser = await _businessUserService.GetBusinessUserById(id.ToGuid());

            if (businessUser == null)
            {
                response.HasError = true;
                response.Message = $"{id} id {Resource.Resource.UserNotFound}";
                return Ok(response);
            }

            response.Data = await _businessUserService.DeleteBusinessUserAsync(businessUser);

            return Ok(response);
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
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
        [Authorize(Roles = "BusinessAdmin,Business,BusinessWorker,Admin")]
        public async Task<IActionResult> Update([FromBody] BusinessUser updateBusinessUser)
        {
            ResponseModel<BusinessUserResponseModel> response = new ResponseModel<BusinessUserResponseModel>();

            if (updateBusinessUser.fullName.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("firstname", Resource.Resource.NotEmpty));
            }

            if (!updateBusinessUser.birthDate.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("birthDate", Resource.Resource.NotEmpty));
            }

            if (!updateBusinessUser.fullName.IsValidFullName())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.ValidNameMessage));
            }

            if (updateBusinessUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!updateBusinessUser.email.IsNullOrEmpty() && !updateBusinessUser.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RecordNotUpdated;
                return Ok(response);
            }

            var businessUser = await HelperMethods.GetSessionBusinessUser(Request, _businessUserService);

            if (businessUser == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFound;
                return Ok(response);
            }

            businessUser.email = updateBusinessUser.email.IsNull(businessUser.email);
            businessUser.fullName = updateBusinessUser.fullName;
            businessUser.birthDate = updateBusinessUser.birthDate;
            businessUser.gender = updateBusinessUser.gender;

            await _businessUserService.UpdateBusinessUserAsync(businessUser);
            response.Data = await _businessUserService.GetBusinessUserResponseModelById(businessUser.id);
            response.Message = Resource.Resource.RegistrationSuccess;

            return Ok(response);
        }

        /// <summary>
        /// Business User Change Password
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
        [Authorize(Roles = "BusinessAdmin,Business,BusinessWorker,Admin")]
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

            var businessUser = await HelperMethods.GetSessionBusinessUser(Request, _businessUserService);

            if (businessUser == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFound;
                return Ok(response);
            }

            if (businessUser.password != updateUser.currentPassword.HashString())
            {
                response.HasError = true;
                response.Message = Resource.Resource.CurrentPasswordNotCorrectly;
                return Ok(response);
            }

            businessUser.password = updateUser.newPassword;
            await _businessUserService.UpdateBusinessUserAsync(businessUser, true);

            response.Data = true;
            response.Message = Resource.Resource.ResetPasswordSuccess;

            return Ok(response);
        }

        /// <summary>
        /// Save Business User
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "fullName" : "Mert DEMİRKIRAN",
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "password": "stms5581",
        ///        "retryPassword": "stms5581",
        ///        "telephone": "+905467335939"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] BusinessUser businessUser)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (businessUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!businessUser.email.IsNullOrEmpty() && !businessUser.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (!businessUser.telephone.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.ValidTelephoneMessage));
            }

            if (businessUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }

            if (businessUser.retryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.NotEmpty));
            }

            if (businessUser.password.IsNotNullOrEmpty() && !businessUser.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (businessUser.retryPassword.IsNotNullOrEmpty() && !businessUser.retryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordMustBeEightCharacters));
            }


            if (!businessUser.password.IsNullOrEmpty() && !businessUser.password.Equals(businessUser.retryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordsMustBeEqual));
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordsMustBeEqual));
            }

            if (businessUser.fullName.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.NotEmpty));
            }

            if (!businessUser.fullName.IsNullOrEmpty() && !businessUser.fullName.IsValidFullName())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.ValidNameMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }

            var systemBusinessUser = await _businessUserService.GetBusinessUserByEmailAsync(businessUser.email);

            if (systemBusinessUser != null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.RegisteredUserEnteredMail;
                return Ok(response);
            }

            businessUser = await _businessUserService.SaveBusinessUserAsync(businessUser);

            response.Message = Resource.Resource.RegistrationSuccess;

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, businessUser.fullName),
                new Claim(ClaimTypes.PrimarySid, businessUser.id.ToString()),
                new Claim(CustomClaimTypes.BusinessId, businessUser.businessId.ToString()),
                new Claim(ClaimTypes.Email, businessUser.email),
                new Claim(ClaimTypes.Role, $"Business{businessUser.role}")
            };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);

            return Ok(response);
        }

        /// <summary>
        /// Validate Business User
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "fullName" : "Mert DEMİRKIRAN",
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "password": "stms5581",
        ///        "retryPassword": "stms5581",
        ///        "telephone": "+905467335939"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] BusinessUser businessUser)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (businessUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!businessUser.email.IsNullOrEmpty() && !businessUser.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (!businessUser.telephone.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.ValidTelephoneMessage));
            }

            if (businessUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }

            if (businessUser.retryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.NotEmpty));
            }

            if (businessUser.password.IsNotNullOrEmpty() && !businessUser.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (businessUser.retryPassword.IsNotNullOrEmpty() && !businessUser.retryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordMustBeEightCharacters));
            }


            if (!businessUser.password.IsNullOrEmpty() && !businessUser.password.Equals(businessUser.retryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordsMustBeEqual));
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordsMustBeEqual));
            }

            if (businessUser.fullName.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.NotEmpty));
            }

            if (!businessUser.fullName.IsNullOrEmpty() && !businessUser.fullName.IsValidFullName())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.ValidNameMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }

            var systemBusinessUser = await _businessUserService.GetBusinessUserByEmailAsync(businessUser.email);

            if (systemBusinessUser != null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.RegisteredUserEnteredMail;
                return Ok(response);
            }

            response.Data = true;

            return Ok(response);
        }

        /// <summary>
        /// User Login Control
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "password": "stms5581"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] BusinessUser loginBusinessUser)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (loginBusinessUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!loginBusinessUser.email.IsNullOrEmpty() && !loginBusinessUser.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (loginBusinessUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }

            if (loginBusinessUser.password.IsNotNullOrEmpty() && !loginBusinessUser.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.LoginFailed;
                return Ok(response);
            }

            var businessUser = await _businessUserService.GetBusinessUserByEmailAndPasswordAsync(loginBusinessUser.email, loginBusinessUser.password);

            if (businessUser == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.LoginFailedUserMessage;
                return Ok(response);
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, businessUser.fullName),
                new Claim(ClaimTypes.PrimarySid, businessUser.id.ToString()),
                new Claim(CustomClaimTypes.BusinessId, businessUser.businessId.ToString()),
                new Claim(ClaimTypes.Email, businessUser.email),
                new Claim(ClaimTypes.Role, $"Business{businessUser.role}")
            };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);
            response.Message = Resource.Resource.LoginSuccess;

            return Ok(response);
        }


        /// <summary>
        /// Send Password Reset Link
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "mertdmkrn37@gmail.com",
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("sendpasswordresetlink")]
        public async Task<IActionResult> SendPasswordResetLink([FromBody] string email)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.PasswordResetLinkNotSend;
                response.HasError = true;
                return Ok(response);
            }

            var isExistUser = await _businessUserService.GetBusinessUserExistsByEmailAsync(email);

            if (!isExistUser)
            {
                response.HasError = true;
                response.Message = $"{Resource.Resource.UserFoundEnteredMail}";
                return Ok(response);
            }

            var systemResetLink = await _resetLinkService.GetResetLinkAsync(email);

            if (systemResetLink != null && systemResetLink.createDate.DifferenceBetweenDates(DateTime.Now, DateType.Hour) < 1)
            {
                response.HasError = true;
                response.Message = Resource.Resource.PasswordResetLinkMessageInOneHour;
                return Ok(response);
            }

            var resetLink = await _resetLinkService.SaveResetLinkAsync(email);

            string mailMessage = string.Format(Resource.Resource.PasswordResetLinkMessage, $"{Constants.BusinessAdminPanelUrl}reset-password/{resetLink.linkId}");

            var mailRequest = new MailRequest()
            {
                ToEmailList = new List<string> { email },
                Subject = $"CareGarden {Resource.Resource.ResetPassword}",
                Body = mailMessage
            };

            BackgroundJob.Enqueue(() => _mailHandler.SendEmailAsync(mailRequest));

            response.Message = $"{Resource.Resource.PasswordResetLinkSend}";
            response.Data = true;

            return Ok(response);
        }


        /// <summary>
        /// Send Password Reset Link
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "linkId": "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("existspasswordresetlink")]
        public async Task<IActionResult> SendPasswordResetLink([FromBody] PasswordResetRequestModel requestModel)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
          
            if (requestModel.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!requestModel.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidTelephoneMessage));
            }

            if (!requestModel.linkId.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("linkId", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.PasswordResetLinkNotFound;
                response.HasError = true;
                return Ok(response);
            }

            var systemResetLink = await _resetLinkService.GetResetLinkAsync(requestModel.email);

            if (systemResetLink == null || systemResetLink.linkId != requestModel.linkId.ToGuidNullable())
            {
                response.HasError = true;
                response.Message = Resource.Resource.PasswordResetLinkNotFound;
                return Ok(response);
            }

            if (systemResetLink.createDate.DifferenceBetweenDates(DateTime.Now, DateType.Hour) > 1)
            {
                response.Message = Resource.Resource.PasswordResetLinkExpired;
                response.HasError = true;
                return Ok(response);
            }

            response.Data = true;
            return Ok(response);
        }


        /// <summary>
        /// User Password Reset
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "password": "mert1453",
        ///        "retryPassword": "mert1453"
        ///        "linkId": "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("passwordreset")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequestModel updateBusinessUser)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (updateBusinessUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (updateBusinessUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }

            if (updateBusinessUser.retryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.NotEmpty));
            }

            if (updateBusinessUser.password.IsNotNullOrEmpty() && !updateBusinessUser.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (updateBusinessUser.retryPassword.IsNotNullOrEmpty() && !updateBusinessUser.retryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (!updateBusinessUser.password.Equals(updateBusinessUser.retryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordsMustBeEqual));
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordsMustBeEqual));
            }

            if (updateBusinessUser.linkId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("linkId", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.ResetPasswordFailed;
                return Ok(response);
            }

            var businessUser = await _businessUserService.GetBusinessUserByEmailAsync(updateBusinessUser.email);

            if (businessUser == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFoundEnteredMail;
                return Ok(response);
            }

            var systemResetLink = await _resetLinkService.GetResetLinkAsync(updateBusinessUser.email);

            if (systemResetLink == null || systemResetLink.linkId != updateBusinessUser.linkId.ToGuidNullable())
            {
                response.HasError = true;
                response.Message = Resource.Resource.PasswordResetLinkNotFound;
                return Ok(response);
            }

            if (systemResetLink.createDate.DifferenceBetweenDates(DateTime.Now, DateType.Hour) > 1)
            {
                response.Message = Resource.Resource.PasswordResetLinkExpired;
                response.HasError = true;
                return Ok(response);
            }

            businessUser.password = updateBusinessUser.password;
            await _businessUserService.UpdateBusinessUserAsync(businessUser, true);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, businessUser.fullName),
                new Claim(ClaimTypes.PrimarySid, businessUser.id.ToString()),
                new Claim(CustomClaimTypes.BusinessId, businessUser.businessId.ToString()),
                new Claim(ClaimTypes.Email, businessUser.email),
                new Claim(ClaimTypes.Role, $"Business{businessUser.role}")
            };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);
            response.Message = Resource.Resource.ResetPasswordSuccess;

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
