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
                    .FirstOrDefaultAsync(x => x.email == email && x.password == password.HashString());
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

        public async Task<IList<BusinessListModel>> GetBusinessByPopularAsync(double? latitude, double? longitude, string? city, int? page, int? take)
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
                        .Where(x => city.IsNotNullOrEmpty() ? x.city.Equals(city) : x.city != null)
                        .Include(x => x.comments)
                        .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name ?? "",
                            discountRate = x.discountRate,
                            workingGenderType = x.workingGenderType.ToString(),
                            imageUrl = x.galleries.FirstOrDefault().imageUrl,
                            averageRating = x.comments.Any() ? x.comments.Average(x => x.point) : 0,
                            countRating = x.comments.Count(),
                            distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0
                        })
                        .OrderByDescending(x => x.averageRating)
                        .ThenBy(x => x.distance)
                        .Skip(page.Value * take.Value)
                        .Take(take.Value)
                        .AsNoTracking()
                        .ToListAsync();
                }

                return await context.Businesses
                    .Include(x => x.comments)
                    .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                    .Where(x => x.isActive == true && x.verified == true)
                    .Where(x => city.IsNotNullOrEmpty() ? x.city.Equals(city) : x.city != null)
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name ?? "",
                        discountRate = x.discountRate,
                        workingGenderType = x.workingGenderType.ToString(),
                        imageUrl = x.galleries.FirstOrDefault().imageUrl,
                        averageRating = x.comments.Any() ? x.comments.Average(x => x.point) : 0,
                        countRating = x.comments.Count(),
                        distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0
                    })
                    .OrderByDescending(x => x.averageRating)
                    .ThenBy(x => x.distance)
                    .AsNoTracking()
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
                        .Include(x => x.comments)
                        .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                        .Include(x => x.favorites.Where(x => x.userId == userId))
                        .Where(x => x.isActive == true && x.verified == true && x.favorites.Any(x => x.userId == userId))
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name ?? "",
                            discountRate = x.discountRate,
                            workingGenderType = x.workingGenderType.ToString(),
                            imageUrl = x.galleries.FirstOrDefault().imageUrl,
                            averageRating = x.comments.Any() ? x.comments.Average(x => x.point) : 0,
                            countRating = x.comments.Count(),
                            distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0
                        })
                        .OrderBy(x => x.distance)
                        .ThenByDescending(x => x.averageRating)
                        .Skip(page.Value * take.Value)
                        .Take(take.Value)
                        .AsNoTracking()
                        .ToListAsync();
                }

                return await context.Businesses
                    .Include(x => x.comments)
                    .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                    .Include(x => x.favorites.Where(x => x.userId == userId))
                    .Where(x => x.isActive == true && x.verified == true && x.favorites.Any(x => x.userId == userId))
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name ?? "",
                        discountRate = x.discountRate,
                        workingGenderType = x.workingGenderType.ToString(),
                        imageUrl = x.galleries.FirstOrDefault().imageUrl,
                        averageRating = x.comments.Any() ? x.comments.Average(x => x.point) : 0,
                        countRating = x.comments.Count(),
                        distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0
                    })
                    .OrderBy(x => x.distance)
                    .ThenByDescending(x => x.averageRating)
                    .AsNoTracking()
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
                        .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name ?? "",
                            discountRate = x.discountRate,
                            workingGenderType = x.workingGenderType.ToString(),
                            imageUrl = x.galleries.FirstOrDefault().imageUrl,
                            averageRating = x.comments.Any() ? x.comments.Average(x => x.point) : 0,
                            countRating = x.comments.Count(),
                            distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0
                        })
                        .OrderBy(x => x.distance)
                        .ThenByDescending(x => x.averageRating)
                        .Skip(page.Value * take.Value)
                        .Take(take.Value)
                        .AsNoTracking()
                        .ToListAsync();
                }

                return await context.Businesses
                    .Where(x => x.isActive == true && x.verified == true)
                    .Include(x => x.comments)
                    .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name,
                        discountRate = x.discountRate,
                        workingGenderType = x.workingGenderType.ToString(),
                        imageUrl = x.galleries.FirstOrDefault().imageUrl,
                        averageRating = x.comments.Any() ? x.comments.Average(x => x.point) : 0,
                        countRating = x.comments.Count(),
                        distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0
                    })
                    .OrderBy(x => x.distance)
                    .ThenByDescending(x => x.averageRating)
                    .AsNoTracking()
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
                        .AsNoTracking()
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