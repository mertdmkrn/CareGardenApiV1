using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Route("business")]
    public class BusinessLoginController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IBusinessPropertiesService _businessPropertiesService;
        private readonly IConfirmationService _contirmationService;
        private readonly ITokenHandler _tokenHandler;
        private readonly ISmsHandler _smsHandler;
        private readonly IMailHandler _mailHandler;

        public BusinessLoginController(
            IBusinessService businessService,
            IBusinessPropertiesService businessPropertiesService,
            IConfirmationService contirmationService,
            ITokenHandler tokenHandler,
            ISmsHandler smsHandler,
            IMailHandler mailHandler)
        {
            _businessService = businessService;
            _businessPropertiesService = businessPropertiesService;
            _contirmationService = contirmationService;
            _tokenHandler = tokenHandler;
            _smsHandler = smsHandler;
            _mailHandler = mailHandler;
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
                response.Message = $"{Resource.Resource.ConfirmationCodeNotSend} {Resource.Resource.ErrorContactMessage}";
                response.HasError = true;
                return Ok(response);
            }

            var systemBusiness = await _businessService.GetBusinessByTelephoneNumberAsync(telephoneNumber);

            if (systemBusiness != null)
            {
                response.HasError = true;
                response.Message = $"{Resource.Resource.BusinessFoundEnteredTelephone} {Resource.Resource.ResetPasswordMessage}";
                return Ok(response);
            }

            var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(telephoneNumber);

            if (systemConfirmationInfo != null && systemConfirmationInfo.createDate.DifferenceBetweenDates(DateTime.Now, DateType.Minute) < 1)
            {
                response.Message = Resource.Resource.ConfirmationCodeMessageInOneMinute;
                response.HasError = true;
                return Ok(response);
            }

            string smsMessage = string.Format(Resource.Resource.ConfirmationMessage, confirmationCode);

            //var sendSms = await _smsHandler.SendSmsAsync(smsMessage, telephoneNumber);

            await _contirmationService.SaveConfirmationInfoAsync(telephoneNumber, confirmationCode.ToString());

            response.Message = $"{Resource.Resource.ConfirmationCodeSend} {smsMessage}";
            response.Data = confirmationCode;

            return Ok(response);
        }

        /// <summary>
        /// Send Email Confirmation Code
        /// </summary>
        /// <returns></returns>
        [HttpPost("sendemailconfirmationcode")]
        public async Task<IActionResult> SendEmailConfirmationCode([FromBody] string email)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            int confirmationCode = new Random().Next(1000, 9999);

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
                response.Message = $"{Resource.Resource.ConfirmationCodeNotSend} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            var business = await _businessService.GetBusinessByEmailAsync(email);

            if (business == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessNotFoundEnteredMail;
                return Ok(response);
            }

            var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(email);

            if (systemConfirmationInfo != null && Math.Abs(systemConfirmationInfo.createDate.DifferenceBetweenDates(DateTime.Now, DateType.Minute)) < 1)
            {
                response.Message = Resource.Resource.ConfirmationCodeMessageInOneMinute;
                response.HasError = true;
                return Ok(response);
            }

            string mailMessage = HelperMethods.GetMailTemplate();
            string content = string.Format(Resource.Resource.ResetPasswordMailMessage, confirmationCode);

            var mailRequest = new MailRequest()
            {
                ToEmailList = new List<string> { email },
                Subject = $"CareGarden {Resource.Resource.ResetPassword}",
                Body = mailMessage.Replace("{content}", content)
            };

            BackgroundJob.Enqueue(() => _mailHandler.SendEmailAsync(mailRequest));

            await _contirmationService.SaveConfirmationInfoAsync(email, confirmationCode.ToString());

            response.Message = Resource.Resource.ConfirmationCodeSend;
            response.Data = true;

            return Ok(response);
        }

        /// <summary>
        /// Verify Confirmation Code
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "target": "+905467335939",
        ///        "code": "7391"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("verifyconfirmationcode")]
        public async Task<IActionResult> VerifyConfirmationCode([FromBody] ConfirmationInfo confirmationInfo)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (confirmationInfo.target.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("target", Resource.Resource.NotEmpty));
            }

            if (confirmationInfo.code.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("code", Resource.Resource.NotEmpty));
            }

            if (!confirmationInfo.target.IsValidEmail() && !confirmationInfo.target.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("target", Resource.Resource.ValidMailOrTelephoneMessage));
            }

            if (response.HasError)
            {
                response.Message = $"{Resource.Resource.ConfirmationCodeNotVerified} {Resource.Resource.ErrorContactMessage}";
                return Ok(response);
            }

            var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(confirmationInfo.target);

            if (systemConfirmationInfo == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.ConfirmationCodeNotVerified;
                return Ok(response);
            }

            if (systemConfirmationInfo.code != confirmationInfo.code)
            {
                response.Message = Resource.Resource.ConfirmationCodeNotVerified;
                response.HasError = true;
                return Ok(response);
            }

            if (systemConfirmationInfo.createDate.DifferenceBetweenDates(DateTime.Now, DateType.Minute) > 1)
            {
                response.Message = Resource.Resource.ConfirmationCodeExpired;
                response.HasError = true;
                return Ok(response);
            }

            response.Message = Resource.Resource.ConfirmationCodeVerified;
            systemConfirmationInfo.createDate = systemConfirmationInfo.createDate.Value.AddDays(-1);
            await _contirmationService.SaveConfirmationInfoAsync(systemConfirmationInfo);
            response.Data = true;

            return Ok(response);
        }

        /// <summary>
        /// Save Business
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "name" : "MERT BEAUTY",
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "password": "stms5581",
        ///        "retryPassword": "stms5581",
        ///        "telephone": "+905467335939"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] Business business)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (business.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!business.email.IsNullOrEmpty() && !business.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (!business.telephone.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.ValidTelephoneMessage));
            }

            if (business.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }


            if (business.retryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.NotEmpty));
            }

            if (business.password.IsNotNullOrEmpty() && !business.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (business.retryPassword.IsNotNullOrEmpty() && !business.retryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordMustBeEightCharacters));
            }


            if (!business.password.IsNullOrEmpty() && !business.password.Equals(business.retryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordsMustBeEqual));
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordsMustBeEqual));
            }

            if (business.name.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.NotEmpty));
            }

            if (!business.name.IsNullOrEmpty() && !business.name.IsValidFullName())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.ValidNameMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }

            var isExistsBusiness = await _businessService.GetBusinessExistsByEmailAsync(business.email);

            if (isExistsBusiness)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessFoundEnteredMail;
                return Ok(response);
            }

            isExistsBusiness = await _businessService.GetBusinessExistsByTelephoneNumberAsync(business.telephone);

            if (isExistsBusiness)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessFoundEnteredTelephone;
                return Ok(response);
            }

            business.nameForUrl = await _businessService.GetNameForUrl(business);

            business = await _businessService.SaveBusinessAsync(business);

            response.Message = Resource.Resource.RegistrationSuccess;

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, business.name),
                    new Claim(ClaimTypes.PrimarySid, business.id.ToString()),
                    new Claim(ClaimTypes.Email, business.email),
                    new Claim(ClaimTypes.Role, "Business")
                };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);

            var sessionUserRole = HelperMethods.GetClaimInfo(Request, ClaimTypes.Role);

            if (sessionUserRole.IsNotNullOrEmpty() && sessionUserRole.Equals("Admin"))
            {
                response.Data.id = business.id;
            }

            BackgroundJob.Enqueue(() => _businessPropertiesService.SaveStaticBusinessPropertiesAsync(business.id));
            BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(business.id));

            return Ok(response);
        }

        /// <summary>
        /// Login Business
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
        public async Task<IActionResult> BusinessLogin([FromBody] Business loginBusiness)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (loginBusiness.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!loginBusiness.email.IsNullOrEmpty() && !loginBusiness.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (loginBusiness.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }

            if (loginBusiness.password.IsNotNullOrEmpty() && !loginBusiness.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.LoginFailed;
                return Ok(response);
            }

            var business = await _businessService.GetBusinessByEmailAndPasswordAsync(loginBusiness.email, loginBusiness.password);

            if (business == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessNotFoundEnteredInfo;
                return Ok(response);
            }

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, business.name),
                    new Claim(ClaimTypes.PrimarySid, business.id.ToString()),
                    new Claim(ClaimTypes.Email, business.email),
                    new Claim(ClaimTypes.Role, "Business")
                };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);
            response.Message = Resource.Resource.LoginSuccess;

            return Ok(response);
        }

        /// <summary>
        /// Business Password Reset
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "password": "mert1453",
        ///        "retryPassword": "mert1453",
        ///        "verifiedCode": "9112"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("passwordreset")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequestModel updateBusiness)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (updateBusiness.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (updateBusiness.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }


            if (updateBusiness.retryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.NotEmpty));
            }

            if (updateBusiness.password.IsNotNullOrEmpty() && !updateBusiness.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (updateBusiness.retryPassword.IsNotNullOrEmpty() && !updateBusiness.retryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (updateBusiness.verifiedCode.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("verifiedCode", Resource.Resource.NotEmpty));
            }


            if (!updateBusiness.password.Equals(updateBusiness.retryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordsMustBeEqual));
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordsMustBeEqual));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.LoginFailed;
                return Ok(response);
            }

            var business = await _businessService.GetBusinessByEmailAsync(updateBusiness.email);

            if (business == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.BusinessFoundEnteredMail;
                return Ok(response);
            }

            var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(updateBusiness.email);

            if (systemConfirmationInfo == null || !systemConfirmationInfo.code.IsNull("").Equals(updateBusiness.verifiedCode))
            {
                response.HasError = true;
                response.Message = Resource.Resource.ConfirmationCodeNotVerified;
                return Ok(response);
            }

            business.password = updateBusiness.password;
            await _businessService.UpdateBusinessAsync(business, true);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, business.name),
                new Claim(ClaimTypes.PrimarySid, business.id.ToString()),
                new Claim(ClaimTypes.Email, business.email),
                new Claim(ClaimTypes.Role, "Business")
            };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);

            response.Message = Resource.Resource.ResetPasswordSuccess;
            return Ok(response);
        }
    }
}
