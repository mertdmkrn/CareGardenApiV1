using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using Org.BouncyCastle.Asn1.X509;
using Nest;

namespace CareGardenApiV1.Repository.Concrete
{
    public class WorkerRepository : IWorkerRepository
    {
        public async Task<bool> DeleteWorkerAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Workers
                    .Where(x => x.id == id)
                    .ExecuteDeleteAsync();

                return true;
            }
        }

        public async Task<bool> DeleteWorkersByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Workers
                    .Where(x => x.businessId == businessId)
                    .ExecuteDeleteAsync();

                return true;
            }
        }

        public async Task<Worker> GetWorkerByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Workers
                    .FindAsync(id);
            }
        }

        public async Task<List<Worker>> GetWorkersByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Workers
                    .AsNoTracking()
                    .Where(x => x.businessId == businessId)
                    .ToListAsync();
            }
        }

        public async Task<List<Worker>> GetWorkersByBusinessServiceIdAsync(Guid businessServiceId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Workers
                    .AsNoTracking()
                    .Where(x => x.serviceIds.Length > x.serviceIds.Replace(businessServiceId.ToString(), "").Length)
                    .ToListAsync();
            }
        }

        public async Task<Worker> SaveWorkerAsync(Worker worker)
        {
            using (var context = new CareGardenApiDbContext())
            {
                worker.isActive = true;
                worker.isAvailable = true;
                await context.Workers.AddAsync(worker);
                await context.SaveChangesAsync();
                return worker;
            }
        }

        public async Task<Worker> UpdateWorkerAsync(Worker worker)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Workers.Update(worker);
                await context.SaveChangesAsync();
                return worker;
            }
        }
    }
}