using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessAdminRepository
    {
        Task<List<BusinessAdminEarningReportData>> GetBusinessAdminEarningReportDataAsync(Guid businessId);
    }
}
