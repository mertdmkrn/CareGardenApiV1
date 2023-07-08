using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Model
{
    [Table("Campaign")]
    public class Campaign
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public string? path { get; set; }

        public string? url { get; set; }
        public bool isActive { get; set; }
        public DateTime? createDate { get; set; }

        public DateTime? updateDate { get; set; }
        public int sortOrder { get; set; }
        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }
    }
}
