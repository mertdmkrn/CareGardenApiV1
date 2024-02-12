using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class SettingRepository : ISettingRepository
    {
        public async Task<IList<Setting>> GetSettingsAsync()
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Settings
                    .AsNoTracking()
                    .OrderBy(x => x.name)
                    .ToListAsync();
            }
        }


        public async Task<Setting> GetSettingByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Settings
                    .FindAsync(id);
            }
        }

        public async Task<Setting> GetSettingByNameAsync(string name)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Settings
                    .FirstOrDefaultAsync(x => x.name.Equals(name));
            }
        }

        public async Task<bool> SaveSettingAsync(Setting setting)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Settings.AddAsync(setting);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateSettingAsync(Setting setting)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Settings.Update(setting);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteSettingAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Settings
                    .Where(x => x.id == id)
                    .ExecuteDeleteAsync();
                return true;
            }
        }

    }
}