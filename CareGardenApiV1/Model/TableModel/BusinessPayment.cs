using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.TableModel
{
    [Table("BusinessPayment")]
    [Index(nameof(businessId))]
    [Index(nameof(date))]
    public class BusinessPayment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; } = Guid.Empty;

        [MaxLength(150)]
        public string? description { get; set; }

        public DateTime date { get; set; }

        public double amount { get; set; }

        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

        public Guid? appointmentId { get; set; }

        [JsonIgnore]
        public Appointment? appointment { get; set; }

        public Guid? businessUserId { get; set; }

        [JsonIgnore]
        public BusinessUser? businessUser { get; set; }
    }
}
