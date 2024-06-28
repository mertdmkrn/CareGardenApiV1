using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class BusinessPagingListModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string logoUrl { get; set; }
        public bool isPaid { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public bool isActive{ get; set; }
        public WorkingGenderType workingGenderType { get; set; }
        public DateTime? createDate { get; set; }
        public int itemCount { get; set; }
    }
}
