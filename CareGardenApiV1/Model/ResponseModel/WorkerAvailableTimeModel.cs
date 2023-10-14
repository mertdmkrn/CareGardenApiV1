namespace CareGardenApiV1.Model.ResponseModel
{
    public class WorkerAvailableTimeModel
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string hourText { get; set; }
        public string dateText { get; set; }
    }
}
