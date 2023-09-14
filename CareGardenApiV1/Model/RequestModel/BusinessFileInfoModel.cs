using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessFileInfoModel
    {
        public Guid? businessId{ get; set; } = Guid.Empty;
        public IFormFile file{ get; set; }
    }
}
