using CareGardenApiV1.Model;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IServicesRepository
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
