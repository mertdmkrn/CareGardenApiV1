using CareGardenApiV1.Model;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IPaymentInfoService
    {
        Task<PaymentInfo> GetPaymentInfoByIdAsync(Guid id);
        Task<List<PaymentInfo>> GetPaymentInfosByBusinessIdAsync(Guid businessId);
        Task<List<PaymentInfo>> GetPaymentInfosByIsPaidAsync(bool isPaid);
        Task<PaymentInfo> SavePaymentInfoAsync(PaymentInfo paymentInfo);
        Task<PaymentInfo> UpdatePaymentInfoAsync(PaymentInfo paymentInfo);
        Task<bool> DeletePaymentInfoAsync(PaymentInfo paymentInfo);
        Task<bool> DeletePaymentInfoByBusinessIdAsync(Guid businessId);
    }
}
