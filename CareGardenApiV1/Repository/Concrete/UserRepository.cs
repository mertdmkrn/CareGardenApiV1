using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Users
                    .FirstOrDefaultAsync(x => x.email.Equals(email) && x.password.Equals(password.HashString()));
            }
        }

        public async Task<User> GetAdminByEmailAndPasswordAsync(string email, string password)
        {

            using (var context = new CareGardenApiDbContext())
            {
                return await context.Users
                    .FirstOrDefaultAsync(x => x.email.Equals(email) && x.password.Equals(password.HashString()) && x.role == "Admin");
            }
        }

        public async Task<UserResponseModel> GetUserResponseModelById(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Users
                    .Include(x => x.favorites)
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
                        favoriteBusinessList = x.favorites.Count > 0 ? x.favorites.Select(x => x.businessId.ToString()).ToHashSet() : new HashSet<string>()
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<User> GetUserById(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Users
                    .FindAsync(id);
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Users
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            using (var context = new CareGardenApiDbContext())
            {
                user.updateDate = DateTime.Now;

                if (user.password.IsNotNullOrEmpty() && user.password.Length <= 8)
                {
                    user.password = user.password.HashString();
                }

                context.Users.Update(user);
                await context.SaveChangesAsync();
                return user;
            }
        }

        public async Task<User> SaveUserAsync(User user)
        {
            using (var context = new CareGardenApiDbContext())
            {
                user.password = user.password.HashString();
                user.createDate = DateTime.Now;
                user.updateDate = user.createDate;
                user.gender = Enums.Gender.Unspecified;
                user.role = user.role.IsNull("User");

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return user;
            }
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {

            using (var context = new CareGardenApiDbContext())
            {
                return await context.Users
                    .FirstOrDefaultAsync(x => x.email.Equals(email));
            }
        }

        public async Task<User> GetUserByTelephoneNumberAsync(string telephoneNumber)
        {

            using (var context = new CareGardenApiDbContext())
            {
                return await context.Users
                    .FirstOrDefaultAsync(x => x.telephone.Equals(telephoneNumber));
            }
        }

        public async Task<List<string>> GetAdminEmailListAsync()
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Users
                    .Where(x => x.role == "Admin")
                    .Select(x => x.email)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }
    }
}