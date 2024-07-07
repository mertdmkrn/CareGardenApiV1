using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            return await _userRepository.GetUserByEmailAndPasswordAsync(email, password);
        }

        public async Task<User> GetAdminByEmailAndPasswordAsync(string email, string password)
        {
            return await _userRepository.GetAdminByEmailAndPasswordAsync(email, password);
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _userRepository.GetUserById(id);
        }     
        
        public async Task<UserResponseModel> GetUserResponseModelById(Guid id)
        {
            return await _userRepository.GetUserResponseModelById(id);
        }

        public async Task<User> SaveUserAsync(User user)
        {
            return await _userRepository.SaveUserAsync(user);
        }

        public async Task<User> UpdateUserAsync(User user, bool isPasswordChanged = false)
        {
            return await _userRepository.UpdateUserAsync(user, isPasswordChanged);
        }
        public async Task<bool> DeleteUserAsync(User user)
        {
            return await _userRepository.DeleteUserAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<List<string>> GetAdminEmailListAsync()
        {
            return await _userRepository.GetAdminEmailListAsync();
        }

        public async Task<User> GetUserByTelephoneNumberAsync(string telephoneNumber)
        {
            return await _userRepository.GetUserByTelephoneNumberAsync(telephoneNumber);
        }

        public async Task<List<UserAdminResponseModel>> GetUsersAsync(UserSearchAdminRequestModel userSearchAdminModel)
        {
            return await _userRepository.GetUsersAsync(userSearchAdminModel);
        }

        public async Task<List<Guid?>> GetUserFavoriteBusinessIds(Guid id)
        {
            return await _userRepository.GetUserFavoriteBusinessIds(id);
        }

        public async Task<List<Guid>> GetUserIds()
        {
            return await _userRepository.GetUserIds();
        }

        public async Task<bool> UpdateHasNotificationAsync(List<Guid> userIds, bool value)
        {
            return await _userRepository.UpdateHasNotificationAsync(userIds, value);
        }
    }
}
