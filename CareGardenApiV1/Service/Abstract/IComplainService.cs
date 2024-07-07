using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IComplainService
    {
        Task<Complain> GetComplainByIdAsync(Guid id);
        Task<List<Complain>> GetComplainsByBusinessIdAsync(Guid businessId);
        Task<List<Complain>> GetComplainsByUserIdAsync(Guid userId);
        Task<Complain> SaveComplainAsync(Complain complain);
        Task<Complain> UpdateComplainAsync(Complain complain);
        Task<bool> DeleteComplainAsync(Complain complain);
    }
}
