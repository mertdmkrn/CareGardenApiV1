using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Table("BusinessService")]
    [Index(nameof(serviceId))]
    public class BusinessServiceModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public Guid? businessId { get; set; }
        public Guid? serviceId { get; set; }
        
        [JsonIgnore]
        public Business? business { get; set; }

        [JsonIgnore]
        public Services? service { get; set; }

    }
}
