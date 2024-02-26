
namespace CareGardenApiV1.Model.ResponseModel
{
    public class KeywordSearchResponseModel
    {
        public List<Services> services { get; set; } = new List<Services>();
        public List<BusinessListModel> businesses { get; set; } = new List<BusinessListModel>();
        public string keyWord { get; set; }
    }
}
