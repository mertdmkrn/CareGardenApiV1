using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class ConfirmationService : IConfirmationService
    {
        private readonly IConfirmationRepository _confirmationRepository;

        public ConfirmationService(IConfirmationRepository confirmationRepository)
        {
            _confirmationRepository = confirmationRepository;
        }

        public async Task<ConfirmationInfo> GetConfirmationInfo(string target)
        {
            return await _confirmationRepository.GetConfirmationInfo(target);
        }

        public async Task<ConfirmationInfo> SaveConfirmationInfoAsync(ConfirmationInfo confirmationInfo)
        {
            var systemConfirmationInfo = await _confirmationRepository.GetConfirmationInfo(confirmationInfo.target);

            if(systemConfirmationInfo == null)
                return await _confirmationRepository.SaveConfirmationInfoAsync(confirmationInfo);

            systemConfirmationInfo.code = confirmationInfo.code;
            return await _confirmationRepository.UpdateConfirmationInfoAsync(systemConfirmationInfo);
        }

        public async Task<ConfirmationInfo> SaveConfirmationInfoAsync(string target, string code)
        {
            var systemConfirmationInfo = await _confirmationRepository.GetConfirmationInfo(target);

            if (systemConfirmationInfo == null)
            {
                ConfirmationInfo confirmationInfo = new ConfirmationInfo();
                confirmationInfo.target = target;
                confirmationInfo.code = code;
                return await _confirmationRepository.SaveConfirmationInfoAsync(confirmationInfo);

            }

            systemConfirmationInfo.code = code;
            return await _confirmationRepository.UpdateConfirmationInfoAsync(systemConfirmationInfo);
        }
    }
}
