using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class BusinessWorkingInfoService : IBusinessWorkingInfoService
    {
        private readonly IBusinessWorkingInfoRepository _businessWorkingInfoRepository;

        public BusinessWorkingInfoService(IBusinessWorkingInfoRepository businessWorkingInfoRepository)
        {
            _businessWorkingInfoRepository = businessWorkingInfoRepository;
        }

        public async Task<bool> DeleteBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo)
        {
            return await _businessWorkingInfoRepository.DeleteBusinessWorkingInfoAsync(businessWorkingInfo);
        }

        public async Task<bool> DeleteBusinessWorkingInfoByBusinessIdAsync(Guid businessId)
        {
            return await _businessWorkingInfoRepository.DeleteBusinessWorkingInfoByBusinessIdAsync(businessId);
        }

        public async Task<BusinessWorkingInfo> GetBusinessWorkingInfoByIdAsync(Guid id)
        {
            return await _businessWorkingInfoRepository.GetBusinessWorkingInfoByIdAsync(id);
        }

        public async Task<List<BusinessWorkingInfo>> GetBusinessWorkingInfosByBusinessIdAsync(Guid businessId)
        {
            return await _businessWorkingInfoRepository.GetBusinessWorkingInfosByBusinessIdAsync(businessId);
        }

        public async Task<BusinessWorkingInfo> SaveBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo)
        {
            return await _businessWorkingInfoRepository.SaveBusinessWorkingInfoAsync(businessWorkingInfo);
        }

        public async Task<List<BusinessWorkingInfo>> SaveBusinessWorkingInfosAsync(List<BusinessWorkingInfo> businessWorkingInfos)
        {
            return await _businessWorkingInfoRepository.SaveBusinessWorkingInfosAsync(businessWorkingInfos);
        }

        public async Task<BusinessWorkingInfo> UpdateBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo)
        {
            return await _businessWorkingInfoRepository.UpdateBusinessWorkingInfoAsync(businessWorkingInfo);
        }
    }
}
