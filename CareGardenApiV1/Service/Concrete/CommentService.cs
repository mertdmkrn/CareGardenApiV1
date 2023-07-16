using CareGardenApiV1.Model;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class CommentService : ICommentService
    {
        private ICommentRepository _commentRepository;

        public CommentService()
        {
            _commentRepository = new CommentRepository();
        }

        public async Task<bool> DeleteCommentAsync(Comment comment)
        {
            return await _commentRepository.DeleteCommentAsync(comment);
        }

        public async Task<bool> DeleteCommentByBusinessIdAsync(Guid businessId)
        {
            return await _commentRepository.DeleteCommentByBusinessIdAsync(businessId);
        }

        public async Task<bool> DeleteCommentByUserIdAsync(Guid userId)
        {
            return await _commentRepository.DeleteCommentByUserIdAsync(userId);
        }

        public async Task<bool> DeleteCommentByIdAsync(Guid id)
        {
            return await _commentRepository.DeleteCommentByIdAsync(id);
        }

        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            return await _commentRepository.GetCommentByIdAsync(id);
        }

        public async Task<List<Comment>> GetCommentsByBusinessIdAsync(Guid businessId)
        {
            return await _commentRepository.GetCommentsByBusinessIdAsync(businessId);
        }

        public async Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId)
        {
            return await _commentRepository.GetCommentsByUserIdAsync(userId);
        }

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            return await _commentRepository.SaveCommentAsync(comment);
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            return await _commentRepository.UpdateCommentAsync(comment);
        }

        public async Task<bool> UpdateCommentReplyIdAsync(Guid id, Guid replyId)
        {
            return await _commentRepository.UpdateCommentReplyIdAsync(id, replyId);
        }
    }
}
