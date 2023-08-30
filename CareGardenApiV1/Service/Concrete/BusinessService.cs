using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nest;
using OneSignalApi.Model;

namespace CareGardenApiV1.Service.Concrete
{
    public class BusinessService : IBusinessService
    {
        private IBusinessRepository _businessRepository;

        public BusinessService()
        {
            _businessRepository = new BusinessRepository();
        }

        public async Task<bool> DeleteBusinessAsync(Business business)
        {
            return await _businessRepository.DeleteBusinessAsync(business);
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

        public async Task<IList<BusinessListModel>> GetBusinessByPopularAsync(BusinessSearchModel businessSearchModel)
        {
            return await _businessRepository.GetBusinessByPopularAsync(businessSearchModel);
        }

        public async Task<Business> GetBusinessByTelephoneNumberAsync(string telephone)
        {
            return await _businessRepository.GetBusinessByTelephoneNumberAsync(telephone);
        }

        public async Task<IList<BusinessListModel>> GetBusinessByUserFavorites(BusinessSearchModel businessSearchModel)
        {
            return await _businessRepository.GetBusinessByUserFavorites(businessSearchModel);
        }

        public async Task<BusinessDetailModel> GetBusinessDetailByIdAsync(Guid id)
        {
            return await _businessRepository.GetBusinessDetailByIdAsync(id);
        }

        public async Task<IList<BusinessDetailModel>> GetBusinessesAsync()
        {
            return await _businessRepository.GetBusinessesAsync();
        }

        public async Task<IList<BusinessListModel>> GetBusinessNearByDistanceAsync(BusinessSearchModel businessSearchModel)
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
    }
}
