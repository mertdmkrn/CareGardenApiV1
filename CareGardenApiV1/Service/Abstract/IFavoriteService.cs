using CareGardenApiV1.Model;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IFavoriteService
    {
        Task<Favorite> GetFavoriteByIdAsync(Guid id);
        Task<List<Favorite>> GetFavoritesByUserIdAsync(Guid userId);
        Task<Favorite> SaveFavoriteAsync(Favorite favorite);
        Task<Favorite> UpdateFavoriteAsync(Favorite favorite);
        Task<bool> DeleteFavoriteAsync(Favorite favorite);
        Task<bool> DeleteFavoriteByUserIdAsync(Guid userId);
        Task<bool> DeleteFavoriteByBusinessIdAsync(Guid businessId);
        Task<bool> DeleteFavoriteByBusinessIdAndUserIdAsync(Guid userId, Guid businessId);
    }
}
