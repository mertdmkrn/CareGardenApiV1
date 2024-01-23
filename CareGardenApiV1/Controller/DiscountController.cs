using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("discount")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IFileHandler _fileHandler;
        private readonly ILoggerHandler _loggerHandler;

        public DiscountController(
            IDiscountService discountService,
            IFileHandler fileHandler,
            ILoggerHandler loggerHandler)
        {
            _discountService = discountService;
            _fileHandler = fileHandler;
            _loggerHandler = loggerHandler;
        }


        /// <summary>
        /// Get Discount By Business Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "businessId": "00000000-0000-0000-0000-000000000000",
        ///        "isActive": true
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getbybusinessid")]
        public async Task<IActionResult> GetByBusinessId([FromBody] Discount discount)
        {
            ResponseModel<List<Discount>> response = new ResponseModel<List<Discount>>();

            if (!discount.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }


            response.Data = discount.isActive
                    ? await _discountService.GetActiveDiscountsByBusinessIdAsync(discount.businessId)
                    : await _discountService.GetDiscountsByBusinessIdAsync(discount.businessId);

            return Ok(response);
        }

        /// <summary>
        /// Save Discount
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "businessId": "00000000-0000-0000-0000-000000000000",
        ///        "isActive": true,
        ///        "description": "Bugün tüm servislerde",
        ///        "descriptionEn": "Every services today",
        ///        "rate": 20,
        ///        "type": 0
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] Discount discount)
        {
            ResponseModel<Discount> response = new ResponseModel<Discount>();

            if (!discount.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdParametreHatasi));
            }

            if (discount.description.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("description", Resource.Resource.BuAlaniBosBirakmayiniz));
            }


            if (discount.descriptionEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("descriptionEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (discount.rate <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("rate", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            response.Data = await _discountService.SaveDiscountAsync(discount);
            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
        }

        /// <summary>
        /// Update Discount
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id": "00000000-0000-0000-0000-000000000000",
        ///        "isActive": false,
        ///        "description": "Bugün tüm servislerde",
        ///        "descriptionEn": "Every services today",
        ///        "rate": 20,
        ///        "type": 0
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Discount updateDiscount)
        {
            ResponseModel<Discount> response = new ResponseModel<Discount>();

            if (updateDiscount.description.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("description", Resource.Resource.BuAlaniBosBirakmayiniz));
            }


            if (updateDiscount.descriptionEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("descriptionEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateDiscount.rate <= 0)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("rate", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }


            var discount = await _discountService.GetDiscountByIdAsync(updateDiscount.id);

            if (discount == null)
            {
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            discount.description = updateDiscount.description;
            discount.descriptionEn = updateDiscount.descriptionEn;
            discount.serviceIds = updateDiscount.serviceIds;
            discount.isActive = updateDiscount.isActive;
            discount.rate = updateDiscount.rate;
            discount.type = updateDiscount.type;

            response.Data = await _discountService.UpdateDiscountAsync(discount);
            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
        }

        /// <summary>
        /// Delete Discount
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
        public async Task<IActionResult> DeleteByBusinessId([FromBody] string? id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (id.IsNullOrEmpty() || !id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            var discount = await _discountService.GetDiscountByIdAsync(id.ToGuid());

            if (discount == null)
            {
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _discountService.DeleteDiscountAsync(discount);
            response.Message = Resource.Resource.KayitSilindi;
            return Ok(response);
        }
    }
}
