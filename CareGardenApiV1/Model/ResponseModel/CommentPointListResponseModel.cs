namespace CareGardenApiV1.Model.ResponseModel
{
    public class CommentPointListResponseModel
    {
        public Guid? businessId { get; set; }
        public double point { get; set; }
        public IEnumerable<Guid?> workerIds { get; set; }
    }
}
