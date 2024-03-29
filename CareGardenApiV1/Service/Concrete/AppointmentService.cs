using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }


        public async Task<bool> DeleteAppointmentAsync(Appointment appointment)
        {
            return await _appointmentRepository.DeleteAppointmentAsync(appointment);
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
        {
            return await _appointmentRepository.GetAppointmentByIdAsync(id);
        }

        public async Task<List<Appointment>> GetAppointmentsByAppointmentSearchModelAsync(AppointmentSearchModel searchModel)
        {
            return await _appointmentRepository.GetAppointmentsByAppointmentSearchModelAsync(searchModel);
        }

        public async Task<Appointment> SaveAppointmentAsync(Appointment appointment)
        {
            return await _appointmentRepository.SaveAppointmentAsync(appointment);
        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            return await _appointmentRepository.UpdateAppointmentAsync(appointment);
        }
        public async Task<bool> ChangeStatusAsync(Guid id, AppointmentStatus status)
        {
            return await _appointmentRepository.ChangeStatusAsync(id, status);
        }
    }
}
