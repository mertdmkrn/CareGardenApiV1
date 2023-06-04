using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IUserService _userService;
        private ITokenHandler _tokenHandler;
        private ISmsHandler _smsHandler;

        public LoginController()
        {
            _userService = new UserService();
            _tokenHandler = new TokenHandler();
            _smsHandler = new SmsHandler();
        }

        /// <summary>
        /// Get Confirmation Code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user/getconfirmationcode")]
        public async Task<IActionResult> GetConfirmationCode([FromBody] string telephoneNumber)
        {
            ResponseModel<int> response = new ResponseModel<int>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));
            int confirmationCode = new Random().Next(1000, 10000);

            string smsMessage = string.Format(Resource.Resource.OnayMesaji, confirmationCode);

            if (telephoneNumber.IsNullOrEmpty() || !telephoneNumber.IsValidTelephoneNumber())
            {
                response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.GecerliTelefonMesaji));
                response.HasError = true;
                response.Message = Resource.Resource.GecerliTelefonMesaji;
            }

            //var sendSms = await _smsHandler.SendSmsAsync(smsMessage, telephoneNumber);
            response.Message = Resource.Resource.DogrulamaKoduGonderildi;
            response.Data = confirmationCode;

            return Ok(response);
        }


        /// <summary>
        /// Create New User
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "fullName" : "Mert DEMİRKIRAN",
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "password": "stms5581",
        ///        "telephone": "05467335939"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("user/save")]
        public async Task<IActionResult> Save([FromBody] User user)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (user.email.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (!user.email.IsValidEmail())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.GecerliMailMesaji));
                }

                if (user.password.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (user.password.IsNotNullOrEmpty() && user.password.Length != 8)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.Sifre8KarakterOlmali));
                }

                if (user.fullName.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("firstname", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.KayitYapilamadi;
                    return BadRequest(response);
                }

                var systemUser = await _userService.GetUserByEmailAsync(user.email);

                if (systemUser != null)
                {
                    response.HasError = true;
                    response.Message += Resource.Resource.GirdiginizMaileAitKullaniciKayitli;
                    return BadRequest(response);
                }

                user = await _userService.SaveUserAsync(user);

                var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, user.fullName),
                    new Claim(ClaimTypes.PrimarySid, user.id.ToString()),
                    new Claim(ClaimTypes.Email, user.email)
                };

                response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitYapilamadi + " Exception => " + ex.Message;
                return Ok(response);
            }
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
        [HttpPost]
        [Route("user/login")]
        public async Task<IActionResult> UserLogin([FromBody] User loginUser)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            if (loginUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (loginUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.GirisYapilamadi;
                return BadRequest(response);
            }

            var user = await _userService.GetUserByEmailAndPasswordAsync(loginUser.email, loginUser.password);

            if (user == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.GirilenBilgilereAitKullaniciBulunamadi;
                return NotFound(response);
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, user.fullName),
                new Claim(ClaimTypes.PrimarySid, user.id.ToString()),
                new Claim(ClaimTypes.Email, user.email)
            };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);

            return Ok(response);
        }
    }
}
