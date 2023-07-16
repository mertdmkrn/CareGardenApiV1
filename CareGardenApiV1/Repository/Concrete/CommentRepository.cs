using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Repository.Concrete
{
    public class CommentRepository : ICommentRepository
    {
        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Comments
                    .FirstOrDefaultAsync(x => x.id == id);
            }
        }

        public async Task<List<Comment>> GetCommentsByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Comments
                    .Include(x => x.reply)
                    .Where(x => x.businessId == businessId && x.commentType == Enums.CommentType.Business)
                    .OrderByDescending(x => x.updateDate)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Comments
                    .Include(x => x.reply)
                    .Where(x => x.userId == userId && x.commentType == Enums.CommentType.User)
                    .OrderByDescending(x => x.updateDate)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            using (var context = new CareGardenApiDbContext())
            {
                comment.createDate = DateTime.Now;
                comment.updateDate = comment.createDate;

                await context.Comments.AddAsync(comment);
                await context.SaveChangesAsync();
                return comment;
            }
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            using (var context = new CareGardenApiDbContext())
            {
                comment.updateDate = DateTime.Now;

                context.Comments.Update(comment);
                await context.SaveChangesAsync();
                return comment;
            }
        }

        public async Task<bool> DeleteCommentAsync(Comment comment)
        {
            using (var context = new CareGardenApiDbContext())
            {
                context.Comments.Remove(comment);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteCommentByBusinessIdAsync(Guid businessId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Comments
                    .Where(x => x.businessId == businessId)
                    .ExecuteDeleteAsync();
                return true;
            }
        }

        public async Task<bool> DeleteCommentByUserIdAsync(Guid userId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Comments
                    .Where(x => x.userId == userId)
                    .ExecuteDeleteAsync();
                return true;
            }
        }

        public async Task<bool> DeleteCommentByIdAsync(Guid id)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Comments
                    .Where(x => x.id == id)
                    .ExecuteDeleteAsync();
                return true;
            }
        }

        public async Task<bool> UpdateCommentReplyIdAsync(Guid id, Guid replyId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                await context.Comments
                    .Where(x => x.id == id)
                    .ExecuteUpdateAsync(x => x.SetProperty(y=> y.replyId, replyId));
                return true;
            }
        }
    }
}