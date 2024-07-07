using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IConfirmationService
    {
        Task<ConfirmationInfo> GetConfirmationInfo(string target);
        Task<ConfirmationInfo> SaveConfirmationInfoAsync(ConfirmationInfo confirmationInfo);
        Task<ConfirmationInfo> SaveConfirmationInfoAsync(string target, string code);
    }
}
