using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Service.Abstract
{
    public interface INotificationService
    {
        Task<NotificationSearchResponseModel> SearchNotificationAsync(NotificationSearchRequestModel notificationSearchModel);
        Task<Notification> SaveNotificationAsync(Notification notification);
        Task<List<Notification>> SaveNotificationsAsync(List<Notification> notifications);
        Task<Notification> UpdateNotificationAsync(Notification notification);
        Task<List<Notification>> UpdateNotificationsAsync(List<Notification> notifications);
        Task<bool> UpdateNotificationsReadAsync(List<Guid> notificationIds);
        Task<bool> UpdateNotificationsReadAsync(Guid? userId, Guid? businessId);
        Task<bool> DeleteNotificationsAsync(List<Guid> notificationIds);
    }
}
