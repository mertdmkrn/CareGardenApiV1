using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("favorite")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly ILoggerHandler _loggerHandler;

        public FavoriteController(
            IFavoriteService favoriteService,
            ILoggerHandler loggerHandler)
        {
            _favoriteService = favoriteService;
            _loggerHandler = loggerHandler;
        }


        /// <summary>
        /// Get Favorite Businesses
        /// </summary>
        /// <returns></returns>
        [HttpPost("get")]
        public async Task<IActionResult> Get()
        {
            ResponseModel<List<Favorite>> response = new ResponseModel<List<Favorite>>();

            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            if (userId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            response.Data = await _favoriteService.GetFavoritesByUserIdAsync(userId.ToGuid());

            return Ok(response);
        }

        /// <summary>
        /// Add Favorite
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
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Guid businessId)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            if (userId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            Favorite favorite = new Favorite()
            {
                userId = userId.ToGuidNullable(),
                businessId = businessId,
            };

            await _favoriteService.SaveFavoriteAsync(favorite);

            response.Data = true;
            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
        }


        /// <summary>
        /// Delete Favorite
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid businessId)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

            if (userId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            await _favoriteService.DeleteFavoriteByBusinessIdAndUserIdAsync(userId.ToGuid(), businessId);

            response.Data = true;
            response.Message = Resource.Resource.KayitSilindi;

            return Ok(response);
        }
    }
}
