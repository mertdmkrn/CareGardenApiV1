using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface ICampaignRepository
    {
        Task<List<Campaign>> GetCampaignsAsync();
        Task<List<Campaign>> GetCampaignByBusinessIdAsync(Guid? businessId);
        Task<Campaign> GetCampaignByIdAsync(Guid id);
        Task<Campaign> SaveCampaignAsync(Campaign campaign);
        Task<Campaign> UpdateCampaignAsync(Campaign campaign);
        Task<bool> DeleteCampaignAsync(Campaign campaign);
    }
}
