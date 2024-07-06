using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Table("BusinessService")]
    [Index(nameof(serviceId)), Index(nameof(businessId))]
    public class BusinessServiceModel
    {
        public BusinessServiceModel()
        {
            this.appointmentDetails = new HashSet<AppointmentDetail>();
            this.workerServicePrices = new HashSet<WorkerServicePrice>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; } = Guid.Empty;

        public Guid? businessId { get; set; } 
        public Guid? serviceId { get; set; }

        [MaxLength(100)]
        public string? name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [MaxLength(100)]
        public string? nameEn { get; set; }

        public string? spot { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? spotEn { get; set; }   

        public int minDuration { get; set; }

        public int maxDuration { get; set; }

        public double price { get; set; } 
        public bool isPopular { get; set; }

        [NotMapped]
        public double discountPrice { get; set; }

        [NotMapped]
        public double discountRate { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

        [JsonIgnore]
        public Services? service { get; set; }

        [JsonIgnore]
        public ICollection<AppointmentDetail> appointmentDetails { get; set; }

        [JsonIgnore]
        public ICollection<WorkerServicePrice> workerServicePrices { get; set; }
    }
}
