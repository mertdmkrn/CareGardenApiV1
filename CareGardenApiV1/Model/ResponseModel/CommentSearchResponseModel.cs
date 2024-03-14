namespace CareGardenApiV1.Model.ResponseModel
{
    public class CommentSearchResponseModel
    {
        public Guid id { get; set; } = Guid.Empty;
        public string? comment { get; set; }
        public string? dayInfo { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string? userName { get; set; }
        public string? userImageUrl { get; set; }
        public string? staffInfos { get; set; } 
        public string? serviceInfos { get; set; }
        public double point { get; set; }
        public string reply { get; set; }
    }
}
