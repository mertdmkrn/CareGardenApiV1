using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Table("Services")]
    public class Services
    {

        public Services()
        {
            this.businessServices = new HashSet<BusinessServiceModel>();
        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [MaxLength(100)]
        public string? name { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [MaxLength(100)]
        public string? nameEn { get; set; }

        [MaxLength(50)]
        public string? className { get; set; }

        [MaxLength(50)]
        public string? colorCode { get; set; }
        public int sortOrder { get; set; }

        public virtual ICollection<BusinessServiceModel> businessServices { get; set; }
    }
}
