using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;

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
                    .WhereIf(searchModel.startDate.HasValue, x => x.startDate >= searchModel.startDate)
                    .WhereIf(searchModel.endDate.HasValue, x => x.startDate < searchModel.endDate)
                    .ToListAsync();
            }

        }

        public async Task<List<AppointmentListModel>> GetAppointmentsListModelByAppointmentSearchModelAsync(AppointmentSearchModel searchModel)
        {
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            return await _context.Appointments
                .AsNoTracking()
                .Where(x => x.userId.Equals(searchModel.userId))
                .WhereIf(searchModel.isHistory, x => x.startDate < searchModel.startDate)
                .WhereIf(!searchModel.isHistory, x => x.startDate >= searchModel.startDate)
                .Select(x => new AppointmentListModel
                {
                    id = x.id,
                    status = x.status,
                    startDate = x.startDate,
                    endDate = x.endDate,
                    description = x.description,
                    business = x.business == null
                        ? null
                        : new AppointmentBusinessListModel
                        {
                            id = x.business.id,
                            address = x.business.address,
                            name = x.business.name,
                            logoUrl = x.business.logoUrl,
                            telephone = x.business.telephone
                        },
                    comment = x.comments.Any()
                        ? new Comment
                        {
                            id = x.comments.ElementAt(0).id,
                            comment = x.comments.ElementAt(0).comment,
                            point = x.comments.ElementAt(0).point,
                            aspectsOfPoint = x.comments.ElementAt(0).aspectsOfPoint,
                            workerPoint = x.comments.ElementAt(0).workerPoint,
                            aspectsOfWorkerPoint = x.comments.ElementAt(0).aspectsOfWorkerPoint,
                            isShowProfile = x.comments.ElementAt(0).isShowProfile,
                            createDate = x.comments.ElementAt(0).createDate,
                            updateDate = x.comments.ElementAt(0).updateDate
                        }
                        : null,
                    details = x.details.Select(d => new AppointmentDetailListModel
                    {
                        workerName = d.worker != null ? d.worker.name : null,
                        workerImagePath = d.worker != null ? d.worker.path : null,
                        serviceName = isTurkish ?  d.businessService != null ? d.businessService.name : null 
                                                : d.businessService != null ? d.businessService.nameEn : null,
                        minDuration = d.businessService != null ? d.businessService.minDuration : null,
                        maxDuration = d.businessService != null ? d.businessService.maxDuration : null,
                        price = d.price,
                        discountPrice = d.discountPrice,
                        discountRate = ((d.price - d.discountPrice) / d.price) * 100,
                    })
                })
                .OrderBy(x => x.startDate)
                .Skip(searchModel.page.Value * searchModel.take.Value)
                .Take(searchModel.take.Value)
                .ToListAsync();
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

        public async Task<bool> ChangeStatusAsync(AppointmentChangeModel changeModel)
        {
            await _context.Appointments
             .WhereIf(changeModel.id.HasValue, x => x.id.Equals(changeModel.id))
             .WhereIf(changeModel.userId.HasValue, x => x.userId.Equals(changeModel.userId))
             .WhereIf(changeModel.date.HasValue, x => x.startDate < changeModel.date)
             .ExecuteUpdateAsync(x => x.SetProperty(y => y.status, changeModel.status));

            return true;
        }
    }
}