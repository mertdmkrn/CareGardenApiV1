using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.TableModel
{
    [Table("BusinessGallery")]
    [Index(nameof(businessId))]
    public class BusinessGallery
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public string? imageUrl { get; set; }

        [MaxLength(9)]
        public string? size { get; set; }

        public bool isProfilePhoto { get; set; }
        public bool isSliderPhoto { get; set; }
        public int sortOrder { get; set; }

        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

    }
}
