using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.TableModel
{
    [Table("BusinessProperties")]
    [Index(nameof(businessId))]
    public class BusinessProperties
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [MaxLength(100)]
        public string? key { get; set; }

        public string? value { get; set; }

        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }
    }
}
