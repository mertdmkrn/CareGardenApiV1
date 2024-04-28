using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.RequestModel
{
    public class AppointmentChangeModel
    {
        public Guid? id { get; set; }
        public AppointmentStatus status { get; set; }
        public bool? isForceDelete { get; set; }
    }
}
