using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Route("campaign")]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly IMemoryCache _memoryCache;
        private const string cacheKey = "campaigns";

        public CampaignController(
            ICampaignService campaignService,
            IMemoryCache memoryCache)
        {
            _campaignService = campaignService;
            _memoryCache = memoryCache;
        }


        /// <summary>
        /// Get Campaigns
        /// </summary>
        /// <returns></returns>
        [HttpPost("getall")]
        public async Task<IActionResult> GetAll()
        {
            ResponseModel<List<Campaign>> response = new ResponseModel<List<Campaign>>();

            if (_memoryCache.TryGetValue(cacheKey, out object list))
            {
                response.Data = (List<Campaign>)list;
            }
            else
            {
                response.Data = await _campaignService.GetCampaignsAsync();
                _memoryCache.Set(cacheKey, response.Data, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal
                });
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Active Campaigns
        /// </summary>
        /// <returns></returns>
        [HttpPost("getactives")]
        public async Task<IActionResult> GetActives()
        {
            ResponseModel<List<CampaingInfo>> response = new ResponseModel<List<CampaingInfo>>();
            var campaigns = new List<Campaign>();
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            if (_memoryCache.TryGetValue(cacheKey, out object list))
            {
                campaigns = (List<Campaign>)list;
            }
            else
            {
                campaigns = await _campaignService.GetCampaignsAsync();
                _memoryCache.Set(cacheKey, campaigns, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal
                });
            }

            response.Data = campaigns
                .Where(x => x.isActive)
                .Where(x => !x.expireDate.HasValue || x.expireDate > DateTime.Now)
                .Select(x => new CampaingInfo
                {
                    id = x.id,
                    businessId = x.businessId,
                    path = x.path,
                    url = x.url,
                    title = isTurkish ? x.title : x.titleEn,
                    about = isTurkish ? x.about : x.aboutEn,
                    condition = isTurkish ? x.condition : x.conditionEn,
                    expireDate = x.expireDate,
                    dayInfo = x.expireDate.GetRelativeDate(Resource.Resource.Culture.ToString()),
                    sortOrder = x.sortOrder
                })
                .OrderBy(x => x.sortOrder)
                .ToList();  

            return Ok(response);
        }

        /// <summary>
        /// Get Campaign By BusinessId
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbybusinessid")]
        public async Task<IActionResult> GetByBusinessId([FromBody] string businessId)
        {
            ResponseModel<List<Campaign>> response = new ResponseModel<List<Campaign>>();

            if (!businessId.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _campaignService.GetCampaignByBusinessIdAsync(businessId.ToGuidNullable());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = $"{businessId} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Campaign By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbyid")]
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            ResponseModel<Campaign> response = new ResponseModel<Campaign>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _campaignService.GetCampaignByIdAsync(id.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = $"{id} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            return Ok(response);
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
        ///        "title": "Lorem",
        ///        "titleEn": "Lorem",
        ///        "about": "Lorem",
        ///        "aboutEn": "Lorem",
        ///        "condition": "Lorem",
        ///        "conditionEn": "Lorem",
        ///        "expireDate": "2024-05-01",
        ///        "isActive" : true,
        ///        "sortOrder" : 1
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] Campaign campaign)
        {
            ResponseModel<Campaign> response = new ResponseModel<Campaign>();

            if (campaign.path.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("path", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (campaign.title.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("title", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (campaign.titleEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("titleEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (campaign.condition.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("condition", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (campaign.conditionEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("conditionEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            campaign = await _campaignService.SaveCampaignAsync(campaign);
            response.Data = campaign;
            _memoryCache.Remove(cacheKey);

            return Ok(response);
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
        ///        "title": "Lorem",
        ///        "titleEn": "Lorem",
        ///        "about": "Lorem",
        ///        "aboutEn": "Lorem",
        ///        "condition": "Lorem",
        ///        "conditionEn": "Lorem",
        ///        "expireDate": "2024-05-01",
        ///        "isActive" : true,
        ///        "sortOrder" : 1
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Campaign updateCampaign)
        {
            ResponseModel<Campaign> response = new ResponseModel<Campaign>();

            if (updateCampaign.path.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("path", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateCampaign.title.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("title", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateCampaign.titleEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("titleEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateCampaign.condition.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("condition", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateCampaign.conditionEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("conditionEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.GuncellemeYapilamadi;
                return Ok(response);
            }

            Campaign campaign = await _campaignService.GetCampaignByIdAsync(updateCampaign.id);

            if (campaign == null)
            {
                response.HasError = true;
                response.Message += $"{updateCampaign.id} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            campaign.path = updateCampaign.path;
            campaign.url = updateCampaign.url;
            campaign.businessId = updateCampaign.businessId;
            campaign.isActive = updateCampaign.isActive;
            campaign.sortOrder = updateCampaign.sortOrder;
            campaign.title = updateCampaign.title.IsNull(campaign.title);
            campaign.titleEn = updateCampaign.titleEn.IsNull(campaign.titleEn);
            campaign.about = updateCampaign.titleEn.IsNull(campaign.titleEn);
            campaign.aboutEn = updateCampaign.titleEn.IsNull(campaign.titleEn);
            campaign.condition = updateCampaign.titleEn.IsNull(campaign.titleEn);
            campaign.conditionEn = updateCampaign.titleEn.IsNull(campaign.titleEn);
            campaign.expireDate = updateCampaign.expireDate.HasValue ? updateCampaign.expireDate : campaign.expireDate;

            campaign = await _campaignService.UpdateCampaignAsync(campaign);
            response.Data = campaign;
            _memoryCache.Remove(cacheKey);

            return Ok(response);
        }

        /// <summary>
        /// Delete Campaign (Role = 'Admin')
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

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

            Campaign campaign = await _campaignService.GetCampaignByIdAsync(id.ToGuid());

            if (campaign == null)
            {
                response.HasError = true;
                response.Message += $"{id} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            response.Data = await _campaignService.DeleteCampaignAsync(campaign);
            _memoryCache.Remove(cacheKey);

            return Ok(response);
        }
    }
}
