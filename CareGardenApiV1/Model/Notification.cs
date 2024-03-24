using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model
{
    [Table("Notification")]
    [Index(nameof(userId), nameof(businessId))]
    public class Notification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public DateTime? date { get; set; }

        public string? description { get; set; }

        public string? descriptionEn { get; set; }

        public NoticationType type { get; set; }

        public Guid? redirectId { get; set; }
        public string? redirectUrl { get; set; }
        public bool isRead { get; set; }

        public Guid? userId { get; set; }
        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }
        [JsonIgnore]
        public User? user { get; set; }

    }
}
