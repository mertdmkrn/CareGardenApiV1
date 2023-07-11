using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class BusinessListModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public double distance { get; set; }
        public int discountRate { get; set; }
        public double averageRating { get; set; }
        public int countRating { get; set; }
        public string workingGenderType { get; set; }
        public string imageUrl { get; set; }
    }
}
