using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessSearchModel
    {
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public int? page { get; set; }
        public int? take { get; set; }
        [JsonIgnore]
        public string? city { get; set; }
        [JsonIgnore]
        public Guid userId{ get; set; } = Guid.Empty;
    }
}
