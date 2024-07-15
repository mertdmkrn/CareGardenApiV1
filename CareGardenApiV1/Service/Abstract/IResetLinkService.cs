using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IResetLinkService
    {
        Task<ResetLink> GetResetLinkAsync(string email);
        Task<ResetLink> SaveResetLinkAsync(ResetLink resetLink);
        Task<ResetLink> SaveResetLinkAsync(string email);
    }
}
