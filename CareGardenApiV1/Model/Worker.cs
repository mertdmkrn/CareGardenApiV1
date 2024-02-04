using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model
{
    [Index(nameof(businessId))]
    [Table("Worker")]
    public class Worker
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; } = Guid.Empty;

        [MaxLength(150)]
        public string? name { get; set; }
        [MaxLength(100)]
        public string? title { get; set; }
        public string? path { get; set; }
        public bool isActive { get; set; }
        public bool isAvailable { get; set; }
        public string serviceIds { get; set; } = string.Empty;
        public Guid? businessId { get; set; }
        public Guid? createdUserId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }
    }
}
