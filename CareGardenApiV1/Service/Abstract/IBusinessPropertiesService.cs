using CareGardenApiV1.Model;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessPropertiesService
    {
        Task<BusinessProperties> GetBusinessPropertiesByIdAsync(Guid id);
        Task<List<BusinessProperties>> GetBusinessPropertiesByBusinessIdAsync(Guid businessId);
        Task<BusinessProperties> GetBusinessPropertiesByBusinessIdAndKeyAsync(Guid businessId, string key);
        Task<BusinessProperties> SaveBusinessPropertiesAsync(BusinessProperties businessProperties);
        Task<BusinessProperties> UpdateBusinessPropertiesAsync(BusinessProperties businessProperties);
        Task<bool> DeleteBusinessPropertiesAsync(BusinessProperties businessProperties);
        Task<bool> DeleteBusinessPropertiesByBusinessIdAsync(Guid businessId);
    }
}
