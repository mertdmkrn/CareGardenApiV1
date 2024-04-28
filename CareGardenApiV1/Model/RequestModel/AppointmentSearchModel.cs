namespace CareGardenApiV1.Model.RequestModel
{
    public class AppointmentSearchModel
    {
        public Guid? businessId { get; set; } = Guid.Empty;
        public Guid? userId { get; set; } = Guid.Empty;
        public Guid? businessServiceId { get; set; } = Guid.Empty;
        public Guid? appointmentId { get; set; } = Guid.Empty;
        public Guid? workerId { get; set; } = Guid.Empty;
        public DateTime? startDate  { get; set; }
        public DateTime? endDate { get; set; }
        public int? page { get; set; }
        public int? take { get; set; }
    }
}
