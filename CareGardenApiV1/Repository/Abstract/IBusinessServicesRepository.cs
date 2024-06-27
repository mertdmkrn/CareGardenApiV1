using CareGardenApiV1.Model;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessServicesRepository
    {
        Task<BusinessServiceModel> GetBusinessServiceByIdAsync(Guid id); 
        Task<BusinessServiceModel> GetBusinessServicePriceByIdAsync(Guid id);
        Task<List<BusinessServiceModel>> GetBusinessServicesByIdsAsync(List<Guid> ids);
        Task<List<BusinessServiceModel>> GetBusinessServicesByServiceIdAsync(Guid serviceId);
        Task<List<BusinessServiceModel>> GetBusinessServicesByBusinessIdAsync(Guid businessId);
        Task<List<WorkerDetailServiceInfo>> GetWorkerDetailServiceInfoByIdsAsync(List<Guid> ids);
        Task<List<BusinessServiceModel>> SaveBusinessServicesAsync(List<BusinessServiceModel> businessServices);
        Task<BusinessServiceModel> SaveBusinessServiceAsync(BusinessServiceModel businessService);
        Task<BusinessServiceModel> UpdateBusinessServiceAsync(BusinessServiceModel businessService);
        Task<bool> DeleteBusinessServiceAsync(Guid id);
        Task<bool> DeleteBusinessServiceByBusinessIdAsync(Guid businessId);
    }
}
