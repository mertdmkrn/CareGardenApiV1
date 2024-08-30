namespace CareGardenApiV1.Model.ResponseModel;

public class BusinessAdminServiceReportResponseModel
{
    public List<BusinessAdminServiceReportData> serviceDatas { get; set; }
    public List<BusinessAdminServiceReportData> businessServiceDatas { get; set; }
}

public class BusinessAdminServiceReportData
{
    public string serviceName { get; set; }
    public string businessServiceName { get; set; }
    public int appointmentCount { get; set; }
    public double percentage { get; set; }
}