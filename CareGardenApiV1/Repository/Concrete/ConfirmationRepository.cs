using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;

namespace CareGardenApiV1.Repository.Concrete
{
    public class ConfirmationRepository : IConfirmationRepository
    {
        public async Task<ConfirmationInfo> GetConfirmationInfo(string target)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.ConfirmationInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.target.Equals(target));
            }
        }

        public async Task<ConfirmationInfo> SaveConfirmationInfoAsync(ConfirmationInfo confirmationInfo)
        {
            using (var context = new CareGardenApiDbContext())
            {
                confirmationInfo.createDate = DateTime.Now;

                await context.ConfirmationInfos.AddAsync(confirmationInfo);
                await context.SaveChangesAsync();
                return confirmationInfo;
            }
        }

        public async Task<ConfirmationInfo> UpdateConfirmationInfoAsync(ConfirmationInfo confirmationInfo)
        {
            using (var context = new CareGardenApiDbContext())
            {
                confirmationInfo.createDate = DateTime.Now;

                context.ConfirmationInfos.Update(confirmationInfo);
                await context.SaveChangesAsync();
                return confirmationInfo;
            }
        }

        public async Task<bool> DeleteConfirmationInfoAsync(ConfirmationInfo confirmationInfo)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.ConfirmationInfos.Remove(confirmationInfo);
                await context.SaveChangesAsync();
                return true;
            }
        }

    }
}