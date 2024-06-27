using CareGardenApiV1.Model;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class BusinessServicesService : IBusinessServicesService
    {
        private readonly IBusinessServicesRepository _businessServicesRepository;

        public BusinessServicesService(IBusinessServicesRepository businessServicesRepository)
        {
            _businessServicesRepository = businessServicesRepository;
        }

        public async Task<bool> DeleteBusinessServiceAsync(Guid id)
        {
            return await _businessServicesRepository.DeleteBusinessServiceAsync(id);          
        }

        public async Task<bool> DeleteBusinessServiceByBusinessIdAsync(Guid businessId)
        {
            return await _businessServicesRepository.DeleteBusinessServiceByBusinessIdAsync(businessId);          
        }

        public async Task<BusinessServiceModel> GetBusinessServiceByIdAsync(Guid id)
        {
            return await _businessServicesRepository.GetBusinessServiceByIdAsync(id);          
        }

        public async Task<BusinessServiceModel> GetBusinessServicePriceByIdAsync(Guid id)
        {
            return await _businessServicesRepository.GetBusinessServicePriceByIdAsync(id);
        }

        public async Task<List<BusinessServiceModel>> GetBusinessServicesByIdsAsync(List<Guid> ids)
        {
            return await _businessServicesRepository.GetBusinessServicesByIdsAsync(ids);
        }

        public async Task<List<BusinessServiceModel>> GetBusinessServicesByBusinessIdAsync(Guid businessId)
        {
            return await _businessServicesRepository.GetBusinessServicesByBusinessIdAsync(businessId);          
        }

        public async Task<List<BusinessServiceModel>> GetBusinessServicesByServiceIdAsync(Guid serviceId)
        {
            return await _businessServicesRepository.GetBusinessServicesByServiceIdAsync(serviceId);          
        }

        public async Task<BusinessServiceModel> SaveBusinessServiceAsync(BusinessServiceModel businessService)
        {
            return await _businessServicesRepository.SaveBusinessServiceAsync(businessService);          
        }

        public async Task<List<BusinessServiceModel>> SaveBusinessServicesAsync(List<BusinessServiceModel> businessServices)
        {
            return await _businessServicesRepository.SaveBusinessServicesAsync(businessServices);          
        }

        public async Task<BusinessServiceModel> UpdateBusinessServiceAsync(BusinessServiceModel businessService)
        {
            return await _businessServicesRepository.UpdateBusinessServiceAsync(businessService);          
        }

        public async Task<List<WorkerDetailServiceInfo>> GetWorkerDetailServiceInfoByIdsAsync(List<Guid> ids)
        {
            return await _businessServicesRepository.GetWorkerDetailServiceInfoByIdsAsync(ids);
        }
    }
}
