using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;
using NetTopologySuite.Geometries;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using OneSignalApi.Model;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessRepository : IBusinessRepository
    {
        public async Task<bool> DeleteBusinessAsync(Business business)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Businesses.Remove(business);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Business> GetBusinessByEmailAndPasswordAsync(string email, string password)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Businesses
                    .FirstOrDefaultAsync(x => x.email == email && x.password == password);
            }
        }

        public async Task<Business> GetBusinessByEmailAsync(string email)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Businesses
                    .FirstOrDefaultAsync(x => x.email == email);
            }
        }

        public async Task<Business> GetBusinessByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Businesses
                    .FindAsync(id);
            }
        }

        public async Task<IList<BusinessListModel>> GetBusinessByPopularAsync(double? latitude, double? longitude, int? page, int? take)
        {
            using (var context = new CareGardenApiDbContext())
            {
                Point? userLocation = null;
                var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

                if (latitude.HasValue && longitude.HasValue)
                {
                    userLocation = gf.CreatePoint(new Coordinate(latitude.Value, longitude.Value));
                }

                if (page.HasValue && take.HasValue)
                {
                    return await context.Businesses
                        .Where(x => x.isActive == true && x.verified == true)
                        .Include(x => x.comments)
                        .Include(x => x.galleries)
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name,
                            discountRate = x.discountRate,
                            workingGenderType = x.workingGenderType,
                            assets = x.galleries.Select(x => new BusinessGallery { imageUrl = x.imageUrl, size = x.size }).ToList(),
                            averageRating = x.comments.Average(x => x.point),
                            countRating = x.comments.Count(),
                            distance = userLocation != null ? x.location.Distance(gf.CreateGeometry(userLocation)) : 0
                        })
                        .OrderByDescending(x => x.averageRating)
                        .ThenBy(x => x.distance)
                        .Skip(page.Value * take.Value)
                        .Take(take.Value)
                        .ToListAsync();
                }

                return await context.Businesses
                    .Where(x => x.isActive == true && x.verified == true)
                    .Include(x => x.comments)
                    .Include(x => x.galleries)
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name,
                        discountRate = x.discountRate,
                        workingGenderType = x.workingGenderType,
                        assets = x.galleries.Select(x => new BusinessGallery { imageUrl = x.imageUrl, size = x.size }).ToList(),
                        averageRating = x.comments.Average(x => x.point),
                        countRating = x.comments.Count(),
                        distance = userLocation != null ? x.location.Distance(gf.CreateGeometry(userLocation)) : 0
                    })
                    .OrderByDescending(x => x.averageRating)
                    .ThenBy(x => x.distance)
                    .ToListAsync();
            }
        }

        public async Task<Business> GetBusinessByTelephoneNumberAsync(string telephone)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Businesses
                    .FirstOrDefaultAsync(x => x.telephone == telephone);
            }
        }

        public async Task<IList<BusinessListModel>> GetBusinessByUserFavorites(double? latitude, double? longitude, Guid userId, int? page, int? take)
        {
            using (var context = new CareGardenApiDbContext())
            {
                Point? userLocation = null;
                var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

                if (latitude.HasValue && longitude.HasValue)
                {
                    userLocation = gf.CreatePoint(new Coordinate(latitude.Value, longitude.Value));
                }

                if (page.HasValue && take.HasValue)
                {
                    return await context.Businesses
                        .Where(x => x.isActive == true && x.verified == true)
                        .Include(x => x.comments)
                        .Include(x => x.galleries)
                        .Include(x => x.favorites.Where(x => x.userId == userId))
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name,
                            discountRate = x.discountRate,
                            workingGenderType = x.workingGenderType,
                            assets = x.galleries.Select(x => new BusinessGallery { imageUrl = x.imageUrl, size = x.size }).ToList(),
                            averageRating = x.comments.Average(x => x.point),
                            countRating = x.comments.Count(),
                            distance = userLocation != null ? x.location.Distance(gf.CreateGeometry(userLocation)) : 0
                        })
                        .OrderBy(x => x.distance)
                        .ThenByDescending(x => x.averageRating)
                        .Skip(page.Value * take.Value)
                        .Take(take.Value)
                        .ToListAsync();
                }

                return await context.Businesses
                    .Where(x => x.isActive == true && x.verified == true)
                    .Include(x => x.comments)
                    .Include(x => x.galleries)
                    .Include(x => x.favorites.Where(x => x.userId == userId))
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name,
                        discountRate = x.discountRate,
                        workingGenderType = x.workingGenderType,
                        assets = x.galleries.Select(x => new BusinessGallery { imageUrl = x.imageUrl, size = x.size }).ToList(),
                        averageRating = x.comments.Average(x => x.point),
                        countRating = x.comments.Count(),
                        distance = userLocation != null ? x.location.Distance(gf.CreateGeometry(userLocation)) : 0
                    })
                    .OrderBy(x => x.distance)
                    .ThenByDescending(x => x.averageRating)
                    .ToListAsync();
            }
        }

        public async Task<IList<BusinessListModel>> GetBusinessNearByDistanceAsync(double? latitude, double? longitude, int? page, int? take)
        {
            using (var context = new CareGardenApiDbContext())
            {
                Point? userLocation = null;
                var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

                if (latitude.HasValue && longitude.HasValue)
                {
                    userLocation = gf.CreatePoint(new Coordinate(latitude.Value, longitude.Value));
                }

                if (page.HasValue && take.HasValue)
                {
                    return await context.Businesses
                        .Where(x => x.isActive == true && x.verified == true)
                        .Include(x => x.comments)
                        .Include(x => x.galleries)
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name,
                            discountRate = x.discountRate,
                            workingGenderType = x.workingGenderType,
                            assets = x.galleries.Select(x => new BusinessGallery { imageUrl = x.imageUrl, size = x.size }).ToList(),
                            averageRating = x.comments.Average(x => x.point),
                            countRating = x.comments.Count(),
                            distance = userLocation != null ? x.location.Distance(gf.CreateGeometry(userLocation)) : 0
                        })
                        .OrderBy(x => x.distance)
                        .ThenByDescending(x => x.averageRating)
                        .Skip(page.Value * take.Value)
                        .Take(take.Value)
                        .ToListAsync();
                }

                return await context.Businesses
                    .Where(x => x.isActive == true && x.verified == true)
                    .Include(x => x.comments)
                    .Include(x => x.galleries)
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name,
                        discountRate = x.discountRate,
                        workingGenderType = x.workingGenderType,
                        assets = x.galleries.Select(x => new BusinessGallery { imageUrl = x.imageUrl, size = x.size }).ToList(),
                        averageRating = x.comments.Average(x => x.point),
                        countRating = x.comments.Count(),
                        distance = userLocation != null ? x.location.Distance(gf.CreateGeometry(userLocation)) : 0
                    })
                    .OrderBy(x => x.distance)
                    .ThenByDescending(x => x.averageRating)
                    .ToListAsync();
            }
        }

        public async Task<Dictionary<string, string>> GetBusinessSelectListAsync()
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Businesses
                        .Where(x => x.isActive == true && x.verified == true)
                        .Select(x => new BusinessListModel { id = x.id, name = x.name })
                        .ToDictionaryAsync(x => x.id.ToString(), x => x.name);
            }
        }

        public async Task<Business> SaveBusinessAsync(Business business)
        {
            using (var context = new CareGardenApiDbContext())
            {
                business.password = business.password.HashString();
                business.createDate = DateTime.Now;
                business.updateDate = business.createDate;

                await context.Businesses.AddAsync(business);
                await context.SaveChangesAsync();
                return business;
            }
        }

        public async Task<Business> UpdateBusinessAsync(Business business)
        {
            using (var context = new CareGardenApiDbContext())
            {
                business.updateDate = DateTime.Now;

                if (business.password.IsNotNullOrEmpty() && business.password.Length <= 8)
                {
                    business.password = business.password.HashString();
                }

                context.Businesses.Update(business);
                await context.SaveChangesAsync();
                return business;
            }
        }
    }
}