using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.TableModel
{
    [Table("BusinessWorkingInfo")]
    [Index(nameof(businessId))]
    public class BusinessWorkingInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [MaxLength(20)]
        public string? mondayWorkHours { get; set; }
        [MaxLength(20)]
        public string? tuesdayWorkHours { get; set; }
        [MaxLength(20)]
        public string? wednesdayWorkHours { get; set; }
        [MaxLength(20)]
        public string? thursdayWorkHours { get; set; }
        [MaxLength(20)]
        public string? fridayWorkHours { get; set; }
        [MaxLength(20)]
        public string? saturdayWorkHours { get; set; }
        [MaxLength(20)]
        public string? sundayWorkHours { get; set; }

        public Guid? businessId { get; set; }
        public bool officialHolidayAvailable { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

    }
}
