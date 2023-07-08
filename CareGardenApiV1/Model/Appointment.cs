using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static CareGardenApiV1.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CareGardenApiV1.Model
{
    [Table("Appointment")]
    [Index(nameof(userId), nameof(businessId), nameof(date), nameof(status))]
    public class Appointment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public DateTime? date { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string? description { get; set; }
        public AppointmentStatus status { get; set; }
        public Guid? userId { get; set; }
        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

        [JsonIgnore]
        public User? user { get; set; }
    }
}
