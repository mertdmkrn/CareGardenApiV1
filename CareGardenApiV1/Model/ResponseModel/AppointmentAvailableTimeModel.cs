using Org.BouncyCastle.Bcpg.OpenPgp;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class AppointmentAvailableTimeModel
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
