using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

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

        public AdminController(
            IBusinessService businessService,
            IUserService userService,
            IFaqService faqService,
            IFileHandler fileHandler,
            IMemoryCache memoryCache,
            IMailHandler mailHandler)
        {
            _businessService = businessService;
            _userService = userService;
            _faqService = faqService;
            _fileHandler = fileHandler;
            _memoryCache = memoryCache;
            _mailHandler = mailHandler;
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

            if (updateBusiness.id == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.BusinessNotVerified;
                response.HasError = true;
                return Ok(response);
            }

            Business business = await _businessService.GetBusinessByIdAsync(updateBusiness.id);

            if (business == null)
            {
                response.Message = $"{Resource.Resource.BusinessNotFound} {Resource.Resource.BusinessNotVerified}";
                response.HasError = true;
                return Ok(response);
            }

            business.isActive = updateBusiness.isActive;
            business.verified = updateBusiness.verified;

            await _businessService.UpdateBusinessAsync(business);
            response.Message = Resource.Resource.BusinessVerified;
            response.Data = true;

            return Ok(response);
        }

        /// <summary>
        /// Get Users.
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "gender": 3,
        ///        "city": "İstanbul",
        ///        "role": "ADMIN",
        ///        "page": 0,
        ///        "take": 20
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getusers")]
        public async Task<IActionResult> GetUsers([FromBody] UserSearchAdminRequestModel userSearchAdminModel)
        {
            ResponseModel<List<UserAdminResponseModel>> response = new ResponseModel<List<UserAdminResponseModel>>();

            response.Data = await _userService.GetUsersAsync(userSearchAdminModel);
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

            if (user.password.IsNotNullOrEmpty() && !user.password.Length.Between(8, 20))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("password", Resource.Resource.PasswordMustBeEightCharacters));
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

            var addingUserName = HelperMethods.GetClaimInfo(Request, ClaimTypes.Name);
            string mailMessage = HelperMethods.GetMailTemplate();

            StringBuilder builder = new StringBuilder();
            builder.Append(user.fullName)
                   .Append(" <a href='https://mertdmkrn.github.io/cg-admin/'>CG Admin</a>'e hoşgeldin. Giriş Bilgilerin; <br><br>")
                   .Append("Email : ").Append(user.email).Append("<br>Şifre : ").Append(user.password).Append("<br><br> ")
                   .Append("<p style='text-align:right; font-size:11.5px; padding-right:10px'>Sisteme Ekleyen : ").Append(addingUserName).Append("</p>");

            string content = builder.ToString();

            user.role = "Admin";

            user = await _userService.SaveUserAsync(user);
            response.Message = Resource.Resource.RegistrationSuccess;

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

        /// <summary>
        /// Delete User
        /// </summary>
        /// <returns></returns>
        [HttpPost("deleteuser")]
        public async Task<IActionResult> DeleteUser([FromBody] Guid userId)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var userEmail = HelperMethods.GetClaimInfo(Request, ClaimTypes.Email);

            if (userEmail.IsNullOrEmpty() || !userEmail.Equals("mertdmkrn37@gmail.com"))
            { 
                response.HasError = true;
                response.Data = false;
                response.Message = Resource.Resource.NotPermission;
            }

            User user = new User{ id = userId };

            response.Data = await _userService.DeleteUserAsync(user);
            response.Message = Resource.Resource.RecordDeleted;

            return Ok(response);
        }

        /// <summary>
        /// It is used to upload a new file and get a url.
        /// </summary>
        /// <returns></returns>
        [HttpPost("uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            if (file == null)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("file", Resource.Resource.NotEmpty));
                response.Message = Resource.Resource.ImageNotUploaded;
                return Ok(response);
            }

            response.Data = await _fileHandler.UploadFreeImageServer(file);
            response.Message = Resource.Resource.ImageUploaded;

            return Ok(response);
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

            if (faq.question.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("question", Resource.Resource.NotEmpty));
            }

            if (faq.questionEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("questionEn", Resource.Resource.NotEmpty));
            }

            if (faq.answer.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("answer", Resource.Resource.NotEmpty));
            }

            if (faq.answerEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("answerEn", Resource.Resource.NotEmpty));
            }

            if (faq.category.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("category", Resource.Resource.NotEmpty));
            }

            var index = Constants.FaqCategories.IndexOf(faq.category);

            if (response.HasError && index < 0)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }

            faq.categoryEn = Constants.FaqEnCategories[index];

            response.Data = await _faqService.SaveFaqAsync(faq);
            response.Message = Resource.Resource.RegistrationSuccess;
            _memoryCache.Remove("faqs");

            return Ok(response);
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

            if (updateFaq.question.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("question", Resource.Resource.NotEmpty));
            }

            if (updateFaq.questionEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("questionEn", Resource.Resource.NotEmpty));
            }

            if (updateFaq.answer.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("answer", Resource.Resource.NotEmpty));
            }

            if (updateFaq.answerEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("answerEn", Resource.Resource.NotEmpty));
            }

            if (updateFaq.category.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("category", Resource.Resource.NotEmpty));
            }

            var index = Constants.FaqCategories.IndexOf(updateFaq.category);

            if (response.HasError && index < 0)
            {
                response.Message = Resource.Resource.RecordNotUpdated;
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
            response.Message = Resource.Resource.RecordUpdated;

            return Ok(response);
        }

        /// <summary>
        /// Delete Faq
        /// </summary>
        /// <returns></returns>
        [HttpPost("deletefaq")]
        public async Task<IActionResult> DeleteFaq([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (id.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.NotEmpty));
            }

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

            response.Data = await _faqService.DeleteFaqAsync(id.ToGuid());
            response.Message = Resource.Resource.RecordDeleted;
            _memoryCache.Remove("faqs");

            return Ok(response);
        }

        /// <summary>
        /// Get Faq By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getfaq")]
        public async Task<IActionResult> GetFaq([FromBody] string id)
        {
            ResponseModel<Faq> response = new ResponseModel<Faq>();

            if (id.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.NotEmpty));
            }

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RecordNotFound;
                return Ok(response);
            }

            response.Data = await _faqService.GetFaqByIdAsync(id.ToGuid());

            return Ok(response);
        }


        /// <summary>
        /// Get Faq Categories
        /// </summary>
        /// <returns></returns>
        [HttpPost("getfaqcategories")]
        public async Task<IActionResult> GetFaqCategories()
        {
            ResponseModel<List<string>> response = new ResponseModel<List<string>>();

            response.Data = Constants.FaqCategories;
            return Ok(response);
        }

        /// <summary>
        /// Search Business
        /// </summary>
        /// <returns></returns>
        [HttpPost("searchbusiness")]
        public async Task<IActionResult> SearchBusiness([FromBody] BusinessSearchAdminRequestModel searchAdminModel)
        {
            ResponseModel<List<BusinessPagingListResponseModel>> response = new ResponseModel<List<BusinessPagingListResponseModel>>();

            response.Data = await _businessService.GetBusinessLiteListAsync(searchAdminModel);
            return Ok(response);
        }
    }
}
