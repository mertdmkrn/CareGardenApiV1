namespace CareGardenApiV1.Model.ResponseModel
{
    public class BusinessServicesInfo
    {
        public string serviceName { get; set; }
        public string className { get; set; }
        public List<BusinessServiceModel> businessServices { get; set; } = new List<BusinessServiceModel>();

    }
}
