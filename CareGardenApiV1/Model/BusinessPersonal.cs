using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareGardenApiV1.Model
{
    [Table("BusinessPersonal")]
    [Index(nameof(businessId))]
    public class BusinessPersonal
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; } = Guid.Empty;

        public Guid businessId { get; set; }
        public string? businessServiceIds { get; set; }
       
        [MaxLength(100)]
        public string? fullName { get; set; }

        [MaxLength(200)]
        public string? imageUrl { get; set; }

        [MaxLength(100)]
        public string? title { get; set; }

    }
}
