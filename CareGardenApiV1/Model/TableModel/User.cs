using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.TableModel
{
    [Table("User")]
    [Index(nameof(email))]
    [Index(nameof(telephone))]
    public class User
    {
        public User()
        {
            comments = new HashSet<Comment>();
            favorites = new HashSet<Favorite>();
            appointments = new HashSet<Appointment>();
            complains = new HashSet<Complain>();
            notifications = new HashSet<Notification>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; } = Guid.Empty;

        [MaxLength(150)]
        public string? fullName { get; set; }

        public Gender gender { get; set; }

        [MaxLength(150)]
        public string? email { get; set; }

        [MaxLength(20)]
        public string? telephone { get; set; }


        [MaxLength(50)]
        public string? password { get; set; }

        [NotMapped]
        public string? retryPassword { get; set; }


        [MaxLength(50)]
        public string? city { get; set; }

        public DateTime? createDate { get; set; }

        public DateTime? updateDate { get; set; }

        public DateTime? birthDate { get; set; }

        public string? services { get; set; }

        [JsonIgnore]
        [MaxLength(30)]
        public string? role { get; set; }

        public string? imageUrl { get; set; }

        public bool isBan { get; set; }

        public double? latitude { get; set; }
        public double? longitude { get; set; }

        [JsonIgnore]
        [Column(TypeName = "geometry (point)")]
        public Point? location { get; set; }

        public bool hasNotification { get; set; }

        [JsonIgnore]
        public virtual ICollection<Comment> comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<Favorite> favorites { get; set; }
        [JsonIgnore]
        public virtual ICollection<Appointment> appointments { get; set; }
        [JsonIgnore]
        public virtual ICollection<Complain> complains { get; set; }
        [JsonIgnore]
        public virtual ICollection<Notification> notifications { get; set; }

    }
}
