using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<bool> DeleteDiscountAsync(Discount discount)
        {
            return await _discountRepository.DeleteDiscountAsync(discount);
        }

        public async Task<List<Discount>> GetActiveDiscountsByBusinessIdAsync(Guid? businessId)
        {
            return await _discountRepository.GetActiveDiscountsByBusinessIdAsync(businessId);
        }

        public async Task<Discount> GetDiscountByIdAsync(Guid id)
        {
            return await _discountRepository.GetDiscountByIdAsync(id);
        }

        public async Task<List<Discount>> GetDiscountsByBusinessIdAsync(Guid? businessId)
        {
            return await _discountRepository.GetDiscountsByBusinessIdAsync(businessId);
        }

        public async Task<Discount> SaveDiscountAsync(Discount discount)
        {
            return await _discountRepository.SaveDiscountAsync(discount);
        }

        public async Task<Discount> UpdateDiscountAsync(Discount discount)
        {
            return await _discountRepository.UpdateDiscountAsync(discount);
        }
    }
}
