using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.RequestModel
{
    public class KeywordSearchModel
    {
        public string keyword { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}
