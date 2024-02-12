using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<IList<Setting>> GetSettingsAsync()
        {
            return await _settingRepository.GetSettingsAsync();
        }

        public async Task<Setting> GetSettingByIdAsync(Guid id)
        {
            return await _settingRepository.GetSettingByIdAsync(id);
        }

        public async Task<Setting> GetSettingByNameAsync(string name)
        {
            return await _settingRepository.GetSettingByNameAsync(name);
        }

        public async Task<bool> SaveSettingAsync(Setting setting)
        {
            return await _settingRepository.SaveSettingAsync(setting);
        }

        public async Task<bool> UpdateSettingAsync(Setting setting)
        {
            return await _settingRepository.UpdateSettingAsync(setting);
        }

        public async Task<bool> DeleteSettingAsync(Guid id)
        {
            return await _settingRepository.DeleteSettingAsync(id);
        }
    }
}
