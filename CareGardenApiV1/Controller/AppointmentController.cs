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
        private readonly IWorkerServicePriceService _workerServicePriceService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IAppointmentDetailService appointmentDetailService,
            IBusinessWorkingInfoService businessWorkingInfoService,
            IBusinessServicesService businessServicesService,
            IBusinessService businessService,
            IWorkerService workerService,
            IWorkerServicePriceService workerServicePriceService)
        {
            _appointmentService = appointmentService;
            _appointmentDetailService = appointmentDetailService;
            _businessWorkingInfoService = businessWorkingInfoService;
            _businessServicesService = businessServicesService;
            _businessService = businessService;
            _workerService = workerService;
            _workerServicePriceService = workerServicePriceService;
        }

        /// <summary>
        /// Get Appointments
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "isHistory" : false,
        ///        "page" : 0,
        ///        "take" : 5
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("get")]
        public async Task<IActionResult> Get([FromBody] AppointmentSearchModel searchModel)
        {
            ResponseModel<List<AppointmentListModel>> response = new ResponseModel<List<AppointmentListModel>>();

            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            if (userId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            searchModel.userId = userId.ToGuid();
            searchModel.startDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Turkey Standard Time");
            searchModel.page ??= 0;
            searchModel.take ??= 5;
            
            response.Data = await _appointmentService.GetAppointmentsListModelByAppointmentSearchModelAsync(searchModel);

            return Ok(response);
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
        /// Save Appoinment
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///        "userId" : "00000000-0000-0000-0000-000000000000",
        ///        "description" : "Yeniden geliyorum.",
        ///        "startDate" : "2024-10-07 15:00",
        ///        "serviceWorkerInfos" : [
        ///             {
        ///                 "businessServiceId" : "00000000-0000-0000-0000-000000000000",
        ///                 "workerId" : "00000000-0000-0000-0000-000000000000"
        ///             },
        ///             {
        ///                 "businessServiceId" : "00000000-0000-0000-0000-000000000000",
        ///                 "workerId" : "00000000-0000-0000-0000-000000000000"
        ///             },
        ///             {
        ///                 "businessServiceId" : "00000000-0000-0000-0000-000000000000",
        ///                 "workerId" : "00000000-0000-0000-0000-000000000000"
        ///             }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] AppointmentSaveModel appointmentSaveModel)
        {
            ResponseModel<Appointment> response = new ResponseModel<Appointment>();

            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            if (userId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            appointmentSaveModel.userId = appointmentSaveModel.userId.IsNotNullOrEmpty() ? appointmentSaveModel.userId : userId.ToGuid();

            if (!appointmentSaveModel.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!appointmentSaveModel.userId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("userId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!appointmentSaveModel.startDate.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("startDate", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (appointmentSaveModel.serviceWorkerInfos.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("serviceWorkerInfos", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (appointmentSaveModel.serviceWorkerInfos.Exists(x => !x.workerId.HasValue))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("workerId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            var workerIds = appointmentSaveModel.serviceWorkerInfos.Select(x => x.workerId.Value).ToList();
            var businessServicesIds = appointmentSaveModel.serviceWorkerInfos.Select(x => x.businessServiceId).ToList();
            var startDate = appointmentSaveModel.startDate.Value;

            bool isExistsAppointment = await _appointmentDetailService.IsExistsAppointment(workerIds, startDate);
            var businesses = await _businessService.GetBusinessListForCache();
            var business = businesses.FirstOrDefault(x => x.id.Equals(appointmentSaveModel.businessId.Value));

            if (isExistsAppointment || !HelperMethods.GetBusinessOpenSpecialDate(business.workingInfo, business.officialDayAvailable, startDate))
            {
                response.Message = Resource.Resource.RandevuMevcut;
                response.HasError = true;
                return Ok(response);
            }

            var activeDiscounts = business.discounts?
                .Where(x => x.type == DiscountType.AllDay
                            || (x.type == DiscountType.WeekDay &&
                                startDate.DayOfWeek >= DayOfWeek.Monday &&
                                startDate.DayOfWeek <= DayOfWeek.Friday)
                            || (x.type == DiscountType.WeekEnd &&
                                (startDate.DayOfWeek == DayOfWeek.Saturday ||
                                 startDate.DayOfWeek == DayOfWeek.Sunday)))
                .OrderBy(x => x.rate)
                .ToList();

            var businessServices = await _businessServicesService.GetBusinessServicesByIdsAsync(businessServicesIds);
            var workerServicePrices = await _workerServicePriceService.GetWorkerServicePricesByBusinessServiceIdsAsync(businessServicesIds);

            var totalWorkMinutes = businessServices.Sum(x => x.maxDuration.IsNull(x.minDuration));

            Appointment appointment = new Appointment()
            {
                businessId = business.id,
                userId = appointmentSaveModel.userId,
                startDate = startDate,
                endDate = startDate.AddMinutes(totalWorkMinutes),
                description = appointmentSaveModel.description
            };

            foreach (var serviceWorkerInfo in appointmentSaveModel.serviceWorkerInfos)
            {
                var businessService = businessServices.FirstOrDefault(x => x.id.Equals(serviceWorkerInfo.businessServiceId));
                var workerServicePrice = workerServicePrices.FirstOrDefault(x => x.businessServiceId.Equals(serviceWorkerInfo.businessServiceId)
                    && x.workerId.Equals(serviceWorkerInfo.workerId));
                var discount = activeDiscounts?.FirstOrDefault(d => d.serviceIds.IsNullOrEmpty() || d.serviceIds.Contains(businessService.id.ToString()));

                var price = workerServicePrice?.price ?? businessService?.price ?? 0;

                AppointmentDetail appointmentDetail = new AppointmentDetail()
                {
                    workerId = serviceWorkerInfo.workerId,
                    businessServiceId = serviceWorkerInfo.businessServiceId,
                    date = appointment.startDate,
                    price = price,
                    discountPrice = price * (1 - (discount?.rate ?? 0) / 100)
                };

                appointment.totalPrice += appointmentDetail.price;
                appointment.totalDiscountPrice += appointmentDetail.discountPrice;

                appointment.details.Add(appointmentDetail);
            }

            response.Data = await _appointmentService.SaveAppointmentAsync(appointment);
            response.Message = Resource.Resource.KayitBasarili;

            BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(appointmentSaveModel.businessId.Value));

            return Ok(response);
        }

        ///// <summary>
        ///// Delete Appointment
        ///// </summary>
        ///// <remarks>
        ///// **Sample request body:**
        /////
        /////     { 
        /////        "id" : "00000000-0000-0000-0000-000000000000",
        /////        "isForceDelete" : true
        /////     }
        /////     
        ///// </remarks>
        ///// <returns></returns>
        //[HttpPost("delete")]
        //public async Task<IActionResult> Delete([FromBody] AppointmentChangeModel appointmentChangeModel)
        //{
        //    ResponseModel<bool> response = new ResponseModel<bool>();

        //    if (!appointmentChangeModel.id.HasValue)
        //    {
        //        response.HasError = true;
        //        response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
        //        response.Message = Resource.Resource.KayitSilinemedi;
        //        return Ok(response);
        //    }

        //    Appointment appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentChangeModel.id.Value);

        //    if (appointment == null)
        //    {
        //        response.HasError = true;
        //        response.Message = Resource.Resource.KayitBulunamadi;
        //        return Ok(response);
        //    }

        //    var sessionUserId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid).IsNull(Guid.Empty.ToString());

        //    if (appointment.startDate.DifferenceBetweenDates(DateTimeOffset.UtcNow.UtcDateTime, DateType.Hour) < 9 && appointment.userId == sessionUserId.ToGuid())
        //    {
        //        if (!appointmentChangeModel.isForceDelete.HasValue || (appointmentChangeModel.isForceDelete.HasValue && !appointmentChangeModel.isForceDelete.Value))
        //        {
        //            response.HasError = true;
        //            response.Message = Resource.Resource.RandevuIptalUyari;
        //            return Ok(response);
        //        }
        //        else
        //        {
        //            // Todo: Güvenirliği azalt
        //        }
        //    }

        //    response.Data = await _appointmentService.DeleteAppointmentAsync(appointment);
        //    response.Message = Resource.Resource.KayitSilindi;

        //    BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(appointment.businessId.Value));

        //    return Ok(response);
        //}

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

            var workers = await _workerService.GetWorkersByAppointmentSearchModelAsync(new AppointmentSearchModel()
            {
                businessServiceId = appointmentInfo.businessServiceId
            });

            if (workers.IsNullOrEmpty())
            {
                workers = await _workerService.GetWorkersByAppointmentSearchModelAsync(new AppointmentSearchModel()
                {
                    businessId = appointmentInfo.businessId
                });

                if(workers.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KayitBulunamadi;
                    return Ok(response);
                }
            }

            await setWorkersAvailableDate(workers.Where(x => x.isActive).ToList(), appointmentInfo);

            workers = workers
                .OrderBy(x => x.availableDate)
                .ThenByDescending(x => x.rating)
                .ThenBy(x => x.name)
                .ToList();


            if(workers.Exists(x => x.availableDate.HasValue))
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
            var workerServicePrices = await _workerServicePriceService.GetWorkerServicePricesSearchAsync(businessServiceId: appointmentInfo.businessServiceId.Value);

            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            foreach (var worker in workers)
            {
                var businessStartDate = nowDate;
                var intervalDay = 14;
                var workerServicePrice = workerServicePrices?.FirstOrDefault(x => x.workerId.Equals(worker.id));
                var price = workerServicePrice?.price ?? businessService?.price ?? 0;

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

                    setWorkerAvailableDate(worker, business.appointmentTimeInterval, businessStartDate, nowDate, pastAppointments);

                    if (!worker.availableDate.HasValue)
                    {
                        businessStartDate = businessStartDate.AddDays(1);
                        intervalDay--;

                        if(intervalDay == 0) break;
                    }
                }


                worker.isActive = worker.availableDate.HasValue;
                worker.availableDateStr = worker.isActive ? worker.availableDate.Value.ToString((isTurkish ? "dd/MM HH:mm" : "MM/dd h:mm tt"), Resource.Resource.Culture) : string.Empty;

                var activeDiscount = worker.isActive
                    ? discounts?
                        .Where(x => x.type == DiscountType.AllDay
                                    || (x.type == DiscountType.WeekDay && worker.availableDate.Value.DayOfWeek >= DayOfWeek.Monday && worker.availableDate.Value.DayOfWeek <= DayOfWeek.Friday)
                                    || (x.type == DiscountType.WeekEnd && worker.availableDate.Value.DayOfWeek == DayOfWeek.Saturday || worker.availableDate.Value.DayOfWeek == DayOfWeek.Sunday))
                        .MaxBy(x => x.rate)
                    : null;

                worker.price = activeDiscount == null ? price : price * (1 - (activeDiscount.rate / 100));
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

            var startDate = new DateTime(businessStartDate.Year, businessStartDate.Month, businessStartDate.Day).Add(startWorkTime);
            var endDate = new DateTime(businessStartDate.Year, businessStartDate.Month, businessStartDate.Day).Add(endWorkTime);

            for (var date = startDate; date < endDate; date = date.AddMinutes(appointmentTimeInterval))
            {
                if (date < nowDate)
                    continue;

                if (pastAppointments == null || (pastAppointments != null && !pastAppointments.Any(x => x.date == date && x.workerId.Equals(worker.id))))
                {
                    worker.availableDate = date;
                    break;
                }
            }
        }

        /// <summary>
        /// Get Appointment Dates 
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     {
        ///         "businessId" : "00000000-0000-0000-0000-000000000000",
        ///         "page" : 0,
        ///         "hasGetAnyAvailibityWorker" : false,
        ///         "serviceWorkerInfos" : [
        ///             {
        ///                 "businessServiceId" : "00000000-0000-0000-0000-000000000000",
        ///                 "workerId" : "00000000-0000-0000-0000-000000000000"
        ///             },
        ///             {
        ///                 "businessServiceId" : "00000000-0000-0000-0000-000000000000",
        ///                 "workerId" : "00000000-0000-0000-0000-000000000000"
        ///             },
        ///             {
        ///                 "businessServiceId" : "00000000-0000-0000-0000-000000000000",
        ///                 "workerId" : "00000000-0000-0000-0000-000000000000"
        ///             }
        ///         ]
        ///     }
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getappointmentdates")]
        public async Task<IActionResult> GetAppointmentDates([FromBody] AppointmentDatesModel appointmentInfo)
        {
            ResponseModel<AppointmentAvailableInfoModel> response = new ResponseModel<AppointmentAvailableInfoModel>();

            var businesses = await _businessService.GetBusinessListForCache();
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");
            var business = businesses.FirstOrDefault(x => x.id == appointmentInfo.businessId);

            if (business == null) return Ok(response);

            var workers = appointmentInfo.hasGetAnyAvailibityWorker
                ? await _workerService.GetWorkersByAppointmentSearchModelAsync(new AppointmentSearchModel()
                {
                    businessId = appointmentInfo.businessId,
                    isActive = true
                })
                : await _workerService.GetWorkersByWorkerIdsAsync(appointmentInfo.serviceWorkerInfos
                    .Select(x => x.workerId).ToList());

            Dictionary<DayOfWeek, List<Tuple<TimeSpan, TimeSpan>>> workersWorkTimesDict = new Dictionary<DayOfWeek, List<Tuple<TimeSpan, TimeSpan>>>();

            foreach (var worker in workers)
            {
                setWorkerTimes(worker.mondayWorkHours, DayOfWeek.Monday, workersWorkTimesDict);
                setWorkerTimes(worker.tuesdayWorkHours, DayOfWeek.Tuesday, workersWorkTimesDict);
                setWorkerTimes(worker.wednesdayWorkHours, DayOfWeek.Wednesday, workersWorkTimesDict);
                setWorkerTimes(worker.thursdayWorkHours, DayOfWeek.Thursday, workersWorkTimesDict);
                setWorkerTimes(worker.fridayWorkHours, DayOfWeek.Friday, workersWorkTimesDict);
                setWorkerTimes(worker.saturdayWorkHours, DayOfWeek.Saturday, workersWorkTimesDict);
                setWorkerTimes(worker.sundayWorkHours, DayOfWeek.Sunday, workersWorkTimesDict);
            }

            var intervalDay = 7;

            var nowDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now.AddMinutes(business.appointmentTimeInterval), "Turkey Standard Time");
            var startDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Today.AddDays(intervalDay * appointmentInfo.page), "Turkey Standard Time");
            var endDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Today.AddDays(intervalDay * (appointmentInfo.page + 1)), "Turkey Standard Time");

            var pastAppointments = await _appointmentDetailService.GetAppointmentDetailsByWorkerIdsAndDateAsync(new AppointmentSearchModel { startDate = nowDate, endDate = endDate, workerIds = workers.Select(x => x.id).ToHashSet() });

            if (appointmentInfo.hasGetAnyAvailibityWorker)
            {
                var groupingPastAppointments = pastAppointments.GroupBy(x => x.workerId.Value).ToList();
                foreach (var item in appointmentInfo.serviceWorkerInfos)
                {
                    var serviceWorkers = workers
                        .Where(x => x.serviceIds.IsNull("").Contains(item.businessServiceId.ToString()))
                        .ToList();

                    if (serviceWorkers.IsNullOrEmpty())
                    {
                        serviceWorkers = workers;
                    }

                    var worker = groupingPastAppointments
                        .Where(x => serviceWorkers
                            .Exists(y => y.id.Equals(x.Key)))
                            .MinBy(x => x.Count());

                    item.workerId = !worker.IsNullOrEmpty()
                        ? worker.Key
                        : serviceWorkers.FirstOrDefault().id;
                }
            }

            List<AppointmentAvailableTimeModel> models = new();

            for (int i = 0; i < intervalDay; i++)
            {
                var date = startDate.AddDays(i);

                AppointmentAvailableTimeModel model = new();
                model.date = date;

                bool isBusinessOpen = HelperMethods.GetBusinessOpenSpecialDate(business.workingInfo, business.officialDayAvailable, date);

                if (!isBusinessOpen)
                {
                    model.isActive = false;
                    models.Add(model);
                    continue;
                }
                else
                {
                    model.isActive = true;
                }

                string businessWorkHours = HelperMethods.GetBusinessWorkInfoHours(business.workingInfo, date);

                var workingHoursParts = businessWorkHours.Split('-');

                if (workingHoursParts.Length != 2)
                {
                    model.isActive = false;
                    models.Add(model);
                    continue;
                }

                if (!TimeSpan.TryParse(workingHoursParts[0], out var startWorkTime) ||
                    !TimeSpan.TryParse(workingHoursParts[1], out var endWorkTime))
                {
                    model.isActive = false;
                    models.Add(model);
                    continue;
                }

                var startWorkDate = new DateTime(date.Year, date.Month, date.Day).Add(startWorkTime);
                var endWorkDate = new DateTime(date.Year, date.Month, date.Day).Add(endWorkTime);

                var tempDate = startWorkDate;

                while (true)
                {
                    if (tempDate < nowDate)
                    {
                        tempDate = tempDate.AddMinutes(business.appointmentTimeInterval);
                        continue;
                    }

                    if (tempDate >= endWorkDate) break;

                    TimeModel timeModel = new();
                    timeModel.date = tempDate;
                    timeModel.hourStr = $"{tempDate.Hour.ToString().PadLeft(2, '0')}:{tempDate.Minute.ToString().PadLeft(2, '0')}";

                    var hourTimeSpan = TimeSpan.Parse(timeModel.hourStr);

                    var workTimeTuples = workersWorkTimesDict.ContainsKey(tempDate.DayOfWeek) 
                        ? workersWorkTimesDict[tempDate.DayOfWeek]
                        : null;


                    timeModel.isActive = workTimeTuples != null && !pastAppointments.Exists(x => x.date == tempDate)
                                         && !workTimeTuples.Exists(x => x.Item1 > hourTimeSpan && x.Item2 < hourTimeSpan);

                    if (timeModel.isActive)
                    {
                        model.dateList.Add(timeModel);
                    }

                    tempDate = tempDate.AddMinutes(business.appointmentTimeInterval);
                }

                model.isActive = model.isActive && !model.dateList.IsNullOrEmpty();

                models.Add(model);
            }

            response.Data = new AppointmentAvailableInfoModel()
            {
                dateInfos = models,
                serviceWorkerInfos = appointmentInfo.serviceWorkerInfos
            };

            return Ok(response);
        }

        private void setWorkerTimes(string workHours, DayOfWeek dayOfWeek, Dictionary<DayOfWeek, List<Tuple<TimeSpan, TimeSpan>>> workersWorkTimesDict)
        {
            if (!workHours.IsNullOrEmpty())
            {
                var workHoursArr = workHours.Split('-');
                var startTime = TimeSpan.Parse(workHoursArr.FirstOrDefault());
                var endTime = TimeSpan.Parse(workHoursArr.LastOrDefault());
                var tuple = Tuple.Create(startTime, endTime);

                if (!workersWorkTimesDict.ContainsKey(dayOfWeek))
                {
                    workersWorkTimesDict[dayOfWeek] = new List<Tuple<TimeSpan, TimeSpan>>();
                }

                workersWorkTimesDict[dayOfWeek].Add(tuple);
            }
        }
    }
}
