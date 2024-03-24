using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;

        public CampaignService(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public async Task<bool> DeleteCampaignAsync(Campaign campaign)
        {
           return await _campaignRepository.DeleteCampaignAsync(campaign);
        }

        public async Task<List<Campaign>> GetCampaignByBusinessIdAsync(Guid? businessId)
        {
            return await _campaignRepository.GetCampaignByBusinessIdAsync(businessId);
        }

        public async Task<Campaign> GetCampaignByIdAsync(Guid id)
        {
            return await _campaignRepository.GetCampaignByIdAsync(id);
        }

        public async Task<List<Campaign>> GetCampaignsAsync()
        {
            return await _campaignRepository.GetCampaignsAsync();
        }

        public async Task<Campaign> SaveCampaignAsync(Campaign campaign)
        {
            return await _campaignRepository.SaveCampaignAsync(campaign);
        }

        public async Task<Campaign> UpdateCampaignAsync(Campaign campaign)
        {
            return await _campaignRepository.UpdateCampaignAsync(campaign);
        }
    }
}
