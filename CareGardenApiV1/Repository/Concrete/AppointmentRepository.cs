using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.RequestModel;
using System.Linq;

namespace CareGardenApiV1.Repository.Concrete
{
    public class AppointmentRepository : IAppointmentRepository
    {

        public async Task<bool> DeleteAppointmentAsync(Appointment appointment)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Appointments.Remove(appointment);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Appointments
                    .FindAsync(id);
            }
        }

        public async Task<List<Appointment>> GetAppointmentsByAppointmentSearchModelAsync(AppointmentSearchModel searchModel)
        {
            using (var context = new CareGardenApiDbContext())
            {
                if (searchModel.page.HasValue && searchModel.take.HasValue)
                {
                    return await context.Appointments
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
                    return await context.Appointments
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
        }

        public async Task<Appointment> SaveAppointmentAsync(Appointment appointment)
        {
            using (var context = new CareGardenApiDbContext())
            {
                appointment.createDate = DateTime.UtcNow;
                appointment.updateDate = appointment.createDate;

                await context.Appointments.AddAsync(appointment);
                await context.SaveChangesAsync();
                return appointment;
            }
        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            using (var context = new CareGardenApiDbContext())
            {
                appointment.updateDate = DateTime.UtcNow;

                await context.Appointments.AddAsync(appointment);
                await context.SaveChangesAsync();
                return appointment;
            }
        }

        public async Task<bool> ChangeStatusAsync(Guid id, AppointmentStatus status)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Appointments
                 .Where(x => x.id == id)
                 .ExecuteUpdateAsync(x => x.SetProperty(y => y.status, status));

                return true;
            }
        }
    }
}