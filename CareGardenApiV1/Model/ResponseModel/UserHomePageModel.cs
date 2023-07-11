namespace CareGardenApiV1.Model.ResponseModel
{
    public class UserHomePageModel
    {
        public IList<BusinessListModel> favoriteBusinessList { get; set; }
        public IList<BusinessListModel> popularBusinessList { get; set; }
        public IList<BusinessListModel> nearByBusinessList { get; set; }
    }
}
