using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessUserRepository
    {
        Task<BusinessUser> GetBusinessUserByEmailAndPasswordAsync(string email, string password);
        Task<BusinessUser> GetBusinessAdminByEmailAndPasswordAsync(string email, string password);
        Task<BusinessUser> GetBusinessUserByEmailAsync(string email);
        Task<BusinessUser> GetBusinessUserByTelephoneNumberAsync(string telephoneNumber);
        Task<bool> GetBusinessUserExistsByEmailAsync(string email);
        Task<bool> GetBusinessUserExistsByTelephoneNumberAsync(string telephoneNumber);
        Task<BusinessUser> SaveBusinessUserAsync(BusinessUser businessUser);
        Task<BusinessUser> UpdateBusinessUserAsync(BusinessUser businessUser, bool isPasswordChanged = false);
        Task<BusinessUser> GetBusinessUserById(Guid id);
        Task<BusinessUserResponseModel> GetBusinessUserResponseModelById(Guid id);
        Task<bool> DeleteBusinessUserAsync(BusinessUser businessUser);
        Task<bool> UpdateHasNotificationAsync(List<Guid> businessUserIds, bool value);
    }
}
