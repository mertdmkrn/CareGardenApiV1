using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IFaqRepository
    {
        Task<IList<Faq>> GetFaqsAsync();
        Task<Faq> GetFaqByIdAsync(Guid id);
        Task<bool> SaveFaqAsync(Faq faq);
        Task<bool> UpdateFaqAsync(Faq faq);
        Task<bool> DeleteFaqAsync(Guid id);
    }
}
