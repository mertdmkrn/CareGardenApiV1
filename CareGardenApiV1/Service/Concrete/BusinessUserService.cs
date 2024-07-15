using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class BusinessUserService : IBusinessUserService
    {
        private readonly IBusinessUserRepository _businessUserRepository;

        public BusinessUserService(IBusinessUserRepository businessUserRepository)
        {
            _businessUserRepository = businessUserRepository;
        }

        public async Task<bool> DeleteBusinessUserAsync(BusinessUser businessUser)
        {
            return await _businessUserRepository.DeleteBusinessUserAsync(businessUser);
        }

        public async Task<BusinessUser> GetBusinessAdminByEmailAndPasswordAsync(string email, string password)
        {
            return await _businessUserRepository.GetBusinessAdminByEmailAndPasswordAsync(email, password);
        }

        public async Task<BusinessUser> GetBusinessUserByEmailAndPasswordAsync(string email, string password)
        {
            return await _businessUserRepository.GetBusinessUserByEmailAndPasswordAsync(email, password);
        }

        public async Task<BusinessUser> GetBusinessUserByEmailAsync(string email)
        {
            return await _businessUserRepository.GetBusinessUserByEmailAsync(email);
        }

        public async Task<BusinessUser> GetBusinessUserById(Guid id)
        {
            return await _businessUserRepository.GetBusinessUserById(id);
        }

        public async Task<BusinessUser> GetBusinessUserByTelephoneNumberAsync(string telephoneNumber)
        {
            return await _businessUserRepository.GetBusinessUserByTelephoneNumberAsync(telephoneNumber);
        }

        public async Task<bool> GetBusinessUserExistsByEmailAsync(string email)
        {
            return await _businessUserRepository.GetBusinessUserExistsByEmailAsync(email);
        }

        public async Task<bool> GetBusinessUserExistsByTelephoneNumberAsync(string telephoneNumber)
        {
            return await _businessUserRepository.GetBusinessUserExistsByTelephoneNumberAsync(telephoneNumber);
        }

        public async Task<BusinessUserResponseModel> GetBusinessUserResponseModelById(Guid id)
        {
            return await _businessUserRepository.GetBusinessUserResponseModelById(id);
        }

        public async Task<BusinessUser> SaveBusinessUserAsync(BusinessUser businessUser)
        {
            return await _businessUserRepository.SaveBusinessUserAsync(businessUser);
        }

        public async Task<BusinessUser> UpdateBusinessUserAsync(BusinessUser businessUser, bool isPasswordChanged = false)
        {
            return await _businessUserRepository.UpdateBusinessUserAsync(businessUser, isPasswordChanged);
        }

        public async Task<bool> UpdateHasNotificationAsync(List<Guid> businessUserIds, bool value)
        {
            return await _businessUserRepository.UpdateHasNotificationAsync(businessUserIds, value);
        }
    }
}
