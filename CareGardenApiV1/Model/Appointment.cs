using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CareGardenApiV1.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CareGardenApiV1.Model
{
    [Table("Appointment")]
    [Index(nameof(userId))]
    [Index(nameof(businessId))]
    [Index(nameof(startDate))]
    [Index(nameof(status))]
    public class Appointment
    {
        public Appointment()
        {
            this.details = new HashSet<AppointmentDetail>();
            this.comments = new HashSet<Comment>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        
        [MaxLength(20)]
        public string? userTelephone { get; set; }

        [MaxLength(100)]
        public string? userName { get; set; }

        [MaxLength(150)]
        public string? userEmail { get; set; }

        [MaxLength(200)]
        public string? description { get; set; }
        
        public AppointmentStatus status { get; set; }
        public Guid? userId { get; set; }
        public Guid? businessId { get; set; }

        public double totalPrice { get; set; }
        
        public double totalDiscountPrice { get; set; }
        
        public bool isGuest { get; set; }
        
        [MaxLength(200)]
        public string? cancellationDescription { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

        [JsonIgnore]
        public User? user { get; set; }

        [JsonIgnore]
        [NotMapped]
        public Comment? comment { get; set; }

        

        public virtual ICollection<AppointmentDetail> details { get; set; }

        public virtual ICollection<Comment> comments { get; set; }


    }
}
