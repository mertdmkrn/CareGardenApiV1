using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessGalleryRepository : IBusinessGalleryRepository
    {
        public async Task<BusinessGallery> GetBusinessGalleryByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.BusinessGalleries
                    .FirstOrDefaultAsync(x => x.id == id);
            }
        }

        public async Task<List<BusinessGallery>> GetBusinessGalleriesByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.BusinessGalleries
                    .Where(x => x.businessId == businessId)
                    .OrderBy(x => x.size)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<BusinessGallery> SaveBusinessGalleryAsync(BusinessGallery businessGallery)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessGalleries.AddAsync(businessGallery);
                await context.SaveChangesAsync();
                return businessGallery;
            }
        }

        public async Task<List<BusinessGallery>> SaveBusinessGalleriesAsync(List<BusinessGallery> businessGalleries)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessGalleries.AddRangeAsync(businessGalleries);
                await context.SaveChangesAsync();
                return businessGalleries;
            }
        }

        public async Task<BusinessGallery> UpdateBusinessGalleryAsync(BusinessGallery businessGallery)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.BusinessGalleries.Update(businessGallery);
                await context.SaveChangesAsync();
                return businessGallery;
            }
        }

        public async Task<bool> DeleteBusinessGalleryAsync(BusinessGallery businessGallery)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.BusinessGalleries.Remove(businessGallery);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteBusinessGalleryByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessGalleries
                    .Where(x => x.businessId == businessId)
                    .ExecuteDeleteAsync();
                return true;
            }
        }

        public async Task<bool> DeleteBusinessGalleryByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.BusinessGalleries
                    .Where(x => x.id == id)
                    .ExecuteDeleteAsync();
                return true;
            }
        }
    }
}