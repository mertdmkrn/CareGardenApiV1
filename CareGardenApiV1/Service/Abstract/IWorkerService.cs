using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IWorkerService
    {
        Task<Worker> GetWorkerByIdAsync(Guid id);
        Task<List<Worker>> GetWorkersByBusinessIdAsync(Guid businessId);
        Task<List<AppointmentWorkerResponseModel>> GetWorkersByAppointmentSearchModelAsync(AppointmentSearchRequestModel searchModel);
        Task<List<AppointmentWorkerResponseModel>> GetWorkersByWorkerIdsAsync(List<Guid?> workerIds);
        Task<WorkerDetailResponseModel> GetWorkerDetailByIdAsync(Guid id);
        Task<Worker> SaveWorkerAsync(Worker worker);
        Task<Worker> UpdateWorkerAsync(Worker worker);
        Task<bool> DeleteWorkerAsync(Guid id);
        Task<bool> DeleteWorkersByBusinessIdAsync(Guid businessId);
    }
}
