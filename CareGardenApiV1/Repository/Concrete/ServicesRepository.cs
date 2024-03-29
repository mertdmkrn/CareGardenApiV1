using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly CareGardenApiDbContext _context;

        public ServicesRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Services>> GetServicesAsync()
        {
            return await _context.Services
                .OrderBy(x => x.sortOrder)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Services> GetServiceByIdAsync(Guid id)
        {
            return await _context.Services
                .FindAsync(id);
        }

        public async Task<Services> GetServiceByNameAsync(string name)
        {
            return await _context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.name.Equals(name));
        }

        public async Task<Services> GetServiceByNameEnAsync(string nameEn)
        {
            return await _context.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.nameEn.Equals(nameEn));
        }

        public async Task<Services> SaveServiceAsync(Services services)
        {
            await _context.Services.AddAsync(services);
            await _context.SaveChangesAsync();
            return services;
        }

        public async Task<Services> UpdateServiceAsync(Services services)
        {
            _context.Services.Update(services);
            await _context.SaveChangesAsync();
            return services;
        }

        public async Task<bool> DeleteServiceAsync(Services services)
        {
            _context.Services.Remove(services);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}