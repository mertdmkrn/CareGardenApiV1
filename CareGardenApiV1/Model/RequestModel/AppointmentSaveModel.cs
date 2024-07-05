namespace CareGardenApiV1.Model.RequestModel
{
    public class AppointmentSaveModel
    {
        public Guid? userId { get; set; } = Guid.Empty;
        public Guid? businessId { get; set; } = Guid.Empty;
        public string? userName { get; set; }
        public string? userTelephone { get; set; }
        public string? userEmail { get; set; }
        public DateTime? startDate { get; set; }
        public string description { get; set; }
        public List<ServiceWorkerInfo> serviceWorkerInfos { get; set; }
    }
}
