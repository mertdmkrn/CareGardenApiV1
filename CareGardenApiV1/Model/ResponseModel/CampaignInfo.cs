namespace CareGardenApiV1.Model.ResponseModel
{
    public class CampaingInfo
    {
        public Guid id { get; set; } = Guid.Empty;
        public Guid? businessId { get; set; } = Guid.Empty;
        public string? path { get; set; }
        public string? url { get; set; }
        public string? title { get; set; }
        public string? about { get; set; }
        public string? condition { get; set; }
        public DateTime? expireDate { get; set; }
        public string? dayInfo { get; set; }
        public int sortOrder { get; set; }
    }
}
