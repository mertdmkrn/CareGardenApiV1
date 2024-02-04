using System.Text.Json.Serialization;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Model.RequestModel
{
    public class UserSearchAdminModel
    {
        public int page { get; set; }
        public int take { get; set; }
        public string? city { get; set; }
        public string? email { get; set; }
        public string? role { get; set; }
        public Gender gender { get; set; }
    }
}
