using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface IAppointmentService
    {
        Task<Appointment> GetAppointmentByIdAsync(Guid id);
        Task<List<Appointment>> GetAppointmentsByAppointmentSearchModelAsync(AppointmentSearchModel searchModel);
        Task<Appointment> SaveAppointmentAsync(Appointment appointment);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(Appointment appointment);
        Task<bool> ChangeStatusAsync(Guid id, AppointmentStatus status);
    }
}
