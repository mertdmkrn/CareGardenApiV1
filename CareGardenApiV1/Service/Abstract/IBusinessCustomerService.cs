using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IBusinessCustomerService
    {
        Task<BusinessCustomer> GetBusinessCustomerByIdAsync(Guid id);
        Task<List<BusinessCustomer>> GetBusinessCustomerByBusinessIdAsync(Guid businessId);
        Task<BusinessCustomer> SaveBusinessCustomerAsync(BusinessCustomer businessCustomer);
        Task<bool> UpdateBusinessCustomerAsync(BusinessCustomer businessCustomer);
        Task<bool> DeleteBusinessCustomerAsync(BusinessCustomer businessCustomer);
    }
}
