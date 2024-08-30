using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class BusinessAdminService : IBusinessAdminService
    {
        private readonly IBusinessAdminRepository _businessAdminRepository;

        public BusinessAdminService(IBusinessAdminRepository businessAdminRepository)
        {
            _businessAdminRepository = businessAdminRepository;
        }


        public async Task<BusinessAdminEarningReportResponseModel> GetBusinessAdminEarningReportDataAsync(Guid businessId)
        {
            var responseModel = new BusinessAdminEarningReportResponseModel();
    
            responseModel.dailyList = await _businessAdminRepository.GetBusinessAdminEarningReportDataAsync(businessId);

            responseModel.lastWeekEarning = responseModel.dailyList
                .Where(x => x.date >= DateTime.Today.AddDays(-7))
                .Sum(x => x.earning);
            
            var totalEarning = responseModel.dailyList.Sum(x => x.earning);

            if (totalEarning == 0 || responseModel.lastWeekEarning == 0) return responseModel;
            
            var twoWeeksAgoEarning = totalEarning - responseModel.lastWeekEarning;
            responseModel.lastWeekEarningPercentage = twoWeeksAgoEarning != 0 
                ? (responseModel.lastWeekEarning / twoWeeksAgoEarning) * 100 - 100
                : 0;

            return responseModel;
        }

        public async Task<BusinessAdminTotalDataResponseModel> GetBusinessAdminTotalDataAsync(Guid businessId)
        {
            var totalDataResponseModel = new BusinessAdminTotalDataResponseModel();

            totalDataResponseModel.customerCount = await _businessAdminRepository.GetCustomerCountAsync(businessId);

            var appointmentStatusCountDict = await _businessAdminRepository.GetAppointmentStatusCountAsync(businessId);
            
            totalDataResponseModel.activeAppointmentCount = appointmentStatusCountDict.GetValueOrDefault(AppointmentStatus.Approved);
            totalDataResponseModel.pendingAppointmentCount = appointmentStatusCountDict.GetValueOrDefault(AppointmentStatus.Pending);
            
            return totalDataResponseModel;         
        }

        public async Task<List<BusinessAdminWorkerReportResponseModel>> GetWorkerReportAsync(BusinessAdminReportRequestModel requestModel)
        {
            return await _businessAdminRepository.GetWorkerReportAsync(requestModel);
        }

        public async Task<BusinessAdminServiceReportResponseModel> GetServiceReportAsync(BusinessAdminReportRequestModel requestModel)
        {
            BusinessAdminServiceReportResponseModel serviceReportResponseModel = new();

            var serviceReportDatas = await _businessAdminRepository.GetServiceReportDatasAsync(requestModel);

            var totalAppointmentCount = serviceReportDatas.Sum(x => x.appointmentCount);

            serviceReportResponseModel.businessServiceDatas = serviceReportDatas
                .ConvertAll(x => new BusinessAdminServiceReportData
                {
                    serviceName = x.serviceName,
                    businessServiceName = x.businessServiceName,
                    appointmentCount = x.appointmentCount,
                    percentage = totalAppointmentCount > 0
                        ? (x.appointmentCount / (double)totalAppointmentCount) * 100
                        : 0
                });
            
            serviceReportResponseModel.serviceDatas = serviceReportDatas
                .GroupBy(x => x.serviceName)
                .Select(g => new BusinessAdminServiceReportData
                {
                    serviceName = g.Key,
                    appointmentCount = g.Sum(x => x.appointmentCount),
                    percentage = totalAppointmentCount > 0
                        ? (g.Sum(x => x.appointmentCount) / (double)totalAppointmentCount) * 100
                        : 0
                })
                .ToList();

            return serviceReportResponseModel;
        }

        public async Task<BusinessAdminAppointmentReportResponseModel> GetAppointmentReportAsync(BusinessAdminReportRequestModel requestModel)
        {
            return await _businessAdminRepository.GetAppointmentReportAsync(requestModel);
        }
    }
}
