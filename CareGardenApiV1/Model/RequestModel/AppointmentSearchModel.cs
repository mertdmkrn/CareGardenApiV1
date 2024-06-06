using CareGardenApiV1.Helpers;
using Newtonsoft.Json;

namespace CareGardenApiV1.Model.RequestModel
{
    public class AppointmentSearchModel
    {
        public Guid? businessId { get; set; } = Guid.Empty;
        public Guid? userId { get; set; } = Guid.Empty;
        public Guid? businessServiceId { get; set; } = Guid.Empty;
        public Guid? appointmentId { get; set; } = Guid.Empty;
        public Guid? workerId { get; set; } = Guid.Empty;
        public HashSet<Guid> workerIds { get; set; } = new();
        public DateTime? startDate  { get; set; }
        public DateTime? endDate { get; set; }
        public AppointmentStatus status { get; set; } = AppointmentStatus.All;

        public int? page { get; set; }
        public int? take { get; set; }
        [JsonIgnore]
        public bool? isActive { get; set; }
        public bool? isHistory { get; set; }
    }
}
