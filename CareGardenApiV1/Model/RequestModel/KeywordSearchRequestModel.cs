namespace CareGardenApiV1.Model.RequestModel
{
    public class KeywordSearchRequestModel
    {
        public string keyWord { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}
