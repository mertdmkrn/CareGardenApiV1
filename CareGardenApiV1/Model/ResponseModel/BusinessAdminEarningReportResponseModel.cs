namespace CareGardenApiV1.Model.ResponseModel;

public class BusinessAdminEarningReportResponseModel
{
    public List<BusinessAdminEarningReportData> dailyList { get; set; }
    public double lastWeekEarning { get; set; }
    public int lastWeekEarningPercentage { get; set; }
}

public class BusinessAdminEarningReportData
{
    public DateTime date { get; set; }
    public double earning { get; set; }
    public double expend { get; set; }
    public double profit { get; set; }
    public string dayStr { get; set; }
}