using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessGalleryRepository : IBusinessGalleryRepository
    {
        private readonly CareGardenApiDbContext _context;

        public BusinessGalleryRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessGallery> GetBusinessGalleryByIdAsync(Guid id)
        {
            return await _context.BusinessGalleries
                .FindAsync(id);
        }

        public async Task<List<BusinessGallery>> GetBusinessGalleriesByBusinessIdAsync(Guid businessId)
        {
            return await _context.BusinessGalleries
                .Where(x => x.businessId == businessId)
                .OrderBy(x => x.size)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BusinessGallery> SaveBusinessGalleryAsync(BusinessGallery businessGallery)
        {
            await _context.BusinessGalleries.AddAsync(businessGallery);
            await _context.SaveChangesAsync();
            return businessGallery;
        }

        public async Task<List<BusinessGallery>> SaveBusinessGalleriesAsync(List<BusinessGallery> businessGalleries)
        {
            await _context.BusinessGalleries.AddRangeAsync(businessGalleries);
            await _context.SaveChangesAsync();
            return businessGalleries;
        }

        public async Task<bool> UpdateBusinessGalleryAsync(BusinessGallery businessGallery)
        {
            _context.BusinessGalleries.Update(businessGallery);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBusinessGalleryAsync(BusinessGallery businessGallery)
        {
            _context.BusinessGalleries.Remove(businessGallery);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBusinessGalleryByBusinessIdAsync(Guid businessId)
        {
            await _context.BusinessGalleries
                .Where(x => x.businessId == businessId)
                .ExecuteDeleteAsync();
            return true;
        }

        public async Task<bool> DeleteBusinessGalleryByIdAsync(Guid id)
        {
            await _context.BusinessGalleries
                .Where(x => x.id == id)
                .ExecuteDeleteAsync();
            return true;
        }
    }
}