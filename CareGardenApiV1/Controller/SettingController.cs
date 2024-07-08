using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Route("setting")]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;
        private readonly IMemoryCache _memoryCache;

        private const string cacheKey = "settings";


        public SettingController(ISettingService settingService, IMemoryCache memoryCache)
        {
            _settingService = settingService;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Get All Settings
        /// </summary>
        /// <returns></returns>
        [HttpPost("getall")]
        public async Task<IActionResult> GetAll()
        {
            ResponseModel<IList<Setting>> response = new ResponseModel<IList<Setting>>();

            if (_memoryCache.TryGetValue(cacheKey, out object list))
            {
                response.Data = (IList<Setting>)list;
            }
            else
            {
                response.Data = await _settingService.GetSettingsAsync();
                _memoryCache.Set(cacheKey, response.Data, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal
                });
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Setting By Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        /// "00000000-0000-0000-0000-000000000000"
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getbyid")]
        public async Task<IActionResult> GetById([FromBody] string? id)
        {
            ResponseModel<Setting> response = new ResponseModel<Setting>();

            if (id.IsNullOrEmpty() || !id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.ErrorMessage;
                return Ok(response);
            }

            if (_memoryCache.TryGetValue(cacheKey, out object list))
            {
                var settings = (IList<Setting>)list;
                response.Data = settings.FirstOrDefault(x => x.id == id.ToGuid());
            }
            else
            {
                response.Data = await _settingService.GetSettingByIdAsync(id.ToGuid());
            }

            return Ok(response);
        }

        /// <summary>
        /// Get Setting By Name
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        /// 
        /// "MailSubject"
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getbyname")]
        public async Task<IActionResult> GetByName([FromBody] string? name)
        {
            if (name.IsNullOrEmpty())
            {
                ResponseModel<string> response = new ResponseModel<string>();
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.ErrorMessage;
                return Ok(response);
            }

            Setting setting = null;


            if (_memoryCache.TryGetValue(cacheKey, out object list))
            {
                var settings = (IList<Setting>)list;
                setting = settings.FirstOrDefault(x => x.name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                setting = await _settingService.GetSettingByNameAsync(name);
            }


            if (setting == null)
            {
                ResponseModel<string> response = new ResponseModel<string>();
                response.HasError = true;
                response.Message = Resource.Resource.RecordNotFound;
                return Ok(response);
            }

            var language = HttpContext.Request.Headers["Language"].ToString().IsNull("en");

            if (setting.type == SettingType.String)
            {
                ResponseModel<string> response = new();
                var valueArr = setting.value.Split('|', StringSplitOptions.RemoveEmptyEntries);

                response.Data = Extensions.GetLangugeValue(language, valueArr.FirstOrDefault(), valueArr.LastOrDefault());
                return Ok(response);
            }
            else if (setting.type == SettingType.ListString)
            {
                ResponseModel<List<string>> response = new();
                response.Data = new List<string>();
                var valuesArr = setting.value.Split('~', StringSplitOptions.RemoveEmptyEntries);

                foreach (var values in valuesArr)
                {
                    var valueArr = values.Trim().Split('|', StringSplitOptions.RemoveEmptyEntries);
                    response.Data.Add(Extensions.GetLangugeValue(language, valueArr.FirstOrDefault(), valueArr.LastOrDefault()));
                }

                return Ok(response);
            }
            else if (setting.type == SettingType.Integer)
            {
                ResponseModel<int> response = new();
                response.Data = setting.value.ToInt(0);

                return Ok(response);
            }
            else if (setting.type == SettingType.ListInteger)
            {
                ResponseModel<List<int>> response = new();

                response.Data = setting.value.Split('~', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => x.ToInt(0))
                                    .ToList();

                return Ok(response);
            }
            else if (setting.type == SettingType.Boolean)
            {
                ResponseModel<bool> response = new();

                response.Data = setting.value.ToBoolean();
                return Ok(response);
            }
            else
            {
                ResponseModel<string> response = new();

                response.Data = "";
                return Ok(response);
            }
        }

        /// <summary>
        /// Save Setting
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "name" : "Title",
        ///        "description" : "Mobil uygulama başlık.",
        ///        "type" : 0,
        ///        "value" : "CareGarden"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] Setting setting)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (setting.name.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.NotEmpty));
            }

            if (setting.description.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("description", Resource.Resource.NotEmpty));
            }

            if (setting.value.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("valuw", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }

            response.Data = await _settingService.SaveSettingAsync(setting);
            response.Message = Resource.Resource.RegistrationSuccess;
            _memoryCache.Remove(cacheKey);

            return Ok(response);
        }

        /// <summary>
        /// Update Worker
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id": "00000000-0000-0000-0000-000000000000",
        ///        "value": "Care Garden"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Setting updateSetting)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (updateSetting.value.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("value", Resource.Resource.NotEmpty));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.RegistrationFailed;
                return Ok(response);
            }

            Setting setting = null;


            if (_memoryCache.TryGetValue(cacheKey, out object list))
            {
                var settings = (IList<Setting>)list;
                setting = settings.FirstOrDefault(x => x.id == updateSetting.id);
            }
            else
            {
                setting = await _settingService.GetSettingByIdAsync(updateSetting.id);
            }

            if (setting == null)
            {
                response.Message = Resource.Resource.RecordNotFound;
                return Ok(response);
            }

            setting.value = updateSetting.value;   

            response.Data = await _settingService.UpdateSettingAsync(setting);
            response.Message = Resource.Resource.RegistrationSuccess;
            _memoryCache.Remove(cacheKey);

            return Ok(response);
        }

        /// <summary>
        /// Delete Setting By Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        /// "00000000-0000-0000-0000-000000000000"
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] string? id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (id.IsNullOrEmpty() || !id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdErrorMessage));
                response.Message = Resource.Resource.RecordNotFound;
                return Ok(response);
            }

            response.Data = await _settingService.DeleteSettingAsync(id.ToGuid());
            response.Message = Resource.Resource.RecordDeleted;
            _memoryCache.Remove(cacheKey);

            return Ok(response);
        }
    }
}
