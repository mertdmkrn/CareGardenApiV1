using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface ISettingRepository
    {
        Task<IList<Setting>> GetSettingsAsync();
        Task<Setting> GetSettingByIdAsync(Guid id);
        Task<Setting> GetSettingByNameAsync(string name);
        Task<bool> SaveSettingAsync(Setting faq);
        Task<bool> UpdateSettingAsync(Setting faq);
        Task<bool> DeleteSettingAsync(Guid id);
    }
}
