using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Table("BusinessWorkingInfo")]
    [Index(nameof(businessId), nameof(date))]
    public class BusinessWorkingInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public DateTime? date { get; set; }

        [MaxLength(5)]
        public string? startHour { get; set; }

        [MaxLength(5)]
        public string? endHour { get; set; }

        public int appointmentTimeInterval { get; set; }
        public int appointmentPeopleCount { get; set; }

        public bool isOffDay { get; set; }

        public Guid? businessId { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }

    }
}
