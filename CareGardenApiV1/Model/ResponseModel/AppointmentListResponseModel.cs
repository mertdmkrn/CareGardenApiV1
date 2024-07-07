using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class AppointmentListResponseModel
    {
        public Guid id {  get; set; }
        public AppointmentStatus status {  get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string? description { get; set; }
        public string? cancellationDescription { get; set; }
        public AppointmentBusinessListModel? business { get; set; }
        public Comment? comment { get; set; }
        public IEnumerable<AppointmentDetailListModel>? details { get; set; }
    }

    public class AppointmentDetailListModel
    {
        public string? workerName { get; set; }
        public string? workerImagePath { get; set; }
        public double price { get; set; }
        public double discountPrice { get; set; }
        public double discountRate { get; set; }
        public string? serviceName { get; set; }
        public int? minDuration  { get; set; }
        public int? maxDuration  { get; set; }
    }
    
    public class AppointmentBusinessListModel
    {
        public Guid id { get; set; }
        public string? logoUrl { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? telephone { get; set; }
    }
}
