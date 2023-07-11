using CareGardenApiV1.Model;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessService
    {
        Task<Business> GetBusinessByIdAsync(Guid id);
        Task<Dictionary<string, string>> GetBusinessSelectListAsync();
        Task<Business> GetBusinessByEmailAndPasswordAsync(string email, string password);
        Task<Business> GetBusinessByEmailAsync(string email);
        Task<Business> GetBusinessByTelephoneNumberAsync(string telephone);
        Task<IList<BusinessListModel>> GetBusinessNearByDistanceAsync(double? latitude, double? longitude, int? page, int? take);
        Task<IList<BusinessListModel>> GetBusinessByUserFavorites(double? latitude, double? longitude, Guid userId, int? page, int? take);
        Task<IList<BusinessListModel>> GetBusinessByPopularAsync(double? latitude, double? longitude, string? city, int? page, int? take);
        Task<Business> SaveBusinessAsync(Business business);
        Task<Business> UpdateBusinessAsync(Business business);
        Task<bool> DeleteBusinessAsync(Business business);
    }
}
