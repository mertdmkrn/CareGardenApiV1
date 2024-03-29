using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.RequestModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly CareGardenApiDbContext _context;

        public AppointmentRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
        {
            return await _context.Appointments
                .FindAsync(id);

        }

        public async Task<List<Appointment>> GetAppointmentsByAppointmentSearchModelAsync(AppointmentSearchModel searchModel)
        {
            if (searchModel.page.HasValue && searchModel.take.HasValue)
            {
                return await _context.Appointments
                .AsNoTracking()
                    .WhereIf(searchModel.businessId.IsNotNullOrEmpty(), x => x.businessId.Equals(searchModel.businessId))
                    .WhereIf(searchModel.userId.IsNotNullOrEmpty(), x => x.userId.Equals(searchModel.userId))
                    .WhereIf(searchModel.workerId.IsNotNullOrEmpty(), x => x.workerId.Equals(searchModel.workerId))
                    .WhereIf(searchModel.businessServiceId.IsNotNullOrEmpty(), x => x.businessServiceId.Equals(searchModel.businessServiceId))
                    .WhereIf(searchModel.startDate.HasValue, x => x.startDate >= searchModel.startDate)
                    .WhereIf(searchModel.endDate.HasValue, x => x.startDate < searchModel.endDate)
                    .Skip(searchModel.page.Value * searchModel.take.Value)
                    .Take(searchModel.take.Value)
                    .ToListAsync();
            }
            else
            {
                return await _context.Appointments
                    .AsNoTracking()
                    .WhereIf(searchModel.businessId.IsNotNullOrEmpty(), x => x.businessId.Equals(searchModel.businessId))
                    .WhereIf(searchModel.userId.IsNotNullOrEmpty(), x => x.userId.Equals(searchModel.userId))
                    .WhereIf(searchModel.workerId.IsNotNullOrEmpty(), x => x.workerId.Equals(searchModel.workerId))
                    .WhereIf(searchModel.businessServiceId.IsNotNullOrEmpty(), x => x.businessServiceId.Equals(searchModel.businessServiceId))
                    .WhereIf(searchModel.startDate.HasValue, x => x.startDate >= searchModel.startDate)
                    .WhereIf(searchModel.endDate.HasValue, x => x.startDate < searchModel.endDate)
                    .ToListAsync();
            }

        }

        public async Task<Appointment> SaveAppointmentAsync(Appointment appointment)
        {
            appointment.createDate = DateTime.UtcNow;
            appointment.updateDate = appointment.createDate;

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;

        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            appointment.updateDate = DateTime.UtcNow;

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> ChangeStatusAsync(Guid id, AppointmentStatus status)
        {
            await _context.Appointments
             .Where(x => x.id == id)
             .ExecuteUpdateAsync(x => x.SetProperty(y => y.status, status));

            return true;
        }
    }
}