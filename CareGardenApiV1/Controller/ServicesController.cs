using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Route("service")]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;
        private readonly ILoggerHandler _loggerHandler;
        private readonly IMemoryCache _memoryCache;
        private const string cacheKey = "services";

        public ServicesController(
            IServicesService servicesService,
            ILoggerHandler loggerHandler,
            IMemoryCache memoryCache)
        {
            _servicesService = servicesService;
            _loggerHandler = loggerHandler;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Get Services
        /// </summary>
        /// <returns></returns>
        [HttpPost("getall")]
        public async Task<IActionResult> GetAll()
        {
            ResponseModel<List<Services>> response = new ResponseModel<List<Services>>();

            if (_memoryCache.TryGetValue(cacheKey, out object list))
            {
                response.Data = (List<Services>)list;
            }
            else
            {
                response.Data = await _servicesService.GetServicesAsync();
                _memoryCache.Set(cacheKey, response.Data, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal
                });
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Service By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbyid")]
        public async Task<IActionResult> GetById([FromBody] string id)
        {
            ResponseModel<Services> response = new ResponseModel<Services>();

            if (!id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.IdParametreHatasi;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _servicesService.GetServiceByIdAsync(id.ToGuid());

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = $"{id} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Service By Name
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbyname")]
        public async Task<IActionResult> GetByName([FromBody] string name)
        {
            ResponseModel<Services> response = new ResponseModel<Services>();

            if (name.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.BuAlaniBosBirakmayiniz));
                response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _servicesService.GetServiceByNameAsync(name);

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Service By NameEn
        /// </summary>
        /// <returns></returns>
        [HttpPost("getbynameen")]
        public async Task<IActionResult> GetByNameEn([FromBody] string nameEn)
        {
            ResponseModel<Services> response = new ResponseModel<Services>();

            if (nameEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("nameEn", Resource.Resource.BuAlaniBosBirakmayiniz));
                response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
            }

            if (response.HasError)
                return Ok(response);

            response.Data = await _servicesService.GetServiceByNameEnAsync(nameEn);

            if (response.Data == null)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Save Service (Role = 'Admin')
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "name" : "Makyaj",
        ///        "nameEn": "Make Up",
        ///        "className": "makeup",
        ///        "colorCode": "358541",
        ///        "sortOrder": 1
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] Services services)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (services.name.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (services.nameEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("nameEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (services.className.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("className", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (services.colorCode.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("colorCode", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!services.colorCode.Replace("#", "").IsNullOrEmpty() && (services.colorCode.Replace("#", "").Length % 3 != 0 || services.colorCode.Replace("#", "").Length > 6))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("colorCode", Resource.Resource.RenkKoduHatasi));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            Services service = await _servicesService.SaveServiceAsync(services);
            response.Data = true;
            _memoryCache.Remove(cacheKey);

            return Ok(response);
        }


        /// <summary>
        /// Update Service (Role = 'Admin')
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
        ///        "name" : "Makyaj",
        ///        "nameEn": "Make Up",
        ///        "className": "makeup",
        ///        "colorCode": "564564",
        ///        "sortOrder": 1
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Services updateServices)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (updateServices.name.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateServices.nameEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("nameEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateServices.className.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("className", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateServices.colorCode.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("colorCode", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!updateServices.colorCode.Replace("#", "").IsNullOrEmpty() && (updateServices.colorCode.Replace("#", "").Length % 3 != 0 || updateServices.colorCode.Replace("#", "").Length > 6))
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("colorCode", Resource.Resource.RenkKoduHatasi));
            }
            if (response.HasError)
            {
                response.Message = Resource.Resource.GuncellemeYapilamadi;
                return Ok(response);
            }

            Services services = await _servicesService.GetServiceByIdAsync(updateServices.id);

            if (services == null)
            {
                response.HasError = true;
                response.Message += $"{updateServices.id} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            services.name = updateServices.name;
            services.nameEn = updateServices.nameEn;
            services.className = updateServices.className;
            services.colorCode = updateServices.colorCode;
            services.sortOrder = updateServices.sortOrder;

            services = await _servicesService.UpdateServiceAsync(services);
            response.Data = true;
            _memoryCache.Remove(cacheKey);

            return Ok(response);
        }

        /// <summary>
        /// Delete Service (Role = 'Admin')
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
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

            Services services = await _servicesService.GetServiceByIdAsync(id.ToGuid());

            if (services == null)
            {
                response.HasError = true;
                response.Message += $"{id} id {Resource.Resource.KayitBulunamadi}";
                return Ok(response);
            }

            response.Data = await _servicesService.DeleteServiceAsync(services);
            _memoryCache.Remove(cacheKey);

            return Ok(response);
        }
    }
}
