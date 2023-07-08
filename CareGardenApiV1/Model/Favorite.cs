using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static CareGardenApiV1.Helpers.Enums;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Table("Favorite")]
    [Index(nameof(userId))]
    public class Favorite
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public Guid? userId { get; set; }
        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }
        [JsonIgnore]
        public User? user { get; set; }
    }
}
