using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Repository.Abstract;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IFileHandler _fileHandler;
        private readonly ILoggerHandler _loggerHandler;
        private readonly IElasticHandler _elasticHandler;

        public CommentController(
            ICommentService commentService,
            IFileHandler fileHandler,
            ILoggerHandler loggerHandler,
            IElasticHandler elasticHandler)
        {
            _commentService = commentService;
            _fileHandler = fileHandler;
            _loggerHandler = loggerHandler;
            _elasticHandler = elasticHandler;
        }

        /// <summary>
        /// Get CommentBy Session User (User or Business)
        /// </summary>
        /// <returns></returns>
        [HttpPost("get")]
        public async Task<IActionResult> GetBySessionUser()
        {
            ResponseModel<List<Comment>> response = new ResponseModel<List<Comment>>();
            Resource.Resource.Culture = new CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

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
                response.Message = $"Exception => {ex.Message}";
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
        [HttpPost("getbybusinessid")]
        public async Task<IActionResult> GetByBusinessId([FromBody] Guid businessId)
        {
            ResponseModel<List<Comment>> response = new ResponseModel<List<Comment>>();
            Resource.Resource.Culture = new CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                response.Data = await _commentService.GetCommentsByBusinessIdAsync(businessId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = $"Exception => {ex.Message}";
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
        ///        "businessId": "00000000-0000-0000-0000-000000000000",
        ///        "filterType": 0,
        ///        "orderType": 0,
        ///        "page": 1,
        ///        "take": 10
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] CommentSearchModel searchModel)
        {
            ResponseModel<List<Comment>> response = new ResponseModel<List<Comment>>();
            Resource.Resource.Culture = new CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                response.Data = await _commentService.GetSearchCommentsAsync(searchModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = $"Exception => {ex.Message}";
                return Ok(response);
            }
        }

        /// <summary>
        /// Get Statistics By Business Id
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
        [HttpPost("getstatisticsbybusinessid")]
        public async Task<IActionResult> GetStatisticsByBusinessId([FromBody] Guid businessId)
        {
            ResponseModel<Dictionary<string, dynamic>> response = new ResponseModel<Dictionary<string, dynamic>>();
            Resource.Resource.Culture = new CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                response.Data = await _commentService.GetCommentStatisticsByBusinessId(businessId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = $"Exception => {ex.Message}";
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
        [HttpPost("getbyuserid")]
        public async Task<IActionResult> GetCommentByUserId([FromBody] Guid userId)
        {
            ResponseModel<List<Comment>> response = new ResponseModel<List<Comment>>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(Request.Headers["Language"].ToString().IsNull("en"));

            try
            {
                response.Data = await _commentService.GetCommentsByUserIdAsync(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = $"Exception => {ex.Message}";
                return Ok(response);
            }
        }

        /// <summary>
        /// Save Comment (İşyeri cevabı ise replyId dolu olacak. Cevap verilen yorumun id si yazılması yeterli. Kullanıcı yorum yapmışsa businessId dolu olmalı.)
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
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] Comment comment)
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

                if (comment.businessId.HasValue)
                {
                    BackgroundJob.Enqueue(() => _elasticHandler.UpdateOrCreateIndexBusiness(comment.businessId.Value));
                }

                return Ok(response);

            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = $"Exception => {ex.Message}";
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
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] Comment updateComment)
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

                if (comment.businessId.HasValue)
                {
                    BackgroundJob.Enqueue(() => _elasticHandler.UpdateOrCreateIndexBusiness(comment.businessId.Value));
                }

                response.Data = true;
                response.Message = Resource.Resource.KayitBasarili;

                return Ok(response);

            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = $"Exception => {ex.Message}";
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
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
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
                response.Message = $"Exception => {ex.Message}";
                return Ok(response);
            }
        }
    }
}
