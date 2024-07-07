using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.Extensions.Caching.Memory;
using static CareGardenApiV1.Helpers.Constants;

namespace CareGardenApiV1.Service.Concrete
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IMemoryCache _memoryCache;

        public BusinessService(IBusinessRepository businessRepository, IMemoryCache memoryCache)
        {
            _businessRepository = businessRepository;
            _memoryCache = memoryCache;
        }

        public async Task<bool> DeleteBusinessAsync(Business business)
        {
            return await _businessRepository.DeleteBusinessAsync(business);
        }

        public async Task<IList<BusinessListResponseModel>> ExploreBusinesses(BusinessExploreModel businessExploreModel)
        {
            return await _businessRepository.ExploreBusinesses(businessExploreModel);
        }

        public async Task<Business> GetBusinessByEmailAndPasswordAsync(string email, string password)
        {
            return await _businessRepository.GetBusinessByEmailAndPasswordAsync(email, password);
        }

        public async Task<Business> GetBusinessByEmailAsync(string email)
        {
            return await _businessRepository.GetBusinessByEmailAsync(email);
        }

        public async Task<Business> GetBusinessByIdAsync(Guid id)
        {
            return await _businessRepository.GetBusinessByIdAsync(id);
        }
        public async Task<Business> GetBusinessAllByIdAsync(Guid id)
        {
            return await _businessRepository.GetBusinessAllByIdAsync(id);
        }

        public async Task<IList<BusinessListResponseModel>> GetBusinessByPopularAsync(BusinessSearchRequestModel businessSearchModel)
        {
            return await _businessRepository.GetBusinessByPopularAsync(businessSearchModel);
        }

        public async Task<Business> GetBusinessByTelephoneNumberAsync(string telephone)
        {
            return await _businessRepository.GetBusinessByTelephoneNumberAsync(telephone);
        }

        public async Task<IList<BusinessListResponseModel>> GetBusinessByUserFavorites(BusinessSearchRequestModel businessSearchModel)
        {
            return await _businessRepository.GetBusinessByUserFavorites(businessSearchModel);
        }

        public async Task<BusinessDetailResponseModel> GetBusinessDetailByIdAsync(Guid id)
        {
            return await _businessRepository.GetBusinessDetailByIdAsync(id);
        }

        public async Task<IList<BusinessDetailResponseModel>> GetBusinessesAsync()
        {
            return await _businessRepository.GetBusinessesAsync();
        }

        public async Task<IList<BusinessListResponseModel>> GetBusinessNearByDistanceAsync(BusinessSearchRequestModel businessSearchModel)
        {
            return await _businessRepository.GetBusinessNearByDistanceAsync(businessSearchModel);
        }

        public async Task<List<Tuple<string, string>>> GetBusinessSelectListAsync()
        {
            return await _businessRepository.GetBusinessSelectListAsync();
        }

        public async Task<Business> SaveBusinessAsync(Business business)
        {
            return await _businessRepository.SaveBusinessAsync(business);
        }

        public async Task<Business> UpdateBusinessAsync(Business business)
        {
            return await _businessRepository.UpdateBusinessAsync(business);
        }

        public async Task<List<BusinessPagingListResponseModel>> GetBusinessLiteListAsync(BusinessSearchAdminRequestModel searchAdminModel)
        {
            return await _businessRepository.GetBusinessLiteListAsync(searchAdminModel);
        }

        public async Task<IList<BusinessListResponseModel>> GetBusinessListModelAsync(Guid? id = null)
        {
            return await _businessRepository.GetBusinessListModelAsync(id);
        }
        
        public async Task<IList<BusinessListResponseModel>> GetBusinessListForCache(bool cache = true)
        {
            return await _businessRepository.GetBusinessListForCache(cache);
        }

        public async Task<bool> UpdateMemoryBusinessList(Guid id)
        {
            var businessList = await _businessRepository.GetBusinessListForCache();
            var businesses = await _businessRepository.GetBusinessListModelAsync(id);

            var business = businesses.FirstOrDefault();

            if (business != null)
            {
                var indexOf = businessList.ToList().FindIndex(x => x.id == id);

                if (indexOf != -1 && !businessList[indexOf].Equals(business))
                {
                    businessList[indexOf] = business;
                }
                else if(indexOf == -1)
                {
                    businessList.Add(business);
                }

                _memoryCache.Set(CacheKeys.BusinessList, businessList, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    Priority = CacheItemPriority.High
                });
            }

            return true;
        }

        public async Task<List<Guid>> GetBusinessIds()
        {
            return await _businessRepository.GetBusinessIds();
        }

        public async Task<bool> UpdateHasNotificationAsync(List<Guid> businessIds, bool value)
        {
            return await _businessRepository.UpdateHasNotificationAsync(businessIds, value);
        }

        public async Task<BusinessDetailResponseModel> GetBusinessDetailByNameForUrlAsync(string nameForUrl)
        {
            return await _businessRepository.GetBusinessDetailByNameForUrlAsync(nameForUrl);
        }

        public async Task<string> GetNameForUrl(Business business)
        {
            string nameForUrl = business.name.GenerateUrlFriendlyName();
            var businesses = await GetBusinessListForCache();

            nameForUrl = businesses.Any(b => b.nameForUrl == nameForUrl)
                    ? $"{nameForUrl}-{business.city.IsNull("2").GenerateUrlFriendlyName()}"
                    : nameForUrl;

            return nameForUrl;
        }
    }
}
