using CareGardenApiV1.Model;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IWorkerService
    {
        Task<Worker> GetWorkerByIdAsync(Guid id);
        Task<List<Worker>> GetWorkersByBusinessIdAsync(Guid businessId);
        Task<List<Worker>> GetWorkersByBusinessServiceIdAsync(Guid businessServiceId);
        Task<Worker> SaveWorkerAsync(Worker worker);
        Task<Worker> UpdateWorkerAsync(Worker worker);
        Task<bool> DeleteWorkerAsync(Guid id);
        Task<bool> DeleteWorkersByBusinessIdAsync(Guid businessId);
    }
}
