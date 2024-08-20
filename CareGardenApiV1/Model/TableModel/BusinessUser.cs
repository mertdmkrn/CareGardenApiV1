using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;
using System.Xml.Linq;

namespace CareGardenApiV1.Model.TableModel
{
    [Table("BusinessUser")]
    [Index(nameof(email))]
    [Index(nameof(telephone))]
    public class BusinessUser
    {
        public BusinessUser()
        {
            businessPayments = new HashSet<BusinessPayment>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; } = Guid.Empty;

        [MaxLength(150)]
        public string? fullName { get; set; }

        [MaxLength(100)]
        public string? title { get; set; }

        public Gender gender { get; set; }

        [MaxLength(150)]
        public string? email { get; set; }

        [MaxLength(20)]
        public string? telephone { get; set; }

        [MaxLength(50)]
        public string? password { get; set; }

        [NotMapped]
        public string? retryPassword { get; set; }

        public DateTime? createDate { get; set; }

        public DateTime? updateDate { get; set; }

        public DateTime? birthDate { get; set; }

        [JsonIgnore]
        [MaxLength(30)]
        public string? role { get; set; }

        public string? imageUrl { get; set; }

        public bool isBan { get; set; }
        public bool hasNotification { get; set; }

        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

        public virtual ICollection<BusinessPayment> businessPayments { get; set; }
    }
}
