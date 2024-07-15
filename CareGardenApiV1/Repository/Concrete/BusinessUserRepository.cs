using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessUserRepository : IBusinessUserRepository
    {
        private readonly CareGardenApiDbContext _context;

        public BusinessUserRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<BusinessUser> GetBusinessUserByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.BusinessUsers
                .FirstOrDefaultAsync(x => x.email.Equals(email) && x.password.Equals(password.HashString()));
        }

        public async Task<BusinessUser> GetBusinessAdminByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.BusinessUsers
                .FirstOrDefaultAsync(x => x.email.Equals(email) && x.password.Equals(password.HashString()) && x.role == "Admin");
        }

        public async Task<BusinessUser> GetBusinessUserById(Guid id)
        {
            return await _context.BusinessUsers
                .FindAsync(id);
        }

        public async Task<List<BusinessUser>> GetUsersAsync()
        {
            return await _context.BusinessUsers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BusinessUser> UpdateBusinessUserAsync(BusinessUser businessUser, bool isPasswordChanged = false)
        {
            businessUser.updateDate = DateTime.Now;

            if (isPasswordChanged)
            {
                businessUser.password = businessUser.password.HashString();
            }

            _context.BusinessUsers.Update(businessUser);
            await _context.SaveChangesAsync();
            return businessUser;
        }

        public async Task<BusinessUser> SaveBusinessUserAsync(BusinessUser businessUser)
        {
            businessUser.password = businessUser.password.HashString();
            businessUser.createDate = DateTime.Now;
            businessUser.updateDate = businessUser.createDate;
            businessUser.gender = Gender.Unspecified;
            businessUser.role = businessUser.role.IsNull("Admin");

            await _context.BusinessUsers.AddAsync(businessUser);
            await _context.SaveChangesAsync();
            return businessUser;
        }

        public async Task<bool> DeleteBusinessUserAsync(BusinessUser businessUser)
        {
            _context.BusinessUsers.Remove(businessUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessUser> GetBusinessUserByEmailAsync(string email)
        {
            return await _context.BusinessUsers
                .FirstOrDefaultAsync(x => x.email.Equals(email));
        }

        public async Task<BusinessUser> GetBusinessUserByTelephoneNumberAsync(string telephoneNumber)
        {
            return await _context.BusinessUsers
                .FirstOrDefaultAsync(x => x.telephone.Equals(telephoneNumber));
        }

        public async Task<bool> GetBusinessUserExistsByEmailAsync(string email)
        {
            return await _context.BusinessUsers
                .AsNoTracking()
                .AnyAsync(x => x.email.Equals(email));
        }

        public async Task<bool> GetBusinessUserExistsByTelephoneNumberAsync(string telephoneNumber)
        {
            return await _context.BusinessUsers
                .AsNoTracking()
                .AnyAsync(x => x.telephone.Equals(telephoneNumber));
        }

        public async Task<bool> UpdateHasNotificationAsync(List<Guid> businessUserIds, bool value)
        {
            await _context.BusinessUsers
                .Where(x => businessUserIds.Contains(x.id))
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.hasNotification, value));

            return true;
        }

        public async Task<BusinessUserResponseModel> GetBusinessUserResponseModelById(Guid id)
        {
            return await _context.BusinessUsers
                .AsNoTracking()
                .Where(x => x.id == id)
                .Select(x => new BusinessUserResponseModel
                {
                    id = x.id,
                    businessId = x.businessId,
                    fullName = x.fullName,
                    title = x.title,
                    email = x.email,
                    telephone = x.telephone,
                    imageUrl = x.imageUrl,
                    birthDate = x.birthDate,
                    gender = (int)x.gender,
                    hasNotification = x.hasNotification,
                    business = x.business
                })
                .FirstOrDefaultAsync();
        }
    }
}