using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly CareGardenApiDbContext _context;

        public CampaignRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Campaign>> GetCampaignsAsync()
        {
            return await _context.Campaigns
                .OrderBy(x => x.sortOrder)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Campaign> SaveCampaignAsync(Campaign campaign)
        {
            campaign.createDate = DateTime.Now;
            campaign.updateDate = campaign.createDate;

            await _context.Campaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();
            return campaign;
        }

        public async Task<Campaign> UpdateCampaignAsync(Campaign campaign)
        {
            campaign.updateDate = DateTime.Now;

            _context.Campaigns.Update(campaign);
            await _context.SaveChangesAsync();
            return campaign;
        }

        public async Task<bool> DeleteCampaignAsync(Campaign campaign)
        {
            _context.Campaigns.Remove(campaign);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Campaign>> GetCampaignByBusinessIdAsync(Guid? businessId)
        {
            return await _context.Campaigns
                .Where(x => x.businessId == businessId)
                .OrderBy(x => x.sortOrder)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Campaign> GetCampaignByIdAsync(Guid id)
        {
            return await _context.Campaigns
                .FirstOrDefaultAsync(x => x.id == id);
        }
    }
}