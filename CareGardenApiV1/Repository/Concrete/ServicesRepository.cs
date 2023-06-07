using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using Org.BouncyCastle.Asn1.X509;

namespace CareGardenApiV1.Repository.Concrete
{
    public class ServicesRepository : IServicesRepository
    {
        public async Task<List<Services>> GetServicesAsync()
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Services.ToListAsync();
            }
        }

        public async Task<Services> GetServiceByIdAsync(int id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Services
                    .FindAsync(id);
            }
        }

        public async Task<Services> GetServiceByNameAsync(string name)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Services
                    .FirstOrDefaultAsync(x => x.name.Equals(name));
            }
        }

        public async Task<Services> GetServiceByNameEnAsync(string nameEn)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Services
                    .FirstOrDefaultAsync(x => x.nameEn.Equals(nameEn));
            }
        }

        public async Task<Services> SaveServiceAsync(Services services)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Services.AddAsync(services);
                await context.SaveChangesAsync();
                return services;
            }
        }

        public async Task<Services> UpdateServiceAsync(Services services)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Services.Update(services);
                await context.SaveChangesAsync();
                return services;
            }
        }

        public async Task<bool> DeleteServiceAsync(Services services)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Services.Remove(services);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}