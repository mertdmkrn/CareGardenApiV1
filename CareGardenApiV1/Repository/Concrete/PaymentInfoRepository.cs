using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class PaymentInfoRepository : IPaymentInfoRepository
    {
        private readonly CareGardenApiDbContext _context;

        public PaymentInfoRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentInfo> GetPaymentInfoByIdAsync(Guid id)
        {
            return await _context.PaymentInfos
                .FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<List<PaymentInfo>> GetPaymentInfosByBusinessIdAsync(Guid businessId)
        {
            return await _context.PaymentInfos
                .Where(x => x.businessId == businessId)
                .OrderByDescending(x => x.date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PaymentInfo>> GetPaymentInfosByIsPaidAsync(bool isPaid)
        {
            return await _context.PaymentInfos
                .Where(x => x.isPaid == isPaid)
                .OrderByDescending(x => x.date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PaymentInfo> SavePaymentInfoAsync(PaymentInfo paymentInfo)
        {
            await _context.PaymentInfos.AddAsync(paymentInfo);
            await _context.SaveChangesAsync();
            return paymentInfo;
        }

        public async Task<PaymentInfo> UpdatePaymentInfoAsync(PaymentInfo paymentInfo)
        {
            _context.PaymentInfos.Update(paymentInfo);
            await _context.SaveChangesAsync();
            return paymentInfo;
        }

        public async Task<bool> DeletePaymentInfoAsync(PaymentInfo paymentInfo)
        {
            _context.PaymentInfos.Remove(paymentInfo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePaymentInfoByBusinessIdAsync(Guid businessId)
        {
            await _context.PaymentInfos
                .Where(x => x.businessId == businessId)
                .ExecuteDeleteAsync();
            return true;
        }
    }
}