using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class BusinessGalleryService : IBusinessGalleryService
    {
        private IBusinessGalleryRepository _businessGalleryRepository;

        public BusinessGalleryService()
        {
            _businessGalleryRepository = new BusinessGalleryRepository();
        }

        public async Task<bool> DeleteBusinessGalleryAsync(BusinessGallery businessGallery)
        {
            return await _businessGalleryRepository.DeleteBusinessGalleryAsync(businessGallery);
        }

        public async Task<bool> DeleteBusinessGalleryByBusinessIdAsync(Guid businessId)
        {
             return await _businessGalleryRepository.DeleteBusinessGalleryByBusinessIdAsync(businessId);
        }

        public async Task<List<BusinessGallery>> GetBusinessGalleriesByBusinessIdAsync(Guid businessId)
        {
             return await _businessGalleryRepository.GetBusinessGalleriesByBusinessIdAsync(businessId);
        }

        public async Task<BusinessGallery> GetBusinessGalleryByIdAsync(Guid id)
        {
             return await _businessGalleryRepository.GetBusinessGalleryByIdAsync(id);
        }  

        public async Task<List<BusinessGallery>> SaveBusinessGalleriesAsync(List<BusinessGallery> businessGalleries)
        {
             return await _businessGalleryRepository.SaveBusinessGalleriesAsync(businessGalleries);
        }

        public async Task<BusinessGallery> SaveBusinessGalleryAsync(BusinessGallery businessGallery)
        {
             return await _businessGalleryRepository.SaveBusinessGalleryAsync(businessGallery);
        }

        public async Task<BusinessGallery> UpdateBusinessGalleryAsync(BusinessGallery businessGallery)
        {
             return await _businessGalleryRepository.UpdateBusinessGalleryAsync(businessGallery);
        }
    }
}
