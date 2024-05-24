using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize(Roles = "Admin,Business")]
    [Route("workerserviceprice")]
    public class WorkerServicePriceController : ControllerBase
    {
        private readonly IWorkerServicePriceService _workerServicePriceService;
        private readonly IBusinessServicesService _businessServicesService;

        public WorkerServicePriceController(IWorkerServicePriceService workerServicePriceService, IBusinessServicesService businessServicesService)
        {
            _workerServicePriceService = workerServicePriceService;
            _businessServicesService = businessServicesService;
        }

        /// <summary>
        /// Get Worker Service Price By Id
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
            ResponseModel<WorkerServicePrice> response = new ResponseModel<WorkerServicePrice>();

            if (id.IsNullOrEmpty() || !id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _workerServicePriceService.GetWorkerServicePriceByIdAsync(id.ToGuid());
            return Ok(response);
        }

        /// <summary>
        /// Search Worker Service Prices
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        businessServiceId: "00000000-0000-0000-0000-000000000000",
        ///        workerId: "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] WorkerServicePrice workerServicePrice)
        {
            ResponseModel<List<WorkerServicePrice>> response = new ResponseModel<List<WorkerServicePrice>>();

            if (!workerServicePrice.businessServiceId.HasValue && !workerServicePrice.workerId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _workerServicePriceService.GetWorkerServicePricesSearchAsync(workerServicePrice.workerId, workerServicePrice.businessServiceId);
            return Ok(response);
        }
        
        /// <summary>
        /// Save Worker Servive Price
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "businessServiceId" : "00000000-0000-0000-0000-000000000000",
        ///        "workerId" : "00000000-0000-0000-0000-000000000000",
        ///        "price" : 1000
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] WorkerServicePrice workerServicePrice)
        {
            ResponseModel<WorkerServicePrice> response = new ResponseModel<WorkerServicePrice>();

            if (!workerServicePrice.businessServiceId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessServiceId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!workerServicePrice.workerId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("workerId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }
            
            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }
            
            var businessService = await _businessServicesService.GetBusinessServicePriceByIdAsync(workerServicePrice.businessServiceId.Value);

            if (businessService == null || businessService.price > workerServicePrice.price)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitYapilamadi; // Hata mesajı eklenecek.
                return Ok(response);
            }

            response.Data = await _workerServicePriceService.SaveWorkerServicePriceAsync(workerServicePrice);
            response.Message = Resource.Resource.KayitBasarili;
            return Ok(response);
        }

        /// <summary>
        /// Update Worker Service Price
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
        ///        "price" : 1000
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] WorkerServicePrice workerServicePrice)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
           
            response.Data = await _workerServicePriceService.UpdateWorkerServicePriceAsync(workerServicePrice);
            response.Message = Resource.Resource.KayitBasarili;
            return Ok(response);
        }

        /// <summary>
        /// Delete Worker Service Price By Id
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
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] string? id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (id.IsNullOrEmpty() || !id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _workerServicePriceService.DeleteWorkerServicePriceAsync(id.ToGuid());
            response.Message = Resource.Resource.KayitSilindi;

            return Ok(response);
        }
    }
}
