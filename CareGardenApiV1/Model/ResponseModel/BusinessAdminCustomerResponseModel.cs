namespace CareGardenApiV1.Model.ResponseModel;

public class BusinessAdminCustomerResponseModel
{
    public string name { get; set; }
    public string imageUrl { get; set; }
    public string email { get; set; }
    public DateTime? createDate { get; set; }
    public DateTime? lastAppointmentDate { get; set; }
    public int appointmentCount { get; set; }
    public double totalSpent { get; set; }
    public bool isBusinessCustomer { get; set; }
}