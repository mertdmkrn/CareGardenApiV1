using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class FaqRepository : IFaqRepository
    {
        private readonly CareGardenApiDbContext _context;

        public FaqRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Faq>> GetFaqsAsync()
        {
            return await _context.Faqs
                .AsNoTracking()
                .OrderBy(x => x.sortOrder)
                .ToListAsync();
        }


        public async Task<Faq> GetFaqByIdAsync(Guid id)
        {
            return await _context.Faqs
                .FindAsync(id);
        }

        public async Task<bool> SaveFaqAsync(Faq faq)
        {
            await _context.Faqs.AddAsync(faq);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateFaqAsync(Faq faq)
        {
            _context.Faqs.Update(faq);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFaqAsync(Guid id)
        {
            await _context.Faqs
                .Where(x => x.id == id)
                .ExecuteDeleteAsync();
            return true;
        }
    }
}