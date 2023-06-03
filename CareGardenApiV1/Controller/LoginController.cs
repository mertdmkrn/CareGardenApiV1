using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X500;
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

            response.Message = "Doğrulama Kodu, telefonunuza gönderildi.";
            //var sendSms = await _smsHandler.SendSmsAsync(smsMessage, telephoneNumber);

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
            ResponseModel<User> response = new ResponseModel<User>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (user.email.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("email", "Email boş bırakılmamalı."));
                    response.Message += "Email boş bırakılmamalı.";
                }

                if (!user.email.IsValidEmail())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("email", "Geçerli bir email adresi giriniz."));
                    response.Message += "Geçerli bir email adresi giriniz.";
                }

                if (user.password.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("password", "Şifre boş bırakılmamalı."));
                    response.Message += "Şifre boş bırakılmamalı.";
                }

                if (user.password.IsNotNullOrEmpty() && user.password.Length != 8)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("password", "Şifre 8 haneli olmalıdır."));
                    response.Message += "Şifre 8 haneli olmalıdır.";
                }

                if (user.fullName.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("firstname", "İsim boş bırakılmamalı."));
                    response.Message += "İsim boş bırakılmamalı.";
                }

                if (response.HasError)
                    return BadRequest(response);

                response.Data = await _userService.SaveUserAsync(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message += " Exception => " + ex.Message;
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
            ResponseModel<TokenInfo<User>> response = new ResponseModel<TokenInfo<User>>();

            if (loginUser.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", "Email boş bırakılmamalı."));
                response.Message += "Email boş bırakılmamalı.";
            }

            if (loginUser.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", "Şifre boş bırakılmamalı."));
                response.Message += "Şifre boş bırakılmamalı.";
            }

            if (response.HasError)
                return BadRequest(response);

            var user = await _userService.GetUserByEmailAndPasswordAsync(loginUser.email, loginUser.password);

            if (user == null)
            {
                response.HasError = true;
                response.Message = "Girdiğiniz bilgilere ait kullanıcı bulunamadı.";
                return NotFound(response);
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, user.fullName),
                new Claim(ClaimTypes.Email, user.email)
            };

            response.Data = new TokenInfo<User>();
            response.Data.token = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);
            response.Data.userInfo = user;

            return Ok(response);
        }
    }
}
