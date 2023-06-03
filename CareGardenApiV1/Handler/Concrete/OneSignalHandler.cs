using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using CareGardenApiV1.Helpers;
using OneSignalApi.Api;
using OneSignalApi.Client;
using OneSignalApi.Model;

namespace CareGardenApiV1.Handler.Concrete
{
    public class OneSignalHandler : IOneSignalHandler
    {
        public async Task<bool> CreateNotification(NotificationRequest notificationRequest)
        {
            var config = HelperMethods.GetConfiguration();
            var appInstance = getAppInstance(config);

            var notification = new Notification(appId: config["OneSignal:AppId"])
            {
                Headings = new StringMap(tr: notificationRequest.Headings),
                Contents = new StringMap(tr: notificationRequest.Contents),
                IncludedSegments = notificationRequest.IncludedSegments
            };

            var response = await appInstance.CreateNotificationAsync(notification);

            return true;
        }

        private DefaultApi? getAppInstance(IConfiguration config)
        {
            var appConfig = new Configuration();
            appConfig.BasePath = "https://onesignal.com/api/v1";
            appConfig.AccessToken = config["OneSignal:AccessToken"];
            var appInstance = new DefaultApi(appConfig);

            return appInstance;
        }
    }
}
