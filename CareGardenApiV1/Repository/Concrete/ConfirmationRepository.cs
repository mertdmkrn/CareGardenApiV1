using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class ConfirmationRepository : IConfirmationRepository
    {
        private readonly CareGardenApiDbContext _context;

        public ConfirmationRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<ConfirmationInfo> GetConfirmationInfo(string target)
        {
            return await _context.ConfirmationInfos
                .OrderByDescending(x => x.createDate)
                .FirstOrDefaultAsync(x => x.target.Equals(target));
        }

        public async Task<ConfirmationInfo> SaveConfirmationInfoAsync(ConfirmationInfo confirmationInfo)
        {
            confirmationInfo.createDate = DateTime.Now;

            await _context.ConfirmationInfos.AddAsync(confirmationInfo);
            await _context.SaveChangesAsync();
            return confirmationInfo;
        }

        public async Task<ConfirmationInfo> UpdateConfirmationInfoAsync(ConfirmationInfo confirmationInfo)
        {
            confirmationInfo.createDate = DateTime.Now;

            _context.ConfirmationInfos.Update(confirmationInfo);
            await _context.SaveChangesAsync();
            return confirmationInfo;
        }

        public async Task<bool> DeleteConfirmationInfoAsync(ConfirmationInfo confirmationInfo)
        {
            _context.ConfirmationInfos.Remove(confirmationInfo);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}