using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Route("comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IBusinessService _businessService;

        public CommentController(
            ICommentService commentService,
            IBusinessService businessService)
        {
            _commentService = commentService;
            _businessService = businessService;
        }

        /// <summary>
        /// Get Comment By Business Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "page": 0,
        ///        "take": 5
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("get")]
        [Authorize]
        public async Task<IActionResult> Get([FromBody] CommentSearchModel commentSearchModel)
        {
            ResponseModel<List<CommentListResponseModel>> response = new ResponseModel<List<CommentListResponseModel>>();

            var id = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);
            var userRole = HelperMethods.GetClaimInfo(Request, ClaimTypes.Role);

            if (id.IsNullOrEmpty() || userRole.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            if(userRole.Equals("Business"))
            {
                commentSearchModel.businessId = id.ToGuid();
            }
            else
            {
                commentSearchModel.userId = id.ToGuid();
            }

            response.Data = await _commentService.GetSearchCommentListAsync(commentSearchModel);

            return Ok(response);
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

            response.Data = await _commentService.GetCommentsByBusinessIdAsync(businessId);
            return Ok(response);
        }

        /// <summary>
        /// Search Comments
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
            ResponseModel<List<CommentSearchResponseModel>> response = new ResponseModel<List<CommentSearchResponseModel>>();
            var language = Request.Headers["Language"].ToString().IsNull("en");

            var comments = await _commentService.GetSearchCommentsAsync(searchModel);

            comments.ConvertAll(x => x.dayInfo = x.updateDate.HasValue
                                                                    ? x.updateDate.Value.GetRelativeDate(language)
                                                                    : x.createDate.HasValue
                                                                      ? x.createDate.Value.GetRelativeDate(language)
                                                                      : string.Empty);

            response.Data = comments;
            return Ok(response);
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

            response.Data = await _commentService.GetCommentStatisticsByBusinessId(businessId);
            return Ok(response);
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
        [Authorize]
        public async Task<IActionResult> GetCommentByUserId([FromBody] Guid userId)
        {
            ResponseModel<List<Comment>> response = new ResponseModel<List<Comment>>();

            response.Data = await _commentService.GetCommentsByUserIdAsync(userId);
            return Ok(response);
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
        ///        "appointmentId" : "00000000-0000-0000-0000-000000000000",
        ///        "replyId" : "00000000-0000-0000-0000-000000000000",
        ///        "comment" : "Harika bir hizmet teşekkürler.",
        ///        "point" : 5
        ///        "aspectsOfPoint" : "1~2",
        ///        "workerPoint" : 5
        ///        "aspectsOfWorkerPoint" : "1~2",
        ///        "isShowProfile": true
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        [Authorize]
        public async Task<IActionResult> Save([FromBody] Comment comment)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var id = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);
            var userRole = HelperMethods.GetClaimInfo(Request, ClaimTypes.Role);

            if (id.IsNullOrEmpty() || userRole.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
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

            if (!userRole.Equals("Business") && comment.appointmentId.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("appointmentId", Resource.Resource.BuAlaniBosBirakmayiniz));

            }
            
            if (userRole.Equals("Business"))
            {
                comment.appointmentId = null;
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            if (userRole.Equals("Business"))
            {
                comment.businessId = id.ToGuidNullable();
                comment.commentType = CommentType.Business;
            }
            else
            {
                comment.userId = id.ToGuidNullable();
                comment.commentType = CommentType.User;
            }

            await _commentService.SaveCommentAsync(comment);

            if (comment.replyId.HasValue && comment.commentType == CommentType.Business)
            {
                await _commentService.UpdateCommentReplyIdAsync(comment.replyId.Value, comment.id);
            }

            response.Data = true;
            response.Message = Resource.Resource.KayitBasarili;

            if (comment.businessId.HasValue)
            {
                BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(comment.businessId.Value));
            }

            return Ok(response);
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
        ///        "aspectsOfPoint" : "1~2",
        ///        "workerPoint" : 5
        ///        "aspectsOfWorkerPoint" : "1~2",
        ///        "isShowProfile": true
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] Comment updateComment)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var id = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);
            var userRole = HelperMethods.GetClaimInfo(Request, ClaimTypes.Role);

            if (id.IsNullOrEmpty() || userRole.IsNullOrEmpty())
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }
            
            var comment = await _commentService.GetCommentByIdAsync(updateComment.id);
            bool isPointChanged = comment.point != updateComment.point;

            comment.comment = updateComment.comment.IsNull(comment.comment);
            comment.point = updateComment.point.IsNull(comment.point);
            comment.aspectsOfPoint = updateComment.aspectsOfPoint.IsNull(comment.aspectsOfPoint);
            comment.workerPoint = updateComment.workerPoint.IsNull(comment.workerPoint);
            comment.aspectsOfWorkerPoint = updateComment.aspectsOfWorkerPoint.IsNull(comment.aspectsOfWorkerPoint);
            comment.isShowProfile = updateComment.isShowProfile;

            await _commentService.UpdateCommentAsync(comment);

            if (comment.businessId.HasValue && isPointChanged)
            {
                BackgroundJob.Enqueue(() => _businessService.UpdateMemoryBusinessList(comment.businessId.Value));    
            }

            response.Data = true;
            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
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
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

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
    }
}
