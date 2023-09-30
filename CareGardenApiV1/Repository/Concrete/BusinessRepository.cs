using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;
using NetTopologySuite.Geometries;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using OneSignalApi.Model;
using CareGardenApiV1.Model.RequestModel;
using Nest;
using static CareGardenApiV1.Helpers.Enums;
using System.Linq;

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

        public async Task<Business> GetBusinessAllByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Businesses
                    .AsNoTracking()
                    .Include(x => x.workingInfos)
                    .Include(x => x.services)
                    .Include(x => x.galleries)
                    .Include(x => x.workers)
                    .Include(x => x.properties)
                    .Where(x => x.id == id)
                    .FirstOrDefaultAsync();
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
                        .Where(x => x.isActive == true && x.verified == true)
                        .WhereIf(businessSearchModel.city.IsNotNullOrEmpty(), x => x.city.Equals(businessSearchModel.city))
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
                    .Where(x => x.isActive == true && x.verified == true)
                    .WhereIf(businessSearchModel.city.IsNotNullOrEmpty(), x => x.city.Equals(businessSearchModel.city))
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
                        .Where(x => x.isActive && x.verified)
                        .Where(x => x.favorites.Any(x => x.userId == businessSearchModel.userId))
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name ?? "",
                            discountRate = x.discountRate,
                            workingGenderType = (int)x.workingGenderType,
                            imageUrl = x.galleries.Any() ? x.galleries.FirstOrDefault(x => x.isProfilePhoto).imageUrl : null,
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
                    .Where(x => x.isActive && x.verified)
                    .Where(x => x.favorites.Any(x => x.userId == businessSearchModel.userId))
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

        public async Task<IList<BusinessListModel>> ExploreBusinesses(BusinessExproleModel businessExploreModel)
        {
            using (var context = new CareGardenApiDbContext())
            {
                IList<BusinessListModel> filteredBusinesses = null;
                Point? searchLocation = null;
                var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

                if (businessExploreModel.latitude.HasValue && businessExploreModel.longitude.HasValue)
                {
                    searchLocation = gf.CreatePoint(new Coordinate(businessExploreModel.latitude.Value, businessExploreModel.longitude.Value));
                }

                var businessListQueryable = context.Businesses
                    .Where(x => x.isActive && x.verified)
                    .WhereIf((WorkingGenderType)businessExploreModel.workingGenderType != Enums.WorkingGenderType.All, x => x.workingGenderType == businessExploreModel.workingGenderType)
                    .WhereIf(businessExploreModel.serviceId.HasValue, x => x.services.Any(x => x.serviceId == businessExploreModel.serviceId))
                    .WhereIf(businessExploreModel.offers > 0, x => x.discountRate == businessExploreModel.offers)
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name ?? "",
                        discountRate = x.discountRate,
                        workingGenderType = (int)x.workingGenderType,
                        imageUrl = x.galleries.FirstOrDefault().imageUrl,
                        averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == Enums.CommentType.User).Average(x => x.point) : 0,
                        countRating = x.comments.Where(x => x.commentType == Enums.CommentType.User).Count(),
                        distance = searchLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(searchLocation)) * Constants.DistanceValue : 0,
                        isFeatured = x.isFeatured,
                        hasPromotion = x.hasPromotion,
                        officialDayAvailable = x.officialHolidayAvailable,
                        createDate = x.createDate,
                        workingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null,
                        appointmentPeopleCount = x.appointmentPeopleCount,
                        appointmentTimeInterval = x.appointmentTimeInterval,
                        appointments = businessExploreModel.availableDate.HasValue && x.appointments.Any() ? x.appointments.Where(x => x.date == businessExploreModel.availableDate.Value).ToList() : null
                    });


                switch (businessExploreModel.sortByType)
                {
                    case Enums.SortByType.Recommended:
                        businessListQueryable = businessListQueryable.OrderByDescending(x => x.appointments.Count());
                        break;
                    case Enums.SortByType.MostPopular:
                        businessListQueryable = businessListQueryable.OrderByDescending(x => x.countRating);
                        break;
                    case Enums.SortByType.Newest:
                        businessListQueryable = businessListQueryable.OrderByDescending(x => x.createDate);
                        break;
                    case Enums.SortByType.TopRated:
                        businessListQueryable = businessListQueryable.OrderByDescending(x => x.averageRating);
                        break;
                    case Enums.SortByType.Nearest:
                        businessListQueryable = businessListQueryable.OrderBy(x => x.distance);
                        break;
                    default:
                        businessListQueryable = businessListQueryable.OrderBy(x => x.distance);
                        break;
                }

                filteredBusinesses = await businessListQueryable
                            .AsNoTracking()
                            .ToListAsync();

                filteredBusinesses.ToList().ForEach(x =>
                {
                    if (businessExploreModel.isWithinKilometer > 0 && x.distance > businessExploreModel.isWithinKilometer)
                    {
                        filteredBusinesses.Remove(x);
                        return;
                    }

                    if (businessExploreModel.availableDate.HasValue)
                    {
                        bool isOpen = HelperMethods.GetBusinessOpenSpecialDate(x.workingInfo, x.officialDayAvailable, businessExploreModel.availableDate);

                        if (!isOpen)
                        {
                            filteredBusinesses.Remove(x);
                            return;
                        }

                        var dailyAppointmentCount = HelperMethods.GetDailyAppointmentCount(x.appointments, x.workingInfo, x.officialDayAvailable, businessExploreModel.availableDate.Value, x.appointmentPeopleCount, x.appointmentTimeInterval);

                        if (x.appointments != null && x.appointments.Count() >= dailyAppointmentCount)
                        {
                            filteredBusinesses.Remove(x);
                            return;
                        }
                    }

                    x.isOpen = HelperMethods.GetBusinessOpen(x.workingInfo, x.officialDayAvailable);
                    x.distance = Math.Round(x.distance, 1);
                });

                return businessExploreModel.page.HasValue && businessExploreModel.take.HasValue
                        ? filteredBusinesses
                            .Skip(businessExploreModel.page.Value * businessExploreModel.take.Value)
                            .Take(businessExploreModel.take.Value)
                            .ToList()
                        : filteredBusinesses;
            }
        }

        public async Task<BusinessDetailModel> GetBusinessDetailByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Businesses
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
                        assets = x.galleries,
                        businessServices = x.services,
                        workers = x.workers,
                        properties = x.properties
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IList<BusinessDetailModel>> GetBusinessesAsync()
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Businesses
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
                        isFeatured = x.isFeatured,
                        hasPromotion = x.hasPromotion,
                        averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == Enums.CommentType.User).Average(x => x.point) : 0,
                        countRating = x.comments.Where(x => x.commentType == Enums.CommentType.User).Count(),
                        businessWorkingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null,
                        assets = x.galleries.ToList(),
                        businessServices = x.services.ToList()
                    })
                    .AsNoTracking()
                    .ToListAsync();
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
                        .Where(x => x.isActive == true && x.verified == true)
                        .Select(x => new BusinessListModel
                        {
                            id = x.id,
                            name = x.name ?? "",
                            discountRate = x.discountRate,
                            workingGenderType = (int)x.workingGenderType,
                            imageUrl = x.galleries.Any() ? x.galleries.FirstOrDefault(x => x.isProfilePhoto).imageUrl : null,
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
                    .Where(x => x.isActive == true && x.verified == true)
                    .Select(x => new BusinessListModel
                    {
                        id = x.id,
                        name = x.name,
                        discountRate = x.discountRate,
                        workingGenderType = (int)x.workingGenderType,
                        imageUrl = x.galleries.Any() ? x.galleries.FirstOrDefault(x => x.isProfilePhoto).imageUrl : null,
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
                business.createDate = DateTime.UtcNow;
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
                business.updateDate = DateTime.UtcNow;

                if (business.password.IsNotNullOrEmpty() && business.password.Length <= 8)
                {
                    business.password = business.password.HashString();
                }

                context.Businesses.Update(business);
                await context.SaveChangesAsync();
                return business;
            }
        }

        public async Task<List<BusinessPagingListModel>> GetBusinessLiteListAsync(BusinessSearchAdminModel searchAdminModel)
        {
            using (var context = new CareGardenApiDbContext())
            {
                var list = await context.Businesses
                    .AsNoTracking()
                    .WhereIf(searchAdminModel.city.IsNotNullOrEmpty(), x => x.city == searchAdminModel.city)
                    .WhereIf(searchAdminModel.name.IsNotNullOrEmpty(), x => x.name == searchAdminModel.name)
                    .WhereIf(searchAdminModel.isOnlyActive, x => x.isActive == true)
                    .WhereIf(searchAdminModel.isOnlyNotActive, x => x.isActive == false)
                    .WhereIf((WorkingGenderType)searchAdminModel.workingGenderType != Enums.WorkingGenderType.All, x => x.workingGenderType == searchAdminModel.workingGenderType)
                    .Select(x => new BusinessPagingListModel
                    {
                        id = x.id,
                        name = x.name,
                        city = x.city,
                        province = x.province,
                        createDate = x.createDate,
                        isActive = x.isActive,
                        workingGenderType = x.workingGenderType,
                    })
                    .OrderByDescending(x => x.createDate)
                    .ThenByDescending(x => x.name)
                    .Skip(searchAdminModel.page * searchAdminModel.take)
                    .Take(searchAdminModel.take)
                    .ToListAsync();

                var pageCount = list.Count;

                list.ForEach(x => { x.itemCount = pageCount; });

                return list;
            }

        }
    }
}