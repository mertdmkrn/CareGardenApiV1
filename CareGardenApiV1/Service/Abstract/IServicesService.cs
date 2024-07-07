using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IServicesService
    {
        Task<List<Services>> GetServicesAsync();
        Task<Services> GetServiceByIdAsync(Guid id);
        Task<Services> GetServiceByNameAsync(string name);
        Task<Services> GetServiceByNameEnAsync(string nameEn);
        Task<Services> SaveServiceAsync(Services services);
        Task<Services> UpdateServiceAsync(Services services);
        Task<bool> DeleteServiceAsync(Services services);
    }
}
