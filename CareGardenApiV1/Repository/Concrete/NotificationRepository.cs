using CareGardenApiV1.Helpers;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly CareGardenApiDbContext _context;

        public NotificationRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationSearchResponseModel> SearchNotificationAsync(NotificationSearchRequestModel notificationSearchModel)
        {
            NotificationSearchResponseModel notificationSearchResponseModel = new NotificationSearchResponseModel();
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");
            var now = DateTime.Now;

            var query = _context.Notifications
                            .AsNoTracking()
                            .Where(x => x.publishDate.Value <= now)
                            .WhereIf(notificationSearchModel.businessId.HasValue && notificationSearchModel.businessId != Guid.Empty, x => x.businessId == notificationSearchModel.businessId)
                            .WhereIf(notificationSearchModel.userId.HasValue && notificationSearchModel.userId != Guid.Empty, x => x.userId == notificationSearchModel.userId);
                            
            notificationSearchResponseModel.resultCount = await query.CountAsync();
            notificationSearchResponseModel.unReadCount = await query.CountAsync(x => !x.isRead);

            var list = await query.ToListAsync();

            notificationSearchResponseModel.notifications = await query
                .Select(x => new NotificationResponseModel
                {
                    id = x.id,
                    title = isTurkish ? x.title : x.titleEn,
                    description = isTurkish ? x.description : x.descriptionEn,
                    publishDate = x.publishDate,
                    type = x.type,
                    redirectId = x.redirectId,
                    redirectUrl = x.redirectUrl,
                    isRead = x.isRead,
                    dayInfo = x.publishDate.Value.GetRelativeDate(Resource.Resource.Culture.ToString()),
                })
                .OrderByDescending(x => x.publishDate)
                .Skip(notificationSearchModel.page * notificationSearchModel.take)
                .Take(notificationSearchModel.take)
                .ToListAsync();

            return notificationSearchResponseModel;
        }

        public async Task<Notification> SaveNotificationAsync(Notification notification)
        {
            notification.createDate = DateTime.Now;
            notification.updateDate = notification.createDate;
            notification.publishDate = notification.publishDate.HasValue ? notification.publishDate : notification.createDate;

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<List<Notification>> SaveNotificationsAsync(List<Notification> notifications)
        {
            notifications = notifications
                .Select(x => {
                    x.createDate = DateTime.Now;
                    x.publishDate = x.publishDate.HasValue ? x.publishDate : x.createDate;
                    x.updateDate = x.createDate;
                    return x;
                })
                .ToList();

            await _context.Notifications.AddRangeAsync(notifications);
            await _context.SaveChangesAsync();
            return notifications;
        }

        public async Task<Notification> UpdateNotificationAsync(Notification notification)
        {
            notification.updateDate = DateTime.Now;

            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<List<Notification>> UpdateNotificationsAsync(List<Notification> notifications)
        {
            notifications = notifications
                .Select(x => {
                    x.updateDate = DateTime.Now;
                    return x;
                })
                .ToList();

            _context.Notifications.UpdateRange(notifications);
            await _context.SaveChangesAsync();
            return notifications;
        }

        public async Task<bool> UpdateNotificationsReadAsync(List<Guid> notificationIds)
        {
            await _context.Notifications
                .Where(x => notificationIds.Contains(x.id))
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.isRead, true));

            return true;
        }

        public async Task<bool> UpdateNotificationsReadAsync(Guid? userId, Guid? businessId)
        {
            await _context.Notifications
                .WhereIf(businessId.HasValue, x => x.businessId == businessId)
                .WhereIf(userId.HasValue, x => x.userId == userId)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.isRead, true));

            return true;
        }

        public async Task<bool> DeleteNotificationsAsync(List<Guid> notificationIds)
        {
            await _context.Notifications
                .Where(x => notificationIds.Contains(x.id))
                .ExecuteDeleteAsync();

            return true;
        }
    }
}