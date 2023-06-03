using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareGardenApiV1.Model
{
    [Table("User")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(150)]
        public string? fullName { get; set; }

        public int gender { get; set; }

        [MaxLength(150)]
        public string? email { get; set; }

        [MaxLength(11)]
        public string? telephone { get; set; }

        [MaxLength(50)]
        public string? password { get; set; }


        [MaxLength(50)]
        public string? city { get; set; }

        public DateTime? createDate { get; set; }

        public DateTime? updateDate { get; set; }

        public DateTime? birthDate { get; set; }

        public string? services { get; set; }

    }
}
