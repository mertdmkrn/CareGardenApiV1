using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface IAppointmentDetailService
    {
        Task<AppointmentDetail> GetAppointmentDetailByIdAsync(Guid id);
        Task<List<AppointmentDetail>> GetAppointmentDetailsByAppointmentDetailSearchModelAsync(AppointmentSearchRequestModel searchModel);
        Task<List<AppointmentDetail>> GetAppointmentDetailsByWorkerIdsAndDateAsync(AppointmentSearchRequestModel searchModel);
        Task<AppointmentDetail> SaveAppointmentDetailAsync(AppointmentDetail appointmentDetail);
        Task<AppointmentDetail> UpdateAppointmentDetailAsync(AppointmentDetail appointmentDetail);
        Task<bool> IsExistsAppointment(List<Guid> workerIds, DateTime date);
        Task<bool> DeleteAppointmentDetailAsync(Guid id);
    }
}
