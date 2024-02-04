using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CareGardenApiV1.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CareGardenApiV1.Model
{
    [Table("Appointment")]
    [Index(nameof(userId), nameof(businessId), nameof(workerId), nameof(startDate), nameof(status))]
    public class Appointment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string? description { get; set; }
        public AppointmentStatus status { get; set; }
        public Guid? userId { get; set; }
        public Guid? businessId { get; set; }
        public Guid? workerId { get; set; }
        public Guid? businessServiceId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

        [JsonIgnore]
        public User? user { get; set; }

        [JsonIgnore]
        public Worker? worker { get; set; }

        [JsonIgnore]
        public BusinessServiceModel? businessService { get; set; }
    }
}
