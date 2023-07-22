using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private IFavoriteService _favoriteService;
        private readonly ILoggerHandler _loggerHandler;

        public FavoriteController(ILoggerHandler loggerHandler)
        {
            _favoriteService = new FavoriteService();
            _loggerHandler = loggerHandler;
        }

        /// <summary>
        /// Get Favorite Businesses
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("favorite/get")]
        public async Task<IActionResult> GetFavoriteBusinesses()
        {
            ResponseModel<List<Favorite>> response = new ResponseModel<List<Favorite>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
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
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
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
        [HttpPost]
        [Route("favorite/add")]
        public async Task<IActionResult> AddFavorite([FromBody] Guid businessId)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
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
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
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
        [HttpPost]
        [Route("favorite/delete")]
        public async Task<IActionResult> DeleteFavorite([FromBody] Guid businessId)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
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
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message += "Exception => " + ex.Message;
                return Ok(response);
            }
        }
    }
}
