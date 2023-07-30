using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Table("BusinessWorkingInfo")]
    [Index(nameof(businessId))]
    public class BusinessWorkingInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public string? mondayWorkHours { get; set; }
        public string? tuesdayWorkHours { get; set; }
        public string? wednesdayWorkHours { get; set; }
        public string? thursdayWorkHours { get; set; }
        public string? fridayWorkHours { get; set; }
        public string? saturdayWorkHours { get; set; }
        public string? sundayWorkHours { get; set; }
    
        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

    }
}
