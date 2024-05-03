using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CareGardenApiV1.Helpers;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("appointment")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentDetailService _appointmentDetailService;
        private readonly IBusinessWorkingInfoService _businessWorkingInfoService;
        private readonly IBusinessServicesService _businessServicesService;
        private readonly IBusinessService _businessService;
        private readonly IWorkerService _workerService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IAppointmentDetailService appointmentDetailService,
            IBusinessWorkingInfoService businessWorkingInfoService,
            IBusinessServicesService businessServicesService,
            IBusinessService businessService,
            IWorkerService workerService)
        {
            _appointmentService = appointmentService;
            _appointmentDetailService = appointmentDetailService;
            _businessWorkingInfoService = businessWorkingInfoService;
            _businessServicesService = businessServicesService;
            _businessService = businessService;
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

            BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(appointment.businessId.Value));

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

            if (!appointmentChangeModel.id.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitSilinemedi;
                return Ok(response);
            }

            Appointment appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentChangeModel.id.Value);

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

            BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(appointment.businessId.Value));

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

            if (!appointmentChangeModel.id.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitSilinemedi;
                return Ok(response);
            }

            response.Data = await _appointmentService.ChangeStatusAsync(appointmentChangeModel);
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
            ResponseModel<List<AppointmentWorkerModel>> response = new ResponseModel<List<AppointmentWorkerModel>>();

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

            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            var workers = await _workerService.GetWorkersByBusinessServiceIdAsync(appointmentInfo.businessServiceId.Value);

            if (workers.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            await setWorkersAvailableDate(workers.Where(x => x.isActive).ToList(), appointmentInfo);

            workers = workers
                .Where(x => x.availableDate.HasValue)
                .OrderByDescending(x => x.availableDate)
                .ThenByDescending(x => x.rating)
                .ThenByDescending(x => x.name)
                .ToList();

            if(!workers.IsNullOrEmpty())
            {
                workers.Insert(0, new AppointmentWorkerModel
                {
                    id = workers[0].id,
                    name = isTurkish ? "Herhangi Bir Profesyonel" : "Any Professional",
                    title = isTurkish ? "En Müsait" : "Maximum Availability",
                    rating = workers[0].rating,
                    countRating = workers[0].countRating,
                    availableDate = workers[0].availableDate,
                    availableDateStr = workers[0].availableDateStr,
                    isActive = workers[0].isActive,
                    price = workers[0].price,
                });
            }
            else
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = workers;

            return Ok(response);
        }

        private async Task setWorkersAvailableDate(List<AppointmentWorkerModel> workers, AppointmentSearchModel appointmentInfo)
        {
            var businesses = await _businessService.GetBusinessListForCache();
            var business = businesses.FirstOrDefault(x => x.id == appointmentInfo.businessId);

            if (business == null) return;

            var businessService = await _businessServicesService.GetBusinessServicePriceByIdAsync(appointmentInfo.businessServiceId.Value);

            if (businessService == null) return;

            var discounts = business.discounts?
                .Where(x => x.serviceIds.IsNullOrEmpty() || x.serviceIds.Contains(appointmentInfo.businessServiceId.Value.ToString()));

            var nowDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.AddMinutes(business.appointmentTimeInterval), "Turkey Standard Time");
            var pastAppointments = await _appointmentDetailService.GetAppointmentDetailsByWorkerIdsAndDateAsync(new AppointmentSearchModel { startDate = nowDate, workerIds = workers.Select(x => x.id).ToHashSet() });

            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            foreach (var worker in workers)
            {
                var businessStartDate = nowDate;

                while (!worker.availableDate.HasValue)
                {
                    bool businessIsOpen = HelperMethods.GetBusinessOpenSpecialDate(business.workingInfo, business.officialDayAvailable, businessStartDate);
                    if (!businessIsOpen)
                    {
                        while (!businessIsOpen)
                        {
                            businessStartDate = businessStartDate.AddDays(1);
                            businessIsOpen = HelperMethods.GetBusinessOpenSpecialDate(business.workingInfo, business.officialDayAvailable, businessStartDate);
                        }
                    }

                    var workHours = business.workingInfo.GetBusinessWorkInfoHours(businessStartDate);

                    setWorkerAvailableDate(worker, business.appointmentTimeInterval, businessStartDate, nowDate, pastAppointments);

                    if (!worker.availableDate.HasValue)
                    {
                        businessStartDate.AddDays(1);
                    }
                }

                worker.availableDateStr = worker.availableDate.Value.ToString((isTurkish ? "dd/MM HH:mm" : "MM/dd h:mm tt"), Resource.Resource.Culture);
                var activeDiscount = discounts?
                    .Where(x => x.type == DiscountType.AllDay
                    || (x.type == DiscountType.WeekDay && worker.availableDate.Value.DayOfWeek >= DayOfWeek.Monday && worker.availableDate.Value.DayOfWeek <= DayOfWeek.Friday)
                    || (x.type == DiscountType.WeekEnd && worker.availableDate.Value.DayOfWeek == DayOfWeek.Saturday || worker.availableDate.Value.DayOfWeek == DayOfWeek.Sunday))
                    .OrderBy(x => x.rate)
                    .FirstOrDefault();

                worker.price = activeDiscount == null ? businessService.price : businessService.price * (1 - (activeDiscount.rate / 100));
            }
        }

        private void setWorkerAvailableDate(AppointmentWorkerModel worker, int appointmentTimeInterval, DateTime businessStartDate, DateTime nowDate, IEnumerable<AppointmentDetail> pastAppointments)
        {
            string workerWorkHours = worker.GetWorkerWorkHours(businessStartDate);
            if (workerWorkHours.IsNullOrEmpty()) return;

            var workingHoursParts = workerWorkHours.Split('-');

            if (workingHoursParts.Length != 2) return;

            if (!TimeSpan.TryParse(workingHoursParts[0], out var startWorkTime) ||
                !TimeSpan.TryParse(workingHoursParts[1], out var endWorkTime))
                return;

            var startDate = new DateTime(businessStartDate.Year, businessStartDate.Month, businessStartDate.Day)
            .Add(startWorkTime);
            var endDate = new DateTime(businessStartDate.Year, businessStartDate.Month, businessStartDate.Day)
            .Add(endWorkTime);

            var date = startDate;

            while (true)
            {
                if (date < nowDate)
                {
                    date = date.AddMinutes(appointmentTimeInterval);
                    continue;
                }

                if (pastAppointments == null || (pastAppointments != null && !pastAppointments.Any(x => x.date == date)))
                {
                    worker.availableDate = date;
                    break;
                }

                date.AddMinutes(appointmentTimeInterval);

                if (date >= endDate) break;
            }
        }

        ///// <summary>
        ///// Get Worker Available Times
        ///// </summary>
        ///// <remarks>
        ///// **Sample request body:**
        /////
        /////     { 
        /////        "businessId" : "00000000-0000-0000-0000-000000000000",
        /////        "workerId" : "00000000-0000-0000-0000-000000000000",
        /////        "businessServiceId" : "00000000-0000-0000-0000-000000000000",
        /////        "startDate" : "2023-10-13 10:00"
        /////     }
        /////     
        ///// </remarks>
        ///// <returns></returns>
        //[HttpPost("getworkeravailabletimes")]
        //public async Task<IActionResult> GetWorkerAvailableTime([FromBody] AppointmentSearchModel appointmentInfo)
        //{
        //    ResponseModel<List<WorkerAvailableTimeModel>> response = new ResponseModel<List<WorkerAvailableTimeModel>>();

        //    if (!appointmentInfo.businessId.HasValue)
        //    {
        //        response.HasError = true;
        //        response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdParametreHatasi));
        //        response.Message = Resource.Resource.IdParametreHatasi;
        //        return Ok(response);
        //    }

        //    if (!appointmentInfo.userId.HasValue)
        //    {
        //        response.HasError = true;
        //        response.ValidationErrors.Add(new ValidationError("userId", Resource.Resource.IdParametreHatasi));
        //        response.Message = Resource.Resource.IdParametreHatasi;
        //        return Ok(response);
        //    }

        //    if (!appointmentInfo.workerId.HasValue)
        //    {
        //        response.HasError = true;
        //        response.ValidationErrors.Add(new ValidationError("workerId", Resource.Resource.IdParametreHatasi));
        //        response.Message = Resource.Resource.IdParametreHatasi;
        //        return Ok(response);
        //    }

        //    if (!appointmentInfo.businessServiceId.HasValue)
        //    {
        //        response.HasError = true;
        //        response.ValidationErrors.Add(new ValidationError("workerId", Resource.Resource.IdParametreHatasi));
        //        response.Message = Resource.Resource.IdParametreHatasi;
        //        return Ok(response);
        //    }

        //    var startDate = appointmentInfo.startDate ?? DateTime.Today;
        //    var endDate = startDate.AddDays(15);

        //    var workerAppointments = await _appointmentService.GetAppointmentsByAppointmentSearchModelAsync(new AppointmentSearchModel { workerId = appointmentInfo.workerId, startDate = startDate, endDate = endDate });
        //    var businessWorkingInfo = await _businessWorkingInfoService.GetBusinessWorkingInfosByBusinessIdAsync(appointmentInfo.businessId.Value);
        //    var businessService = await _businessServicesService.GetBusinessServiceByIdAsync(appointmentInfo.businessServiceId.Value);

        //    response.Data = HelperMethods.GetWorkerAvailableTimes(workerAppointments, businessWorkingInfo.FirstOrDefault(), businessService, startDate, endDate);
        //    return Ok(response);
        //}
    }
}
