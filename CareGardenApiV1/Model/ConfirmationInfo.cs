using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CareGardenApiV1.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CareGardenApiV1.Model
{
    [Table("ConfirmationInfo")]
    [Index(nameof(target), nameof(code))]
    public class ConfirmationInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [MaxLength(100)]
        public string target { get; set; }
        
        [MaxLength(4)]
        public string code { get; set; }

        public DateTime? createDate { get; set; }
    }
}
