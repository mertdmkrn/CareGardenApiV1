using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class PaymentInfoService : IPaymentInfoService
    {
        private readonly IPaymentInfoRepository _paymentInfoRepository;

        public PaymentInfoService(IPaymentInfoRepository paymentInfoRepository)
        {
            _paymentInfoRepository = paymentInfoRepository;
        }

        public async Task<bool> DeletePaymentInfoAsync(PaymentInfo paymentInfo)
        {
            return await _paymentInfoRepository.DeletePaymentInfoAsync(paymentInfo);
        }

        public async Task<bool> DeletePaymentInfoByBusinessIdAsync(Guid businessId)
        {
            return await _paymentInfoRepository.DeletePaymentInfoByBusinessIdAsync(businessId);
        }

        public async Task<PaymentInfo> GetPaymentInfoByIdAsync(Guid id)
        {
            return await _paymentInfoRepository.GetPaymentInfoByIdAsync(id);
        }

        public async Task<List<PaymentInfo>> GetPaymentInfosByBusinessIdAsync(Guid businessId)
        {
            return await _paymentInfoRepository.GetPaymentInfosByBusinessIdAsync(businessId);
        }

        public async Task<List<PaymentInfo>> GetPaymentInfosByIsPaidAsync(bool isPaid)
        {
            return await _paymentInfoRepository.GetPaymentInfosByIsPaidAsync(isPaid);
        }

        public async Task<PaymentInfo> SavePaymentInfoAsync(PaymentInfo paymentInfo)
        {
            return await _paymentInfoRepository.SavePaymentInfoAsync(paymentInfo);
        }

        public async Task<PaymentInfo> UpdatePaymentInfoAsync(PaymentInfo paymentInfo)
        {
            return await _paymentInfoRepository.UpdatePaymentInfoAsync(paymentInfo);
        }
    }
}
