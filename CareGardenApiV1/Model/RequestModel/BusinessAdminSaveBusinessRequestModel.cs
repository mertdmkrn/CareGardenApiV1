using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessAdminSaveBusinessRequestModel
    {
        public string? name { get; set; }
        public string? website { get; set; }
        public string? address { get; set; }
        public bool mobileOrOnlineServiceOnly { get; set; }
        public string? serviceIds { get; set; }
        public WorkerSizeType workerSizeType { get; set; }
    }
}
