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
using CareGardenApiV1.Helpers;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using static CareGardenApiV1.Helpers.Constants;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly CareGardenApiDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public BusinessRepository(CareGardenApiDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<Business> GetBusinessByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Businesses
                .FirstOrDefaultAsync(x => x.email == email && x.password == password.HashString());
        }

        public async Task<Business> GetBusinessByEmailAsync(string email)
        {
            return await _context.Businesses
                .FirstOrDefaultAsync(x => x.email == email);
        }

        public async Task<Business> GetBusinessByIdAsync(Guid id)
        {
            return await _context.Businesses
                .FindAsync(id);
        }

        public async Task<Business> GetBusinessAllByIdAsync(Guid id)
        {
            return await _context.Businesses
                .AsNoTracking()
                .Include(x => x.workingInfos)
                .Include(x => x.services)
                .Include(x => x.galleries)
                .Include(x => x.workers)
                .Include(x => x.properties)
                .Include(x => x.discounts)
                .Where(x => x.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<BusinessListModel>> GetBusinessByPopularAsync(BusinessSearchModel businessSearchModel)
        {
            Point? userLocation = null;
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

            if (businessSearchModel.latitude.HasValue && businessSearchModel.longitude.HasValue)
            {
                userLocation = gf.CreatePoint(new Coordinate(businessSearchModel.latitude.Value, businessSearchModel.longitude.Value));
            }

            var businesses = await GetBusinessListForCache();

            if (businessSearchModel.page.HasValue && businessSearchModel.take.HasValue)
            {
                return businesses
                    .WhereIf(businessSearchModel.city.IsNotNullOrEmpty(), x => x.city.Equals(businessSearchModel.city))
                    .Select(x => 
                    {
                        x.isOpen = HelperMethods.GetBusinessOpen(x.workingInfo, x.officialDayAvailable);
                        x.distance = userLocation != null && x.location != null ? Math.Round((x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue), 1) : 0;
                        x.averageRating = Math.Round(x.averageRating, 1);
                        return x;
                    })
                    .OrderByDescending(x => x.averageRating)
                    .ThenBy(x => x.distance)
                    .Skip(businessSearchModel.page.Value * businessSearchModel.take.Value)
                    .Take(businessSearchModel.take.Value)
                    .ToList();
            }

            return businesses
                .WhereIf(businessSearchModel.city.IsNotNullOrEmpty(), x => x.city.Equals(businessSearchModel.city))
                .Select(x =>
                {
                    x.isOpen = HelperMethods.GetBusinessOpen(x.workingInfo, x.officialDayAvailable);
                    x.distance = userLocation != null && x.location != null ? Math.Round((x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue), 1) : 0;
                    x.averageRating = Math.Round(x.averageRating, 1);
                    return x;
                })
                .OrderByDescending(x => x.averageRating)
                .ThenBy(x => x.distance)
                .ToList();
        }

        public async Task<Business> GetBusinessByTelephoneNumberAsync(string telephone)
        {
            return await _context.Businesses
                .FirstOrDefaultAsync(x => x.telephone == telephone);
        }

        public async Task<IList<BusinessListModel>> GetBusinessByUserFavorites(BusinessSearchModel businessSearchModel)
        {
            Point? userLocation = null;
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

            if (businessSearchModel.latitude.HasValue && businessSearchModel.longitude.HasValue)
            {
                userLocation = gf.CreatePoint(new Coordinate(businessSearchModel.latitude.Value, businessSearchModel.longitude.Value));
            }

            var businesses = await GetBusinessListForCache();

            if (businessSearchModel.page.HasValue && businessSearchModel.take.HasValue)
            {
                return businesses
                    .Where(x => businessSearchModel.favoriteBusinessIds.Contains(x.id))
                    .Select(x =>
                    {
                        x.isOpen = HelperMethods.GetBusinessOpen(x.workingInfo, x.officialDayAvailable);
                        x.distance = userLocation != null && x.location != null ? Math.Round((x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue), 1) : 0;
                        x.averageRating = Math.Round(x.averageRating, 1);
                        return x;
                    })
                    .OrderByDescending(x => x.averageRating)
                    .ThenBy(x => x.distance)
                    .Skip(businessSearchModel.page.Value * businessSearchModel.take.Value)
                    .Take(businessSearchModel.take.Value)
                    .ToList();
            }

            return businesses
                .Where(x => businessSearchModel.favoriteBusinessIds.Contains(x.id))
                .Select(x =>
                {
                    x.isOpen = HelperMethods.GetBusinessOpen(x.workingInfo, x.officialDayAvailable);
                    x.distance = userLocation != null && x.location != null ? Math.Round((x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue), 1) : 0;
                    x.averageRating = Math.Round(x.averageRating, 1);
                    return x;
                })
                .OrderByDescending(x => x.averageRating)
                .ThenBy(x => x.distance)
                .ToList();
        }

        public async Task<IList<BusinessListModel>> ExploreBusinesses(BusinessExploreModel businessExploreModel)
        {
            Point? searchLocation = null;
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

            if (businessExploreModel.latitude.HasValue && businessExploreModel.longitude.HasValue)
            {
                searchLocation = gf.CreatePoint(new Coordinate(businessExploreModel.latitude.Value, businessExploreModel.longitude.Value));
            }

            var businesses = await GetBusinessListForCache();

            var resultCount = businesses.Count();

            var filteredBusinesses = businesses
                .WhereIf(businessExploreModel.workingGenderType != WorkingGenderType.All, x => x.workingGenderType.Equals((int)businessExploreModel.workingGenderType))
                .WhereIf(businessExploreModel.serviceId.HasValue, x => x.serviceIds != null && x.serviceIds.Contains(businessExploreModel.serviceId))
                .WhereIf(businessExploreModel.offers.HasValue && businessExploreModel.offers > 0, x => x.discounts != null && x.discounts.Exists(x => x.isActive && x.rate.Equals(businessExploreModel.offers)))
                .WhereIf(businessExploreModel.availableDate.HasValue, x => HelperMethods.IsAvailableAppointmentDay(x.appointments, x.workingInfo, x.officialDayAvailable, businessExploreModel.availableDate.Value))
                .WhereIf(!businessExploreModel.city.IsNullOrEmpty(), x => x.city.Equals(businessExploreModel.city))
                .Select(x =>
                {
                    x.isOpen = HelperMethods.GetBusinessOpen(x.workingInfo, x.officialDayAvailable);
                    x.distance = searchLocation != null && x.location != null ? Math.Round((x.location.Distance(gf.CreateGeometry(searchLocation)) * Constants.DistanceValue), 1) : 0;
                    x.averageRating = Math.Round(x.averageRating, 1);
                    x.resultCount = resultCount;
                    return x;
                })
                .WhereIf(businessExploreModel.isWithinKilometer.HasValue, x => x.distance <= businessExploreModel.isWithinKilometer.Value)
                .OrderByDescendingIf(businessExploreModel.sortByType == SortByType.Recommended, x => $"{x.isRecommended}{x.averageRating}")
                .OrderByDescendingIf(businessExploreModel.sortByType == SortByType.MostPopular, x => x.countRating)
                .OrderByDescendingIf(businessExploreModel.sortByType == SortByType.Newest, x => x.createDate)
                .OrderByDescendingIf(businessExploreModel.sortByType == SortByType.TopRated, x => x.averageRating)
                .OrderByIf(businessExploreModel.sortByType == SortByType.Nearest, x => x.distance)
                .Skip(businessExploreModel.page.Value * businessExploreModel.take.Value)
                .Take(businessExploreModel.take.Value)
                .ToList();

            return filteredBusinesses;
        }

        public async Task<IList<BusinessListModel>> GetBusinessListModelAsync(Guid? id = null)
        {
            return await _context.Businesses
                .AsNoTracking()
                .Where(x => x.isActive && x.verified)
                .WhereIf(id.HasValue, x => x.id.Equals(id))
                .Select(x => new BusinessListModel
                {
                    id = x.id,
                    name = x.name ?? "",
                    discountRate = x.discounts.Any() ? x.discounts.Where(x => x.isActive).Select(x => x.rate).FirstOrDefault() : 0,
                    workingGenderType = (int)x.workingGenderType,
                    imageUrl = x.galleries.FirstOrDefault(x => x.isProfilePhoto).imageUrl,
                    averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == CommentType.User).Average(x => x.point) : 0,
                    countRating = x.comments.Where(x => x.commentType == CommentType.User).Count(),
                    location = x.location,
                    logoUrl = x.logoUrl,
                    city = x.city,
                    isFeatured = x.isFeatured,
                    hasPromotion = x.hasPromotion,
                    officialDayAvailable = x.officialHolidayAvailable,
                    createDate = x.createDate,
                    workingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null,
                    serviceIds = x.services.Any() ? x.services.Select(x => x.serviceId).Distinct().ToList() : null,
                    discounts = x.discounts.Any() ? x.discounts.Select(x => new Discount { isActive = x.isActive, rate = x.rate }).ToList() : null,
                    appointments = x.appointments.Any() ? x.appointments.Where(x => x.startDate >= DateTime.Today.AddMonths(-2)).Select(x => new Appointment { worker = x.worker, startDate = x.startDate, endDate = x.endDate }).ToList() : null
                })
                .ToListAsync();
        }

        public async Task<IList<BusinessListModel>> GetBusinessListForCache()
        {
            IList<BusinessListModel> businessList = null;

            if (_memoryCache.TryGetValue(CacheKeys.BusinessList, out object list))
            {
                businessList = (IList<BusinessListModel>)list;
            }
            else
            {
                businessList = await GetBusinessListModelAsync();

                _memoryCache.Set(CacheKeys.BusinessList, businessList, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    Priority = CacheItemPriority.High
                });
            }

            return businessList;
        }

        public async Task<BusinessDetailModel> GetBusinessDetailByIdAsync(Guid id)
        {
            return await _context.Businesses
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
                    logoUrl = x.logoUrl,
                    latitude = x.latitude,
                    longitude = x.longitude,
                    discountRate = x.discounts.Any() ? x.discounts.Where(x => x.isActive).Select(x => x.rate).FirstOrDefault() : 0,
                    averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == CommentType.User).Average(x => x.point) : 0,
                    countRating = x.comments.Where(x => x.commentType == CommentType.User).Count(),
                    businessWorkingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null,
                    discounts = x.discounts.Where(x => x.isActive).ToList(),
                    assets = x.galleries,
                    businessServices = x.services,
                    workers = x.workers,
                    properties = x.properties
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<IList<BusinessDetailModel>> GetBusinessesAsync()
        {
            return await _context.Businesses
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
                    discountRate = x.discounts.Any() ? x.discounts.Where(x => x.isActive).Select(x => x.rate).FirstOrDefault() : 0,
                    officialDayAvailable = x.officialHolidayAvailable,
                    isFeatured = x.isFeatured,
                    hasPromotion = x.hasPromotion,
                    averageRating = x.comments.Any() ? x.comments.Where(x => x.commentType == CommentType.User).Average(x => x.point) : 0,
                    countRating = x.comments.Where(x => x.commentType == CommentType.User).Count(),
                    businessWorkingInfo = x.workingInfos.Any() ? x.workingInfos.FirstOrDefault() : null,
                    assets = x.galleries.ToList(),
                    businessServices = x.services.ToList()
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IList<BusinessListModel>> GetBusinessNearByDistanceAsync(BusinessSearchModel businessSearchModel)
        {
            Point? userLocation = null;
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

            if (businessSearchModel.latitude.HasValue && businessSearchModel.longitude.HasValue)
            {
                userLocation = gf.CreatePoint(new Coordinate(businessSearchModel.latitude.Value, businessSearchModel.longitude.Value));
            }

            var businesses = await GetBusinessListForCache();

            if (businessSearchModel.page.HasValue && businessSearchModel.take.HasValue)
            {
                return businesses
                    .Select(x =>
                    {
                        x.isOpen = HelperMethods.GetBusinessOpen(x.workingInfo, x.officialDayAvailable);
                        x.distance = userLocation != null && x.location != null ? Math.Round((x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue), 1) : 0;
                        x.averageRating = Math.Round(x.averageRating, 1);
                        return x;
                    })
                    .OrderBy(x => x.distance)
                    .ThenByDescending(x => x.averageRating)
                    .Skip(businessSearchModel.page.Value * businessSearchModel.take.Value)
                    .Take(businessSearchModel.take.Value)
                    .ToList();
            }

            return businesses
                .Select(x =>
                {
                    x.isOpen = HelperMethods.GetBusinessOpen(x.workingInfo, x.officialDayAvailable);
                    x.distance = userLocation != null && x.location != null ? Math.Round((x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue), 1) : 0;
                    x.averageRating = Math.Round(x.averageRating, 1);
                    return x;
                })
                .OrderBy(x => x.distance)
                .ThenByDescending(x => x.averageRating)
                .ToList();
        }

        public async Task<List<Tuple<string, string>>> GetBusinessSelectListAsync()
        {
            return await _context.Businesses
                    .Where(x => x.isActive == true && x.verified == true)
                    .Select(x => new Tuple<string, string>(x.id.ToString(), x.name))
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<Business> SaveBusinessAsync(Business business)
        {
            business.password = business.password.HashString();
            business.createDate = DateTime.UtcNow;
            business.updateDate = business.createDate;

            await _context.Businesses.AddAsync(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<Business> UpdateBusinessAsync(Business business)
        {
            business.updateDate = DateTime.UtcNow;

            if (business.password.IsNotNullOrEmpty() && business.password.Length <= 8)
            {
                business.password = business.password.HashString();
            }

            _context.Businesses.Update(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<List<BusinessPagingListModel>> GetBusinessLiteListAsync(BusinessSearchAdminModel searchAdminModel)
        {
            var query = _context.Businesses
                .AsNoTracking()
                .WhereIf(searchAdminModel.city.IsNotNullOrEmpty(), x => x.city == searchAdminModel.city)
                .WhereIf(searchAdminModel.name.IsNotNullOrEmpty(), x => x.name == searchAdminModel.name)
                .WhereIf(searchAdminModel.isOnlyActive, x => x.isActive == true)
                .WhereIf(searchAdminModel.isOnlyNotActive, x => x.isActive == false)
                .WhereIf((WorkingGenderType)searchAdminModel.workingGenderType != WorkingGenderType.All, x => x.workingGenderType == searchAdminModel.workingGenderType);

            var totalCount = await query.CountAsync();

            var list = await query
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

            list.ForEach(x => { x.itemCount = totalCount; });

            return list;
        }

        public async Task<bool> DeleteBusinessAsync(Business business)
        {
            _context.Businesses.Remove(business);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}