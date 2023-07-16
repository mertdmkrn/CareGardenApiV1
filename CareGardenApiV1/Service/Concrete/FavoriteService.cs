using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class FavoriteService : IFavoriteService
    {
        private IFavoriteRepository _favoriteRepository;

        public FavoriteService()
        {
            _favoriteRepository = new FavoriteRepository();
        }

        public async Task<bool> DeleteFavoriteAsync(Favorite favorite)
        {
            return await _favoriteRepository.DeleteFavoriteAsync(favorite);
        }

        public async Task<bool> DeleteFavoriteByBusinessIdAsync(Guid businessId)
        {
            return await _favoriteRepository.DeleteFavoriteByBusinessIdAsync(businessId);
        }

        public async Task<bool> DeleteFavoriteByUserIdAsync(Guid userId)
        {
            return await _favoriteRepository.DeleteFavoriteByUserIdAsync(userId);
        }    
        public async Task<bool> DeleteFavoriteByBusinessIdAndUserIdAsync(Guid userId, Guid businessId)
        {
            return await _favoriteRepository.DeleteFavoriteByBusinessIdAndUserIdAsync(userId, businessId);
        }

        public async Task<Favorite> GetFavoriteByIdAsync(Guid id)
        {
            return await _favoriteRepository.GetFavoriteByIdAsync(id);
        }

        public async Task<List<Favorite>> GetFavoritesByUserIdAsync(Guid userId)
        {
            return await _favoriteRepository.GetFavoritesByUserIdAsync(userId);
        }

        public async Task<Favorite> SaveFavoriteAsync(Favorite favorite)
        {
            return await _favoriteRepository.SaveFavoriteAsync(favorite);
        }

        public async Task<Favorite> UpdateFavoriteAsync(Favorite favorite)
        {
            return await _favoriteRepository.UpdateFavoriteAsync(favorite);
        }
    }
}
