using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IWorkerServicePriceRepository
    {
        Task<WorkerServicePrice> GetWorkerServicePriceByIdAsync(Guid id);
        Task<List<WorkerServicePrice>> GetWorkerServicePricesSearchAsync(Guid? workerId = null, Guid? businessServiceId = null);
        Task<List<WorkerServicePrice>> GetWorkerServicePricesByBusinessServiceIdsAsync(List<Guid> businessServiceIds);
        Task<WorkerServicePrice> SaveWorkerServicePriceAsync(WorkerServicePrice workerServicePrice);
        Task<bool> UpdateWorkerServicePriceAsync(WorkerServicePrice workerServicePrice);
        Task<bool> UpdateWorkerServicePriceOnlyPriceAsync(WorkerServicePrice workerServicePrice);
        Task<bool> DeleteWorkerServicePriceAsync(Guid id);
    }
}
