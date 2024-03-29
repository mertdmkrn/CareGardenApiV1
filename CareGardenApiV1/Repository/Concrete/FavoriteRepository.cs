using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly CareGardenApiDbContext _context;

        public FavoriteRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<Favorite> GetFavoriteByIdAsync(Guid id)
        {
            return await _context.Favorites
                .FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<List<Favorite>> GetFavoritesByUserIdAsync(Guid userId)
        {
            return await _context.Favorites
                .Where(x => x.userId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Favorite> SaveFavoriteAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
            return favorite;
        }

        public async Task<Favorite> UpdateFavoriteAsync(Favorite favorite)
        {
            _context.Favorites.Update(favorite);
            await _context.SaveChangesAsync();
            return favorite;
        }

        public async Task<bool> DeleteFavoriteAsync(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteFavoriteByUserIdAsync(Guid userId)
        {
            await _context.Favorites
                .Where(x => x.userId == userId)
                .ExecuteDeleteAsync();
            return true;
        }

        public async Task<bool> DeleteFavoriteByBusinessIdAsync(Guid businessId)
        {
            await _context.Favorites
                .Where(x => x.businessId == businessId)
                .ExecuteDeleteAsync();
            return true;
        }

        public async Task<bool> DeleteFavoriteByBusinessIdAndUserIdAsync(Guid userId, Guid businessId)
        {
            await _context.Favorites
                .Where(x => x.userId == userId && x.businessId == businessId)
                .ExecuteDeleteAsync();
            return true;
        }
    }
}