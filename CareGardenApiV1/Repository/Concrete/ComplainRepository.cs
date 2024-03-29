using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class ComplainRepository : IComplainRepository
    {
        private readonly CareGardenApiDbContext _context;

        public ComplainRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<Complain> GetComplainByIdAsync(Guid id)
        {
            return await _context.Complains
                .FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<List<Complain>> GetComplainsByBusinessIdAsync(Guid businessId)
        {
            return await _context.Complains
                .Where(x => x.businessId == businessId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Complain>> GetComplainsByUserIdAsync(Guid userId)
        {
            return await _context.Complains
                .Where(x => x.userId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Complain> SaveComplainAsync(Complain complain)
        {
            await _context.Complains.AddAsync(complain);
            await _context.SaveChangesAsync();
            return complain;
        }

        public async Task<Complain> UpdateComplainAsync(Complain complain)
        {
            _context.Complains.Update(complain);
            await _context.SaveChangesAsync();
            return complain;
        }

        public async Task<bool> DeleteComplainAsync(Complain complain)
        {
            _context.Complains.Remove(complain);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}