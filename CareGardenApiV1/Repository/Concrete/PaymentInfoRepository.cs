using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class PaymentInfoRepository : IPaymentInfoRepository
    {
        public async Task<PaymentInfo> GetPaymentInfoByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.PaymentInfos
                    .FirstOrDefaultAsync(x => x.id == id);
            }
        }

        public async Task<List<PaymentInfo>> GetPaymentInfosByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.PaymentInfos
                    .Where(x => x.businessId == businessId)
                    .OrderByDescending(x => x.date)
                    .ToListAsync();
            }
        }

        public async Task<List<PaymentInfo>> GetPaymentInfosByIsPaidAsync(bool isPaid)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.PaymentInfos
                    .Where(x => x.isPaid == isPaid)
                    .OrderByDescending(x => x.date)
                    .ToListAsync();
            }
        }

        public async Task<PaymentInfo> SavePaymentInfoAsync(PaymentInfo paymentInfo)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.PaymentInfos.AddAsync(paymentInfo);
                await context.SaveChangesAsync();
                return paymentInfo;
            }
        }

        public async Task<PaymentInfo> UpdatePaymentInfoAsync(PaymentInfo paymentInfo)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.PaymentInfos.Update(paymentInfo);
                await context.SaveChangesAsync();
                return paymentInfo;
            }
        }

        public async Task<bool> DeletePaymentInfoAsync(PaymentInfo paymentInfo)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.PaymentInfos.Remove(paymentInfo);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeletePaymentInfoByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.PaymentInfos
                    .Where(x => x.businessId == businessId)
                    .ExecuteDeleteAsync();
                return true;
            }
        }
    }
}