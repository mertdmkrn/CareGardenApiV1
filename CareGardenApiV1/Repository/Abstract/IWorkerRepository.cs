using CareGardenApiV1.Model;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IWorkerRepository
    {
        Task<Worker> GetWorkerByIdAsync(Guid id);
        Task<List<Worker>> GetWorkersByBusinessIdAsync(Guid businessId);
        Task<List<AppointmentWorkerModel>> GetWorkersByBusinessServiceIdAsync(Guid businessServiceId);
        Task<Worker> SaveWorkerAsync(Worker worker);
        Task<Worker> UpdateWorkerAsync(Worker worker);
        Task<bool> DeleteWorkerAsync(Guid id);
        Task<bool> DeleteWorkersByBusinessIdAsync(Guid businessId);
    }
}
