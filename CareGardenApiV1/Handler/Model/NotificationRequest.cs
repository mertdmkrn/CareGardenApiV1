namespace CareGardenApiV1.Handler.Model
{
    public class NotificationRequest
    {
        public string Headings { get; set; }
        public string Contents { get; set; }
        public List<string> IncludedSegments { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
