using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Model
{
    [Table("User")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

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

    }
}
