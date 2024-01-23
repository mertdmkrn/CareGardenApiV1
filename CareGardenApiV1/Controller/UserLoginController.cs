using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
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
        private readonly IConfirmationService _contirmationService;
        private readonly ITokenHandler _tokenHandler;
        private readonly ISmsHandler _smsHandler;
        private readonly IMailHandler _mailHandler;
        private readonly ILoggerHandler _loggerHandler;

        public UserLoginController(
            IUserService userService,
            IConfirmationService contirmationService,
            ITokenHandler tokenHandler,
            ISmsHandler smsHandler,
            IMailHandler mailHandler,
            ILoggerHandler loggerHandler)
        {
            _userService = userService;
            _contirmationService = contirmationService;
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
            int confirmationCode = new Random().Next(1000, 10000);

            if (telephoneNumber.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!telephoneNumber.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.GecerliTelefonMesaji));
            }


            if (response.HasError)
            {
                response.Message = Resource.Resource.OnayKoduGonderilemedi;
                response.HasError = true;
                return Ok(response);
            }

            var systemUser = await _userService.GetUserByTelephoneNumberAsync(telephoneNumber);

            if (systemUser != null)
            {
                response.HasError = true;
                response.Message += $"{Resource.Resource.GirdiginizTelefonNumarasinaAitKullaniciKayitli} {Resource.Resource.SifreYenilemeMesaji}";
                return Ok(response);
            }

            var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(telephoneNumber);

            if (systemConfirmationInfo != null && systemConfirmationInfo.createDate.DifferenceBetweenDates(DateTime.Now, Enums.DateType.Minute) < 1)
            {
                response.Message = Resource.Resource.BirDakikaIcindeOnayKoduMesaji;
                response.HasError = true;
                return Ok(response);
            }

            string smsMessage = string.Format(Resource.Resource.OnayMesaji, confirmationCode);

            //var sendSms = await _smsHandler.SendSmsAsync(smsMessage, telephoneNumber);

            await _contirmationService.SaveConfirmationInfoAsync(telephoneNumber, confirmationCode.ToString());

            response.Message = $"{Resource.Resource.OnayKoduGonderildi} {smsMessage}";
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
            int confirmationCode = new Random().Next(1000, 10000);

            if (email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.GecerliMailMesaji));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.OnayKoduGonderilemedi;
                return Ok(response);
            }

            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                response.HasError = true;
                response.Message += Resource.Resource.GirdiginizMaileAitKullaniciBulunamadi;
                return Ok(response);
            }

            var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(email);

            if (systemConfirmationInfo != null && Math.Abs(systemConfirmationInfo.createDate.DifferenceBetweenDates(DateTime.Now, Enums.DateType.Minute)) < 1)
            {
                response.Message = Resource.Resource.BirDakikaIcindeOnayKoduMesaji;
                response.HasError = true;
                return Ok(response);
            }

            string mailMessage = HelperMethods.GetMailTemplate();

            string content = string.Format(Resource.Resource.SifreYenilemeMailMesaji, confirmationCode);

            var mailRequest = new MailRequest()
            {
                ToEmailList = new List<string> { email },
                Subject = $"CareGarden {Resource.Resource.SifreYenileme}",
                Body = mailMessage.Replace("{content}", content)
            };

            BackgroundJob.Enqueue(() => _mailHandler.SendEmailAsync(mailRequest));

            await _contirmationService.SaveConfirmationInfoAsync(email, confirmationCode.ToString());

            response.Message = Resource.Resource.OnayKoduGonderildi;
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
            int confirmationCode = new Random().Next(1000, 10000);

            if (confirmationInfo.target.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("target", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (confirmationInfo.code.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("code", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!confirmationInfo.target.IsValidEmail() && !confirmationInfo.target.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("target", Resource.Resource.GecerliMailTelefonMesaji));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.OnayKoduDogrulanamadi;
                return Ok(response);
            }

            var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(confirmationInfo.target);

            if (systemConfirmationInfo == null)
            {
                response.HasError = true;
                response.Message += Resource.Resource.OnayKoduDogrulanamadi;
                return Ok(response);
            }

            if (systemConfirmationInfo.code != confirmationInfo.code)
            {
                response.Message = Resource.Resource.OnayKoduDogrulanamadi;
                response.HasError = true;
                return Ok(response);
            }

            if (systemConfirmationInfo.createDate.DifferenceBetweenDates(DateTime.Now, Enums.DateType.Minute) > 1)
            {
                response.Message = Resource.Resource.OnayKoduZamanAsiminaUgradi;
                response.HasError = true;
                return Ok(response);
            }

            response.Message = Resource.Resource.OnayKoduDogrulandi;
            systemConfirmationInfo.createDate = systemConfirmationInfo.createDate.Value.AddDays(-1);
            await _contirmationService.SaveConfirmationInfoAsync(systemConfirmationInfo);
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
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!user.email.IsNullOrEmpty() && !user.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.GecerliMailMesaji));
            }

            if (!user.telephone.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.GecerliTelefonMesaji));
            }

            if (user.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.BuAlaniBosBirakmayiniz));
            }


            if (user.retryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (user.password.IsNotNullOrEmpty() && !user.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (user.retryPassword.IsNotNullOrEmpty() && !user.retryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.Sifre8KarakterOlmali));
            }


            if (!user.password.IsNullOrEmpty() && !user.password.Equals(user.retryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.SifrelerEsitOlmali));
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.SifrelerEsitOlmali));
            }

            if (user.fullName.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!user.fullName.IsNullOrEmpty() && !user.fullName.IsValidFullName())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.GecerliBirIsimGiriniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            var systemUser = await _userService.GetUserByEmailAsync(user.email);

            if (systemUser != null)
            {
                response.HasError = true;
                response.Message += Resource.Resource.GirdiginizMaileAitKullaniciKayitli;
                return Ok(response);
            }

            user = await _userService.SaveUserAsync(user);

            response.Message = Resource.Resource.KayitBasarili;

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
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            if (loginUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!loginUser.email.IsNullOrEmpty() && !loginUser.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.GecerliMailMesaji));
            }

            if (loginUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (loginUser.password.IsNotNullOrEmpty() && !loginUser.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.GirisYapilamadi;
                return Ok(response);
            }

            var user = await _userService.GetUserByEmailAndPasswordAsync(loginUser.email, loginUser.password);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.GirilenBilgilereAitKullaniciBulunamadi;
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
            response.Message = Resource.Resource.GirisBasarili;
            _loggerHandler.LogMessage($"{user.fullName} {Resource.Resource.GirisBasarili}");

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
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetModel updateUser)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();

            if (updateUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateUser.retryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateUser.password.IsNotNullOrEmpty() && !updateUser.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (updateUser.retryPassword.IsNotNullOrEmpty() && !updateUser.retryPassword.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (!updateUser.password.Equals(updateUser.retryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.SifrelerEsitOlmali));
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.SifrelerEsitOlmali));
            }

            if (updateUser.verifiedCode.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("verifiedCode", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.SifreYenilemedi;
                return Ok(response);
            }

            var user = await _userService.GetUserByEmailAsync(updateUser.email);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.GirilenBilgilereAitKullaniciBulunamadi;
                return Ok(response);
            }

            var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(updateUser.email);

            if (systemConfirmationInfo == null || !systemConfirmationInfo.code.IsNull("").Equals(updateUser.verifiedCode))
            {
                response.HasError = true;
                response.Message = $"{Resource.Resource.OnayKoduDogrulanamadi} {Resource.Resource.SifreYenilemedi}";
                return Ok(response);
            }

            user.password = updateUser.password;
            await _userService.UpdateUserAsync(user);

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, user.fullName),
                    new Claim(ClaimTypes.PrimarySid, user.id.ToString()),
                    new Claim(ClaimTypes.Email, user.email),
                    new Claim(ClaimTypes.Locality, user.city.IsNull("")),
                    new Claim(ClaimTypes.Role, user.role)
                };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);
            response.Message = Resource.Resource.SifreYenilemeBasarili;

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
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!loginUser.email.IsNullOrEmpty() && !loginUser.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.GecerliMailMesaji));
            }

            if (loginUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (loginUser.password.IsNotNullOrEmpty() && !loginUser.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.GirisYapilamadi;
                return Ok(response);
            }

            var user = await _userService.GetAdminByEmailAndPasswordAsync(loginUser.email, loginUser.password);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.GirilenBilgilereAitKullaniciBulunamadi;
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
            response.Message = Resource.Resource.GirisBasarili;
            _loggerHandler.LogMessage(user.fullName + " " + Resource.Resource.GirisBasarili);

            return Ok(response);
        }
    }
}
