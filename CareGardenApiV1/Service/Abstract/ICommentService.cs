using CareGardenApiV1.Model;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface ICommentService
    {
        Task<Comment> GetCommentByIdAsync(Guid id);
        Task<List<Comment>> GetCommentsByBusinessIdAsync(Guid businessId);
        Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId);
        Task<Comment> SaveCommentAsync(Comment comment);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(Comment comment);
        Task<bool> DeleteCommentByBusinessIdAsync(Guid businessId);
        Task<bool> DeleteCommentByUserIdAsync(Guid userId);
        Task<bool> DeleteCommentByIdAsync(Guid id);
        Task<bool> UpdateCommentReplyIdAsync(Guid id, Guid replyId);
        Task<Dictionary<string, string>> GetCommentStatisticsByBusinessId(Guid businessId);
        Task<Dictionary<string, string>> GetCommentStatisticsByUserId(Guid userId);
    }
}
