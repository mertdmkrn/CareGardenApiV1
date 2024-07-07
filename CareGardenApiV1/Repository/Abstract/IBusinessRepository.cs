using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessRepository
    {
        Task<Business> GetBusinessByIdAsync(Guid id);
        Task<Business> GetBusinessAllByIdAsync(Guid id);
        Task<List<Tuple<string, string>>> GetBusinessSelectListAsync();
        Task<Business> GetBusinessByEmailAndPasswordAsync(string email, string password);
        Task<Business> GetBusinessByEmailAsync(string email);
        Task<Business> GetBusinessByTelephoneNumberAsync(string telephone);
        Task<List<BusinessPagingListResponseModel>> GetBusinessLiteListAsync(BusinessSearchAdminRequestModel searchAdminModel);
        Task<BusinessDetailResponseModel> GetBusinessDetailByIdAsync(Guid id);
        Task<BusinessDetailResponseModel> GetBusinessDetailByNameForUrlAsync(string nameForUrl);
        Task<IList<BusinessListResponseModel>> GetBusinessListModelAsync(Guid? id = null);
        Task<IList<BusinessListResponseModel>> GetBusinessListForCache(bool cache = true);
        Task<IList<BusinessDetailResponseModel>> GetBusinessesAsync();
        Task<IList<BusinessListResponseModel>> GetBusinessNearByDistanceAsync(BusinessSearchRequestModel businessSearchModel);
        Task<IList<BusinessListResponseModel>> GetBusinessByUserFavorites(BusinessSearchRequestModel businessSearchModel);
        Task<IList<BusinessListResponseModel>> GetBusinessByPopularAsync(BusinessSearchRequestModel businessSearchModel);
        Task<IList<BusinessListResponseModel>> ExploreBusinesses(BusinessExploreModel businessExploreModel);
        Task<Business> SaveBusinessAsync(Business business);
        Task<Business> UpdateBusinessAsync(Business business);
        Task<bool> DeleteBusinessAsync(Business business);
        Task<List<Guid>> GetBusinessIds();
        Task<bool> UpdateHasNotificationAsync(List<Guid> businessIds, bool value);
    }
}
