using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessPropertiesRepository : IBusinessPropertiesRepository
    {
        public async Task<BusinessProperties> GetBusinessPropertiesByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.BusinessProperties
                    .FirstOrDefaultAsync(x => x.id == id);
            }
        }

        public async Task<List<BusinessProperties>> GetBusinessPropertiesByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.BusinessProperties
                    .Where(x => x.businessId == businessId)
                    .OrderBy(x => x.key)
                    .ToListAsync();
            }
        }

        public async Task<BusinessProperties> GetBusinessPropertiesByBusinessIdAndKeyAsync(Guid businessId, string key)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.BusinessProperties
                    .Where(x => x.businessId == businessId && x.key == key)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<BusinessProperties> SaveBusinessPropertiesAsync(BusinessProperties businessProperties)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessProperties.AddAsync(businessProperties);
                await context.SaveChangesAsync();
                return businessProperties;
            }
        }

        public async Task<BusinessProperties> UpdateBusinessPropertiesAsync(BusinessProperties businessProperties)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.BusinessProperties.Update(businessProperties);
                await context.SaveChangesAsync();
                return businessProperties;
            }
        }

        public async Task<bool> DeleteBusinessPropertiesAsync(BusinessProperties businessProperties)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.BusinessProperties.Remove(businessProperties);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteBusinessPropertiesByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessProperties
                    .Where(x => x.businessId == businessId)
                    .ExecuteDeleteAsync();
                return true;
            }
        }
    }
}