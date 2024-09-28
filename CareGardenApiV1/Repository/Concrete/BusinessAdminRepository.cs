using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class BusinessAdminRepository : IBusinessAdminRepository
    {
        private readonly CareGardenApiDbContext _context;

        public BusinessAdminRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<BusinessAdminEarningReportData>> GetBusinessAdminEarningReportDataAsync(Guid businessId)
        {
            return await _context.BusinessPayments
                .AsNoTracking()
                .Where(x => x.businessId == businessId && x.date >= DateTime.Today.AddDays(-14))
                .GroupBy(x => x.date.Date)
                .Select(g => new BusinessAdminEarningReportData
                {
                    date = g.Key,
                    earning = g.Sum(a => a.amount)
                })
                .ToListAsync();
        }

        public async Task<int> GetCustomerCountAsync(Guid businessId)
        {
            return await _context.Appointments
                .AsNoTracking()
                .Where(x => x.businessId == businessId)
                .GroupBy(x => new { x.userId, x.userTelephone })
                .Select(g => g.Key)
                .CountAsync();
        }

        public async Task<Dictionary<AppointmentStatus, int>> GetAppointmentStatusCountAsync(Guid businessId)
        {
            var statuses = new[] { AppointmentStatus.Pending, AppointmentStatus.Approved };

            return await _context.Appointments
                .AsNoTracking()
                .Where(x => x.businessId == businessId)
                .Where(x => statuses.Contains(x.status))
                .GroupBy(x => x.status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }
        
        public async Task<List<BusinessAdminWorkerReportResponseModel>> GetWorkerReportAsync(BusinessAdminReportRequestModel requestModel)
        {
            return await _context.AppointmentDetails
                .AsNoTracking()
                .Where(x => x.appointment.businessId == requestModel.businessId)
                .Where(x => x.appointment.status == AppointmentStatus.Completed)
                .Where(x => x.date >= requestModel.startDate && x.date <= requestModel.endDate)
                .GroupBy(x => new { x.worker.name, x.worker.path, x.worker.title })
                .Select(g => new BusinessAdminWorkerReportResponseModel
                {
                    workerName = g.Key.name,
                    workerImageUrl = g.Key.path,
                    workerTitle = g.Key.title,
                    appointmentCount = g.Count(),
                    totalEarning = g.Sum(a => a.price)
                })
                .OrderByDescending(x => x.appointmentCount)
                .ThenByDescending(x => x.totalEarning)
                .ThenBy(x => x.workerName)
                .Skip(requestModel.page * requestModel.take)
                .Take(requestModel.take)
                .ToListAsync();
        }

        public async Task<List<BusinessAdminServiceReportData>> GetServiceReportDatasAsync(BusinessAdminReportRequestModel requestModel)
        {
            var isTurkish = Resource.Resource.Culture.ToString().Contains("tr");

            return await _context.AppointmentDetails
                .AsNoTracking()
                .Where(x => x.appointment.businessId == requestModel.businessId)
                .Where(x => x.appointment.status == AppointmentStatus.Completed)
                .Where(x => x.date >= requestModel.startDate && x.date <= requestModel.endDate)
                .GroupBy(x => new
                {
                    businessServiceName = (isTurkish ? x.businessService.name : x.businessService.nameEn),
                    serviceName = (isTurkish
                        ? x.businessService.service.name
                        : x.businessService.service.nameEn)
                })
                .Select(g => new BusinessAdminServiceReportData
                {
                    businessServiceName = g.Key.businessServiceName,
                    serviceName = g.Key.serviceName,
                    appointmentCount = g.Count()
                })
                .OrderByDescending(x => x.appointmentCount)
                .ThenBy(x => x.serviceName)
                .ThenBy(x => x.businessServiceName)
                .ToListAsync();
        }

        public async Task<BusinessAdminAppointmentReportResponseModel> GetAppointmentReportAsync(BusinessAdminReportRequestModel requestModel)
        {
            var query = _context.Appointments
                .AsNoTracking()
                .Where(x => x.businessId == requestModel.businessId)
                .Where(x => x.status == AppointmentStatus.Approved)
                .Where(x => x.startDate >= requestModel.startDate && x.startDate <= requestModel.endDate);

            var totalItems = await query.CountAsync();

            var appointments = await query

                .Select(x => new BusinessAdminAppointmentReportInfo
                {
                    date = x.startDate,
                    hour = x.startDate.HasValue ? x.startDate.Value.ToString("HH:mm", new System.Globalization.CultureInfo("tr-TR")) : "",
                    totalDuration = x.startDate.DifferenceBetweenDates(x.endDate, DateType.Minute).FormatDuration(),
                    userTelephone = x.user != null ? x.user.telephone : x.userTelephone,
                    userName = x.user != null ? x.user.fullName : x.userName,
                    userImageUrl = x.user != null ? x.user.imageUrl : null,
                    workerInfos = x.details.Select(d => new BusinessAdminAppointmentReportWorkerInfo
                    {
                        imageUrl = d.worker.path,
                        name = d.worker.name
                    }).ToList()
                })
                .OrderBy(x => x.date)
                .Skip(requestModel.page * requestModel.take)
                .Take(requestModel.take)
                .ToListAsync();

            return new BusinessAdminAppointmentReportResponseModel
            {
                reportInfos = appointments,
                itemCount = totalItems
            };
        }

        public async Task<List<BusinessAdminCustomerResponseModel>> GetCustomersAsync(Guid businessId)
        {
            var appointmentCustomersTask = _context.Appointments
                .AsNoTracking()
                .Where(x => x.businessId == businessId)
                .Where(x => x.status != AppointmentStatus.Cancelled && x.status != AppointmentStatus.Pending)
                .GroupBy(x => new { x.userId, x.userTelephone, x.userEmail, x.user.imageUrl })
                .Select(g => new BusinessAdminCustomerResponseModel
                {
                    name = g.Key.userId != null ? g.FirstOrDefault().user.telephone : g.Key.userTelephone,
                    email = g.Key.userId != null ? g.FirstOrDefault().user.email : g.Key.userEmail,
                    imageUrl = g.Key.userId != null ? g.FirstOrDefault().user.imageUrl : null,
                    createDate = g.Key.userId != null ? g.FirstOrDefault().user.createDate : g.Min(x => x.createDate),
                    lastAppointmentDate = g.Max(x => x.startDate),
                    totalSpent = Math.Round(g.Sum(x => x.totalDiscountPrice), 2),
                    appointmentCount = g.Count(),
                })
                .ToListAsync();

            var businessCustomersTask = _context.BusinessCustomers
                .AsNoTracking()
                .Where(x => x.businessId == businessId)
                .Select(x => new BusinessAdminCustomerResponseModel
                {
                    name = x.fullName,
                    email = x.email,
                    imageUrl = x.imageUrl,
                    createDate = x.createDate,
                    isBusinessCustomer = true
                })
                .ToListAsync();

            await Task.WhenAll(appointmentCustomersTask, businessCustomersTask);

            var appointmentCustomers = await appointmentCustomersTask;
            var businessCustomers = await businessCustomersTask;

            var combinedCustomers = appointmentCustomers
                .Union(businessCustomers.Where(x => !appointmentCustomers.Any(a => a.email == x.email)))
                .OrderBy(x => x.appointmentCount)
                .ToList();

            return combinedCustomers;
        }
    }
}