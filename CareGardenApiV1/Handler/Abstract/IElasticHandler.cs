using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Handler.Abstract
{
    public interface IElasticHandler
    {
        Task<bool> MakeIndexBusiness();
        Task<bool> UpdateOrCreateIndexBusiness(Guid id);
        Task<bool> UpdateOrCreateIndexBusiness(BusinessDetailModel businessDetail);
        Task<bool> DeleteIndexBusiness(Guid id);
    }
}
