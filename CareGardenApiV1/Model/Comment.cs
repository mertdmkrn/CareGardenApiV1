using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static CareGardenApiV1.Helpers.Enums;
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
        public DateTime? createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public Guid? userId { get; set; }
        public Guid? businessId { get; set; }
        public Guid? replyId { get; set; }
        public CommentType commentType { get; set; }
       
        [JsonIgnore]
        public Business? business { get; set; }
        [JsonIgnore]
        public User? user { get; set; }

        public Comment? reply { get; set; }
    }
}
