using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.X500;
using System;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IUserService _userService;
        private readonly IFaqService _faqService;
        private readonly IFileHandler _fileHandler;
        private readonly IMemoryCache _memoryCache;
        private readonly IMailHandler _mailHandler;
        private readonly IElasticHandler _elasticHandler;
        private readonly ILoggerHandler _loggerHandler;

        public AdminController(
            IBusinessService businessService,
            IUserService userService,
            IFaqService faqService,
            IFileHandler fileHandler,
            IMemoryCache memoryCache,
            IMailHandler mailHandler,
            ILoggerHandler loggerHandler,
            IElasticHandler elasticHandler)
        {
            _businessService = businessService;
            _userService = userService;
            _faqService = faqService;
            _fileHandler = fileHandler;
            _memoryCache = memoryCache;
            _mailHandler = mailHandler;
            _loggerHandler = loggerHandler;
            _elasticHandler = elasticHandler;
        }

        /// <summary>
        /// It is used for activation and verification of Business.
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///    { 
        ///        "id" : "9D2B0228-4D0D-4C23-8B49-01A698857709",
        ///        "isActive" : true,
        ///        "verified" : true
        ///    }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("verifybusiness")]
        public async Task<IActionResult> VerifyBusiness(Business updateBusiness)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            if (updateBusiness.id == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.OnayKoduGonderilemedi;
                response.HasError = true;
                return Ok(response);
            }

            Business business = await _businessService.GetBusinessByIdAsync(updateBusiness.id);

            if (business == null)
            {
                response.Message = Resource.Resource.SirketBulunamadi;
                response.HasError = true;
                return Ok(response);
            }

            business.isActive = updateBusiness.isActive;
            business.verified = updateBusiness.verified;

            await _businessService.UpdateBusinessAsync(business);
            response.Message = Resource.Resource.KayitBasarili;
            response.Data = true;

            return Ok(response);
        }

        /// <summary>
        /// Save Admin.
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "fullName" : "Mert DEMİRKIRAN",
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "password": "12345678",
        ///        "telephone": "+905467335939"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
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

                if (user.password.IsNotNullOrEmpty() && !user.password.Length.Between(8, 20))
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.Sifre8KarakterOlmali));
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

                var addingUserName = HelperMethods.GetClaimInfo(Request, ClaimTypes.Name);
                string mailMessage = HelperMethods.GetMailTemplate();

                string content =  user.fullName + " " + "<a href='https://mertdmkrn.github.io/cg-admin/'>CG Admin</a>'e hoşgeldin. Giriş Bilgilerin; <br><br>" +
                                  "Email : " + user.email + "<br>Şifre : " + user.password + "<br><br> " +
                                  "<p style='text-align:right; font-size:11.5px; padding-right:10px'>Sisteme Ekleyen : " + addingUserName + "</p>";

                user.role = "Admin";

                user = await _userService.SaveUserAsync(user);
                response.Message = Resource.Resource.KayitBasarili;


                await _mailHandler.SendEmailAsync(
                    new MailRequest()
                    {
                        ToEmailList = new List<string> { user.email },
                        Subject = "CG Admin' e Hoşgeldin",
                        Body = mailMessage.Replace("{content}", content)
                    }
                );

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
        /// It is used to upload a new file and get a url.
        /// </summary>
        /// <returns></returns>
        [HttpPost("uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (file == null)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("file", Resource.Resource.BuAlaniBosBirakmayiniz));
                    response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
                    return Ok(response);
                }

                response.Data = await _fileHandler.UploadFreeImageServer(file);
                response.Message = Resource.Resource.ResimYuklemeBasarili;

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
        /// Save Faq
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "question" : "Biz Kimiz?",
        ///        "questionEn": "Who Us?",
        ///        "answer": "En iyisiyiz.",
        ///        "answerEn": "We are the best.",
        ///        "category": "Kayıt",
        ///        "sortOrder": 1
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("savefaq")]
        public async Task<IActionResult> SaveFaq([FromBody] Faq faq)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (faq.question.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("question", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (faq.questionEn.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("questionEn", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (faq.answer.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("answer", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (faq.answerEn.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("answerEn", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (faq.category.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("category", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                var index = Constants.FaqCategories.IndexOf(faq.category);

                if (response.HasError && index < 0)
                {
                    response.Message = Resource.Resource.KayitYapilamadi;
                    return Ok(response);
                }

                faq.categoryEn = Constants.FaqEnCategories[index];

                response.Data = await _faqService.SaveFaqAsync(faq);
                response.Message = Resource.Resource.KayitBasarili;
                _memoryCache.Remove("faqs");

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
        /// Update Faq
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id": "00000000-0000-0000-0000-000000000000",
        ///        "question": "Biz Kimiz?",
        ///        "questionEn": "Who Us?",
        ///        "answer": "En iyisiyiz.",
        ///        "answerEn": "We are the best.",
        ///        "category": "Kayıt",
        ///        "sortOrder": 2
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("updatefaq")]
        public async Task<IActionResult> UpdateFaq([FromBody] Faq updateFaq)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (updateFaq.question.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("question", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (updateFaq.questionEn.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("questionEn", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (updateFaq.answer.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("answer", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (updateFaq.answerEn.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("answerEn", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (updateFaq.category.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("category", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                var index = Constants.FaqCategories.IndexOf(updateFaq.category);

                if (response.HasError && index < 0)
                {
                    response.Message = Resource.Resource.GuncellemeYapilamadi;
                    return Ok(response);
                }

                var faq = await _faqService.GetFaqByIdAsync(updateFaq.id);

                faq.question = updateFaq.question;
                faq.questionEn = updateFaq.questionEn;
                faq.answer = updateFaq.answer;
                faq.answerEn = updateFaq.answerEn;
                faq.category = updateFaq.category;
                faq.categoryEn = Constants.FaqEnCategories[index];
                faq.sortOrder = updateFaq.sortOrder;
                _memoryCache.Remove("faqs");

                response.Data = await _faqService.UpdateFaqAsync(faq);
                response.Message = Resource.Resource.KayitBasarili;

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
        /// Delete Faq
        /// </summary>
        /// <returns></returns>
        [HttpPost("deletefaq")]
        public async Task<IActionResult> DeleteFaq([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (id.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

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

                response.Data = await _faqService.DeleteFaqAsync(id.ToGuid());
                response.Message = Resource.Resource.KayitSilindi;
                _memoryCache.Remove("faqs");

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
        /// Get Faq By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getfaq")]
        public async Task<IActionResult> GetFaq([FromBody] string id)
        {
            ResponseModel<Faq> response = new ResponseModel<Faq>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (id.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (!id.IsGuid())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.KayitBulunamadi;
                    return Ok(response);
                }

                response.Data = await _faqService.GetFaqByIdAsync(id.ToGuid());

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
        /// Get Faq Categories
        /// </summary>
        /// <returns></returns>
        [HttpPost("getfaqcategories")]
        public async Task<IActionResult> GetFaqCategories()
        {
            ResponseModel<List<string>> response = new ResponseModel<List<string>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                response.Data = Constants.FaqCategories;
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
        /// It enables elasticsearch indexing of business records.
        /// </summary>
        /// <returns></returns>
        [HttpPost("makeindexbusiness")]
        public async Task<IActionResult> IndexBusiness()
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                BackgroundJob.Enqueue(() => _elasticHandler.MakeIndexBusiness());
                response.Message = Resource.Resource.ElasticIndexlemeIslemiBasladi;
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
        /// Search Business
        /// </summary>
        /// <returns></returns>
        [HttpPost("searchbusiness")]
        public async Task<IActionResult> SearchBusiness([FromBody] BusinessSearchAdminModel searchAdminModel)
        {
            var culture = Request.Headers["Language"].ToString().IsNull("en");
            ResponseModel<List<BusinessPagingListModel>> response = new ResponseModel<List<BusinessPagingListModel>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(culture);

            try
            {
                response.Data = await _businessService.GetBusinessLiteListAsync(searchAdminModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = "Exception => " + ex.Message;
                return Ok(response);
            }
        }
    }
}
