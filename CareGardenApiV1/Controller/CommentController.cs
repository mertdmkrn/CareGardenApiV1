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
    public class CommentController : ControllerBase
    {
        private ICommentService _commentService;
        private IFileHandler _fileHandler;
        private readonly ILoggerHandler _loggerHandler;

        public CommentController(ILoggerHandler loggerHandler)
        {
            _commentService = new CommentService();
            _loggerHandler = loggerHandler;
        }

        /// <summary>
        /// Get CommentBy Session User (User or Business)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("comment/get")]
        public async Task<IActionResult> GetCommentBySessionUser()
        {
            ResponseModel<List<Comment>> response = new ResponseModel<List<Comment>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                var id = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);
                var userRole = HelperMethods.GetClaimInfo(Request, ClaimTypes.Role);

                if (id.IsNullOrEmpty() || userRole.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                response.Data = userRole.Equals("Business")
                    ? await _commentService.GetCommentsByBusinessIdAsync(id.ToGuid())
                    : await _commentService.GetCommentsByUserIdAsync(id.ToGuid());

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
        /// Get Comment By Business Id
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
        [Route("comment/getcommentbybusinessid")]
        public async Task<IActionResult> GetCommentByBusinessId([FromBody] Guid businessId)
        {
            ResponseModel<List<Comment>> response = new ResponseModel<List<Comment>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                var id = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

                if (id.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                response.Data = await _commentService.GetCommentsByBusinessIdAsync(businessId);
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
        /// Get Comment By User Id
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
        [Route("comment/getcommentbyuserid")]
        public async Task<IActionResult> GetCommentByUserId([FromBody] Guid userId)
        {
            ResponseModel<List<Comment>> response = new ResponseModel<List<Comment>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                var id = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);

                if (id.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                response.Data = await _commentService.GetCommentsByUserIdAsync(userId);
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
        /// Add Comment (İşyeri cevabı ise replyId dolu olacak. Cevap verilen yorumun id si yazılması yeterli. Kullanıcı yorum yapmışsa businessId dolu olmalı.)
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "userId" : "00000000-0000-0000-0000-000000000000",
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///        "replyId" : "00000000-0000-0000-0000-000000000000",
        ///        "comment" : "Harika bir hizmet teşekkürler.",
        ///        "point" : 5
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("comment/add")]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                var id = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);
                var userRole = HelperMethods.GetClaimInfo(Request, ClaimTypes.Role);

                if (id.IsNullOrEmpty() || userRole.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                if (comment.comment.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("comment", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (userRole.Equals("Business") && !comment.replyId.HasValue)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("replyId", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (!userRole.Equals("Business") && !comment.businessId.HasValue)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.KayitYapilamadi;
                    return Ok(response);
                }

                if (userRole.Equals("Business"))
                {
                    comment.businessId = id.ToGuidNullable();
                    comment.commentType = Enums.CommentType.Business;
                }
                else
                {
                    comment.userId = id.ToGuidNullable();
                    comment.commentType = Enums.CommentType.User;
                }

                await _commentService.SaveCommentAsync(comment);

                if (comment.replyId.HasValue && comment.commentType == Enums.CommentType.Business) {
                    await _commentService.UpdateCommentReplyIdAsync(comment.replyId.Value, comment.id);
                }

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
        /// Update Comment
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
        ///        "comment" : "Harika bir hizmet teşekkürler.",
        ///        "point" : 5
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("comment/update")]
        public async Task<IActionResult> UpdateComment([FromBody] Comment updateComment)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                var id = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);
                var userRole = HelperMethods.GetClaimInfo(Request, ClaimTypes.Role);

                if (id.IsNullOrEmpty() || userRole.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.Message = Resource.Resource.KullaniciBulunamadi;
                    return Ok(response);
                }

                if (updateComment.comment.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("comment", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (updateComment.id == null)
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.BuAlaniBosBirakmayiniz));
                }

                if (response.HasError)
                {
                    response.Message = Resource.Resource.KayitYapilamadi;
                    return Ok(response);
                }

                var comment = await _commentService.GetCommentByIdAsync(updateComment.id);
                comment.comment = updateComment.comment.IsNull(comment.comment);
                comment.point = updateComment.point;

                await _commentService.UpdateCommentAsync(comment);

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
        /// Delete Comment
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
        [Route("comment/delete")]
        public async Task<IActionResult> DeleteComment([FromBody] Guid id)
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

                await _commentService.DeleteCommentByIdAsync(id);

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
