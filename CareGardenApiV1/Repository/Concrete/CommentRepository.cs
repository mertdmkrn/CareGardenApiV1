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
                    .Where(x => x.businessId == businessId)
                    .OrderByDescending(x => x.updateDate)
                    .ToListAsync();
            }
        }

        public async Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId)
        {
            using (var context = new CareGardenApiDbContext())
            {
                return await context.Comments
                    .Where(x => x.userId == userId)
                    .OrderByDescending(x => x.updateDate)
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
    }
}