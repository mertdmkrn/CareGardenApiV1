using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessServicesRepository : IBusinessServicesRepository
    {
        public async Task<BusinessServiceModel> GetBusinessServiceByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.BusinessServices
                    .FirstOrDefaultAsync(x => x.id == id);
            }
        }

        public async Task<List<BusinessServiceModel>> GetBusinessServicesByServiceIdAsync(Guid serviceId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.BusinessServices
                    .Where(x => x.serviceId == serviceId)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<List<BusinessServiceModel>> GetBusinessServicesByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.BusinessServices
                    .Where(x => x.businessId == businessId)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<List<BusinessServiceModel>> SaveBusinessServicesAsync(List<BusinessServiceModel> businessServices)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessServices.AddRangeAsync(businessServices);
                await context.SaveChangesAsync();
                return businessServices;
            }
        }

        public async Task<BusinessServiceModel> SaveBusinessServiceAsync(BusinessServiceModel businessService)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessServices.AddAsync(businessService);
                await context.SaveChangesAsync();
                return businessService;
            }
        }

        public async Task<BusinessServiceModel> UpdateBusinessServiceAsync(BusinessServiceModel businessService)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.BusinessServices.Update(businessService);
                await context.SaveChangesAsync();
                return businessService;
            }
        }

        public async Task<bool> DeleteBusinessServiceAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessServices
                    .Where(x => x.id == id)
                    .ExecuteDeleteAsync();
                return true;
            }
        }

        public async Task<bool> DeleteBusinessServiceByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessServices
                    .Where(x => x.businessId == businessId)
                    .ExecuteDeleteAsync();
                return true;
            }
        }
    }
}