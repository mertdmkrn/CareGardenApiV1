using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Model
{
    [Table("User")]
    [Index(nameof(email), nameof(telephone))]
    public class User
    {
        public User()
        {
            this.comments = new HashSet<Comment>();
            this.favorites = new HashSet<Favorite>();
            this.appointments = new HashSet<Appointment>();
            this.complains = new HashSet<Complain>();
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

        public double latitude { get; set; }
        public double longitude { get; set; }

        [JsonIgnore]
        public Point? location { get; set; }


        [JsonIgnore]
        public virtual ICollection<Comment> comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<Favorite> favorites { get; set; }
        [JsonIgnore]
        public virtual ICollection<Appointment> appointments { get; set; }
        [JsonIgnore]
        public virtual ICollection<Complain> complains { get; set; }

    }
}
