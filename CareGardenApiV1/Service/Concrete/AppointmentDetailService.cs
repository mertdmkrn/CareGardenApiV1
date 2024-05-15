using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class AppointmentDetailService : IAppointmentDetailService
    {
        private readonly IAppointmentDetailRepository _appointmentDetailRepository;

        public AppointmentDetailService(IAppointmentDetailRepository appointmentDetailRepository)
        {
            _appointmentDetailRepository = appointmentDetailRepository;
        }

        public async Task<bool> IsExistsAppointment(List<Guid> workerIds, DateTime date)
        {
            return await _appointmentDetailRepository.IsExistsAppointment(workerIds, date);
        }

        public async Task<bool> DeleteAppointmentDetailAsync(Guid id)
        {
            return await _appointmentDetailRepository.DeleteAppointmentDetailAsync(id);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailByIdAsync(Guid id)
        {
            return await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(id);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentDetailsByAppointmentDetailSearchModelAsync(AppointmentSearchModel searchModel)
        {
            return await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentDetailSearchModelAsync(searchModel);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentDetailsByWorkerIdsAndDateAsync(AppointmentSearchModel searchModel)
        {
            return await _appointmentDetailRepository.GetAppointmentDetailsByWorkerIdsAndDateAsync(searchModel);
        }

        public async Task<AppointmentDetail> SaveAppointmentDetailAsync(AppointmentDetail appointmentDetail)
        {
            return await _appointmentDetailRepository.SaveAppointmentDetailAsync(appointmentDetail);
        }

        public async Task<AppointmentDetail> UpdateAppointmentDetailAsync(AppointmentDetail appointmentDetail)
        {
            return await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail);
        }
    }
}
