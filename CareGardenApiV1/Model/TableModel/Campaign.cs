using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.TableModel
{
    [Table("Campaign")]
    [Index(nameof(businessId))]
    [Index(nameof(isActive))]
    public class Campaign
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public string? path { get; set; }
        public string? pathEn { get; set; }

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
