using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class ComplainService : IComplainService
    {
        private readonly IComplainRepository _complainRepository;

        public ComplainService(IComplainRepository complainRepository)
        {
            _complainRepository = complainRepository;
        }

        public async Task<bool> DeleteComplainAsync(Complain complain)
        {
            return await _complainRepository.DeleteComplainAsync(complain);
        }

        public async Task<Complain> GetComplainByIdAsync(Guid id)
        {
            return await _complainRepository.GetComplainByIdAsync(id);
        }

        public async Task<List<Complain>> GetComplainsByBusinessIdAsync(Guid businessId)
        {
            return await _complainRepository.GetComplainsByBusinessIdAsync(businessId);
        }

        public async Task<List<Complain>> GetComplainsByUserIdAsync(Guid userId)
        {
            return await _complainRepository.GetComplainsByUserIdAsync(userId);
        }

        public async Task<Complain> SaveComplainAsync(Complain complain)
        {
            return await _complainRepository.SaveComplainAsync(complain);
        }

        public async Task<Complain> UpdateComplainAsync(Complain complain)
        {
            return await _complainRepository.UpdateComplainAsync(complain);
        }
    }
}
