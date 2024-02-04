using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("appointment")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IBusinessWorkingInfoService _businessWorkingInfoService;
        private readonly IBusinessServicesService _businessServicesService;
        private readonly IWorkerService _workerService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IBusinessWorkingInfoService businessWorkingInfoService,
            IBusinessServicesService businessServicesService,
            IWorkerService workerService)
        {
            _appointmentService = appointmentService;
            _businessWorkingInfoService = businessWorkingInfoService;
            _businessServicesService = businessServicesService;
            _workerService = workerService;
        }


        /// <summary>
        /// Get Appointment By Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getbyid")]
        public async Task<IActionResult> GetById([FromBody] string? id)
        {
            ResponseModel<Appointment> response = new ResponseModel<Appointment>();

            if (id.IsNullOrEmpty() || !id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _appointmentService.GetAppointmentByIdAsync(id.ToGuid());
            return Ok(response);
        }

        /// <summary>
        /// Search Appointment
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///        "userId" : "00000000-0000-0000-0000-000000000000",
        ///        "workerId" : "00000000-0000-0000-0000-000000000000",
        ///        "businessServiceId" : "00000000-0000-0000-0000-000000000000",
        ///        "startDate" : "2023-10-07",
        ///        "endDate" : "2023-10-22",
        ///        "page" : 0,
        ///        "take" : 5
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] AppointmentSearchModel searchModel)
        {
            ResponseModel<List<Appointment>> response = new ResponseModel<List<Appointment>>();

            if (!searchModel.userId.HasValue && !searchModel.businessId.HasValue && !searchModel.workerId.HasValue && !searchModel.businessServiceId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("ids", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _appointmentService.GetAppointmentsByAppointmentSearchModelAsync(searchModel);

            return Ok(response);
        }

        /// <summary>
        /// Save Appoinment
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///        "userId" : "00000000-0000-0000-0000-000000000000",
        ///        "workerId" : "00000000-0000-0000-0000-000000000000",
        ///        "businessServiceId" : "00000000-0000-0000-0000-000000000000",    
        ///        "description" : "Yeniden geliyorum.",
        ///        "startDate" : "2023-10-07 15:00",
        ///        "endDate" : "2023-10-07 16:00"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] Appointment appointment)
        {
            ResponseModel<Appointment> response = new ResponseModel<Appointment>();

            if (!appointment.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!appointment.userId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("userId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!appointment.workerId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("workerId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!appointment.businessServiceId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessServiceId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!appointment.startDate.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("startDate", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!appointment.endDate.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("endDate", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            response.Data = await _appointmentService.SaveAppointmentAsync(appointment);
            response.Message = Resource.Resource.KayitBasarili;
            return Ok(response);
        }

        /// <summary>
        /// Delete Appointment
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
        ///        "isForceDelete" : true
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] AppointmentChangeModel appointmentChangeModel)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (appointmentChangeModel.id.IsNullOrEmpty() || !appointmentChangeModel.id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitSilinemedi;
                return Ok(response);
            }

            Appointment appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentChangeModel.id.ToGuid());

            if (appointment == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            var sessionUserId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid).IsNull(Guid.Empty.ToString());

            if (appointment.startDate.DifferenceBetweenDates(DateTimeOffset.UtcNow.UtcDateTime, DateType.Hour) < 9 && appointment.userId == sessionUserId.ToGuid())
            {
                if (!appointmentChangeModel.isForceDelete.HasValue || (appointmentChangeModel.isForceDelete.HasValue && !appointmentChangeModel.isForceDelete.Value))
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.RandevuIptalUyari;
                    return Ok(response);
                }
                else
                {
                    // Todo: Güvenirliği azalt
                }
            }

            response.Data = await _appointmentService.DeleteAppointmentAsync(appointment);
            response.Message = Resource.Resource.KayitSilindi;
            return Ok(response);
        }

        /// <summary>
        /// Change Appointment Status
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
        ///        "status" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("changestatus")]
        public async Task<IActionResult> Update([FromBody] AppointmentChangeModel appointmentChangeModel)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (appointmentChangeModel.id.IsNullOrEmpty() || !appointmentChangeModel.id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitSilinemedi;
                return Ok(response);
            }

            response.Data = await _appointmentService.ChangeStatusAsync(appointmentChangeModel.id.ToGuid(), appointmentChangeModel.status);
            response.Message = Resource.Resource.KayitBasarili;
            return Ok(response);
        }

        /// <summary>
        /// Get Workers Provide Service 
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///        "businessServiceId" : "00000000-0000-0000-0000-000000000000"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getworkersprovideservice")]
        public async Task<IActionResult> GetWorkersProvideService([FromBody] AppointmentSearchModel appointmentInfo)
        {
            ResponseModel<List<Worker>> response = new ResponseModel<List<Worker>>();

            if (!appointmentInfo.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
                return Ok(response);
            }

            if (!appointmentInfo.businessServiceId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("workerId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
                return Ok(response);
            }

            var workers = await _workerService.GetWorkersByBusinessIdAsync(appointmentInfo.businessId.Value);
            response.Data = workers.Where(x => x.serviceIds.ToLower().Contains(appointmentInfo.businessServiceId.Value.ToString().ToLower())).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Get Worker Available Times
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///        "workerId" : "00000000-0000-0000-0000-000000000000",
        ///        "businessServiceId" : "00000000-0000-0000-0000-000000000000",
        ///        "startDate" : "2023-10-13 10:00"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getworkeravailabletimes")]
        public async Task<IActionResult> GetWorkerAvailableTime([FromBody] AppointmentSearchModel appointmentInfo)
        {
            ResponseModel<List<WorkerAvailableTimeModel>> response = new ResponseModel<List<WorkerAvailableTimeModel>>();

            if (!appointmentInfo.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
                return Ok(response);
            }

            if (!appointmentInfo.userId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("userId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
                return Ok(response);
            }

            if (!appointmentInfo.workerId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("workerId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
                return Ok(response);
            }

            if (!appointmentInfo.businessServiceId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("workerId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
                return Ok(response);
            }

            var startDate = appointmentInfo.startDate ?? DateTime.Today;
            var endDate = startDate.AddDays(15);

            var workerAppointments = await _appointmentService.GetAppointmentsByAppointmentSearchModelAsync(new AppointmentSearchModel { workerId = appointmentInfo.workerId, startDate = startDate, endDate = endDate });
            var businessWorkingInfo = await _businessWorkingInfoService.GetBusinessWorkingInfosByBusinessIdAsync(appointmentInfo.businessId.Value);
            var businessService = await _businessServicesService.GetBusinessServiceByIdAsync(appointmentInfo.businessServiceId.Value);

            response.Data = HelperMethods.GetWorkerAvailableTimes(workerAppointments, businessWorkingInfo.FirstOrDefault(), businessService, startDate, endDate);
            return Ok(response);
        }
    }
}
