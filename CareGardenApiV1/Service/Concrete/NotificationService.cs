using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> DeleteNotificationsAsync(List<Guid> notificationIds)
        {
            return await _notificationRepository.DeleteNotificationsAsync(notificationIds);
        }

        public async Task<Notification> SaveNotificationAsync(Notification notification)
        {
            return await _notificationRepository.SaveNotificationAsync(notification);
        }

        public async Task<List<Notification>> SaveNotificationsAsync(List<Notification> notifications)
        {
            return await _notificationRepository.SaveNotificationsAsync(notifications);
        }

        public async Task<NotificationSearchResponseModel> SearchNotificationAsync(NotificationSearchModel notificationSearchModel)
        {
            return await _notificationRepository.SearchNotificationAsync(notificationSearchModel);
        }

        public async Task<Notification> UpdateNotificationAsync(Notification notification)
        {
            return await _notificationRepository.UpdateNotificationAsync(notification);
        }

        public async Task<List<Notification>> UpdateNotificationsAsync(List<Notification> notifications)
        {
            return await _notificationRepository.UpdateNotificationsAsync(notifications);
        }

        public async Task<bool> UpdateNotificationsReadAsync(List<Guid> notificationIds)
        {
            return await _notificationRepository.UpdateNotificationsReadAsync(notificationIds);
        }
    }
}
