using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class BusinessPaymentService : IBusinessPaymentService
    {
        private readonly IBusinessPaymentRepository _businessPaymentRepository;

        public BusinessPaymentService(IBusinessPaymentRepository businessPaymentRepository)
        {
            _businessPaymentRepository = businessPaymentRepository;
        }

        public async Task<List<BusinessPayment>> SearchBusinessPaymentAsync(BusinessPaymentSearchRequestModel businessPaymentSearchRequestModel)
        {
            return await _businessPaymentRepository.SearchBusinessPaymentAsync(businessPaymentSearchRequestModel);
        }

        public async Task<BusinessPayment> GetBusinessPaymentByIdAsync(Guid id)
        {
            return await _businessPaymentRepository.GetBusinessPaymentByIdAsync(id);
        }

        public async Task<BusinessPayment> SaveBusinessPaymentAsync(BusinessPayment businessPayment)
        {
            return await _businessPaymentRepository.SaveBusinessPaymentAsync(businessPayment);
        }

        public async Task<bool> UpdateBusinessPaymentAsync(BusinessPayment businessPayment)
        {
            return await _businessPaymentRepository.UpdateBusinessPaymentAsync(businessPayment);
        }

        public async Task<bool> DeleteBusinessPaymentAsync(BusinessPayment businessPayment)
        {
            return await _businessPaymentRepository.DeleteBusinessPaymentAsync(businessPayment);
        }
    }
}
