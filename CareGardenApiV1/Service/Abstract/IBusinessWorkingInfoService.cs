using CareGardenApiV1.Model;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IBusinessWorkingInfoService
    {
        Task<BusinessWorkingInfo> GetBusinessWorkingInfoByIdAsync(Guid id);
        Task<List<BusinessWorkingInfo>> GetBusinessWorkingInfosByBusinessIdAsync(Guid businessId);
        Task<BusinessWorkingInfo> SaveBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo);
        Task<List<BusinessWorkingInfo>> SaveBusinessWorkingInfosAsync(List<BusinessWorkingInfo> businessWorkingInfos);
        Task<BusinessWorkingInfo> UpdateBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo);
        Task<bool> DeleteBusinessWorkingInfoAsync(BusinessWorkingInfo businessWorkingInfo);
        Task<bool> DeleteBusinessWorkingInfoByBusinessIdAsync(Guid businessId);
    }
}
