namespace CareGardenApiV1.Model.ResponseModel;

public class BusinessAdminWorkerReportResponseModel
{
    public string workerImageUrl { get; set; }
    public string workerName { get; set; }
    public string workerTitle { get; set; }
    public int appointmentCount { get; set; }
    public double totalEarning { get; set; }
}