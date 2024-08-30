namespace CareGardenApiV1.Model.ResponseModel;

public class BusinessAdminAppointmentReportResponseModel
{
    public List<BusinessAdminAppointmentReportInfo> reportInfos { get; set; }
    public int itemCount { get; set; }
}

public class BusinessAdminAppointmentReportInfo
{
    public string userName { get; set; }
    public string userImageUrl { get; set; }
    public string userTelephone { get; set; }
    public DateTime? date { get; set; }
    public double totalDuration { get; set; }
    public List<BusinessAdminAppointmentReportWorkerInfo> workerInfos { get; set; }
}

public class BusinessAdminAppointmentReportWorkerInfo
{
    public string name { get; set; }
    public string imageUrl { get; set; }
}