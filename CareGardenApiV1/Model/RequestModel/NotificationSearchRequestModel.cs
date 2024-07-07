namespace CareGardenApiV1.Model.RequestModel
{
    public class NotificationSearchRequestModel
    {
        public Guid? businessId { get; set; } = Guid.Empty;
        public Guid? userId { get; set; } = Guid.Empty;
        public int page { get; set; }
        public int take { get; set; }
    }
}
