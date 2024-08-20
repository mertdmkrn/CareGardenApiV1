using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize(Roles = "BusinessAdmin,Business,BusinessWorker")]
    [Route("businesspayment")]
    public class BusinessPaymentController : ControllerBase
    {
        private readonly IBusinessPaymentRepository _businessPaymentRepository;

        public BusinessPaymentController(IBusinessPaymentRepository businessPaymentRepository)
        {
            _businessPaymentRepository = businessPaymentRepository;
        }


        /// <summary>
        /// Get BusinessPayment By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbyid")]
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            ResponseModel<BusinessPayment> response = new ResponseModel<BusinessPayment>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessPaymentRepository.GetBusinessPaymentByIdAsync(id.ToGuid());
            return Ok(response);
        }

        /// <summary>
        /// Search BusinessPayment
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbybusinessid")]
        public async Task<IActionResult> Search([FromBody] BusinessPaymentSearchRequestModel requestModel)
        {
            ResponseModel<List<BusinessPayment>> response = new ResponseModel<List<BusinessPayment>>();

            response.Data = await _businessPaymentRepository.SearchBusinessPaymentAsync(requestModel);
            return Ok(response);
        }

        /// <summary>
        /// Save BusinessPayment
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "description": "Açıklama.",
        ///        "appointmentId": "00000000-0000-0000-0000-000000000000",
        ///        "date": "2024-08-20"
        ///        "amount": 5000
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save(BusinessPayment businessPayment)
        {
            ResponseModel<BusinessPayment> response = new ResponseModel<BusinessPayment>();

            if (businessPayment.amount <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("amount", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }

            businessPayment.businessUserId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid)?.ToGuid();
            businessPayment.businessId = HelperMethods.GetClaimInfo(Request, CustomClaimTypes.BusinessId)?.ToGuid();

            response.Data = await _businessPaymentRepository.SaveBusinessPaymentAsync(businessPayment);
            response.Message = Resource.Resource.RegistrationSuccess;

            return Ok(response);
        }

        /// <summary>
        /// Update BusinessProperties
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
        ///        "description": "Açıklama.",
        ///        "date": "2024-08-20"
        ///        "amount": 5000
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> Update(BusinessPayment updateBusinessPayment)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (updateBusinessPayment.amount <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("amount", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RecordNotUpdated;
                return Ok(response);
            }

            var businessPayment = await _businessPaymentRepository.GetBusinessPaymentByIdAsync(updateBusinessPayment.id);

            if (businessPayment == null)
            {
                response.Message = $"{updateBusinessPayment.id} {Resource.Resource.RecordNotFound}";
                return Ok(response);
            }

            businessPayment.description = updateBusinessPayment.description.IsNull(businessPayment.description);
            businessPayment.date = updateBusinessPayment.date;
            businessPayment.businessUserId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid)?.ToGuid();
            businessPayment.amount = updateBusinessPayment.amount;

            response.Data = await _businessPaymentRepository.UpdateBusinessPaymentAsync(businessPayment);
            response.Message = Resource.Resource.RegistrationSuccess;

            return Ok(response);
        }

        /// <summary>
        /// Delete BusinessProperties
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }
            
            var businessPayment = await _businessPaymentRepository.GetBusinessPaymentByIdAsync(id.ToGuid());

            if (businessPayment == null)
            {
                response.HasError = true;
                response.Message = $"{id} {Resource.Resource.RecordNotFound}";
                return Ok(response);
            }

            response.Data = await _businessPaymentRepository.DeleteBusinessPaymentAsync(businessPayment);
            response.Message = Resource.Resource.RecordDeleted;
            return Ok(response);
        }
    }
}
