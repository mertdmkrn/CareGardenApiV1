using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.TableModel
{
    [Table("Business")]
    [Index(nameof(email))]
    [Index(nameof(telephone))]
    [Index(nameof(city))]
    [Index(nameof(nameForUrl))]
    public class Business
    {
        public Business()
        {
            comments = new HashSet<Comment>();
            services = new HashSet<BusinessServiceModel>();
            galleries = new HashSet<BusinessGallery>();
            workingInfos = new HashSet<BusinessWorkingInfo>();
            appointments = new HashSet<Appointment>();
            properties = new HashSet<BusinessProperties>();
            paymentInfos = new HashSet<PaymentInfo>();
            favorites = new HashSet<Favorite>();
            workers = new HashSet<Worker>();
            campaigns = new HashSet<Campaign>();
            complains = new HashSet<Complain>();
            discounts = new HashSet<Discount>();
            notifications = new HashSet<Notification>();
            businessUsers = new HashSet<BusinessUser>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [MaxLength(150)]
        public string? name { get; set; }

        [MaxLength(200)]
        public string? nameForUrl { get; set; }

        public string? description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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

        public double latitude { get; set; }
        public double longitude { get; set; }

        [JsonIgnore]
        [Column(TypeName = "geometry (point)")]
        public Point? location { get; set; }

        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public WorkingGenderType workingGenderType { get; set; }
        public WorkerSizeType workingSizeType { get; set; }
        public string? serviceIds { get; set; }
        public bool officialHolidayAvailable { get; set; }
        public int appointmentTimeInterval { get; set; }
        public int appointmentPeopleCount { get; set; }
        public bool isActive { get; set; }
        public bool verified { get; set; }
        public bool isFeatured { get; set; }
        public bool hasPromotion { get; set; }
        [MaxLength(200)]
        public string? logoUrl { get; set; }

        public bool hasNotification { get; set; }
        public bool mobileOrOnlineServiceOnly { get; set; }


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

        [JsonIgnore]
        public virtual ICollection<Notification> notifications { get; set; }

        [JsonIgnore]
        public virtual ICollection<BusinessUser> businessUsers { get; set; }
    }
}
