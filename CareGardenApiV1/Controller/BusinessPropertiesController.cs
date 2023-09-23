using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    public class BusinessPropertiesController : ControllerBase
    {
        private IBusinessPropertiesService _businessPropertiesService;
        private readonly ILoggerHandler _loggerHandler;

        public BusinessPropertiesController(ILoggerHandler loggerHandler)
        {
            _businessPropertiesService = new BusinessPropertiesService();
            _loggerHandler = loggerHandler;
        }

        /// <summary>
        /// Get BusinessProperties By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("businessproperties/getbyid")]
        public async Task<IActionResult> GetBusinessPropertiesById([FromBody] string id)
        {
            ResponseModel<BusinessProperties> response = new ResponseModel<BusinessProperties>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (!id.IsGuid())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                    response.Message += Resource.Resource.IdParametreHatasi;
                }

                if (response.HasError)
                    return Ok(response);

                response.Data = await _businessPropertiesService.GetBusinessPropertiesByIdAsync(id.ToGuid());
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }

        }

        /// <summary>
        /// Get BusinessProperties By Business Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("businessproperties/getbybusinessid")]
        public async Task<IActionResult> GetBusinessPropertiesByBusinessId([FromBody] string businessId)
        {
            ResponseModel<List<BusinessProperties>> response = new ResponseModel<List<BusinessProperties>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (!businessId.IsGuid())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdParametreHatasi));
                    response.Message += Resource.Resource.IdParametreHatasi;
                }

                if (response.HasError)
                    return Ok(response);

                response.Data = await _businessPropertiesService.GetBusinessPropertiesByBusinessIdAsync(businessId.ToGuid());
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }

        }


        /// <summary>
        /// Get BusinessProperties By Business Id And Key
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("businessproperties/getbybusinessidwithkey")]
        public async Task<IActionResult> GetBusinessPropertiesByBusinessIdWithKey([FromBody] string businessId, string key)
        {
            ResponseModel<BusinessProperties> response = new ResponseModel<BusinessProperties>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (!businessId.IsGuid())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdParametreHatasi));
                    response.Message += Resource.Resource.IdParametreHatasi;
                }

                if (response.HasError)
                    return Ok(response);

                response.Data = await _businessPropertiesService.GetBusinessPropertiesByBusinessIdAndKeyAsync(businessId.ToGuid(), key);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }

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
        [HttpPost]
        [Route("businessproperties/save")]
        public async Task<IActionResult> Save(BusinessProperties businessProperties)
        {
            ResponseModel<BusinessProperties> response = new ResponseModel<BusinessProperties>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (businessProperties.key.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("key", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (businessProperties.value.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("value", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.GuncellemeYapilamadi;
                    return Ok(response);
                }

                response.Data = await _businessPropertiesService.SaveBusinessPropertiesAsync(businessProperties);
                response.Message = Resource.Resource.KayitBasarili;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
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
        [HttpPost]
        [Route("businessproperties/update")]
        public async Task<IActionResult> Update(BusinessProperties updateBusinessProperties)
        {
            ResponseModel<BusinessProperties> response = new ResponseModel<BusinessProperties>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (updateBusinessProperties.key.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("key", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (updateBusinessProperties.value.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("value", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.GuncellemeYapilamadi;
                    return Ok(response);
                }

                var businessProperties = await _businessPropertiesService.GetBusinessPropertiesByIdAsync(updateBusinessProperties.id);

                if (businessProperties == null)
                {
                    response.Message = updateBusinessProperties.id + " " + Resource.Resource.KayitBulunamadi;
                    return Ok(response);
                }

                businessProperties.key = updateBusinessProperties.key;
                businessProperties.value = updateBusinessProperties.value;

                response.Data = await _businessPropertiesService.UpdateBusinessPropertiesAsync(businessProperties);
                response.Message = Resource.Resource.KayitBasarili;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Delete BusinessProperties
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("businessproperties/delete")]
        public async Task<IActionResult> Delete([FromBody] BusinessProperties businessProperties)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                response.Data = await _businessPropertiesService.DeleteBusinessPropertiesAsync(businessProperties);
                response.Message = Resource.Resource.KayitSilindi;
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = Resource.Resource.KayitSilinemedi + " Exception => " + ex.Message;
                return Ok(response);
            }
        }
    }
}
