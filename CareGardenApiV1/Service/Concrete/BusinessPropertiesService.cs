using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class BusinessPropertiesService : IBusinessPropertiesService
    {
        private readonly IBusinessPropertiesRepository _businessPropertiesRepository;

        public BusinessPropertiesService(IBusinessPropertiesRepository businessPropertiesRepository)
        {
            _businessPropertiesRepository = businessPropertiesRepository;
        }

        public async Task<bool> DeleteBusinessPropertiesAsync(BusinessProperties businessProperties)
        {
            return await _businessPropertiesRepository.DeleteBusinessPropertiesAsync(businessProperties);
        }

        public async Task<bool> DeleteBusinessPropertiesByBusinessIdAsync(Guid businessId)
        {
            return await _businessPropertiesRepository.DeleteBusinessPropertiesByBusinessIdAsync(businessId);
        }

        public async Task<BusinessProperties> GetBusinessPropertiesByBusinessIdAndKeyAsync(Guid businessId, string key)
        {
            return await _businessPropertiesRepository.GetBusinessPropertiesByBusinessIdAndKeyAsync(businessId, key);
        }

        public async Task<List<BusinessProperties>> GetBusinessPropertiesByBusinessIdAsync(Guid businessId)
        {
            return await _businessPropertiesRepository.GetBusinessPropertiesByBusinessIdAsync(businessId);
        }

        public async Task<BusinessProperties> GetBusinessPropertiesByIdAsync(Guid id)
        {
            return await _businessPropertiesRepository.GetBusinessPropertiesByIdAsync(id);
        }

        public async Task<BusinessProperties> SaveBusinessPropertiesAsync(BusinessProperties businessProperties)
        {
            return await _businessPropertiesRepository.SaveBusinessPropertiesAsync(businessProperties);
        }

        public async Task<bool> SaveStaticBusinessPropertiesAsync(Guid businessId)
        {
            List<BusinessProperties> businessPropertiesList = new List<BusinessProperties>()
            {
                new BusinessProperties(){ businessId = businessId, key = "Instagram"},
                new BusinessProperties(){ businessId = businessId, key = "Website"},
                new BusinessProperties(){ businessId = businessId, key = "Twitter"},
                new BusinessProperties(){ businessId = businessId, key = "Facebook"}
            };

            return await _businessPropertiesRepository.SaveBusinessPropertiesListAsync(businessPropertiesList);
        }

        public async Task<BusinessProperties> UpdateBusinessPropertiesAsync(BusinessProperties businessProperties)
        {
            return await _businessPropertiesRepository.UpdateBusinessPropertiesAsync(businessProperties);
        }
    }
}
