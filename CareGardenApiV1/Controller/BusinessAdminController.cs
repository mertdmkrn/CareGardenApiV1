using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize(Roles = "BusinessAdmin,Business,BusinessWorker")]
    [Route("businessadmin")]
    public class BusinessAdminController : ControllerBase
    {
        private readonly IBusinessAdminService _businessAdminService;

        public BusinessAdminController(IBusinessAdminService businessAdminService)
        {
            _businessAdminService = businessAdminService;
        }


        /// <summary>
        /// Get Total Datas
        /// </summary>
        /// <returns></returns>
        [HttpPost("gettotaldata")]
        public async Task<IActionResult> GetTotalData()
        {
            ResponseModel<BusinessAdminTotalDataResponseModel> response = new ResponseModel<BusinessAdminTotalDataResponseModel>();
            var businessId = HelperMethods.GetClaimInfo(Request, CustomClaimTypes.BusinessId).ToGuid();
            
            response.Data = await _businessAdminService.GetBusinessAdminTotalDataAsync(businessId);
            return Ok(response);
        }

        /// <summary>
        /// Get Earning Report
        /// </summary>
        /// <returns></returns>
        [HttpPost("getearningreport")]
        public async Task<IActionResult> GetEarningReport()
        {
            ResponseModel<BusinessAdminEarningReportResponseModel> response = new ResponseModel<BusinessAdminEarningReportResponseModel>();
            var businessId = HelperMethods.GetClaimInfo(Request, CustomClaimTypes.BusinessId).ToGuid();
            
            response.Data = await _businessAdminService.GetBusinessAdminEarningReportDataAsync(businessId);
            return Ok(response);
        }

        /// <summary>
        /// Get Worker Report
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "startDate": "2024-08-01",
        ///        "endDate": "2024-09-01"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getworkerreport")]
        public async Task<IActionResult> GetWorkerReport([FromBody]BusinessAdminReportRequestModel requestModel)
        {
            ResponseModel<List<BusinessAdminWorkerReportResponseModel>> response = new ResponseModel<List<BusinessAdminWorkerReportResponseModel>>();

            if (requestModel.startDate >= requestModel.endDate)
            {
                response.HasError = true;
                response.Message = Resource.Resource.StartDateNotBiggerEndDate;
            }
            
            if (requestModel.startDate.DifferenceBetweenDates(requestModel.endDate, DateType.Day) > 31)
            {
                response.HasError = true;
                response.Message = Resource.Resource.DateMaxRangeError;
            }

            if (response.HasError)
            {
                return Ok(response);
            }

            requestModel.businessId = HelperMethods.GetClaimInfo(Request, CustomClaimTypes.BusinessId)?.ToGuid();

            response.Data = await _businessAdminService.GetWorkerReportAsync(requestModel);

            return Ok(response);
        }
        
        /// <summary>
        /// Get Service Report
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "startDate": "2024-08-01",
        ///        "endDate": "2024-09-01"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getservicereport")]
        public async Task<IActionResult> GetServiceReport([FromBody]BusinessAdminReportRequestModel requestModel)
        {
            ResponseModel<BusinessAdminServiceReportResponseModel> response = new ResponseModel<BusinessAdminServiceReportResponseModel>();

            if (requestModel.startDate >= requestModel.endDate)
            {
                response.HasError = true;
                response.Message = Resource.Resource.StartDateNotBiggerEndDate;
            }
            
            if (requestModel.startDate.DifferenceBetweenDates(requestModel.endDate, DateType.Day) > 31)
            {
                response.HasError = true;
                response.Message = Resource.Resource.DateMaxRangeError;
            }

            if (response.HasError)
            {
                return Ok(response);
            }

            requestModel.businessId = HelperMethods.GetClaimInfo(Request, CustomClaimTypes.BusinessId)?.ToGuid();

            response.Data = await _businessAdminService.GetServiceReportAsync(requestModel);

            return Ok(response);
        }

        /// <summary>
        /// Get Appointment Report
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "startDate": "2024-08-01T00:00:00",
        ///        "endDate": "2024-08-01T23:59:59",
        ///        "page": 0,
        ///        "take": 5
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getappointmentreport")]
        public async Task<IActionResult> GetAppointmentReport([FromBody]BusinessAdminReportRequestModel requestModel)
        {
            ResponseModel<BusinessAdminAppointmentReportResponseModel> response = new ResponseModel<BusinessAdminAppointmentReportResponseModel>();

            if (requestModel.startDate >= requestModel.endDate)
            {
                response.HasError = true;
                response.Message = Resource.Resource.StartDateNotBiggerEndDate;
            }
            
            if (requestModel.startDate.DifferenceBetweenDates(requestModel.endDate, DateType.Day) > 31)
            {
                response.HasError = true;
                response.Message = Resource.Resource.DateMaxRangeError;
            }

            if (response.HasError)
            {
                return Ok(response);
            }

            requestModel.businessId = HelperMethods.GetClaimInfo(Request, CustomClaimTypes.BusinessId)?.ToGuid();

            response.Data = await _businessAdminService.GetAppointmentReportAsync(requestModel);

            return Ok(response);
        }

        /// <summary>
        /// Get Customers
        /// </summary>
        /// <returns></returns>
        [HttpPost("getcustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            ResponseModel<List<BusinessAdminCustomerResponseModel>> response = new ResponseModel<List<BusinessAdminCustomerResponseModel>>();
            var businessId = HelperMethods.GetClaimInfo(Request, CustomClaimTypes.BusinessId).ToGuid();

            response.Data = await _businessAdminService.GetCustomersAsync(businessId);
            return Ok(response);
        }


        /// <summary>
        /// Get Calendar Infos
        /// </summary>
        /// <returns></returns>
        [HttpPost("getcalendarinfos")]
        public async Task<IActionResult> GetCalendarInfos()
        {
            ResponseModel<List<BusinessAdminCalendarResponseModel>> response = new ResponseModel<List<BusinessAdminCalendarResponseModel>>();
            var businessId = HelperMethods.GetClaimInfo(Request, CustomClaimTypes.BusinessId).ToGuid();

            response.Data = await _businessAdminService.GetCalendarInfosAsync(businessId);
            return Ok(response);
        }
    }
}
