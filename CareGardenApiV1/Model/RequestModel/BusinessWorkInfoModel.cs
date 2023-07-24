using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessWorkInfoModel
    {
        public List<BusinessWorkingInfo> businessWorkingInfos { get; set; } = new List<BusinessWorkingInfo>();
        public bool officialDayAvailable { get; set; }
        public int appointmentTimeInterval { get; set; }
        public int appointmentPeopleCount { get; set; }
    }
}
