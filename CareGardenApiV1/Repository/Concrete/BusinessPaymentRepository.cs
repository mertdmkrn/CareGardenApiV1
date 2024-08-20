using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessPaymentRepository : IBusinessPaymentRepository
    {
        private readonly CareGardenApiDbContext _context;

        public BusinessPaymentRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessPayment> GetBusinessPaymentByIdAsync(Guid id)
        {
            return await _context.BusinessPayments
                .FindAsync(id);
        }

        public async Task<List<BusinessPayment>> SearchBusinessPaymentAsync(BusinessPaymentSearchRequestModel businessPaymentSearchRequestModel)
        {
            return await _context.BusinessPayments
                .AsNoTracking()
                .Where(x => x.businessId == businessPaymentSearchRequestModel.businessId)
                .WhereIf(businessPaymentSearchRequestModel.startDate.HasValue, x => x.date >= businessPaymentSearchRequestModel.startDate)
                .WhereIf(businessPaymentSearchRequestModel.endDate.HasValue, x => x.date <= businessPaymentSearchRequestModel.endDate)
                .Skip(businessPaymentSearchRequestModel.page * businessPaymentSearchRequestModel.take)
                .Take(businessPaymentSearchRequestModel.take)
                .ToListAsync();
        }

        public async Task<BusinessPayment> SaveBusinessPaymentAsync(BusinessPayment businessPayment)
        {
            await _context.BusinessPayments.AddAsync(businessPayment);
            await _context.SaveChangesAsync();
            return businessPayment;
        }

        public async Task<bool> UpdateBusinessPaymentAsync(BusinessPayment businessPayment)
        {
            _context.BusinessPayments.Update(businessPayment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBusinessPaymentAsync(BusinessPayment businessPayment)
        {
            _context.BusinessPayments.Remove(businessPayment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}