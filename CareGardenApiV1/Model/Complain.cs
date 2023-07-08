using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Table("Complain")]
    [Index(nameof(userId), nameof(businessId))]
    public class Complain
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public DateTime? date { get; set; }

        [MaxLength(200)]
        public string? description { get; set; }

        public Guid? userId { get; set; }
        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }
        [JsonIgnore]
        public User? user { get; set; }

    }
}
