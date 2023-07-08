using CareGardenApiV1.Model;

namespace CareGardenApiV1.Repository.Abstract
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentByIdAsync(Guid id);
        Task<List<Comment>> GetCommentsByBusinessIdAsync(Guid businessId);
        Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId);
        Task<Comment> SaveCommentAsync(Comment comment);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(Comment comment);
        Task<bool> DeleteCommentByBusinessIdAsync(Guid businessId);
        Task<bool> DeleteCommentByUserIdAsync(Guid userId);
    }
}
