using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareGardenApiV1.Model
{
    [Table("Services")]
    public class Services
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [MaxLength(100)]
        public string? name { get; set; }

        [MaxLength(100)]
        public string? nameEn { get; set; }

        [MaxLength(50)]
        public string? className { get; set; }

        [MaxLength(50)]
        public string? colorCode { get; set; }
        public int sortOrder { get; set; }

    }
}
