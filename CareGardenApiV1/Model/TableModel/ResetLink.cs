using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Model.TableModel
{
    [Table("ResetLink")]
    [Index(nameof(email))]
    public class ResetLink
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [MaxLength(100)]
        public string email { get; set; }

        [MaxLength(100)]
        public Guid? linkId { get; set; }

        public DateTime? createDate { get; set; }
    }
}
