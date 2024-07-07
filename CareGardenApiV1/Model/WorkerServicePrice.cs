using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Table("WorkerServicePrice")]
    [Index(nameof(businessServiceId))]
    [Index(nameof(workerId))]
    public class WorkerServicePrice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid? businessServiceId { get; set; }
        public Guid? workerId { get; set; }
        public double price { get; set; }

        [JsonIgnore]
        public BusinessServiceModel? businessService { get; set; }
        [JsonIgnore]
        public Worker? worker { get; set; }

    }
}
