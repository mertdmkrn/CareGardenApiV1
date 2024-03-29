using CareGardenApiV1.Model;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IBusinessServicesService
    {
        Task<BusinessServiceModel> GetBusinessServiceByIdAsync(Guid id);
        Task<List<BusinessServiceModel>> GetBusinessServicesByServiceIdAsync(Guid serviceId);
        Task<List<BusinessServiceModel>> GetBusinessServicesByBusinessIdAsync(Guid businessId);
        Task<List<BusinessServiceModel>> SaveBusinessServicesAsync(List<BusinessServiceModel> businessServices);
        Task<BusinessServiceModel> SaveBusinessServiceAsync(BusinessServiceModel businessService);
        Task<BusinessServiceModel> UpdateBusinessServiceAsync(BusinessServiceModel businessService);
        Task<bool> DeleteBusinessServiceAsync(Guid id);
        Task<bool> DeleteBusinessServiceByBusinessIdAsync(Guid businessId);
    }
}
