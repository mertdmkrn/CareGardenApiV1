using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using OneSignalApi.Model;
using System;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    public class BusinessLoginController : ControllerBase
    {
        private IBusinessService _businessService;
        private IConfirmationService _contirmationService;
        private ITokenHandler _tokenHandler;
        private ISmsHandler _smsHandler;
        private readonly IMailHandler _mailHandler;

        public BusinessLoginController(IMailHandler mailHandler)
        {
            _businessService = new BusinessService();
            _contirmationService = new ConfirmationService();
            _tokenHandler = new TokenHandler();
            _smsHandler = new SmsHandler();
            _mailHandler = mailHandler;
        }

        /// <summary>
        /// Send Telephone Confirmation Code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("business/sendtelephoneconfirmationcode")]
        public async Task<IActionResult> SendTelephoneConfirmationCode([FromBody] string telephoneNumber)
        {
            ResponseModel<int> response = new ResponseModel<int>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));
            int confirmationCode = new Random().Next(1000, 10000);

            try
            {
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

                var systemBusiness = await _businessService.GetBusinessByTelephoneNumberAsync(telephoneNumber);

                if (systemBusiness != null)
                {
                    response.HasError = true;
                    response.Message += Resource.Resource.GirdiginizTelefonNumarasinaAitSirketKayitli + " " + Resource.Resource.SifreYenilemeMesaji;
                    return Ok(response);
                }

                var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(telephoneNumber);
                 
                if(systemConfirmationInfo != null && systemConfirmationInfo.createDate.DifferenceBetweenDates(DateTime.Now, Enums.DateType.Minute) < 1)
                {
                    response.Message = Resource.Resource.BirDakikaIcindeOnayKoduMesaji;
                    response.HasError = true;
                    return Ok(response);
                }

                string smsMessage = string.Format(Resource.Resource.OnayMesaji, confirmationCode);

                //var sendSms = await _smsHandler.SendSmsAsync(smsMessage, telephoneNumber);

                await _contirmationService.SaveConfirmationInfoAsync(telephoneNumber, confirmationCode.ToString());

                response.Message = Resource.Resource.OnayKoduGonderildi;
                response.Data = confirmationCode;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = Resource.Resource.OnayKoduGonderilemedi + " Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Send Email Confirmation Code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("business/sendemailconfirmationcode")]
        public async Task<IActionResult> SendEmailConfirmationCode([FromBody] string email)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));
            int confirmationCode = new Random().Next(1000, 10000);

            try
            {
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

                var business = await _businessService.GetBusinessByEmailAsync(email);

                if (business == null)
                {
                    response.HasError = true;
                    response.Message += Resource.Resource.GirdiginizMaileAitSirketBulunamadi;
                    return Ok(response);
                }

                var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(email);

                if (systemConfirmationInfo != null && systemConfirmationInfo.createDate.DifferenceBetweenDates(DateTime.Now, Enums.DateType.Minute) < 1)
                {
                    response.Message = Resource.Resource.BirDakikaIcindeOnayKoduMesaji;
                    response.HasError = true;
                    return Ok(response);
                }

                string mailMessage = "";

                using (StreamReader reader = new StreamReader(Path.Combine(AppContext.BaseDirectory.Replace("bin\\Debug\\net7.0\\", "") + "Template/MailTemplate.html")))
                {
                    mailMessage = reader.ReadToEnd();
                }

                string content = string.Format(Resource.Resource.SifreYenilemeMailMesaji, confirmationCode);

                await _mailHandler.SendEmailAsync(
                        new MailRequest()
                        {
                            ToEmail = email,
                            Subject = "CareGarden " + Resource.Resource.SifreYenileme,
                            Body = mailMessage.Replace("{content}", content)
                        }
                );

                await _contirmationService.SaveConfirmationInfoAsync(email, confirmationCode.ToString());

                response.Message = Resource.Resource.OnayKoduGonderildi;
                response.Data = true;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = Resource.Resource.OnayKoduGonderilemedi + " Exception => " + ex.Message;
                return Ok(response);
            }
       
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
        [HttpPost]
        [Route("business/verifyconfirmationcode")]
        public async Task<IActionResult> VerifyConfirmationCode([FromBody] ConfirmationInfo confirmationInfo)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));
            int confirmationCode = new Random().Next(1000, 10000);

            try
            {
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
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = Resource.Resource.OnayKoduGonderilemedi + " Exception => " + ex.Message;
                return Ok(response);
            }

        }

        /// <summary>
        /// Create New Business
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "name" : "MERT GÜZELLİK SALONU",
        ///        "nameEn" : "MERT BEAUTY",
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "password": "stms5581",
        ///        "retryPassword": "stms5581",
        ///        "telephone": "+905467335939"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("business/save")]
        public async Task<IActionResult> Save([FromBody] Business business)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (business.email.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (!business.email.IsNullOrEmpty() && !business.email.IsValidEmail())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.GecerliMailMesaji));
                }

                if (!business.telephone.IsValidTelephoneNumber())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("telephoneNumber", Resource.Resource.GecerliTelefonMesaji));
                }

                if (business.password.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.BuAlaniBosBirakmayiniz));
                }


                if (business.retryPassword.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (business.password.IsNotNullOrEmpty() && business.password.Length != 8)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.Sifre8KarakterOlmali));
                }
                
                if (business.retryPassword.IsNotNullOrEmpty() && business.retryPassword.Length != 8)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.Sifre8KarakterOlmali));
                }


                if(!business.password.IsNullOrEmpty() && !business.password.Equals(business.retryPassword))
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.SifrelerEsitOlmali));
                    response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.SifrelerEsitOlmali));
                }

                if (business.name.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if(!business.name.IsNullOrEmpty() && !business.name.IsValidFullName())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.GecerliBirIsimGiriniz));
                }

                if (business.nameEn.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (!business.nameEn.IsNullOrEmpty() && !business.nameEn.IsValidFullName())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("nameEn", Resource.Resource.GecerliBirIsimGiriniz));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.KayitYapilamadi;
                    return Ok(response);
                }

                var systemBusiness = await _businessService.GetBusinessByEmailAsync(business.email);

                if (systemBusiness != null)
                {
                    response.HasError = true;
                    response.Message += Resource.Resource.GirdiginizMaileAitSirketBulunamadi;
                    return Ok(response);
                }

                business = await _businessService.SaveBusinessAsync(business);

                response.Message = Resource.Resource.KayitBasarili;

                var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, business.name),
                    new Claim(ClaimTypes.PrimarySid, business.id.ToString()),
                    new Claim(ClaimTypes.Email, business.email),
                    new Claim(ClaimTypes.Role, "Business")
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
        /// Business Login Control
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
        [Route("business/login")]
        public async Task<IActionResult> BusinessLogin([FromBody] Business loginBusiness)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            if (loginBusiness.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!loginBusiness.email.IsNullOrEmpty() && !loginBusiness.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.GecerliMailMesaji));
            }

            if (loginBusiness.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (loginBusiness.password.IsNotNullOrEmpty() && loginBusiness.password.Length != 8)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.GirisYapilamadi;
                return Ok(response);
            }

            var business = await _businessService.GetBusinessByEmailAndPasswordAsync(loginBusiness.email, loginBusiness.password);

            if (business == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.GirilenBilgilereAitSirketBulunamadi;
                return Ok(response);
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, business.name),
                new Claim(ClaimTypes.PrimarySid, business.id.ToString()),
                new Claim(ClaimTypes.Email, business.email),
                new Claim(ClaimTypes.Role, "Business")
            };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);

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
        [HttpPost]
        [Route("business/passwordreset")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetModel updateBusiness)
        {
            ResponseModel<Token> response = new ResponseModel<Token>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            if (updateBusiness.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateBusiness.password.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.BuAlaniBosBirakmayiniz));
            }


            if (updateBusiness.retryPassword.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateBusiness.password.IsNotNullOrEmpty() && updateBusiness.password.Length != 8)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (updateBusiness.retryPassword.IsNotNullOrEmpty() && updateBusiness.retryPassword.Length != 8)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.Sifre8KarakterOlmali));
            }

            if (updateBusiness.verifiedCode.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("verifiedCode", Resource.Resource.BuAlaniBosBirakmayiniz));
            }


            if (!updateBusiness.password.Equals(updateBusiness.retryPassword))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.SifrelerEsitOlmali));
                response.ValidationErrors.Add(new ValidationError("retryPassword", Resource.Resource.SifrelerEsitOlmali));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.GirisYapilamadi;
                return Ok(response);
            }

            var business = await _businessService.GetBusinessByEmailAsync(updateBusiness.email);

            if (business == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.GirilenBilgilereAitSirketBulunamadi;
                return Ok(response);
            }

            var systemConfirmationInfo = await _contirmationService.GetConfirmationInfo(updateBusiness.email);

            if (systemConfirmationInfo == null || !systemConfirmationInfo.code.IsNull("").Equals(updateBusiness.verifiedCode))
            {
                response.HasError = true;
                response.Message = Resource.Resource.OnayKoduDogrulanamadi + " " + Resource.Resource.SifreYenilemedi;
                return Ok(response);
            }

            business.password = updateBusiness.password;
            await _businessService.UpdateBusinessAsync(business);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, business.name),
                new Claim(ClaimTypes.PrimarySid, business.id.ToString()),
                new Claim(ClaimTypes.Email, business.email),
                new Claim(ClaimTypes.Role, "Business")
            };

            response.Data = _tokenHandler.CreateAccessToken(DateTime.Now.AddDays(60), claims);

            response.Message = Resource.Resource.SifreYenilemeBasarili;

            return Ok(response);
        }
    }
}
