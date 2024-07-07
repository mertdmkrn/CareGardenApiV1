using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IBusinessGalleryService
    {
        Task<BusinessGallery> GetBusinessGalleryByIdAsync(Guid id);
        Task<List<BusinessGallery>> GetBusinessGalleriesByBusinessIdAsync(Guid businessId);
        Task<BusinessGallery> SaveBusinessGalleryAsync(BusinessGallery businessGallery);
        Task<List<BusinessGallery>> SaveBusinessGalleriesAsync(List<BusinessGallery> businessGalleries);
        Task<bool> UpdateBusinessGalleryAsync(BusinessGallery businessGallery);
        Task<bool> DeleteBusinessGalleryAsync(BusinessGallery businessGallery);
        Task<bool> DeleteBusinessGalleryByBusinessIdAsync(Guid businessId);
        Task<bool> DeleteBusinessGalleryByIdAsync(Guid id);
    }
}
