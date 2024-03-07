using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CareGardenApiDbContext _context;

        public CommentRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            return await _context.Comments
                .FirstOrDefaultAsync(x => x.id == id);
        }

        public async Task<List<Comment>> GetCommentsByBusinessIdAsync(Guid businessId)
        {
            return await _context.Comments
                .Include(x => x.reply)
                .Where(x => x.businessId == businessId && x.commentType == CommentType.User)
                .OrderByDescending(x => x.updateDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId)
        {
            return await _context.Comments
                .Include(x => x.reply)
                .Where(x => x.userId == userId && x.commentType == CommentType.User)
                .OrderByDescending(x => x.updateDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            comment.createDate = DateTime.Now;
            comment.updateDate = comment.createDate;

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            comment.updateDate = DateTime.Now;

            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteCommentAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCommentByBusinessIdAsync(Guid businessId)
        {
            await _context.Comments
                .Where(x => x.businessId == businessId)
                .ExecuteDeleteAsync();
            return true;
        }

        public async Task<bool> DeleteCommentByUserIdAsync(Guid userId)
        {
            await _context.Comments
                .Where(x => x.userId == userId)
                .ExecuteDeleteAsync();
            return true;
        }

        public async Task<bool> DeleteCommentByIdAsync(Guid id)
        {
            await _context.Comments
                .Where(x => x.id == id)
                .ExecuteDeleteAsync();
            return true;
        }

        public async Task<bool> UpdateCommentReplyIdAsync(Guid id, Guid replyId)
        {
            await _context.Comments
                .Where(x => x.id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.replyId, replyId));
            return true;
        }

        public async Task<Dictionary<string, dynamic>> GetCommentStatisticsByBusinessId(Guid businessId)
        {
            var retVal = new Dictionary<string, dynamic>();

            var pointList = await _context.Comments
                .AsNoTracking()
                .Where(x => x.businessId == businessId)
                .Where(x => x.commentType == CommentType.User)
                .Select(x => x.point)
                .ToListAsync();

            var pointListGroups = pointList.GroupBy(x => x).OrderByDescending(x => x.Key);

            retVal.Add("Count", pointList.Count());
            retVal.Add("Average", Math.Round(pointList.Average(), 1));

            foreach (var item in pointListGroups)
            {
                retVal.TryAdd(item.Key.ToString(), item.Count());
            }

            return retVal;
        }

        public async Task<Dictionary<string, dynamic>> GetCommentStatisticsByUserId(Guid userId)
        {
            var retVal = new Dictionary<string, dynamic>();

            var pointList = await _context.Comments
                .AsNoTracking()
                .Where(x => x.userId == userId)
                .Where(x => x.commentType == CommentType.User)
                .Select(x => x.point)
                .ToListAsync();

            var pointListGroups = pointList.GroupBy(x => x).OrderByDescending(x => x.Key);

            retVal.Add("Count", pointList.Count());
            retVal.Add("Average", Math.Round(pointList.Average(), 1));

            foreach (var item in pointListGroups)
            {
                retVal.TryAdd(item.Key.ToString(), item.Count());
            }

            return retVal;
        }

        public async Task<List<CommentSearchResponseModel>> GetSearchCommentsAsync(CommentSearchModel searchModel)
        {
            return await _context.Comments
                .AsNoTracking()
                .Where(x => x.businessId == searchModel.businessId)
                .Where(x => x.commentType == CommentType.User)
                .WhereIf(searchModel.filterType == CommentFilterType.Rate1, x => ((int)x.point) == 1)
                .WhereIf(searchModel.filterType == CommentFilterType.Rate2, x => ((int)x.point) == 2)
                .WhereIf(searchModel.filterType == CommentFilterType.Rate3, x => ((int)x.point) == 3)
                .WhereIf(searchModel.filterType == CommentFilterType.Rate4, x => ((int)x.point) == 4)
                .WhereIf(searchModel.filterType == CommentFilterType.Rate5, x => ((int)x.point) == 5)
                .Select(x => new CommentSearchResponseModel
                {
                    id = x.id,
                    createDate = x.createDate,
                    updateDate = x.updateDate,
                    comment = x.comment,
                    point = x.point,
                    userName = x.user.fullName,
                    userImageUrl = x.user.imageUrl,
                    serviceInfos = string.Empty,
                    staffInfos = string.Empty,
                    reply = x.reply
                })
                .OrderByDescendingIf(searchModel.orderType == CommentOrderType.Lastest, x => x.createDate)
                .OrderByIf(searchModel.orderType == CommentOrderType.Oldest, x => x.createDate)
                .OrderByDescendingIf(searchModel.orderType == CommentOrderType.Popular, x => x.point)
                .OrderByIf(searchModel.orderType == CommentOrderType.Worst, x => x.point)
                .Skip(searchModel.page * searchModel.take)
                .Take(searchModel.take)
                .ToAsyncEnumerable()
                .ToListAsync();
        }
    }
}