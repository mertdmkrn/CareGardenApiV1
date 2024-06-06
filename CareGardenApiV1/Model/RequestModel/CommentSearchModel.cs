using CareGardenApiV1.Helpers;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.RequestModel
{
    public class CommentSearchModel
    {
        public Guid? businessId { get; set; }
        [JsonIgnore]
        public Guid? userId { get; set; }
        public int page { get; set; }
        public int take { get; set; }
        public CommentFilterType filterType { get; set; }
        public CommentOrderType orderType { get; set; }
    }
}
