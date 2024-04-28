using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.RequestModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class AppointmentDetailRepository : IAppointmentDetailRepository
    {
        private readonly CareGardenApiDbContext _context;

        public AppointmentDetailRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentDetail> GetAppointmentDetailByIdAsync(Guid id)
        {
            return await _context.AppointmentDetails
                .FindAsync(id);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentDetailsByAppointmentDetailSearchModelAsync(AppointmentSearchModel searchModel)
        {
            if (searchModel.page.HasValue && searchModel.take.HasValue)
            {
                return await _context.AppointmentDetails
                .AsNoTracking()
                    .WhereIf(searchModel.appointmentId.IsNotNullOrEmpty(), x => x.appointmentId.Equals(searchModel.appointmentId))
                    .WhereIf(searchModel.workerId.IsNotNullOrEmpty(), x => x.workerId.Equals(searchModel.workerId))
                    .WhereIf(searchModel.businessServiceId.IsNotNullOrEmpty(), x => x.businessServiceId.Equals(searchModel.businessServiceId))
                    .WhereIf(searchModel.startDate.HasValue, x => x.date >= searchModel.startDate)
                    .WhereIf(searchModel.endDate.HasValue, x => x.date < searchModel.endDate)
                    .Skip(searchModel.page.Value * searchModel.take.Value)
                    .Take(searchModel.take.Value)
                    .ToListAsync();
            }
            else
            {
                return await _context.AppointmentDetails
                    .AsNoTracking()
                    .WhereIf(searchModel.appointmentId.IsNotNullOrEmpty(), x => x.appointmentId.Equals(searchModel.appointmentId))
                    .WhereIf(searchModel.workerId.IsNotNullOrEmpty(), x => x.workerId.Equals(searchModel.workerId))
                    .WhereIf(searchModel.businessServiceId.IsNotNullOrEmpty(), x => x.businessServiceId.Equals(searchModel.businessServiceId))
                    .WhereIf(searchModel.startDate.HasValue, x => x.date >= searchModel.startDate)
                    .WhereIf(searchModel.endDate.HasValue, x => x.date < searchModel.endDate)
                    .ToListAsync();
            }
        }

        public async Task<AppointmentDetail> SaveAppointmentDetailAsync(AppointmentDetail appointmentDetail)
        {
            await _context.AppointmentDetails.AddAsync(appointmentDetail);
            await _context.SaveChangesAsync();
            return appointmentDetail;

        }

        public Task<AppointmentDetail> UpdateAppointmentDetailAsync(AppointmentDetail appointmentDetail)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAppointmentDetailAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}