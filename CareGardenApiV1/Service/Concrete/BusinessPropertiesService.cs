using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
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

        public async Task<BusinessProperties> UpdateBusinessPropertiesAsync(BusinessProperties businessProperties)
        {
            return await _businessPropertiesRepository.UpdateBusinessPropertiesAsync(businessProperties);
        }
    }
}
