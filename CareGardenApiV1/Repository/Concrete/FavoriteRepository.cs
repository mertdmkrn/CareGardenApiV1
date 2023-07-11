using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;

namespace CareGardenApiV1.Repository.Concrete
{
    public class FavoriteRepository : IFavoriteRepository
    {
        public async Task<Favorite> GetFavoriteByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Favorites
                    .FirstOrDefaultAsync(x => x.id == id);
            }
        }

        public async Task<List<Favorite>> GetFavoritesByUserIdAsync(Guid userId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Favorites
                    .Where(x => x.userId == userId)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<Favorite> SaveFavoriteAsync(Favorite favorite)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Favorites.AddAsync(favorite);
                await context.SaveChangesAsync();
                return favorite;
            }
        }

        public async Task<Favorite> UpdateFavoriteAsync(Favorite favorite)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Favorites.Update(favorite);
                await context.SaveChangesAsync();
                return favorite;
            }
        }

        public async Task<bool> DeleteFavoriteAsync(Favorite favorite)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Favorites.Remove(favorite);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteFavoriteByUserIdAsync(Guid userId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Favorites
                    .Where(x => x.userId == userId)
                    .ExecuteDeleteAsync();
                return true;
            }
        }

        public async Task<bool> DeleteFavoriteByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Favorites
                    .Where(x => x.businessId == businessId)
                    .ExecuteDeleteAsync();
                return true;
            }
        }
    }
}