using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IWorkerServicePriceService
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
