using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessSearchAdminRequestModel
    {
        public int page { get; set; }
        public int take { get; set; }
        public string? city { get; set; }
        public string? name { get; set; }
        public WorkingGenderType workingGenderType { get; set; }
        public bool isOnlyActive { get; set; }
        public bool isOnlyNotActive { get; set; }
    }
}
