using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessPropertiesRepository
    {
        Task<BusinessProperties> GetBusinessPropertiesByIdAsync(Guid id);
        Task<List<BusinessProperties>> GetBusinessPropertiesByBusinessIdAsync(Guid businessId);
        Task<BusinessProperties> GetBusinessPropertiesByBusinessIdAndKeyAsync(Guid businessId, string key);
        Task<BusinessProperties> SaveBusinessPropertiesAsync(BusinessProperties businessProperties);
        Task<bool> SaveBusinessPropertiesListAsync(List<BusinessProperties> businessPropertiesList);
        Task<BusinessProperties> UpdateBusinessPropertiesAsync(BusinessProperties businessProperties);
        Task<bool> DeleteBusinessPropertiesAsync(BusinessProperties businessProperties);
        Task<bool> DeleteBusinessPropertiesByBusinessIdAsync(Guid businessId);
    }
}
