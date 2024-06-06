namespace CareGardenApiV1.Model.ResponseModel
{
    public class CommentListResponseModel
    {
        public Guid id { get; set; } = Guid.Empty;
        public string? comment { get; set; }
        public string? businessName { get; set; }
        public double point { get; set; }
        public string? aspectsOfPoint { get; set; }
        public double workerPoint { get; set; }
        public string? aspectsOfWorkerPoint { get; set; }

        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public bool isShowProfile { get; set; }
        public string? staffInfos { get; set; }
        public string? serviceInfos { get; set; }
    }
}
