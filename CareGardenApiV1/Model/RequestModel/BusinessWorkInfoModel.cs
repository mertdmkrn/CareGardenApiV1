using System.Text.Json.Serialization;
using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessWorkInfoModel
    {
        public BusinessWorkingInfo businessWorkingInfo { get; set; } = new BusinessWorkingInfo();
        public bool officialDayAvailable { get; set; }
        public int appointmentTimeInterval { get; set; }
        public int appointmentPeopleCount { get; set; }
        public WorkingGenderType workingGenderType { get; set; }
    }
}
