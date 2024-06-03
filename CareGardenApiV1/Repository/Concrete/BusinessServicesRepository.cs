using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessServicesRepository : IBusinessServicesRepository
    {
        private readonly CareGardenApiDbContext _context;

        public BusinessServicesRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessServiceModel> GetBusinessServiceByIdAsync(Guid id)
        {
            return await _context.BusinessServices
                .FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<List<BusinessServiceModel>> GetBusinessServicesByIdsAsync(List<Guid> ids)
        {
            return await _context.BusinessServices
                .AsNoTracking()
                .Where(x => ids.Contains(x.id))
                .Select(x => new BusinessServiceModel
                {
                    id = x.id,
                    price = x.price,
                    minDuration = x.minDuration,
                    serviceId = x.serviceId,
                    maxDuration = x.maxDuration
                })
                .ToListAsync();
        }

        public async Task<List<BusinessServiceModel>> GetBusinessServicesByServiceIdAsync(Guid serviceId)
        {
            return await _context.BusinessServices
                .Where(x => x.serviceId == serviceId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<BusinessServiceModel>> GetBusinessServicesByBusinessIdAsync(Guid businessId)
        {
            return await _context.BusinessServices
                .Where(x => x.businessId == businessId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<BusinessServiceModel>> SaveBusinessServicesAsync(List<BusinessServiceModel> businessServices)
        {
            await _context.BusinessServices.AddRangeAsync(businessServices);
            await _context.SaveChangesAsync();
            return businessServices;
        }

        public async Task<BusinessServiceModel> SaveBusinessServiceAsync(BusinessServiceModel businessService)
        {
            await _context.BusinessServices.AddAsync(businessService);
            await _context.SaveChangesAsync();
            return businessService;
        }

        public async Task<BusinessServiceModel> UpdateBusinessServiceAsync(BusinessServiceModel businessService)
        {
            _context.BusinessServices.Update(businessService);
            await _context.SaveChangesAsync();
            return businessService;
        }

        public async Task<BusinessServiceModel> GetBusinessServicePriceByIdAsync(Guid id)
        {
            return await _context.BusinessServices
                .AsNoTracking()
                .Where(x => x.id == id)
                .Select(x => new BusinessServiceModel
                {
                    price = x.price,
                    serviceId = x.serviceId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteBusinessServiceAsync(Guid id)
        {
            await _context.BusinessServices
                .Where(x => x.id == id)
                .ExecuteDeleteAsync();
            return true;
        }

        public async Task<bool> DeleteBusinessServiceByBusinessIdAsync(Guid businessId)
        {
            await _context.BusinessServices
                .Where(x => x.businessId == businessId)
                .ExecuteDeleteAsync();
            return true;
        }
    }
}