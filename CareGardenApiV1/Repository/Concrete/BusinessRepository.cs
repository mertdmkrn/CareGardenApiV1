using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;
using NetTopologySuite.Geometries;
using CareGardenApiV1.Model.RequestModel;
using Microsoft.Extensions.Caching.Memory;
using static CareGardenApiV1.Helpers.Constants;
using CareGardenApiV1.Model.TableModel;

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

        public async Task<IList<BusinessListResponseModel>> GetBusinessByPopularAsync(BusinessSearchRequestModel businessSearchModel)
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

        public async Task<bool> GetBusinessExistsByTelephoneNumberAsync(string telephone)
        {
            return await _context.Businesses
                .AnyAsync(x => x.telephone.Equals(telephone));
        }

        public async Task<bool> GetBusinessExistsByEmailAsync(string email)
        {
            return await _context.Businesses
                .AnyAsync(x => x.email.Equals(email));
        }

        public async Task<IList<BusinessListResponseModel>> GetBusinessByUserFavorites(BusinessSearchRequestModel businessSearchModel)
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

        public async Task<IList<BusinessListResponseModel>> ExploreBusinesses(BusinessExploreModel businessExploreModel)
        {
            Point? searchLocation = null;
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

            if (businessExploreModel.latitude.HasValue && businessExploreModel.longitude.HasValue)
            {
                searchLocation = gf.CreatePoint(new Coordinate(businessExploreModel.latitude.Value, businessExploreModel.longitude.Value));
            }

            var businesses = await GetBusinessListForCache();

            var filteredBusinesses = businesses
                .WhereIf(businessExploreModel.workingGenderType != WorkingGenderType.All, x => x.workingGenderType.Equals((int)businessExploreModel.workingGenderType))
                .WhereIf(businessExploreModel.serviceId.HasValue, x => x.serviceIds != null && x.serviceIds.Contains(businessExploreModel.serviceId))
                .WhereIf(!businessExploreModel.offers.IsNullOrEmpty(), x => x.discounts != null && x.discounts.Exists(x => x.isActive && businessExploreModel.offers.Contains((int)x.rate)))
                .WhereIf(!businessExploreModel.favoriteBusinessIds.IsNullOrEmpty(), x => businessExploreModel.favoriteBusinessIds.Contains(x.id))
                .WhereIf(businessExploreModel.availableDate.HasValue, x => HelperMethods.IsAvailableAppointmentDay(x.appointments, x.workingInfo, x.officialDayAvailable, businessExploreModel.availableDate.Value))
                .WhereIf(!businessExploreModel.city.IsNullOrEmpty(), x => x.city.Equals(businessExploreModel.city))
                .Select(x =>
                {
                    x.isOpen = HelperMethods.GetBusinessOpen(x.workingInfo, x.officialDayAvailable);
                    x.distance = searchLocation != null && x.location != null ? Math.Round((x.location.Distance(gf.CreateGeometry(searchLocation)) * Constants.DistanceValue), 1) : 0;
                    x.averageRating = Math.Round(x.averageRating, 1);
                    return x;
                })
                .WhereIf(businessExploreModel.isWithinKilometer.HasValue, x => x.distance <= businessExploreModel.isWithinKilometer.Value)
                .OrderByDescendingIf(businessExploreModel.sortByType == SortByType.Recommended, x => $"{x.isRecommended}{x.averageRating}")
                .OrderByDescendingIf(businessExploreModel.sortByType == SortByType.MostPopular, x => x.countRating)
                .OrderByDescendingIf(businessExploreModel.sortByType == SortByType.Newest, x => x.createDate)
                .OrderByDescendingIf(businessExploreModel.sortByType == SortByType.TopRated, x => x.averageRating)
                .OrderByIf(businessExploreModel.sortByType == SortByType.Nearest || businessExploreModel.sortByType == SortByType.Favorites, x => x.distance)
                .ToList();

            var resultCount = filteredBusinesses.Count();

            return filteredBusinesses
                .Select(x =>
                {
                    x.resultCount = filteredBusinesses.Count;
                    return x;
                })
                .Skip(businessExploreModel.page.Value * businessExploreModel.take.Value)
                .Take(businessExploreModel.take.Value)
                .ToList();
        }

        public async Task<IList<BusinessListResponseModel>> GetBusinessListModelAsync(Guid? id = null)
        {
            return await _context.Businesses
                .AsNoTracking()
                .Where(x => x.isActive && x.verified)
                .WhereIf(id.HasValue, x => x.id.Equals(id))
                .Select(x => new BusinessListResponseModel
                {
                    id = x.id,
                    name = x.name ?? "",
                    nameForUrl = x.nameForUrl ?? "",

                    discountRate = x.discounts
                        .Where(d => d.isActive)
                        .Select(d => d.rate)
                        .FirstOrDefault(),

                    workingGenderType = (int)x.workingGenderType,

                    imageUrl = x.galleries
                        .Where(g => g.isProfilePhoto)
                        .Select(g => g.imageUrl)
                        .FirstOrDefault(),

                    averageRating = x.comments
                    .Where(c => c.commentType == CommentType.User)
                    .Select(c => (double?)c.point)
                    .DefaultIfEmpty()
                    .Average() ?? 0,

                    countRating = x.comments
                        .Count(c => c.commentType == CommentType.User),

                    location = x.location,
                    logoUrl = x.logoUrl,
                    city = x.city,
                    isFeatured = x.isFeatured,
                    hasPromotion = x.hasPromotion,
                    officialDayAvailable = x.officialHolidayAvailable,
                    appointmentTimeInterval = x.appointmentTimeInterval,
                    createDate = x.createDate,

                    workingInfo = x.workingInfos
                        .FirstOrDefault(),

                    serviceIds = x.services
                        .Select(s => s.serviceId)
                        .Distinct()
                        .ToList(),

                    discounts = x.discounts
                        .Where(d => d.isActive)
                        .Select(d => new Discount
                        {
                            isActive = d.isActive,
                            rate = d.rate,
                            serviceIds = d.serviceIds,
                            type = d.type
                        })
                        .ToList(),

                    appointments = x.appointments
                        .Where(a => a.startDate >= DateTime.Today.AddMonths(-2))
                        .Select(a => new Appointment
                        {
                            startDate = a.startDate,
                            endDate = a.endDate
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<IList<BusinessListResponseModel>> GetBusinessListForCache(bool cache = true)
        {
            IList<BusinessListResponseModel> businessList = null;

            if (cache && _memoryCache.TryGetValue(CacheKeys.BusinessList, out object list))
            {
                businessList = (IList<BusinessListResponseModel>)list;
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

        public async Task<BusinessDetailResponseModel> GetBusinessDetailByIdAsync(Guid id)
        {
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            var business = await _context.Businesses
                .AsNoTracking()
                .Where(x => x.id == id)
                .Select(x => new BusinessDetailResponseModel
                {
                    id = x.id,
                    name = x.name,
                    address = x.address,
                    telephone = x.telephone,

                    description = isTurkish ? x.description : (x.descriptionEn ?? x.description),

                    workingGenderType = x.workingGenderType,
                    logoUrl = x.logoUrl,
                    latitude = x.latitude,
                    longitude = x.longitude,

                    averageRating = x.comments
                    .Where(c => c.commentType == CommentType.User)
                    .Select(c => (double?)c.point)
                    .DefaultIfEmpty()
                    .Average() ?? 0,

                    countRating = x.comments
                        .Count(c => c.commentType == CommentType.User),

                    businessWorkingInfo = x.workingInfos
                        .FirstOrDefault(),

                    discounts = x.discounts
                        .Where(d => d.isActive)
                        .ToList(),

                    assets = x.galleries,

                    workers = x.workers
                        .Select(w => new WorkerDetailResponseModel
                        {
                            id = w.id,
                            name = w.name,
                            path = w.path,
                            title = isTurkish ? (w.title ?? w.titleEn) : (w.titleEn ?? w.title),
                            isActive = w.isActive
                        })
                        .ToList(),

                    properties = x.properties
                })
                .FirstOrDefaultAsync();

            if(business != null)
            {
                business.businessServices = await _context.BusinessServices
                    .AsNoTracking()
                    .Where(x => x.businessId == business.id)
                    .ToListAsync();
            }

            return business;
        }

        public async Task<BusinessDetailResponseModel> GetBusinessDetailByNameForUrlAsync(string nameForUrl)
        {
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            var business = await _context.Businesses
                .Where(x => x.nameForUrl.Equals(nameForUrl))
                .AsNoTracking()
                .Select(x => new BusinessDetailResponseModel
                {
                    id = x.id,
                    name = x.name,
                    address = x.address,
                    telephone = x.telephone,

                    description = isTurkish ? x.description : (x.descriptionEn ?? x.description),

                    workingGenderType = x.workingGenderType,
                    logoUrl = x.logoUrl,
                    latitude = x.latitude,
                    longitude = x.longitude,

                    averageRating = x.comments
                        .Where(c => c.commentType == CommentType.User)
                        .Select(c => (double?)c.point)
                        .DefaultIfEmpty()
                        .Average() ?? 0,

                    countRating = x.comments
                        .Count(c => c.commentType == CommentType.User),

                    businessWorkingInfo = x.workingInfos
                        .FirstOrDefault(),

                    discounts = x.discounts
                        .Where(d => d.isActive)
                        .ToList(),

                    properties = x.properties
                })
                .FirstOrDefaultAsync();

            if (business != null)
            {
                business.businessServices = await _context.BusinessServices
                    .AsNoTracking()
                    .Where(x => x.businessId == business.id)
                    .ToListAsync();
            }

            return business;
        }

        public async Task<IList<BusinessDetailResponseModel>> GetBusinessesAsync()
        {
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            return await _context.Businesses
                .Select(x => new BusinessDetailResponseModel
                {
                    id = x.id,
                    name = x.name,
                    address = x.address,
                    telephone = x.telephone,

                    description = isTurkish ? x.description : (x.descriptionEn ?? x.description),

                    workingGenderType = x.workingGenderType,
                    latitude = x.latitude,
                    longitude = x.longitude,
                    officialDayAvailable = x.officialHolidayAvailable,
                    isFeatured = x.isFeatured,
                    hasPromotion = x.hasPromotion,

                    discountRate = x.discounts
                        .Where(d => d.isActive)
                        .Select(d => d.rate)
                        .FirstOrDefault(),

                    averageRating = x.comments
                        .Where(c => c.commentType == CommentType.User)
                        .Select(c => (double?)c.point)
                        .DefaultIfEmpty()
                        .Average() ?? 0,

                    countRating = x.comments
                        .Count(c => c.commentType == CommentType.User),

                    businessWorkingInfo = x.workingInfos
                        .FirstOrDefault(),

                    assets = x.galleries,
                    businessServices = x.services
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IList<BusinessListResponseModel>> GetBusinessNearByDistanceAsync(BusinessSearchRequestModel businessSearchModel)
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
            business.createDate = DateTime.Now;
            business.updateDate = business.createDate;

            await _context.Businesses.AddAsync(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<Business> UpdateBusinessAsync(Business business)
        {
            business.updateDate = DateTime.Now;

            _context.Businesses.Update(business);
            await _context.SaveChangesAsync();
            return business;
        }

        public async Task<List<BusinessPagingListResponseModel>> GetBusinessLiteListAsync(BusinessSearchAdminRequestModel searchAdminModel)
        {
            var query = _context.Businesses
                .AsNoTracking()
                .WhereIf(searchAdminModel.city.IsNotNullOrEmpty(), x => x.city == searchAdminModel.city)
                .WhereIf(searchAdminModel.name.IsNotNullOrEmpty(), x => x.name.ToLower().StartsWith(searchAdminModel.name.ToLower()))
                .WhereIf(searchAdminModel.isOnlyActive, x => x.isActive == true)
                .WhereIf(searchAdminModel.isOnlyNotActive, x => x.isActive == false)
                .WhereIf((WorkingGenderType)searchAdminModel.workingGenderType != WorkingGenderType.All, x => x.workingGenderType == searchAdminModel.workingGenderType);

            var totalCount = await query.CountAsync();

            var list = await query
                .Select(x => new BusinessPagingListResponseModel
                {
                    id = x.id,
                    name = x.name,
                    nameForUrl = x.nameForUrl,
                    city = x.city,
                    province = x.province,
                    email = x.email,
                    logoUrl = x.logoUrl,
                    createDate = x.createDate,
                    isActive = x.isActive,
                    workingGenderType = x.workingGenderType
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


        public async Task<List<Guid>> GetBusinessIds()
        {
            return await _context.Businesses
                .AsNoTracking()
                .Select(x => x.id)
                .ToListAsync();
        }

        public async Task<bool> UpdateHasNotificationAsync(List<Guid> businessIds, bool value)
        {
            await _context.Businesses
                .Where(x => businessIds.Contains(x.id))
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.hasNotification, value));

            return true;
        }
    }
}