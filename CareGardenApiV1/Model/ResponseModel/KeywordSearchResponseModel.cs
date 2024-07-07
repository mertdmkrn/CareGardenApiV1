
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class KeywordSearchResponseModel
    {
        public List<Services> services { get; set; } = new List<Services>();
        public List<BusinessListResponseModel> businesses { get; set; } = new List<BusinessListResponseModel>();
        public string keyWord { get; set; }
    }
}
