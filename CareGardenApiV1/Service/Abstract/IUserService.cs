using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAndPasswordAsync(string email, string password);
        Task<User> GetAdminByEmailAndPasswordAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByTelephoneNumberAsync(string telephoneNumber);
        Task<bool> GetUserExistsByEmailAsync(string email);
        Task<bool> GetUserExistsByTelephoneNumberAsync(string telephoneNumber);
        Task<List<UserAdminResponseModel>> GetUsersAsync(UserSearchAdminRequestModel userSearchAdminModel);
        Task<User> SaveUserAsync(User user);
        Task<User> UpdateUserAsync(User user, bool isPasswordChanged = false);
        Task<User> GetUserById(Guid id);
        Task<UserResponseModel> GetUserResponseModelById(Guid id);
        Task<bool> DeleteUserAsync(User user);
        Task<List<string>> GetAdminEmailListAsync();
        Task<List<Guid?>> GetUserFavoriteBusinessIds(Guid id);
        Task<List<Guid>> GetUserIds();
        Task<bool> UpdateHasNotificationAsync(List<Guid> userIds, bool value);
        Task<int> GetOpenAIRequestCountAsync(Guid id);
        Task<bool> UpdateOpenAIRequestCountAsync(Guid id, int count);
    }
}
