using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class FaqService : IFaqService
    {
        private readonly IFaqRepository _faqRepository;

        public FaqService(IFaqRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public async Task<bool> DeleteFaqAsync(Guid id)
        {
            return await _faqRepository.DeleteFaqAsync(id);

        }

        public async Task<Faq> GetFaqByIdAsync(Guid id)
        {
            return await _faqRepository.GetFaqByIdAsync(id);
        }

        public async Task<IList<Faq>> GetFaqsAsync()
        {
            return await _faqRepository.GetFaqsAsync();
        }

        public async Task<bool> SaveFaqAsync(Faq faq)
        {
            return await _faqRepository.SaveFaqAsync(faq);
        }

        public async Task<bool> UpdateFaqAsync(Faq faq)
        {
            return await _faqRepository.UpdateFaqAsync(faq);
        }
    }
}
