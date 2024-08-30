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
    [Route("businesscustomer")]
    public class BusinessCustomerController : ControllerBase
    {
        private readonly IBusinessCustomerRepository _businessCustomerRepository;

        public BusinessCustomerController(IBusinessCustomerRepository businessCustomerRepository)
        {
            _businessCustomerRepository = businessCustomerRepository;
        }


        /// <summary>
        /// Get BusinessCustomer By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbyid")]
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            ResponseModel<BusinessCustomer> response = new ResponseModel<BusinessCustomer>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessCustomerRepository.GetBusinessCustomerByIdAsync(id.ToGuid());
            return Ok(response);
        }

        /// <summary>
        /// Get BusinessCustomer By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbybusinessid")]
        public async Task<IActionResult> GetByBusinessId([FromBody] string id)
        {
            ResponseModel<List<BusinessCustomer>> response = new ResponseModel<List<BusinessCustomer>>();
            
            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessCustomerRepository.GetBusinessCustomerByBusinessIdAsync(id.ToGuid());
            return Ok(response);
        }

        /// <summary>
        /// Save BusinessCustomer
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "fullName": "Mert Demirkıran",
        ///        "email": "mertdmkrn37@gmail.com",
        ///        "telephone": "+905467335939",
        ///        "gender": 1
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save(BusinessCustomer businessCustomer)
        {
            ResponseModel<BusinessCustomer> response = new ResponseModel<BusinessCustomer>();

            if (businessCustomer.fullName.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("userName", Resource.Resource.NotEmpty));
            }

            if (businessCustomer.telephone.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephone", Resource.Resource.NotEmpty));
            }

            if (!businessCustomer.telephone.IsNullOrEmpty() && !businessCustomer.telephone.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephone",
                    Resource.Resource.ValidTelephoneMessage));
            }

            if (businessCustomer.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!businessCustomer.email.IsNullOrEmpty() && !businessCustomer.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email",
                    Resource.Resource.ValidEmailMessage));
            }
            
            if (response.HasError)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }
            
            businessCustomer.businessId = HelperMethods.GetClaimInfo(Request, CustomClaimTypes.BusinessId)?.ToGuid();

            response.Data = await _businessCustomerRepository.SaveBusinessCustomerAsync(businessCustomer);
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
        public async Task<IActionResult> Update(BusinessCustomer updateBusinessCustomer)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (updateBusinessCustomer.fullName.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("userName", Resource.Resource.NotEmpty));
            }

            if (updateBusinessCustomer.telephone.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephone", Resource.Resource.NotEmpty));
            }

            if (!updateBusinessCustomer.telephone.IsNullOrEmpty() && !updateBusinessCustomer.telephone.IsValidTelephoneNumber())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("telephone",
                    Resource.Resource.ValidTelephoneMessage));
            }

            if (updateBusinessCustomer.email.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email", Resource.Resource.NotEmpty));
            }

            if (!updateBusinessCustomer.email.IsNullOrEmpty() && !updateBusinessCustomer.email.IsValidEmail())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("email",
                    Resource.Resource.ValidEmailMessage));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RecordNotUpdated;
                return Ok(response);
            }

            var businessCustomer = await _businessCustomerRepository.GetBusinessCustomerByIdAsync(updateBusinessCustomer.id);

            if (businessCustomer == null)
            {
                response.Message = $"{updateBusinessCustomer.id} {Resource.Resource.RecordNotFound}";
                return Ok(response);
            }

            businessCustomer.email = updateBusinessCustomer.email.IsNull(businessCustomer.email);
            businessCustomer.fullName = updateBusinessCustomer.fullName.IsNull(businessCustomer.fullName);
            businessCustomer.telephone = updateBusinessCustomer.telephone.IsNull(businessCustomer.telephone);
            businessCustomer.gender = updateBusinessCustomer.gender;

            response.Data = await _businessCustomerRepository.UpdateBusinessCustomerAsync(businessCustomer);
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
            
            var businessCustomer = await _businessCustomerRepository.GetBusinessCustomerByIdAsync(id.ToGuid());

            if (businessCustomer == null)
            {
                response.HasError = true;
                response.Message = $"{id} {Resource.Resource.RecordNotFound}";
                return Ok(response);
            }

            response.Data = await _businessCustomerRepository.DeleteBusinessCustomerAsync(businessCustomer);
            response.Message = Resource.Resource.RecordDeleted;
            return Ok(response);
        }
    }
}
