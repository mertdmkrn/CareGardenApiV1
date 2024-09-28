using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessAdminRepository
    {
        Task<List<BusinessAdminEarningReportData>> GetBusinessAdminEarningReportDataAsync(Guid businessId);
        Task<int> GetCustomerCountAsync(Guid businessId);
        Task<Dictionary<AppointmentStatus, int>> GetAppointmentStatusCountAsync(Guid businessId);
        Task<List<BusinessAdminWorkerReportResponseModel>> GetWorkerReportAsync(BusinessAdminReportRequestModel requestModel);
        Task<List<BusinessAdminServiceReportData>> GetServiceReportDatasAsync(BusinessAdminReportRequestModel requestModel);
        Task<BusinessAdminAppointmentReportResponseModel> GetAppointmentReportAsync(BusinessAdminReportRequestModel requestModel);
        Task<List<BusinessAdminCustomerResponseModel>> GetCustomersAsync(Guid businessId);
    }
}