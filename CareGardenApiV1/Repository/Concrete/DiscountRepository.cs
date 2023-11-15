using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using Org.BouncyCastle.Asn1.X509;

namespace CareGardenApiV1.Repository.Concrete
{
    public class DiscountRepository : IDiscountRepository
    {
        public async Task<List<Campaign>> GetCampaignsAsync()
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Campaigns
                    .OrderBy(x => x.sortOrder)
                    .AsNoTracking()
                    .ToListAsync();
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
                    .AsNoTracking()
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

        public async Task<Discount> GetDiscountByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Discounts
                    .FirstOrDefaultAsync(x => x.id == id);
            }
        }

        public async Task<List<Discount>> GetDiscountsByBusinessIdAsync(Guid? businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Discounts
                    .Where(x => x.businessId == businessId)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<List<Discount>> GetActiveDiscountsByBusinessIdAsync(Guid? businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Discounts
                    .Where(x => x.businessId == businessId)
                    .Where(x => x.isActive == true)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<Discount> SaveDiscountAsync(Discount discount)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Discounts.AddAsync(discount);
                await context.SaveChangesAsync();
                return discount;
            }
        }

        public async Task<Discount> UpdateDiscountAsync(Discount discount)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Discounts.Update(discount);
                await context.SaveChangesAsync();
                return discount;
            }
        }

        public async Task<bool> DeleteDiscountAsync(Discount discount)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Discounts.Remove(discount);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}