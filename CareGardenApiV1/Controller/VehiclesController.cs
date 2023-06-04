using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X500;
using System;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    public class VehiclesController : ControllerBase
    {

        /// <summary>
        /// Get Cities
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("vehicles/getcities")]
        public async Task<IActionResult> GetCities()
        {
            ResponseModel<List<string>> response = new ResponseModel<List<string>>();
            response.Data = Constants.Cities;

            return Ok(response);
        }
    }
}
