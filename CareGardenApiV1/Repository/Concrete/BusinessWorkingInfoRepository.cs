using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessWorkingInfoRepository : IBusinessWorkingInfoRepository
    {
        public async Task<BusinessWorkingInfo> GetBusinessWorkingInfoByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.BusinessWorkingInfos
                    .FirstOrDefaultAsync(x => x.id == id);
            }
        }

        public async Task<List<BusinessWorkingInfo>> GetBusinessWorkingInfosByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.BusinessWorkingInfos
                    .Where(x => x.businessId == businessId)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<BusinessWorkingInfo> SaveBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessWorkingInfos.AddAsync(businessWorkingInfo);
                await context.SaveChangesAsync();
                return businessWorkingInfo;
            }
        }

        public async Task<List<BusinessWorkingInfo>> SaveBusinessWorkingInfosAsync(List<BusinessWorkingInfo> businessWorkingInfos)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessWorkingInfos.AddRangeAsync(businessWorkingInfos);
                await context.SaveChangesAsync();
                return businessWorkingInfos;
            }
        }

        public async Task<BusinessWorkingInfo> UpdateBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.BusinessWorkingInfos.Update(businessWorkingInfo);
                await context.SaveChangesAsync();
                return businessWorkingInfo;
            }
        }

        public async Task<bool> DeleteBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.BusinessWorkingInfos.Remove(businessWorkingInfo);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteBusinessWorkingInfoByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessWorkingInfos
                    .Where(x => x.businessId == businessId)
                    .ExecuteDeleteAsync();
                return true;
            }
        }
    }
}