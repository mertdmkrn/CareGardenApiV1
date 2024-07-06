using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Index(nameof(businessId))]
    [Table("Worker")]
    public class Worker
    {
        public Worker()
        {
            this.appointmentDetails = new HashSet<AppointmentDetail>();
            this.workerServicePrices = new HashSet<WorkerServicePrice>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; } = Guid.Empty;

        [MaxLength(150)]
        public string? name { get; set; }
        [MaxLength(100)]
        public string? title { get; set; }
        [MaxLength(100)]
        public string? titleEn { get; set; }
        public string? about { get; set; }
        public string? aboutEn { get; set; }
        public string? path { get; set; }
        public bool isActive { get; set; }
        public bool isAvailable { get; set; }
        public string serviceIds { get; set; } = string.Empty;   
        [NotMapped]
        public double point { get; set; }
        [NotMapped]
        public double countRating { get; set; }
        [MaxLength(20)]
        public string? mondayWorkHours { get; set; }
        [MaxLength(20)]
        public string? tuesdayWorkHours { get; set; }
        [MaxLength(20)]
        public string? wednesdayWorkHours { get; set; }
        [MaxLength(20)]
        public string? thursdayWorkHours { get; set; }
        [MaxLength(20)]
        public string? fridayWorkHours { get; set; }
        [MaxLength(20)]
        public string? saturdayWorkHours { get; set; }
        [MaxLength(20)]
        public string? sundayWorkHours { get; set; }
        public Guid? businessId { get; set; }
        public Guid? createdUserId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

        [JsonIgnore]
        public ICollection<AppointmentDetail> appointmentDetails { get; set; }

        [JsonIgnore]
        public ICollection<WorkerServicePrice> workerServicePrices { get; set; }

    }
}
