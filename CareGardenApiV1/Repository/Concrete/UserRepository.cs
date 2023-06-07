using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

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

        public async Task<User> GetUserById(int id)
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
                user.role = "User";

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
    }
}