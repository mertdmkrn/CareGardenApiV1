using CareGardenApiV1.Model;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IDiscountService
    {
        Task<Discount> GetDiscountByIdAsync(Guid id);
        Task<List<Discount>> GetDiscountsByBusinessIdAsync(Guid? businessId);
        Task<List<Discount>> GetActiveDiscountsByBusinessIdAsync(Guid? businessId);
        Task<Discount> SaveDiscountAsync(Discount discount);
        Task<Discount> UpdateDiscountAsync(Discount discount);
        Task<bool> DeleteDiscountAsync(Discount discount);
    }
}
