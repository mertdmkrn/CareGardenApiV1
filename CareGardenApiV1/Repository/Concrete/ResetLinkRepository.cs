using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class ResetLinkRepository : IResetLinkRepository
    {
        private readonly CareGardenApiDbContext _context;

        public ResetLinkRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<ResetLink> GetResetLinkAsync(string email)
        {
            return await _context.ResetLinks
                .OrderByDescending(x => x.createDate)
                .FirstOrDefaultAsync(x => x.email.Equals(email));
        }

        public async Task<ResetLink> SaveResetLinkAsync(ResetLink resetLink)
        {
            resetLink.createDate = DateTime.Now;
            resetLink.linkId = Guid.NewGuid();

            await _context.ResetLinks.AddAsync(resetLink);
            await _context.SaveChangesAsync();
            return resetLink;
        }

        public async Task<ResetLink> UpdateResetLinkAsync(ResetLink resetLink)
        {
            resetLink.createDate = DateTime.Now;
            resetLink.linkId = Guid.NewGuid();

            _context.ResetLinks.Update(resetLink);
            await _context.SaveChangesAsync();
            return resetLink;
        }

        public async Task<bool> DeleteResetLinkAsync(ResetLink resetLink)
        {
            _context.ResetLinks.Remove(resetLink);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}