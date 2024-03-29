using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly CareGardenApiDbContext _context;

        public DiscountRepository(CareGardenApiDbContext context)
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

        public async Task<Discount> GetDiscountByIdAsync(Guid id)
        {
            return await _context.Discounts
                .FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<List<Discount>> GetDiscountsByBusinessIdAsync(Guid? businessId)
        {
            return await _context.Discounts
                .Where(x => x.businessId == businessId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Discount>> GetActiveDiscountsByBusinessIdAsync(Guid? businessId)
        {
            return await _context.Discounts
                .Where(x => x.businessId == businessId)
                .Where(x => x.isActive == true)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Discount> SaveDiscountAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
            return discount;
        }

        public async Task<Discount> UpdateDiscountAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
            return discount;
        }

        public async Task<bool> DeleteDiscountAsync(Discount discount)
        {
            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}