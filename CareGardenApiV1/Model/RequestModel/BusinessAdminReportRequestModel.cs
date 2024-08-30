using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessAdminReportRequestModel
    {
        public Guid? businessId { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int page { get; set; }
        public int take { get; set; }
    }
}
