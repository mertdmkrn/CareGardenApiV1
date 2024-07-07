using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IWorkerRepository
    {
        Task<Worker> GetWorkerByIdAsync(Guid id);
        Task<List<Worker>> GetWorkersByBusinessIdAsync(Guid businessId);
        Task<List<AppointmentWorkerResponseModel>> GetWorkersByAppointmentSearchModelAsync(AppointmentSearchRequestModel searchModel);
        Task<WorkerDetailResponseModel> GetWorkerDetailByIdAsync(Guid id);
        Task<List<AppointmentWorkerResponseModel>> GetWorkersByWorkerIdsAsync(List<Guid?> workerIds);
        Task<Worker> SaveWorkerAsync(Worker worker);
        Task<Worker> UpdateWorkerAsync(Worker worker);
        Task<bool> DeleteWorkerAsync(Guid id);
        Task<bool> DeleteWorkersByBusinessIdAsync(Guid businessId);
    }
}
