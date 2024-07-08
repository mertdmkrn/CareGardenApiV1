using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("favorite")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        /// <summary>
        /// Get Favorite Businesses By Session User
        /// </summary>
        /// <returns></returns>
        [HttpPost("get")]
        public async Task<IActionResult> Get()
        {
            ResponseModel<List<Favorite>> response = new ResponseModel<List<Favorite>>();

            var userId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

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

            Favorite favorite = new Favorite()
            {
                userId = userId.ToGuidNullable(),
                businessId = businessId,
            };

            await _favoriteService.SaveFavoriteAsync(favorite);

            response.Data = true;
            response.Message = Resource.Resource.RegistrationSuccess;

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

            await _favoriteService.DeleteFavoriteByBusinessIdAndUserIdAsync(userId.ToGuid(), businessId);

            response.Data = true;
            response.Message = Resource.Resource.RecordDeleted;

            return Ok(response);
        }
    }
}
