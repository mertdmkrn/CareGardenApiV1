using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.TableModel
{
    [Table("BusinessCustomer")]
    [Index(nameof(businessId))]
    public class BusinessCustomer
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
        public string? imageUrl { get; set; }
        public DateTime? createDate { get; set; }

        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

    }
}
