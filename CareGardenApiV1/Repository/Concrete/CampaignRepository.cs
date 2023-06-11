using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using Org.BouncyCastle.Asn1.X509;

namespace CareGardenApiV1.Repository.Concrete
{
    public class CampaignRepository : ICampaignRepository
    {
        public async Task<List<Campaign>> GetCampaignsAsync()
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Campaigns.OrderBy(x => x.sortOrder).ToListAsync();
            }
        }

        public async Task<Campaign> SaveCampaignAsync(Campaign campaign)
        {
            using (var context = new CareGardenApiDbContext())
            {
                campaign.createDate = DateTime.Now;
                campaign.updateDate = campaign.createDate;

                await context.Campaigns.AddAsync(campaign);
                await context.SaveChangesAsync();
                return campaign;
            }
        }

        public async Task<Campaign> UpdateCampaignAsync(Campaign campaign)
        {
            using (var context = new CareGardenApiDbContext())
            {
                campaign.updateDate = DateTime.Now;

                context.Campaigns.Update(campaign);
                await context.SaveChangesAsync();
                return campaign;
            }
        }

        public async Task<bool> DeleteCampaignAsync(Campaign campaign)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Campaigns.Remove(campaign);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<Campaign>> GetCampaignByBusinessIdAsync(Guid? businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Campaigns
                    .Where(x => x.businessId == businessId)
                    .OrderBy(x => x.sortOrder)
                    .ToListAsync();
            }
        }


        public async Task<Campaign> GetCampaignByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Campaigns
                    .FirstOrDefaultAsync(x => x.id == id);
            }
        }
    }
}