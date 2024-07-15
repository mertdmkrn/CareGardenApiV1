using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IResetLinkRepository
    {
        Task<ResetLink> GetResetLinkAsync(string email);
        Task<ResetLink> SaveResetLinkAsync(ResetLink resetLink);
        Task<ResetLink> UpdateResetLinkAsync(ResetLink resetLink);
        Task<bool> DeleteResetLinkAsync(ResetLink resetLink);
    }
}
