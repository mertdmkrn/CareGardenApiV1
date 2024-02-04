using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model
{
    [Table("Business")]
    [Index(nameof(email), nameof(telephone), nameof(city))]
    public class Business
    {
        public Business()
        {
            this.comments = new HashSet<Comment>();
            this.services = new HashSet<BusinessServiceModel>();
            this.galleries = new HashSet<BusinessGallery>();
            this.workingInfos = new HashSet<BusinessWorkingInfo>();
            this.appointments = new HashSet<Appointment>();
            this.properties = new HashSet<BusinessProperties>();
            this.paymentInfos = new HashSet<PaymentInfo>();
            this.favorites = new HashSet<Favorite>();
            this.workers = new HashSet<Worker>();
            this.campaigns = new HashSet<Campaign>();
            this.complains = new HashSet<Complain>();
            this.discounts = new HashSet<Discount>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [MaxLength(100)]
        public string? name { get; set; }

        public string? description { get; set; }
        public string? descriptionEn { get; set; }

        [MaxLength(80)]
        public string? city { get; set; }

        [MaxLength(80)]
        public string? province { get; set; }
       
        [MaxLength(80)]
        public string? district { get; set; }

        [MaxLength(200)]
        public string? address { get; set; }

        [MaxLength(20)]
        public string? telephone { get; set; }

        [MaxLength(100)]
        public string? email { get; set; }

        [MaxLength(50)]
        public string? password { get; set; }

        [NotMapped]
        public string? retryPassword { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }

        [JsonIgnore]
        [Column(TypeName = "geometry (point)")]
        public Point? location { get; set; }


        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public WorkingGenderType workingGenderType { get; set; }
        public bool officialHolidayAvailable { get; set; }
        public int appointmentTimeInterval { get; set; }
        public int appointmentPeopleCount { get; set; }
        public bool isActive { get; set; }
        public bool verified { get; set; }
        public bool isFeatured { get; set; }
        public bool hasPromotion { get; set; }

        public virtual ICollection<Comment> comments { get; set; }
        public virtual ICollection<BusinessGallery> galleries { get; set; }
        public virtual ICollection<BusinessServiceModel> services { get; set; }
        public virtual ICollection<BusinessWorkingInfo> workingInfos { get; set; }
        public virtual ICollection<Appointment> appointments { get; set; }
        public virtual ICollection<BusinessProperties> properties { get; set; }
        public virtual ICollection<PaymentInfo> paymentInfos { get; set; }
        public virtual ICollection<Favorite> favorites { get; set; }
        public virtual ICollection<Worker> workers { get; set; }
        public virtual ICollection<Campaign> campaigns { get; set; }
        public virtual ICollection<Complain> complains { get; set; }
        public virtual ICollection<Discount> discounts { get; set; }
    }
}
