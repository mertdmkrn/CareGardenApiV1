using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Service.Abstract;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Route("user")]
    public class UserLoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfirmationService _confirmationService;
        private readonly ITokenHandler _tokenHandler;
        private readonly ISmsHandler _smsHandler;
        private readonly IMailHandler _mailHandler;
        private readonly ILoggerHandler _loggerHandler;

        public UserLoginController(
            IUserService userService,
            IConfirmationService confirmationService,
            ITokenHandler tokenHandler,
            ISmsHandler smsHandler,
            IMailHandler mailHandler,
            ILoggerHandler loggerHandler)
        {
            _userService = userService;
            _confirmationService = confirmationService;
            _tokenHandler = tokenHandler;
            _smsHandler = smsHandler;
            _mailHandler = mailHandler;
            _loggerHandler = loggerHandler;
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

            var isExistUser = await _userService.GetUserExistsByTelephoneNumberAsync(telephoneNumber);

            if (!isExistUser)
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
                response.Message = Resource.Resource.ConfirmationCodeNotSend;
                return Ok(response);
            }

            var isExistUser = await _userService.GetUserExistsByEmailAsync(email);

            if (!isExistUser)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFoundEnteredMail;
                return Ok(response);
            }

            var systemConfirmationInfo = await _confirmationService.GetConfirmationInfo(email);

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

            await _confirmationService.SaveConfirmationInfoAsync(email, confirmationCode.ToString());

            response.Message = Resource.Resource.ConfirmationCodeSend;
            response.Data = true;

            return Ok(response);
        }

        /// <summary>
        /// Send Code For Appointment Code
        /// </summary>
        /// <returns></returns>
        [HttpPost("sendcodeforappointment")]
        public async Task<IActionResult> SendCodeForAppointment([FromBody] string telephoneNumber)
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

            var systemConfirmationInfo = await _confirmationService.GetConfirmationInfo(telephoneNumber);

            if (systemConfirmationInfo != null && systemConfirmationInfo.createDate.DifferenceBetweenDates(DateTime.Now, DateType.Minute) < 1)
            {
                response.Message = Resource.Resource.ConfirmationCodeMessageInOneMinute;
                response.HasError = true;
                return Ok(response);
            }

            string smsMessage = string.Format(Resource.Resource.ConfirmationCodeForAppointment, confirmationCode);

            //var sendSms = await _smsHandler.SendSmsAsync(smsMessage, telephoneNumber);

            await _confirmationService.SaveConfirmationInfoAsync(telephoneNumber, confirmationCode.ToString());

            response.Message = $"{Resource.Resource.ConfirmationCodeSend} {smsMessage}";
            response.Data = confirmationCode;

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
            int confirmationCode = new Random().Next(1000, 9999);

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
                response.Message = Resource.Resource.ConfirmationCodeNotVerified;
                return Ok(response);
            }

            var systemConfirmationInfo = await _confirmationService.GetConfirmationInfo(confirmationInfo.target);

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

            systemConfirmationInfo.createDate = systemConfirmationInfo.createDate.Value.AddDays(-1);
            await _confirmationService.SaveConfirmationInfoAsync(systemConfirmationInfo);

            response.Message = Resource.Resource.ConfirmationCodeVerified;
            response.Data = true;

            return Ok(response);
        }

        /// <summary>
        /// Save User
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
        public async Task<IActionResult> Save([FromBody] User user)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (user.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!user.email.IsNullOrEmpty() && !user.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (!user.telephone.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.ValidTelephoneMessage));
            }

            if (user.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }

            if (user.retryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.NotEmpty));
            }

            if (user.password.IsNotNullOrEmpty() && !user.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (user.retryPassword.IsNotNullOrEmpty() && !user.retryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordMustBeEightCharacters));
            }


            if (!user.password.IsNullOrEmpty() && !user.password.Equals(user.retryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordsMustBeEqual));
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordsMustBeEqual));
            }

            if (user.fullName.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.NotEmpty));
            }

            if (!user.fullName.IsNullOrEmpty() && !user.fullName.IsValidFullName())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.ValidNameMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }

            var systemUser = await _userService.GetUserByEmailAsync(user.email);

            if (systemUser != null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.RegisteredUserEnteredMail;
                return Ok(response);
            }

            user = await _userService.SaveUserAsync(user);

            response.Message = Resource.Resource.RegistrationSuccess;

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, user.fullName),
                    new Claim(ClaimTypes.PrimarySid, user.id.ToString()),
                    new Claim(ClaimTypes.Email, user.email),
                    new Claim(ClaimTypes.Locality, user.city.IsNull("")),
                    new Claim(ClaimTypes.Role, user.role)
                };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);

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
        public async Task<IActionResult> UserLogin([FromBody] User loginUser)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (loginUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!loginUser.email.IsNullOrEmpty() && !loginUser.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (loginUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }

            if (loginUser.password.IsNotNullOrEmpty() && !loginUser.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.LoginFailed;
                return Ok(response);
            }

            var user = await _userService.GetUserByEmailAndPasswordAsync(loginUser.email, loginUser.password);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.LoginFailedUserMessage;
                return Ok(response);
            }

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, user.fullName),
                    new Claim(ClaimTypes.PrimarySid, user.id.ToString()),
                    new Claim(ClaimTypes.Email, user.email),
                    new Claim(ClaimTypes.Locality, user.city.IsNull("")),
                    new Claim(ClaimTypes.Role, user.role)
                };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);
            response.Message = Resource.Resource.LoginSuccess;

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
        ///        "retryPassword": "mert1453",
        ///        "verifiedCode": "9112"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("passwordreset")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequestModel updateUser)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (updateUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (updateUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }

            if (updateUser.retryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.NotEmpty));
            }

            if (updateUser.password.IsNotNullOrEmpty() && !updateUser.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (updateUser.retryPassword.IsNotNullOrEmpty() && !updateUser.retryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (!updateUser.password.Equals(updateUser.retryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordsMustBeEqual));
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.PasswordsMustBeEqual));
            }

            if (updateUser.verifiedCode.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("verifiedCode", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.ResetPasswordFailed;
                return Ok(response);
            }

            var user = await _userService.GetUserByEmailAsync(updateUser.email);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.UserNotFoundEnteredMail;
                return Ok(response);
            }

            var systemConfirmationInfo = await _confirmationService.GetConfirmationInfo(updateUser.email);

            if (systemConfirmationInfo == null || !systemConfirmationInfo.code.IsNull("").Equals(updateUser.verifiedCode))
            {
                response.HasError = true;
                response.Message = $"{Resource.Resource.ConfirmationCodeNotVerified} {Resource.Resource.ResetPassword}";
                return Ok(response);
            }

            user.password = updateUser.password;
            await _userService.UpdateUserAsync(user, true);

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, user.fullName),
                    new Claim(ClaimTypes.PrimarySid, user.id.ToString()),
                    new Claim(ClaimTypes.Email, user.email),
                    new Claim(ClaimTypes.Locality, user.city.IsNull("")),
                    new Claim(ClaimTypes.Role, user.role)
                };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);
            response.Message = Resource.Resource.ResetPasswordSuccess;

            return Ok(response);
        }

        /// <summary>
        /// Admin Login Control
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
        [HttpPost("adminlogin")]
        public async Task<IActionResult> AdminLogin([FromBody] User loginUser)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (loginUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!loginUser.email.IsNullOrEmpty() && !loginUser.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.ValidEmailMessage));
            }

            if (loginUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.NotEmpty));
            }

            if (loginUser.password.IsNotNullOrEmpty() && !loginUser.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.LoginFailed;
                return Ok(response);
            }

            var user = await _userService.GetAdminByEmailAndPasswordAsync(loginUser.email, loginUser.password);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.LoginFailedUserMessage;
                return Ok(response);
            }

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, user.fullName),
                    new Claim(ClaimTypes.PrimarySid, user.id.ToString()),
                    new Claim(ClaimTypes.Email, user.email),
                    new Claim(ClaimTypes.Locality, user.city.IsNull("")),
                    new Claim(ClaimTypes.Role, user.role)
                };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);
            response.Message = Resource.Resource.LoginSuccess;

            return Ok(response);
        }
    }
}
