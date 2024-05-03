using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IAppointmentDetailRepository
    {
        Task<AppointmentDetail> GetAppointmentDetailByIdAsync(Guid id);
        Task<List<AppointmentDetail>> GetAppointmentDetailsByAppointmentDetailSearchModelAsync(AppointmentSearchModel searchModel);
        Task<List<AppointmentDetail>> GetAppointmentDetailsByWorkerIdsAndDateAsync(AppointmentSearchModel searchModel);
        Task<AppointmentDetail> SaveAppointmentDetailAsync(AppointmentDetail appointmentDetail);
        Task<AppointmentDetail> UpdateAppointmentDetailAsync(AppointmentDetail appointmentDetail);
        Task<bool> DeleteAppointmentDetailAsync(Guid id);
    }
}
