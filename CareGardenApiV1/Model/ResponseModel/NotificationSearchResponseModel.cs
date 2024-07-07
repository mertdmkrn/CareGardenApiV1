using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class NotificationSearchResponseModel
    {
        public List<NotificationResponseModel> notifications { get; set; }
        public int resultCount { get; set; }
        public int unReadCount { get; set; }
    }

    public class NotificationResponseModel
    {
        public Guid id { get; set; } = Guid.Empty;
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime? publishDate { get; set; }
        public string? dayInfo { get; set; }
        public NotificationType type { get; set; }
        public Guid? redirectId { get; set; } = Guid.Empty;
        public string? redirectUrl { get; set; }
        public bool isRead { get; set; }

        public BusinessListResponseModel relatedBusiness { get; set; }
    }
}
