using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentByIdAsync(Guid id);
        Task<List<Comment>> GetCommentsByBusinessIdAsync(Guid businessId);
        Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId);
        Task<List<CommentSearchResponseModel>> GetSearchCommentsAsync(CommentSearchModel searchModel);
        Task<List<CommentListResponseModel>> GetSearchCommentListAsync(CommentSearchModel searchModel);
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
