using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Model
{
    [Table("ConfirmationInfo")]
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
