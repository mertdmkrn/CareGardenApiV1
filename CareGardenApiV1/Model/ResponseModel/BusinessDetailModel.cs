using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class BusinessDetailModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string workingDaysInfo { get; set; }
        public string workingHoursInfo { get; set; }
        public double distance { get; set; }
        public int discountRate { get; set; }
        public double averageRating { get; set; }
        public int countRating { get; set; }
        public WorkingGenderType workingGenderType { get; set; }
        public List<BusinessGallery> assets { get; set; }
    }
}
