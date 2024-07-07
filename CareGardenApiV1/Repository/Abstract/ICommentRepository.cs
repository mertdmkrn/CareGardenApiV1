using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentByIdAsync(Guid id);
        Task<List<Comment>> GetCommentsByBusinessIdAsync(Guid businessId);
        Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId);
        Task<List<CommentSearchResponseModel>> GetSearchCommentsAsync(CommentSearchRequestModel searchModel);
        Task<List<CommentListResponseModel>> GetSearchCommentListAsync(CommentSearchRequestModel searchModel);
        Task<List<CommentPointListResponseModel>> GetCommentPointListForCache(bool cache = true);
        Task<Comment> SaveCommentAsync(Comment comment);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task<bool> UpdateCommentReplyIdAsync(Guid id, Guid replyId);
        Task<bool> DeleteCommentAsync(Comment comment);
        Task<bool> DeleteCommentByBusinessIdAsync(Guid businessId);
        Task<bool> DeleteCommentByUserIdAsync(Guid userId);
        Task<bool> DeleteCommentByIdAsync(Guid id);
        Task<Dictionary<string, dynamic>> GetCommentStatisticsByBusinessId(Guid businessId);
        Task<Dictionary<string, dynamic>> GetCommentStatisticsByUserId(Guid userId);
    }
}
