using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IConfirmationRepository
    {
        Task<ConfirmationInfo> GetConfirmationInfo(string target);
        Task<ConfirmationInfo> SaveConfirmationInfoAsync(ConfirmationInfo confirmationInfo);
        Task<ConfirmationInfo> UpdateConfirmationInfoAsync(ConfirmationInfo confirmationInfo);
        Task<bool> DeleteConfirmationInfoAsync(ConfirmationInfo confirmationInfo);
    }
}
