using CareGardenApiV1.Model;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAndPasswordAsync(string email, string password);
        Task<User> GetAdminByEmailAndPasswordAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByTelephoneNumberAsync(string telephoneNumber);
        Task<List<User>> GetUsersAsync();
        Task<User> SaveUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<User> GetUserById(Guid id);
        Task<UserResponseModel> GetUserResponseModelById(Guid id);
        Task<bool> DeleteUserAsync(User user);
        Task<List<string>> GetAdminEmailListAsync();
    }
}
