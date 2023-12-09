using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Repository.Concrete;
using OneSignalApi.Model;

namespace CareGardenApiV1.Service.Concrete
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
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

        public async Task<Dictionary<string, dynamic>> GetCommentStatisticsByBusinessId(Guid businessId)
        {
            return await _commentRepository.GetCommentStatisticsByBusinessId(businessId);
        }

        public async Task<Dictionary<string, dynamic>> GetCommentStatisticsByUserId(Guid userId)
        {
            return await _commentRepository.GetCommentStatisticsByUserId(userId);
        }

        public async Task<List<Comment>> GetSearchCommentsAsync(CommentSearchModel searchModel)
        {
            return await _commentRepository.GetSearchCommentsAsync(searchModel);
        }
    }
}
