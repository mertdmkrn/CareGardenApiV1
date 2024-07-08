using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("businessproperties")]
    public class BusinessPropertiesController : ControllerBase
    {
        private readonly IBusinessPropertiesService _businessPropertiesService;

        public BusinessPropertiesController(IBusinessPropertiesService businessPropertiesService)
        {
            _businessPropertiesService = businessPropertiesService;
        }


        /// <summary>
        /// Get BusinessProperties By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbyid")]
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            ResponseModel<BusinessProperties> response = new ResponseModel<BusinessProperties>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessPropertiesService.GetBusinessPropertiesByIdAsync(id.ToGuid());
            return Ok(response);
        }

        /// <summary>
        /// Get BusinessProperties By Business Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbybusinessid")]
        public async Task<IActionResult> GetByBusinessId([FromBody] string businessId)
        {
            ResponseModel<List<BusinessProperties>> response = new ResponseModel<List<BusinessProperties>>();

            if (!businessId.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessPropertiesService.GetBusinessPropertiesByBusinessIdAsync(businessId.ToGuid());
            return Ok(response);
        }


        /// <summary>
        /// Get BusinessProperties By Business Id And Key
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbybusinessidwithkey")]
        public async Task<IActionResult> GetByBusinessIdWithKey([FromBody] string businessId, string key)
        {
            ResponseModel<BusinessProperties> response = new ResponseModel<BusinessProperties>();

            if (!businessId.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.IdErrorMessage;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessPropertiesService.GetBusinessPropertiesByBusinessIdAndKeyAsync(businessId.ToGuid(), key);
            return Ok(response);
        }

        /// <summary>
        /// Save BusinessProperties
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "key" : "WhatsappUrl",
        ///        "address" : "https://web.whatsapp.com/",
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save(BusinessProperties businessProperties)
        {
            ResponseModel<BusinessProperties> response = new ResponseModel<BusinessProperties>();

            if (businessProperties.key.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("key", Resource.Resource.NotEmpty));
            }

            if (businessProperties.value.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("value", Resource.Resource.NotEmpty));
            }

            if (!businessProperties.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }

            var data = await _businessPropertiesService.GetBusinessPropertiesByBusinessIdAndKeyAsync(businessProperties.businessId.Value, businessProperties.key);

            if (data != null)
            {
                response.HasError = true;
                response.Message = string.Format(Resource.Resource.RecordExists, businessProperties.key);
                return Ok(response);
            }

            response.Data = await _businessPropertiesService.SaveBusinessPropertiesAsync(businessProperties);
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
        ///        "key" : "FacebookUrl",
        ///        "address" : "https://web.whatsapp.com/",
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> Update(BusinessProperties updateBusinessProperties)
        {
            ResponseModel<BusinessProperties> response = new ResponseModel<BusinessProperties>();

            if (updateBusinessProperties.key.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("key", Resource.Resource.NotEmpty));
            }

            if (updateBusinessProperties.value.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("value", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RecordNotUpdated;
                return Ok(response);
            }

            var businessProperties = await _businessPropertiesService.GetBusinessPropertiesByIdAsync(updateBusinessProperties.id);

            if (businessProperties == null)
            {
                response.Message = $"{updateBusinessProperties.id} {Resource.Resource.RecordNotFound}";
                return Ok(response);
            }

            if (businessProperties.key != updateBusinessProperties.key)
            {
                var data = await _businessPropertiesService.GetBusinessPropertiesByBusinessIdAndKeyAsync(businessProperties.businessId.Value, updateBusinessProperties.key);

                if (data != null)
                {
                    response.HasError = true;
                    response.Message = string.Format(Resource.Resource.RecordExists, businessProperties.key);
                    return Ok(response);
                }
            }

            businessProperties.key = updateBusinessProperties.key;
            businessProperties.value = updateBusinessProperties.value;

            response.Data = await _businessPropertiesService.UpdateBusinessPropertiesAsync(businessProperties);
            response.Message = Resource.Resource.RegistrationSuccess;

            return Ok(response);
        }

        /// <summary>
        /// Delete BusinessProperties
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] BusinessProperties businessProperties)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            response.Data = await _businessPropertiesService.DeleteBusinessPropertiesAsync(businessProperties);
            response.Message = Resource.Resource.RecordDeleted;
            return Ok(response);
        }
    }
}
