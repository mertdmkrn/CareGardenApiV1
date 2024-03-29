using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface INotificationService
    {
        Task<NotificationSearchResponseModel> SearchNotificationAsync(NotificationSearchModel notificationSearchModel);
        Task<Notification> SaveNotificationAsync(Notification notification);
        Task<List<Notification>> SaveNotificationsAsync(List<Notification> notifications);
        Task<Notification> UpdateNotificationAsync(Notification notification);
        Task<List<Notification>> UpdateNotificationsAsync(List<Notification> notifications);
        Task<bool> UpdateNotificationsReadAsync(List<Guid> notificationIds);
        Task<bool> DeleteNotificationsAsync(List<Guid> notificationIds);
    }
}
