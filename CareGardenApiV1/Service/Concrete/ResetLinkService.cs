using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class ResetLinkService : IResetLinkService
    {
        private readonly IResetLinkRepository _resetLinkRepository;

        public ResetLinkService(IResetLinkRepository resetLinkRepository)
        {
            _resetLinkRepository = resetLinkRepository;
        }

        public async Task<ResetLink> SaveResetLinkAsync(string email)
        {
            var systemResetLink = await GetResetLinkAsync(email);

            if (systemResetLink == null)
            {
                ResetLink resetLink = new ResetLink();
                resetLink.email = email;
                return await _resetLinkRepository.SaveResetLinkAsync(resetLink);
            }

            return await _resetLinkRepository.UpdateResetLinkAsync(systemResetLink);
        }

        public async Task<ResetLink> SaveResetLinkAsync(ResetLink resetLink)
        {
            var systemResetLink = await GetResetLinkAsync(resetLink.email);

            if (systemResetLink == null)
                return await _resetLinkRepository.SaveResetLinkAsync(resetLink);

            return await _resetLinkRepository.UpdateResetLinkAsync(systemResetLink);
        }

        public async Task<ResetLink> GetResetLinkAsync(string email)
        {
            return await _resetLinkRepository.GetResetLinkAsync(email);
        }
    }
}
