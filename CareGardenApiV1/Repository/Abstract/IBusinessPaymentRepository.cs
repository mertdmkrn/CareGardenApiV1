using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessPaymentRepository
    {
        Task<BusinessPayment> GetBusinessPaymentByIdAsync(Guid id);
        Task<List<BusinessPayment>> SearchBusinessPaymentAsync(BusinessPaymentSearchRequestModel businessPaymentSearchRequestModel);
        Task<BusinessPayment> SaveBusinessPaymentAsync(BusinessPayment businessPayment);
        Task<bool> UpdateBusinessPaymentAsync(BusinessPayment businessPayment);
        Task<bool> DeleteBusinessPaymentAsync(BusinessPayment businessPayment);
    }
}
