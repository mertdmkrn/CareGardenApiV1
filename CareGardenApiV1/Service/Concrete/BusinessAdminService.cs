using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;

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
    
            var dailyList = await _businessAdminRepository.GetBusinessAdminEarningReportDataAsync(businessId);

            responseModel.dailyList = new();

            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.Today.AddDays(-i);
                var dailyItem = dailyList.FirstOrDefault(x => x.date == date);

                responseModel.dailyList.Add(new BusinessAdminEarningReportData
                {
                    date = date,
                    dayStr = date.ToString("ddd", Resource.Resource.Culture),
                    earning = dailyItem?.earning ?? 0
                });

                responseModel.lastWeekEarning += dailyItem?.earning ?? 0;
            }
            
            var totalEarning = dailyList.Sum(x => x.earning);

            if (totalEarning == 0 || responseModel.lastWeekEarning == 0) return responseModel;
            
            var twoWeeksAgoEarning = totalEarning - responseModel.lastWeekEarning;
            responseModel.lastWeekEarningPercentage = twoWeeksAgoEarning != 0
                ? Math.Round((responseModel.lastWeekEarning / twoWeeksAgoEarning) * 100 - 100, 2, MidpointRounding.ToPositiveInfinity)
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
                        ? Math.Round((x.appointmentCount / (double)totalAppointmentCount) * 100, 1, MidpointRounding.ToEven)
                        : 0
                })
                .OrderByDescending(x => x.appointmentCount)
                .ToList();
            
            serviceReportResponseModel.serviceDatas = serviceReportDatas
                .GroupBy(x => x.serviceName)
                .Select(g => new BusinessAdminServiceReportData
                {
                    serviceName = g.Key,
                    appointmentCount = g.Sum(x => x.appointmentCount),
                    percentage = totalAppointmentCount > 0
                        ? Math.Round((g.Sum(x => x.appointmentCount) / (double)totalAppointmentCount) * 100, 1, MidpointRounding.ToEven)
                        : 0
                })
                .OrderByDescending(x => x.percentage)
                .ToList();


            return serviceReportResponseModel;
        }

        public async Task<BusinessAdminAppointmentReportResponseModel> GetAppointmentReportAsync(BusinessAdminReportRequestModel requestModel)
        {
            return await _businessAdminRepository.GetAppointmentReportAsync(requestModel);
        }

        public async Task<List<BusinessAdminCustomerResponseModel>> GetCustomersAsync(Guid businessId)
        {
            return await _businessAdminRepository.GetCustomersAsync(businessId);
        }

        public async Task<List<BusinessAdminCalendarResponseModel>> GetCalendarInfosAsync(Guid businessId)
        {
            return await _businessAdminRepository.GetCalendarInfosAsync(businessId);
        }
    }
}
