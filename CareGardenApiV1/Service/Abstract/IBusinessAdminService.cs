using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IBusinessAdminService
    {
        Task<BusinessAdminEarningReportResponseModel> GetBusinessAdminEarningReportDataAsync(Guid businessId);
        Task<BusinessAdminTotalDataResponseModel> GetBusinessAdminTotalDataAsync(Guid businessId);
        Task<List<BusinessAdminWorkerReportResponseModel>> GetWorkerReportAsync(BusinessAdminReportRequestModel requestModel);
        Task<BusinessAdminServiceReportResponseModel> GetServiceReportAsync(BusinessAdminReportRequestModel requestModel);
        Task<BusinessAdminAppointmentReportResponseModel> GetAppointmentReportAsync(BusinessAdminReportRequestModel requestModel);
        Task<List<BusinessAdminCustomerResponseModel>> GetCustomersAsync(Guid businessId);
    }
}