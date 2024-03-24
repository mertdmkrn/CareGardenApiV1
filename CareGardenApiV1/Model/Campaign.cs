using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model
{
    [Table("Campaign")]
    [Index(nameof(businessId))]
    public class Campaign
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public string? path { get; set; }

        public string? url { get; set; }

        [MaxLength(150)]
        public string? title { get; set; }

        [MaxLength(150)]
        public string? titleEn { get; set; }

        [MaxLength(250)]
        public string? about { get; set; }

        [MaxLength(250)]
        public string? aboutEn { get; set; }

        public string? condition { get; set; }

        public string? conditionEn { get; set; }

        public bool isActive { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public DateTime? expireDate { get; set; }
        public int sortOrder { get; set; }
        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }
    }
}
