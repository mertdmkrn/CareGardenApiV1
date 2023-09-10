using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;

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
        Task<BusinessDetailModel> GetBusinessDetailByIdAsync(Guid id);
        Task<IList<BusinessDetailModel>> GetBusinessesAsync();
        Task<IList<BusinessListModel>> GetBusinessNearByDistanceAsync(BusinessSearchModel businessSearchModel);
        Task<IList<BusinessListModel>> GetBusinessByUserFavorites(BusinessSearchModel businessSearchModel);
        Task<IList<BusinessListModel>> GetBusinessByPopularAsync(BusinessSearchModel businessSearchModel);
        Task<IList<BusinessListModel>> ExploreBusinesses(BusinessExproleModel businessExploreModel);
        Task<Business> SaveBusinessAsync(Business business);
        Task<Business> UpdateBusinessAsync(Business business);
        Task<bool> DeleteBusinessAsync(Business business);
    }
}
