using System.Text.Json.Serialization;
using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Model.RequestModel
{
    public class CommentSearchModel
    {
        public Guid businessId { get; set; }
        public int page { get; set; }
        public int take { get; set; }
        public CommentFilterType filterType { get; set; }
        public CommentOrderType orderType { get; set; }
    }
}
