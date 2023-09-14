using CareGardenApiV1.Model;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessGalleryService
    {
        Task<BusinessGallery> GetBusinessGalleryByIdAsync(Guid id);
        Task<List<BusinessGallery>> GetBusinessGalleriesByBusinessIdAsync(Guid businessId);
        Task<BusinessGallery> SaveBusinessGalleryAsync(BusinessGallery businessGallery);
        Task<List<BusinessGallery>> SaveBusinessGalleriesAsync(List<BusinessGallery> businessGalleries);
        Task<BusinessGallery> UpdateBusinessGalleryAsync(BusinessGallery businessGallery);
        Task<bool> DeleteBusinessGalleryAsync(BusinessGallery businessGallery);
        Task<bool> DeleteBusinessGalleryByBusinessIdAsync(Guid businessId);
        Task<bool> DeleteBusinessGalleryByIdAsync(Guid id);
    }
}
