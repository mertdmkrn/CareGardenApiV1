using System.Text.Json.Serialization;

namespace CareGardenApiV1.Handler.Model
{
    public class MailRequest
    {
        [JsonIgnore]
        public List<string> ToEmailList { get; set; } = new List<string>();
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();

    }
}
