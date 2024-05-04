namespace CareGardenApiV1.Model.RequestModel
{
    public class AppointmentDatesModel
    {
        public int page { get; set; }
        public Guid? businessId { get; set; } = Guid.Empty;
        public List<ServiceWorkerInfo> serviceWorkerInfos { get; set; }
    }

    public class ServiceWorkerInfo
    {
        public Guid businessServiceId { get; set; }
        public Guid? workerId { get; set; } = Guid.Empty;

    }
}
