using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CareGardenApiV1.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CareGardenApiV1.Model
{
    [Table("AppointmentDetail")]
    [Index(nameof(appointmentId))]
    [Index(nameof(workerId))]
    public class AppointmentDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid? appointmentId { get; set; }
        public Guid? workerId { get; set; }
        public Guid? businessServiceId { get; set; }
        public double price { get; set; }
        public double discountPrice { get; set; }

        public DateTime? date { get; set; }

        [JsonIgnore]
        public Appointment? appointment { get; set; }

        [JsonIgnore]
        public Worker? worker { get; set; }

        [JsonIgnore]
        public BusinessServiceModel? businessService { get; set; }
    }
}
