using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessAdminRepository : IBusinessAdminRepository
    {
        private readonly CareGardenApiDbContext _context;

        public BusinessAdminRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

       
        public async Task<List<BusinessAdminEarningReportData>> GetBusinessAdminEarningReportDataAsync(Guid businessId)
        {
            return await _context.BusinessPayments
                .AsNoTracking()
                .Where(x => x.businessId == businessId && x.date < DateTime.Today.AddDays(-14))
                .GroupBy(x => x.date.Date)
                .Select(g => new BusinessAdminEarningReportData
                {
                    date = g.Key,
                    earning = g.Sum(a => a.amount),
                    dayStr = g.Key.ToString("ddd", Resource.Resource.Culture) 
                })
                .ToListAsync();
        }
    }
}