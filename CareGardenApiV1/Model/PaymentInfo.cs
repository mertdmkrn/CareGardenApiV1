using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static CareGardenApiV1.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Table("PaymentInfo")]
    [Index(nameof(businessId), nameof(paidType))]
    public class PaymentInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [MaxLength(200)]
        public string? description { get; set; }

        public DateTime? date { get; set; }
        public DateTime? payDate { get; set; }
        public double amount { get; set; }
        public double payAmount { get; set; }
        public string? receiptFilePath { get; set; }
        public PaidType paidType { get; set; }
        public bool isPaid { get; set; }
        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

    }
}
