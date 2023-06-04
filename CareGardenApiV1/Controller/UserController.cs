using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private ITokenHandler _tokenHandler;
        private ISmsHandler _smsHandler;

        public UserController()
        {
            _userService = new UserService();
            _tokenHandler = new TokenHandler();
            _smsHandler = new SmsHandler();
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user/getbyid")]
        public async Task<IActionResult> GetUserById([FromBody] int id)
        {
            ResponseModel<User> response = new ResponseModel<User>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (id == 0)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                    response.Message += Resource.Resource.IdParametreHatasi;
                }

                if (response.HasError)
                    return BadRequest(response);

                response.Data = await _userService.GetUserById(id);

                if (response.Data == null)
                {
                    response.HasError = true;
                    response.Message += id + " id " + Resource.Resource.KullaniciBulunamadi;
                    return NotFound(response);
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
        /// Get User
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user/get")]
        public async Task<IActionResult> GetUser()
        {
            ResponseModel<User> response = new ResponseModel<User>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                var tokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

                if (token == null)
                { 
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return NotFound(response);
                }

                var userId = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value?.ToString();

                if (userId.IsNullOrEmpty()) 
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return NotFound(response);
                }

                var user = await _userService.GetUserById(userId.ToInt());
                
                if (user == null)
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return NotFound(response);
                }

                response.Data = user;

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
        /// Delete User
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user/delete")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));
            
            try
            {
                if (id == 0)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.KayitSilinemedi;
                    return BadRequest(response);
                }

                User user = await _userService.GetUserById(id);

                if (user == null)
                {
                    response.HasError = true;
                    response.Message = id + " id " + Resource.Resource.KullaniciBulunamadi;
                    return NotFound(response);
                }

                response.Data = await _userService.DeleteUserAsync(user);

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
        /// Update User
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : 1,
        ///        "fullName" : "Mert DEMİRKIRAN",
        ///        "birthDate": "1998-08-01",
        ///        "gender": 1,
        ///        "city": "İstanbul",
        ///        "services": "Make Up;Beauty Saloon"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("user/update")]
        public async Task<IActionResult> Update([FromBody] User updateUser)
        {
            ResponseModel<User> response = new ResponseModel<User>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (updateUser.fullName.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("firstname", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (!updateUser.birthDate.HasValue)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("birthDate", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.GuncellemeYapilamadi;
                    return BadRequest(response);
                }

                User user = await _userService.GetUserById(updateUser.id);

                if (user == null)
                {
                    response.HasError = true;
                    response.Message += updateUser.id + " id " + Resource.Resource.KullaniciBulunamadi;
                    return NotFound(response);
                }

                user.fullName = updateUser.fullName;
                user.birthDate = updateUser.birthDate;
                user.gender = updateUser.gender;
                user.city = updateUser.city;
                user.services = updateUser.services;

                response.Data = await _userService.UpdateUserAsync(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = Resource.Resource.GuncellemeYapilamadi + " Exception => " + ex.Message;
                return Ok(response);
            }
        }
    }
}
