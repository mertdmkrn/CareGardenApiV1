using CareGardenApiV1.Handler.Model;

namespace CareGardenApiV1.Handler.Abstract
{
    public interface IOneSignalHandler
    {
        Task<bool> CreateNotification(NotificationRequest notificationRequest);
    }
}
