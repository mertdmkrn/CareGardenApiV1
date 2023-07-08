using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
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
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IFileHandler _fileHandler;

        public UserController()
        {
            _userService = new UserService();
            _fileHandler = new FileHandler();
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user/getbyid")]
        public async Task<IActionResult> GetUserById([FromBody] string id)
        {
            ResponseModel<User> response = new ResponseModel<User>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (!id.IsGuid())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                    response.Message += Resource.Resource.IdParametreHatasi;
                }

                if (response.HasError)
                    return Ok(response);

                response.Data = await _userService.GetUserById(id.ToGuid());

                if (response.Data == null)
                {
                    response.HasError = true;
                    response.Message += id + " id " + Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                response.Data.password = null;
                response.Data.retryPassword = null;

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
        /// Get Session User
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
                var user = await HelperMethods.GetSessionUser(Request, _userService);
                
                if (user == null)
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                user.password = null;
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
        /// User Set Profile Photo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user/setprofilephoto")]
        public async Task<IActionResult> SetProfilePhoto(IFormFile file)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                if (file == null)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("file", Resource.Resource.BuAlaniBosBirakmayiniz));
                    response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
                    return Ok(response);
                }

                var user = await HelperMethods.GetSessionUser(Request, _userService);

                if (user == null)
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                string fileName = user.fullName.ToLower().TurkishChrToEnglishChr().Replace(" ", "-") + "-" + DateTime.Now.ToString("ddMMhhmmss") + "." + file.FileName.Split(".").LastOrDefault();
                user.imageUrl = string.Format("{0}://{1}/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, "UploadedFiles/UserImages/" + fileName);
                await _fileHandler.UploadFile(file, "UserImages", fileName);
                await _userService.UpdateUserAsync(user);

                response.Message = Resource.Resource.ResimYuklemeBasarili;
                response.Data = true;

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

                User user = await _userService.GetUserById(id.ToGuid());

                if (user == null)
                {
                    response.HasError = true;
                    response.Message = id + " id " + Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                response.Data = await _userService.DeleteUserAsync(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KayitSilinemedi + " Exception => " + ex.Message;
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

                if (!updateUser.fullName.IsValidFullName())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("fullName", Resource.Resource.GecerliBirIsimGiriniz));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.GuncellemeYapilamadi;
                    return Ok(response);
                }

                var user = await HelperMethods.GetSessionUser(Request, _userService);

                if (user == null)
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                user.fullName = updateUser.fullName;
                user.birthDate = updateUser.birthDate;
                user.gender = updateUser.gender;
                user.city = updateUser.city;
                user.services = updateUser.services.TrimEnd(';');

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
