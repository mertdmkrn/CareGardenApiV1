using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class WorkerServicePriceRepository : IWorkerServicePriceRepository
    {
        private readonly CareGardenApiDbContext _context;

        public WorkerServicePriceRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<WorkerServicePrice> GetWorkerServicePriceByIdAsync(Guid id)
        {
            return await _context.WorkerServicePrices
                .FirstOrDefaultAsync(x => x.id.Equals(id));
        }

        public async Task<List<WorkerServicePrice>> GetWorkerServicePricesSearchAsync(Guid? workerId = null, Guid? businessServiceId = null)
        {
            return await _context.WorkerServicePrices
                .AsNoTracking()
                .WhereIf(workerId.IsNotNullOrEmpty(), x => x.workerId.Equals(workerId))
                .WhereIf(businessServiceId.IsNotNullOrEmpty(), x => x.businessServiceId.Equals(businessServiceId))
                .ToListAsync();
        }
        
        public async Task<List<WorkerServicePrice>> GetWorkerServicePricesByBusinessServiceIdsAsync(List<Guid> businessServiceIds)
        {
            return await _context.WorkerServicePrices
                .AsNoTracking()
                .Where(x => businessServiceIds.Contains(x.businessServiceId.Value))
                .ToListAsync();
        }

        public async Task<WorkerServicePrice> SaveWorkerServicePriceAsync(WorkerServicePrice workerServicePrice)
        {
            await _context.WorkerServicePrices.AddAsync(workerServicePrice);
            await _context.SaveChangesAsync();
            return workerServicePrice;
        }

        public async Task<bool> UpdateWorkerServicePriceAsync(WorkerServicePrice workerServicePrice)
        {
            await _context.WorkerServicePrices
                .Where(x => x.id.Equals(workerServicePrice.id))
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.price, workerServicePrice.price));

            return true;
        }

        public async Task<bool> UpdateWorkerServicePriceOnlyPriceAsync(WorkerServicePrice workerServicePrice)
        {
            await _context.WorkerServicePrices
                .Where(x => x.businessServiceId.Equals(workerServicePrice.businessServiceId))
                .Where(x => x.price < workerServicePrice.price)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.price, workerServicePrice.price));

            return true;        
        }

        public async Task<bool> DeleteWorkerServicePriceAsync(Guid id)
        {
            await _context.WorkerServicePrices
                .Where(x => x.id == id)
                .ExecuteDeleteAsync();
            return true;
        }
    }
}