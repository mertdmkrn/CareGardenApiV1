using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CareGardenApiV1.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Model.TableModel
{
    [Index(nameof(name), IsUnique = true)]
    [Table("Setting")]
    public class Setting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; } = Guid.Empty;

        [MaxLength(50)]
        public string? name { get; set; }

        [MaxLength(200)]
        public string? description { get; set; }

        public SettingType type { get; set; }

        public string? value { get; set; }
    }
}
