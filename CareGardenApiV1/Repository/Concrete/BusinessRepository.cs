using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;
using NetTopologySuite.Geometries;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using OneSignalApi.Model;
using CareGardenApiV1.Model.RequestModel;

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

        public async Task<IList<BusinessListModel>> GetBusinessByPopularAsync(BusinessSearchModel businessSearchModel)
        {
            using (var context = new CareGardenApiDbContext())
            {
                Point? userLocation = null;
                var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

                if (businessSearchModel.latitude.HasValue && businessSearchModel.longitude.HasValue)
                {
                    userLocation = gf.CreatePoint(new Coordinate(businessSearchModel.latitude.Value, businessSearchModel.longitude.Value));
                }

                if (businessSearchModel.page.HasValue && businessSearchModel.take.HasValue)
                {
                    return await context.Businesses
                        .Include(x => x.comments.Where(x => x.commentType == Enums.CommentType.User))
                        .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                        .Include(x => x.workingInfos)
                        .Where(x => x.isActive == true && x.verified == true)
                        .Where(x => businessSearchModel.city.IsNotNullOrEmpty() ? x.city.Equals(businessSearchModel.city) : x.city != null)
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name ?? "",
                            discountRate = x.discountRate,
                            workingGenderType = (int)x.workingGenderType,
                            imageUrl = x.galleries.FirstOrDefault().imageUrl,
                            averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == Enums.CommentType.User).Average(x => x.point) : 0,
                            countRating = x.comments.Where(x => x.commentType == Enums.CommentType.User).Count(),
                            distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0,
                            isFeatured = x.isFeatured,
                            hasPromotion = x.hasPromotion,
                            officialDayAvailable = x.officialHolidayAvailable,
                            workingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null
                        })
                        .OrderByDescending(x => x.averageRating)
                        .ThenBy(x => x.distance)
                        .Skip(businessSearchModel.page.Value * businessSearchModel.take.Value)
                        .Take(businessSearchModel.take.Value)
                        .AsNoTracking()
                        .ToListAsync();
                }

                return await context.Businesses
                    .Include(x => x.comments.Where(x => x.commentType == Enums.CommentType.User))
                    .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                    .Include(x => x.workingInfos)
                    .Where(x => x.isActive == true && x.verified == true)
                    .Where(x => businessSearchModel.city.IsNotNullOrEmpty() ? x.city.Equals(businessSearchModel.city) : x.city != null)
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name ?? "",
                        discountRate = x.discountRate,
                        workingGenderType = (int)x.workingGenderType,
                        imageUrl = x.galleries.FirstOrDefault().imageUrl,
                        averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == Enums.CommentType.User).Average(x => x.point) : 0,
                        countRating = x.comments.Where(x => x.commentType == Enums.CommentType.User).Count(),
                        distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0,
                        isFeatured = x.isFeatured,
                        hasPromotion = x.hasPromotion,
                        officialDayAvailable = x.officialHolidayAvailable,
                        workingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null
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

        public async Task<IList<BusinessListModel>> GetBusinessByUserFavorites(BusinessSearchModel businessSearchModel)
        {
            using (var context = new CareGardenApiDbContext())
            {
                Point? userLocation = null;
                var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

                if (businessSearchModel.latitude.HasValue && businessSearchModel.longitude.HasValue)
                {
                    userLocation = gf.CreatePoint(new Coordinate(businessSearchModel.latitude.Value, businessSearchModel.longitude.Value));
                }

                if (businessSearchModel.page.HasValue && businessSearchModel.take.HasValue)
                {
                    return await context.Businesses
                        .Include(x => x.comments.Where(x => x.commentType == Enums.CommentType.User))
                        .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                        .Include(x => x.favorites.Where(x => x.userId == businessSearchModel.userId))
                        .Include(x => x.workingInfos)
                        .Where(x => x.isActive == true && x.verified == true && x.favorites.Any(x => x.userId == businessSearchModel.userId))
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name ?? "",
                            discountRate = x.discountRate,
                            workingGenderType = (int)x.workingGenderType,
                            imageUrl = x.galleries.FirstOrDefault().imageUrl,
                            averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == Enums.CommentType.User).Average(x => x.point) : 0,
                            countRating = x.comments.Where(x => x.commentType == Enums.CommentType.User).Count(),
                            distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0,
                            isFeatured = x.isFeatured,
                            hasPromotion = x.hasPromotion,
                            officialDayAvailable = x.officialHolidayAvailable,
                            workingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null
                        })
                        .OrderBy(x => x.distance)
                        .ThenByDescending(x => x.averageRating)
                        .Skip(businessSearchModel.page.Value * businessSearchModel.take.Value)
                        .Take(businessSearchModel.take.Value)
                        .AsNoTracking()
                        .ToListAsync();
                }

                return await context.Businesses
                    .Include(x => x.comments.Where(x => x.commentType == Enums.CommentType.User))
                    .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                    .Include(x => x.favorites.Where(x => x.userId == businessSearchModel.userId))
                    .Include(x => x.workingInfos)
                    .Where(x => x.isActive == true && x.verified == true && x.favorites.Any(x => x.userId == businessSearchModel.userId))
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name ?? "",
                        discountRate = x.discountRate,
                        workingGenderType = (int)x.workingGenderType,
                        imageUrl = x.galleries.FirstOrDefault().imageUrl,
                        averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == Enums.CommentType.User).Average(x => x.point) : 0,
                        countRating = x.comments.Where(x => x.commentType == Enums.CommentType.User).Count(),
                        distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0,
                        isFeatured = x.isFeatured,
                        hasPromotion = x.hasPromotion,
                        officialDayAvailable = x.officialHolidayAvailable,
                        workingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null
                    })
                    .OrderBy(x => x.distance)
                    .ThenByDescending(x => x.averageRating)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<BusinessDetailModel> GetBusinessDetailByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Businesses
                    .Include(x => x.galleries)
                    .Include(x => x.services)
                    .Include(x => x.services)
                    .Include(x => x.workingInfos)
                    .Include(x => x.comments.Where(x => x.commentType == Enums.CommentType.User))
                    .Where(x => x.id == id)
                    .Select(x => new BusinessDetailModel
                    {
                        id = x.id,
                        name = x.name,
                        address = x.address,
                        telephone = x.telephone,
                        description = x.description,
                        descriptionEn = x.descriptionEn,
                        workingGenderType = x.workingGenderType,
                        latitude = x.latitude,
                        longitude = x.longitude,
                        discountRate = x.discountRate,
                        officialDayAvailable = x.officialHolidayAvailable,
                        averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == Enums.CommentType.User).Average(x => x.point) : 0,
                        countRating = x.comments.Where(x => x.commentType == Enums.CommentType.User).Count(),
                        businessWorkingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null,
                        assets = x.galleries.ToList(),
                        businessServices = x.services.ToList()
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IList<BusinessListModel>> GetBusinessNearByDistanceAsync(BusinessSearchModel businessSearchModel)
        {
            using (var context = new CareGardenApiDbContext())
            {
                Point? userLocation = null;
                var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

                if (businessSearchModel.latitude.HasValue && businessSearchModel.longitude.HasValue)
                {
                    userLocation = gf.CreatePoint(new Coordinate(businessSearchModel.latitude.Value, businessSearchModel.longitude.Value));
                }

                if (businessSearchModel.page.HasValue && businessSearchModel.take.HasValue)
                {
                    return await context.Businesses
                        .AsNoTracking()
                        .Include(x => x.comments.Where(x => x.commentType == Enums.CommentType.User))
                        .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                        .Include(x => x.workingInfos)
                        .Where(x => x.isActive == true && x.verified == true)
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name ?? "",
                            discountRate = x.discountRate,
                            workingGenderType = (int)x.workingGenderType,
                            imageUrl = x.galleries.FirstOrDefault().imageUrl,
                            averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == Enums.CommentType.User).Average(x => x.point) : 0,
                            countRating = x.comments.Where(x => x.commentType == Enums.CommentType.User).Count(),
                            distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0,
                            isFeatured = x.isFeatured,
                            hasPromotion = x.hasPromotion,
                            officialDayAvailable = x.officialHolidayAvailable,
                            workingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null
                        })
                        .OrderBy(x => x.distance)
                        .ThenByDescending(x => x.averageRating)
                        .Skip(businessSearchModel.page.Value * businessSearchModel.take.Value)
                        .Take(businessSearchModel.take.Value)
                        .ToListAsync();
                }

                return await context.Businesses
                    .AsNoTracking()
                    .Include(x => x.comments.Where(x => x.commentType == Enums.CommentType.User))
                    .Include(x => x.galleries.Where(x => x.isProfilePhoto))
                    .Include(x => x.workingInfos)
                    .Where(x => x.isActive == true && x.verified == true)
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name,
                        discountRate = x.discountRate,
                        workingGenderType = (int)x.workingGenderType,
                        imageUrl = x.galleries.FirstOrDefault().imageUrl,
                        averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == Enums.CommentType.User).Average(x => x.point) : 0,
                        countRating = x.comments.Where(x => x.commentType == Enums.CommentType.User).Count(),
                        distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0,
                        isFeatured = x.isFeatured,
                        hasPromotion = x.hasPromotion,
                        officialDayAvailable = x.officialHolidayAvailable,
                        workingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null
                    })
                    .OrderBy(x => x.distance)
                    .ThenByDescending(x => x.averageRating)
                    .ToListAsync();
            }
        }

        public async Task<List<Tuple<string, string>>> GetBusinessSelectListAsync()
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Businesses
                        .Where(x => x.isActive == true && x.verified == true)
                        .Select(x => new Tuple<string, string>(x.id.ToString(), x.name))
                        .AsNoTracking()
                        .ToListAsync();
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