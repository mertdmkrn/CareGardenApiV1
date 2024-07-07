using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessWorkInfoRequestModel
    {
        public Guid businessId { get; set; } = Guid.Empty;

        public BusinessWorkingInfo businessWorkingInfo { get; set; } = new BusinessWorkingInfo();
        public bool officialHolidayAvailable { get; set; }
        public int appointmentTimeInterval { get; set; }
        public int appointmentPeopleCount { get; set; }
        public WorkingGenderType workingGenderType { get; set; }
    }
}
