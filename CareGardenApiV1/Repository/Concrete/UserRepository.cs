using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly CareGardenApiDbContext _context;

        public UserRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.email.Equals(email) && x.password.Equals(password.HashString()));
        }

        public async Task<User> GetAdminByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.email.Equals(email) && x.password.Equals(password.HashString()) && x.role == "Admin");
        }

        public async Task<UserResponseModel> GetUserResponseModelById(Guid id)
        {
            return await _context.Users
                .Where(x => x.id == id)
                .Select(x => new UserResponseModel
                {
                    id = x.id,
                    fullName = x.fullName,
                    telephone = x.telephone,
                    email = x.email,
                    city = x.city,
                    imageUrl = x.imageUrl,
                    services = x.services,
                    gender = (int)x.gender,
                    birthDate = x.birthDate,
                    latitude = x.latitude.HasValue ? x.latitude.Value : 0,
                    longitude = x.longitude.HasValue ? x.longitude.Value : 0,
                    favoriteBusinessList = x.favorites.Any() ? x.favorites.Select(x => x.businessId.ToString()).ToHashSet() : new HashSet<string>(),
                    hasNotification = x.hasNotification
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users
                .FindAsync(id);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> UpdateUserAsync(User user, bool isPasswordChanged = false)
        {
            user.updateDate = DateTime.Now;

            if (isPasswordChanged)
            {
                user.password = user.password.HashString();
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> SaveUserAsync(User user)
        {
            user.password = user.password.HashString();
            user.createDate = DateTime.Now;
            user.updateDate = user.createDate;
            user.gender = Gender.Unspecified;
            user.role = user.role.IsNull("User");

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.email.Equals(email));
        }

        public async Task<User> GetUserByTelephoneNumberAsync(string telephoneNumber)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.telephone.Equals(telephoneNumber));
        }

        public async Task<bool> GetUserExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(x => x.email.Equals(email));
        }

        public async Task<bool> GetUserExistsByTelephoneNumberAsync(string telephoneNumber)
        {
            return await _context.Users
                .AnyAsync(x => x.telephone.Equals(telephoneNumber));
        }

        public async Task<List<string>> GetAdminEmailListAsync()
        {
            return await _context.Users
                .Where(x => x.role == "Admin")
                .Select(x => x.email)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Guid>> GetUserIds()
        {
            return await _context.Users
                .AsNoTracking()
                .Select(x => x.id)
                .ToListAsync();
        }

        public async Task<List<UserAdminResponseModel>> GetUsersAsync(UserSearchAdminRequestModel userSearchAdminModel)
        {
            var query = _context.Users
            .AsNoTracking()
            .WhereIf(userSearchAdminModel.email.IsNotNullOrEmpty(), x => x.email.Equals(userSearchAdminModel.email))
            .WhereIf(userSearchAdminModel.gender != Gender.Nothing, x => x.gender.Equals(userSearchAdminModel.gender))
            .WhereIf(userSearchAdminModel.role.IsNotNullOrEmpty(), x => x.role.Equals(userSearchAdminModel.role))
            .WhereIf(userSearchAdminModel.city.IsNotNullOrEmpty(), x => x.city.Equals(userSearchAdminModel.city));

            var totalCount = await query.CountAsync();

            var list = await query
                .Select(x => new UserAdminResponseModel
                {
                    id = x.id,
                    fullName = x.fullName,
                    telephone = x.telephone,
                    email = x.email,
                    city = x.city,
                    imageUrl = x.imageUrl,
                    gender = (int)x.gender,
                    birthDate = x.birthDate,
                    createDate = x.createDate,
                    role = x.role,
                    isBan = x.isBan,
                    complains = x.complains
                })
                .OrderByDescending(x => x.createDate)
                .ThenByDescending(x => x.fullName)
                .Skip(userSearchAdminModel.page * userSearchAdminModel.take)
                .Take(userSearchAdminModel.take)
                .ToListAsync();

            list.ForEach(x => { x.itemCount = totalCount; });

            return list;
        }

        public async Task<List<Guid?>> GetUserFavoriteBusinessIds(Guid id)
        {
            return await _context.Users
                .Where(x => x.id == id)
                .SelectMany(x => x.favorites.Select(x => x.businessId))
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> UpdateHasNotificationAsync(List<Guid> userIds, bool value)
        {
            await _context.Users
                .Where(x => userIds.Contains(x.id))
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.hasNotification, value));

            return true;
        }
    }
}