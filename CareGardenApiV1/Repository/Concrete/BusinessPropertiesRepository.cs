using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessPropertiesRepository : IBusinessPropertiesRepository
    {
        private readonly CareGardenApiDbContext _context;

        public BusinessPropertiesRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessProperties> GetBusinessPropertiesByIdAsync(Guid id)
        {
            return await _context.BusinessProperties
                .FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<List<BusinessProperties>> GetBusinessPropertiesByBusinessIdAsync(Guid businessId)
        {
            return await _context.BusinessProperties
                .Where(x => x.businessId == businessId)
                .OrderBy(x => x.key)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BusinessProperties> GetBusinessPropertiesByBusinessIdAndKeyAsync(Guid businessId, string key)
        {
            return await _context.BusinessProperties
                .Where(x => x.businessId == businessId && x.key == key)
                .FirstOrDefaultAsync();
        }

        public async Task<BusinessProperties> SaveBusinessPropertiesAsync(BusinessProperties businessProperties)
        {
            await _context.BusinessProperties.AddAsync(businessProperties);
            await _context.SaveChangesAsync();
            return businessProperties;
        }

        public async Task<BusinessProperties> UpdateBusinessPropertiesAsync(BusinessProperties businessProperties)
        {
            _context.BusinessProperties.Update(businessProperties);
            await _context.SaveChangesAsync();
            return businessProperties;
        }

        public async Task<bool> DeleteBusinessPropertiesAsync(BusinessProperties businessProperties)
        {
            _context.BusinessProperties.Remove(businessProperties);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBusinessPropertiesByBusinessIdAsync(Guid businessId)
        {
            await _context.BusinessProperties
                .Where(x => x.businessId == businessId)
                .ExecuteDeleteAsync();
            return true;
        }
    }
}