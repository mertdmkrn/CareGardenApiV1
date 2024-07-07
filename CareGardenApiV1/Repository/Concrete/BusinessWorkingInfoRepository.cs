using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessWorkingInfoRepository : IBusinessWorkingInfoRepository
    {
        private readonly CareGardenApiDbContext _context;

        public BusinessWorkingInfoRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessWorkingInfo> GetBusinessWorkingInfoByIdAsync(Guid id)
        {
            return await _context.BusinessWorkingInfos
                .FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<List<BusinessWorkingInfo>> GetBusinessWorkingInfosByBusinessIdAsync(Guid businessId)
        {
            return await _context.BusinessWorkingInfos
                .Where(x => x.businessId == businessId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BusinessWorkingInfo> SaveBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo)
        {
            await _context.BusinessWorkingInfos.AddAsync(businessWorkingInfo);
            await _context.SaveChangesAsync();
            return businessWorkingInfo;
        }

        public async Task<List<BusinessWorkingInfo>> SaveBusinessWorkingInfosAsync(List<BusinessWorkingInfo> businessWorkingInfos)
        {
            await _context.BusinessWorkingInfos.AddRangeAsync(businessWorkingInfos);
            await _context.SaveChangesAsync();
            return businessWorkingInfos;
        }

        public async Task<BusinessWorkingInfo> UpdateBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo)
        {
            _context.BusinessWorkingInfos.Update(businessWorkingInfo);
            await _context.SaveChangesAsync();
            return businessWorkingInfo;
        }

        public async Task<bool> DeleteBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo)
        {
            _context.BusinessWorkingInfos.Remove(businessWorkingInfo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBusinessWorkingInfoByBusinessIdAsync(Guid businessId)
        {
            await _context.BusinessWorkingInfos
                .Where(x => x.businessId == businessId)
                .ExecuteDeleteAsync();
            return true;
        }
    }
}