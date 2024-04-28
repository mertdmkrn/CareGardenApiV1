using CareGardenApiV1.Model;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;

        public WorkerService(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<bool> DeleteWorkerAsync(Guid id)
        {
            return await _workerRepository.DeleteWorkerAsync(id);
        }

        public async Task<bool> DeleteWorkersByBusinessIdAsync(Guid businessId)
        {
            return await _workerRepository.DeleteWorkersByBusinessIdAsync(businessId);
        }

        public async Task<Worker> GetWorkerByIdAsync(Guid id)
        {
            return await _workerRepository.GetWorkerByIdAsync(id);
        }

        public async Task<List<Worker>> GetWorkersByBusinessIdAsync(Guid businessId)
        {
            return await _workerRepository.GetWorkersByBusinessIdAsync(businessId);
        }

        public async Task<List<AppointmentWorkerModel>> GetWorkersByBusinessServiceIdAsync(Guid businessServiceId)
        {
            return await _workerRepository.GetWorkersByBusinessServiceIdAsync(businessServiceId);
        }

        public async Task<Worker> SaveWorkerAsync(Worker worker)
        {
            return await _workerRepository.SaveWorkerAsync(worker);
        }

        public async Task<Worker> UpdateWorkerAsync(Worker worker)
        {
            return await _workerRepository.UpdateWorkerAsync(worker);
        }
    }
}
