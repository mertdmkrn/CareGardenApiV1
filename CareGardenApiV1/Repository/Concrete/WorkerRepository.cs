using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Repository.Concrete
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly CareGardenApiDbContext _context;

        public WorkerRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteWorkerAsync(Guid id)
        {
            await _context.Workers
                .Where(x => x.id == id)
                .ExecuteDeleteAsync();

            return true;
        }

        public async Task<bool> DeleteWorkersByBusinessIdAsync(Guid businessId)
        {
            await _context.Workers
                .Where(x => x.businessId == businessId)
                .ExecuteDeleteAsync();

            return true;
        }

        public async Task<Worker> GetWorkerByIdAsync(Guid id)
        {
            return await _context.Workers
                .FindAsync(id);
        }

        public async Task<List<Worker>> GetWorkersByBusinessIdAsync(Guid businessId)
        {
            return await _context.Workers
                .AsNoTracking()
                .Where(x => x.businessId == businessId)
                .ToListAsync();
        }

        public async Task<List<AppointmentWorkerModel>> GetWorkersByBusinessServiceIdAsync(Guid businessServiceId)
        {
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            return await _context.Workers
                .AsNoTracking()
                .Where(x => x.serviceIds.ToLower().Contains(businessServiceId.ToString()))
                .Select(x => new AppointmentWorkerModel
                {
                    id = x.id,
                    name = x.name,
                    path = x.path,
                    title = isTurkish ? x.title : x.titleEn.IsNull(x.title),
                    isActive = x.isActive
                })
                .ToListAsync();
        }

        public async Task<Worker> SaveWorkerAsync(Worker worker)
        {
            worker.isActive = true;

            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();
            return worker;
        }

        public async Task<Worker> UpdateWorkerAsync(Worker worker)
        {
            _context.Workers.Update(worker);
            await _context.SaveChangesAsync();
            return worker;
        }
    }
}