using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class FaqRepository : IFaqRepository
    {
        public async Task<IList<Faq>> GetFaqsAsync()
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Faqs
                    .AsNoTracking()
                    .OrderBy(x => x.sortOrder)
                    .ToListAsync();
            }
        }


        public async Task<Faq> GetFaqByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Faqs
                    .FindAsync(id);
            }
        }

        public async Task<bool> SaveFaqAsync(Faq faq)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Faqs.AddAsync(faq);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateFaqAsync(Faq faq)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Faqs.Update(faq);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteFaqAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Faqs
                    .Where(x => x.id == id)
                    .ExecuteDeleteAsync();
                return true;
            }
        }

    }
}