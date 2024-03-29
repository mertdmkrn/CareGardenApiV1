using CareGardenApiV1.Model;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IFaqService
    {
        Task<IList<Faq>> GetFaqsAsync();
        Task<Faq> GetFaqByIdAsync(Guid id);
        Task<bool> SaveFaqAsync(Faq faq);
        Task<bool> UpdateFaqAsync(Faq faq);
        Task<bool> DeleteFaqAsync(Guid id);
    }
}
