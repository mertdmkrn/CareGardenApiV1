namespace CareGardenApiV1.Model.ResponseModel;

public class BusinessAdminTotalDataResponseModel
{
    public int customerCount { get; set; }
    public int monthlyCustomerIncreasePercentage { get; set; }
    public int activeAppointmentCount { get; set; }
    public int pendingAppointmentCount { get; set; }
}