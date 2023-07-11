using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class ComplainRepository : IComplainRepository
    {
        public async Task<Complain> GetComplainByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Complains
                    .FirstOrDefaultAsync(x => x.id == id);
            }
        }

        public async Task<List<Complain>> GetComplainsByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Complains
                    .Where(x => x.businessId == businessId)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<List<Complain>> GetComplainsByUserIdAsync(Guid userId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Complains
                    .Where(x => x.userId == userId)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<Complain> SaveComplainAsync(Complain complain)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Complains.AddAsync(complain);
                await context.SaveChangesAsync();
                return complain;
            }
        }

        public async Task<Complain> UpdateComplainAsync(Complain complain)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Complains.Update(complain);
                await context.SaveChangesAsync();
                return complain;
            }
        }

        public async Task<bool> DeleteComplainAsync(Complain complain)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Complains.Remove(complain);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}