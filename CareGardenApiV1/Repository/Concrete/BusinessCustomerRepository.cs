using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.TableModel;
namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessCustomerRepository : IBusinessCustomerRepository
    {
        private readonly CareGardenApiDbContext _context;

        public BusinessCustomerRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessCustomer> GetBusinessCustomerByIdAsync(Guid id)
        {
            return await _context.BusinessCustomers
                .FindAsync(id);
        }

        public async Task<List<BusinessCustomer>> GetBusinessCustomerByBusinessIdAsync(Guid businessId)
        {
            return await _context.BusinessCustomers
                .AsNoTracking()
                .Where(x => x.businessId == businessId)
                .ToListAsync();
        }

        public async Task<BusinessCustomer> SaveBusinessCustomerAsync(BusinessCustomer businessCustomer)
        {
            businessCustomer.createDate = DateTime.Now;
            await _context.BusinessCustomers.AddAsync(businessCustomer);
            await _context.SaveChangesAsync();
            return businessCustomer;
        }

        public async Task<bool> UpdateBusinessCustomerAsync(BusinessCustomer businessCustomer)
        {
            _context.BusinessCustomers.Update(businessCustomer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBusinessCustomerAsync(BusinessCustomer businessCustomer)
        {
            _context.BusinessCustomers.Remove(businessCustomer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}