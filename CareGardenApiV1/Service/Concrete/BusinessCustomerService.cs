using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class BusinessCustomerService : IBusinessCustomerService
    {
        private readonly IBusinessCustomerRepository _businessCustomerRepository;

        public BusinessCustomerService(IBusinessCustomerRepository businessCustomerRepository)
        {
            _businessCustomerRepository = businessCustomerRepository;
        }

        public async Task<List<BusinessCustomer>> GetBusinessCustomerByBusinessIdAsync(Guid businessId)
        {
            return await _businessCustomerRepository.GetBusinessCustomerByBusinessIdAsync(businessId);
        }

        public async Task<BusinessCustomer> GetBusinessCustomerByIdAsync(Guid id)
        {
            return await _businessCustomerRepository.GetBusinessCustomerByIdAsync(id);
        }

        public async Task<BusinessCustomer> SaveBusinessCustomerAsync(BusinessCustomer businessCustomer)
        {
            return await _businessCustomerRepository.SaveBusinessCustomerAsync(businessCustomer);
        }

        public async Task<bool> UpdateBusinessCustomerAsync(BusinessCustomer businessCustomer)
        {
            return await _businessCustomerRepository.UpdateBusinessCustomerAsync(businessCustomer);
        }

        public async Task<bool> DeleteBusinessCustomerAsync(BusinessCustomer businessCustomer)
        {
            return await _businessCustomerRepository.DeleteBusinessCustomerAsync(businessCustomer);
        }
    }
}
