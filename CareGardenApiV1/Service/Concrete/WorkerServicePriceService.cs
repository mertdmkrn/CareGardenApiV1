using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class WorkerServicePriceService : IWorkerServicePriceService
    {
        private readonly IWorkerServicePriceRepository _workerServicePriceRepository;

        public WorkerServicePriceService(IWorkerServicePriceRepository workerServicePriceRepository)
        {
            _workerServicePriceRepository = workerServicePriceRepository;
        }
        
        public async Task<WorkerServicePrice> GetWorkerServicePriceByIdAsync(Guid id)
        {
            return await _workerServicePriceRepository.GetWorkerServicePriceByIdAsync(id);
        }

        public async Task<List<WorkerServicePrice>> GetWorkerServicePricesSearchAsync(Guid? workerId = null, Guid? businessServiceId = null)
        {
            return await _workerServicePriceRepository.GetWorkerServicePricesSearchAsync(workerId, businessServiceId);
        }

        public async Task<List<WorkerServicePrice>> GetWorkerServicePricesByBusinessServiceIdsAsync(List<Guid> businessServiceIds)
        {
            return await _workerServicePriceRepository.GetWorkerServicePricesByBusinessServiceIdsAsync(businessServiceIds);
        }

        public async Task<WorkerServicePrice> SaveWorkerServicePriceAsync(WorkerServicePrice workerServicePrice)
        {
            return await _workerServicePriceRepository.SaveWorkerServicePriceAsync(workerServicePrice);
        }

        public async Task<bool> UpdateWorkerServicePriceAsync(WorkerServicePrice workerServicePrice)
        {
            return await _workerServicePriceRepository.UpdateWorkerServicePriceAsync(workerServicePrice);
        }

        public async Task<bool> UpdateWorkerServicePriceOnlyPriceAsync(WorkerServicePrice workerServicePrice)
        {
            return await _workerServicePriceRepository.UpdateWorkerServicePriceOnlyPriceAsync(workerServicePrice);
        }

        public async Task<bool> DeleteWorkerServicePriceAsync(Guid id)
        {
            return await _workerServicePriceRepository.DeleteWorkerServicePriceAsync(id);
        }
    }
}
