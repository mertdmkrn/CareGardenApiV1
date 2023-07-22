using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private ICampaignService _campaingService;
        private IFileHandler _fileHandler;
        private readonly ILoggerHandler _loggerHandler;

        public CampaignController(ILoggerHandler loggerHandler)
        {
            _campaingService = new CampaignService();
            _fileHandler = new FileHandler();
            _loggerHandler = loggerHandler;
        }

        /// <summary>
        /// Get Services
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("campaign/getall")]
        public async Task<IActionResult> GetCampaigns()
        {
            ResponseModel<List<Campaign>> response = new ResponseModel<List<Campaign>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                response.Data = await _campaingService.GetCampaignsAsync();
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

        /// <summary>
        /// Get Campaign By BusinessId
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("campaign/getbybusinessid")]
        public async Task<IActionResult> GetCampaignByBusinessId([FromBody] string businessId)
        {
            ResponseModel<List<Campaign>> response = new ResponseModel<List<Campaign>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (!businessId.IsGuid())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                    response.Message = Resource.Resource.IdParametreHatasi;
                }

                if (response.HasError)
                    return Ok(response);

                response.Data = await _campaingService.GetCampaignByBusinessIdAsync(businessId.ToGuidNullable());

                if (response.Data == null)
                {
                    response.HasError = true;
                    response.Message = businessId + " id " + Resource.Resource.KayitBulunamadi;
                    return Ok(response);
                }

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

        /// <summary>
        /// Get Campaign By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("campaign/getbyid")]
        public async Task<IActionResult> GetCampaignById([FromBody] string id)
        {
            ResponseModel<Campaign> response = new ResponseModel<Campaign>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (!id.IsGuid())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                    response.Message = Resource.Resource.IdParametreHatasi;
                }

                if (response.HasError)
                    return Ok(response);

                response.Data = await _campaingService.GetCampaignByIdAsync(id.ToGuid());

                if (response.Data == null)
                {
                    response.HasError = true;
                    response.Message = id + " id " + Resource.Resource.KayitBulunamadi;
                    return Ok(response);
                }

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

        /// <summary>
        /// Campaign Upload Image
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("campaign/uploadimage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
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

                string fileName = "campaign" + "-" + DateTime.Now.ToString("ddMMhhmmss") + "." + file.FileName.Split(".").LastOrDefault();
                await _fileHandler.UploadFile(file, "CampaignImages", fileName);
                string imageUrl = await _fileHandler.UploadFreeImageServer(file);

                if (imageUrl.IsNullOrEmpty())
                    imageUrl = string.Format("{0}://{1}/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, "StaticFiles/UploadedFiles/CampaignImages/" + fileName);

                response.Message = Resource.Resource.ResimYuklemeBasarili;
                response.Data = imageUrl;

                return Ok(response);

            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Save Campaign (Role = 'Admin')
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "path" : "https://www.google.com",
        ///        "url": "https://www.google.com",
        ///        "businessId": "00000000-0000-0000-0000-000000000000",
        ///        "isActive" : true
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("campaign/save")]
        public async Task<IActionResult> Save([FromBody] Campaign campaign)
        {
            ResponseModel<Campaign> response = new ResponseModel<Campaign>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));
            
            try
            {
                if (campaign.path.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("path", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (campaign.url.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("url", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.KayitYapilamadi;
                    return Ok(response);
                }

                campaign = await _campaingService.SaveCampaignAsync(campaign);
                response.Data = campaign;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = Resource.Resource.KayitYapilamadi + " Exception => " + ex.Message;
                return Ok(response);
            }
        }


        /// <summary>
        /// Update Campaign (Role = 'Admin')
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
        ///        "path" : "https://www.google.com",
        ///        "url": "https://www.google.com",
        ///        "businessId": "00000000-0000-0000-0000-000000000000",
        ///        "isActive" : true,
        ///        "sortOrder" : 1
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("campaign/update")]
        public async Task<IActionResult> Update([FromBody] Campaign updateCampaign)
        {
            ResponseModel<Campaign> response = new ResponseModel<Campaign>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (updateCampaign.path.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("path", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (updateCampaign.url.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("url", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.GuncellemeYapilamadi;
                    return Ok(response);
                }

                Campaign campaign = await _campaingService.GetCampaignByIdAsync(updateCampaign.id);

                if (campaign == null)
                {
                    response.HasError = true;
                    response.Message += campaign.id + " id " + Resource.Resource.KayitBulunamadi;
                    return Ok(response);
                }

                campaign.path = updateCampaign.path;
                campaign.url = updateCampaign.url;
                campaign.businessId = updateCampaign.businessId;
                campaign.isActive = updateCampaign.isActive;
                campaign.sortOrder = updateCampaign.sortOrder;

                campaign = await _campaingService.UpdateCampaignAsync(campaign);
                response.Data = campaign;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = Resource.Resource.GuncellemeYapilamadi + " Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Delete Campaign (Role = 'Admin')
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("campaign/delete")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
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

                Campaign campaign = await _campaingService.GetCampaignByIdAsync(id.ToGuid());

                if (campaign == null)
                {
                    response.HasError = true;
                    response.Message += id + " id " + Resource.Resource.KayitBulunamadi;
                    return Ok(response);
                }

                response.Data = await _campaingService.DeleteCampaignAsync(campaign);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = Resource.Resource.KayitSilinemedi + " Exception => " + ex.Message;
                return Ok(response);
            }
        }
    }
}
