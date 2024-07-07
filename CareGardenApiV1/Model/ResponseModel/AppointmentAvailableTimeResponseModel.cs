using CareGardenApiV1.Model.RequestModel;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace CareGardenApiV1.Model.ResponseModel
{

    public class AppointmentAvailableInfoModel
    {
        public List<AppointmentAvailableTimeResponseModel> dateInfos {  get; set; }
        public List<ServiceWorkerInfo> serviceWorkerInfos { get; set; }
    }

    public class AppointmentAvailableTimeResponseModel
    {
        public DateTime date { get; set; }
        public bool isActive { get; set; }
        public List<TimeModel> dateList { get; set; } = new();
    }

    public class TimeModel
    {
        public DateTime date { get; set; }
        public string hourStr { get; set; }
        public bool isActive { get; set; }
    }
}
