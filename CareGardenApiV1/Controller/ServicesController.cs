using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private IServicesService _servicesService;

        public ServicesController()
        {
            _servicesService = new ServicesService();
        }

        /// <summary>
        /// Get Services
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("service/getall")]
        public async Task<IActionResult> GetServices()
        {
            ResponseModel<List<Services>> response = new ResponseModel<List<Services>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                response.Data = await _servicesService.GetServicesAsync();
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = "Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Get Service By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("service/getbyid")]
        public async Task<IActionResult> GetServicesById([FromBody] string id)
        {
            ResponseModel<Services> response = new ResponseModel<Services>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
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
                    response.Message = id + " id " + Resource.Resource.KayitBulunamadi;
                    return Ok(response);
                }

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = "Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Get Service By Name
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("service/getbyname")]
        public async Task<IActionResult> GetServicesByName([FromBody] string name)
        {
            ResponseModel<Services> response = new ResponseModel<Services>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
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
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Get Service By NameEn
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("service/getbynameen")]
        public async Task<IActionResult> GetServicesByNameEn([FromBody] string nameEn)
        {
            ResponseModel<Services> response = new ResponseModel<Services>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
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
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
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
        [HttpPost]
        [Route("service/save")]
        public async Task<IActionResult> Save([FromBody] Services services)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));
            
            try
            {
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

                if (!services.colorCode.IsNullOrEmpty() && (services.colorCode.Length % 3 != 0 || services.colorCode.Length > 6))
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

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi + " Exception => " + ex.Message;
                return Ok(response);
            }
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
        [HttpPost]
        [Route("service/update")]
        public async Task<IActionResult> Update([FromBody] Services updateServices)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {

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

                if (!updateServices.colorCode.IsNullOrEmpty() && (updateServices.colorCode.Length % 3 != 0 || updateServices.colorCode.Length > 6))
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
                    response.Message += updateServices.id + " id " + Resource.Resource.KayitBulunamadi;
                    return Ok(response);
                }

                services.name = updateServices.name;
                services.nameEn = updateServices.nameEn;
                services.className = updateServices.className;
                services.colorCode = updateServices.colorCode;
                services.sortOrder = updateServices.sortOrder;

                services = await _servicesService.UpdateServiceAsync(services);
                response.Data = true;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = Resource.Resource.GuncellemeYapilamadi + " Exception => " + ex.Message;
                return Ok(response);
            }
        }

        /// <summary>
        /// Delete Service (Role = 'Admin')
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("service/delete")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
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
                    response.Message += id + " id " + Resource.Resource.KayitBulunamadi;
                    return Ok(response);
                }

                response.Data = await _servicesService.DeleteServiceAsync(services);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitSilinemedi + " Exception => " + ex.Message;
                return Ok(response);
            }
        }
    }
}
