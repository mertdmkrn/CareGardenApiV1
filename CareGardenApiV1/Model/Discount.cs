using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Model
{
    [Table("Discount")]
    [Index(nameof(businessId), nameof(isActive))]
    public class Discount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid? businessId { get; set; }
        public string serviceIds { get; set; } = string.Empty;
        public bool isActive { get; set; }
        public string description { get; set; }
        public string descriptionEn { get; set; }
        public DiscountType discountType { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }
    }
}
