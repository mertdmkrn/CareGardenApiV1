using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CareGardenApiV1.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model
{
    [Table("Comment")]
    [Index(nameof(userId), nameof(businessId))]
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [MaxLength(300)]
        public string? comment { get; set; }

        public double point { get; set; }
        public double workerPoint { get; set; }

        [MaxLength(20)]
        public string? aspectsOfPoint { get; set; }
        [MaxLength(20)]
        public string? aspectsOfWorkerPoint { get; set; }

        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public Guid? userId { get; set; }
        public Guid? businessId { get; set; }
        public Guid? appointmentId { get; set; }
        public Guid? replyId { get; set; }
        public CommentType commentType { get; set; }
        public bool isShowProfile { get; set; }

        [JsonIgnore]
        public Business? business { get; set; }
        [JsonIgnore]
        public User? user { get; set; }
        [JsonIgnore]
        public Appointment? appointment { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IEnumerable<Guid?>? workerIds { get; set; }

        public Comment? reply { get; set; }
    }
}
