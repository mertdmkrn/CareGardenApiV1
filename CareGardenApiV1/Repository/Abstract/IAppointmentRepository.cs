using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetAppointmentByIdAsync(Guid id);
        Task<List<Appointment>> GetAppointmentsByAppointmentSearchModelAsync(AppointmentSearchRequestModel searchModel);
        Task<List<AppointmentListResponseModel>> GetAppointmentsListModelByAppointmentSearchModelAsync(AppointmentSearchRequestModel searchModel);
        Task<Appointment> SaveAppointmentAsync(Appointment appointment);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(Appointment appointment);
        Task<bool> ChangeStatusAsync(AppointmentChangeRequestModel changeModel);
    }
}
