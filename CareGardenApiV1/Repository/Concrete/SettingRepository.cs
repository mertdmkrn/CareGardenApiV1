using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class SettingRepository : ISettingRepository
    {
        private readonly CareGardenApiDbContext _context;

        public SettingRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Setting>> GetSettingsAsync()
        {
            return await _context.Settings
                .AsNoTracking()
                .OrderBy(x => x.name)
                .ToListAsync();
        }

        public async Task<Setting> GetSettingByIdAsync(Guid id)
        {
            return await _context.Settings
                .FindAsync(id);
        }

        public async Task<Setting> GetSettingByNameAsync(string name)
        {
            return await _context.Settings
                .FirstOrDefaultAsync(x => x.name.Equals(name));
        }

        public async Task<bool> SaveSettingAsync(Setting setting)
        {
            await _context.Settings.AddAsync(setting);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSettingAsync(Setting setting)
        {
            _context.Settings.Update(setting);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSettingAsync(Guid id)
        {
            await _context.Settings
                .Where(x => x.id == id)
                .ExecuteDeleteAsync();
            return true;
        }
    }
}