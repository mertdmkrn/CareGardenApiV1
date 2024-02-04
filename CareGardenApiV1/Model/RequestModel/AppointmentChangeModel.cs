using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.RequestModel
{
    public class AppointmentChangeModel
    {
        public string? id { get; set; }
        public AppointmentStatus status { get; set; }
        public bool? isForceDelete { get; set; }
    }
}
