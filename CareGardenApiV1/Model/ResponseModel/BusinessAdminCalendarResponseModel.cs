using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.ResponseModel;

public class BusinessAdminCalendarResponseModel
{
    public Guid id { get; set; }
    public DateTime? startDate { get; set; }
    public DateTime? endDate { get; set; }
    public AppointmentStatus status { get; set; }
    public List<BusinessAdminCalendarWorkerInfo> workers { get; set; }
    public BusinessAdminCalendarUserInfo user { get; set; }
}

public class BusinessAdminCalendarWorkerInfo
{
    public Guid? id { get; set; }
    public string name { get; set; }
    public string imageUrl { get; set; }
    public Guid? businessServiceId { get; set; }
    public string businessServiceName { get; set; }
}

public class BusinessAdminCalendarUserInfo
{
    public Guid? id { get; set; }
    public string name { get; set; }
    public string telephone { get; set; }
    public string imageUrl { get; set; }
}