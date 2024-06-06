using CareGardenApiV1.Helpers;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.RequestModel
{
    public class AppointmentChangeModel
    {
        public Guid? id { get; set; }
        [JsonIgnore]
        public Guid? userId { get; set; }
        [JsonIgnore]
        public DateTime? date { get; set; }

        public string? cancellationDescription { get; set; }
        public AppointmentStatus status { get; set; }
    }
}
