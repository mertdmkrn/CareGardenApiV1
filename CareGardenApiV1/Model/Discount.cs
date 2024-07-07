using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model
{
    [Table("Discount")]
    [Index(nameof(businessId))]
    [Index(nameof(isActive))]
    public class Discount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid? businessId { get; set; }
        public string serviceIds { get; set; } = string.Empty;
        public bool isActive { get; set; }
        public string description { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string descriptionEn { get; set; } = string.Empty;

        public double rate { get; set; }
        public DiscountType type { get; set; }
        public string? colorCode { get; set; } = string.Empty;

        [NotMapped]
        public string title { get; set; } = string.Empty;

        [JsonIgnore]
        public Business? business { get; set; }
    }
}
