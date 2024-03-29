using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Route("businessservice")]
    public class BusinessServicesController : ControllerBase
    {
        private readonly IBusinessServicesService _businessServicesService;
        private readonly IBusinessService _businessService;
        public BusinessServicesController(IBusinessServicesService businessServicesService, IBusinessService businessService)
        {
            _businessServicesService = businessServicesService;
            _businessService = businessService;
        }


        /// <summary>
        /// Get Service By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbyid")]
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            ResponseModel<BusinessServiceModel> response = new ResponseModel<BusinessServiceModel>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessServicesService.GetBusinessServiceByIdAsync(id.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = $"{id} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Service By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbyserviceid")]
        public async Task<IActionResult> GetByServiceId([FromBody] string serviceId)
        {
            ResponseModel<List<BusinessServiceModel>> response = new ResponseModel<List<BusinessServiceModel>>();

            if (!serviceId.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("serviceId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessServicesService.GetBusinessServicesByServiceIdAsync(serviceId.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = $"{serviceId} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Service By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbybusinessid")]
        public async Task<IActionResult> GetByBusinessId([FromBody] string businessId)
        {
            ResponseModel<List<BusinessServiceModel>> response = new ResponseModel<List<BusinessServiceModel>>();

            if (!businessId.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("serviceId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _businessServicesService.GetBusinessServicesByBusinessIdAsync(businessId.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = $"{businessId} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Save Business Service
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///        "serviceId": "00000000-0000-0000-0000-000000000000",
        ///        "name": "Saç Kesim",
        ///        "nameEn": "Hair Cut",
        ///        "spot": "Tam isteğinize göre bir saç kesime hazır olun.",
        ///        "spotEn": "Get ready for a haircut just to your liking.",
        ///        "minDuration": 30,
        ///        "maxDuration": 45,
        ///        "price": 150
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] BusinessServiceModel businessService)
        {
            ResponseModel<BusinessServiceModel> response = new ResponseModel<BusinessServiceModel>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            if (!businessService.serviceId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("serviceId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!businessService.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (businessService.name.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (businessService.nameEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("nameEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (businessService.spot.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("spot", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (businessService.spotEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("spotEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (businessService.minDuration <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("minDuration", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (businessService.price <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("price", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            businessService.maxDuration = businessService.maxDuration.IsNull(businessService.minDuration);

            response.Message = Resource.Resource.KayitBasarili;
            response.Data = await _businessServicesService.SaveBusinessServiceAsync(businessService);

            BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(businessService.businessId.Value));

            return Ok(response);
        }


        /// <summary>
        /// Update Business Service
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///        "serviceId": "00000000-0000-0000-0000-000000000000",
        ///        "name": "Saç Kesim",
        ///        "nameEn": "Hair Cut",
        ///        "spot": "Tam isteğinize göre bir saç kesime hazır olun.",
        ///        "spotEn": "Get ready for a haircut just to your liking.",
        ///        "minDuration": 30,
        ///        "maxDuration": 45,
        ///        "price": 150,
        ///        "isPopular": true
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] BusinessServiceModel updateBusinessService)
        {
            ResponseModel<BusinessServiceModel> response = new ResponseModel<BusinessServiceModel>();

            if (!updateBusinessService.serviceId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("serviceId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!updateBusinessService.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateBusinessService.name.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateBusinessService.nameEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("nameEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateBusinessService.spot.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("spot", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateBusinessService.spotEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("spotEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateBusinessService.minDuration <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("minDuration", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateBusinessService.price <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("price", Resource.Resource.BuAlaniBosBirakmayiniz));
            }


            if (response.HasError)
            {
                response.Message = Resource.Resource.GuncellemeYapilamadi;
                return Ok(response);
            }

            BusinessServiceModel businessService = await _businessServicesService.GetBusinessServiceByIdAsync(updateBusinessService.id);

            if (businessService == null)
            {
                response.HasError = true;
                response.Message += $"{updateBusinessService.id} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            businessService.serviceId = updateBusinessService.serviceId;
            businessService.businessId = updateBusinessService.businessId;
            businessService.name = updateBusinessService.name;
            businessService.nameEn = updateBusinessService.nameEn;
            businessService.spot = updateBusinessService.spot;
            businessService.spotEn = updateBusinessService.spotEn;
            businessService.minDuration = updateBusinessService.minDuration;
            businessService.maxDuration = updateBusinessService.maxDuration.IsNull(updateBusinessService.minDuration);
            businessService.price = updateBusinessService.price;
            businessService.isPopular = updateBusinessService.isPopular;

            response.Message = Resource.Resource.KayitBasarili;
            response.Data = await _businessServicesService.UpdateBusinessServiceAsync(businessService);

            BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(businessService.businessId.Value));

            return Ok(response);
        }

        /// <summary>
        /// Delete BusinessService
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitSilinemedi;
                return Ok(response);
            }

            await _businessServicesService.DeleteBusinessServiceAsync(id.ToGuid());

            response.Message = Resource.Resource.KayitSilindi;
            response.Data = true;

            return Ok(response);
        }
    }
}
