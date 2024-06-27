using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class WorkerDetailResponseModel
    {
        public Guid id { get; set; }
        public string? path { get; set; }
        public string? name { get; set; }
        public string? title { get; set; }
        public string? about { get; set; }
        public double point { get; set; }
        public int countRating { get; set; }

        public List<WorkerDetailServiceInfo> serviceInfos { get; set; }
        [JsonIgnore]
        public string[] serviceIds { get; set; }
    }

    public class WorkerDetailServiceInfo
    {
        public string? name { get; set; }
        public string? className { get; set; }
    }
}
