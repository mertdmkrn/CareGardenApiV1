using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class BusinessUserResponseModel
    {
        public Guid id { get; set; }
        public Guid? businessId { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string title { get; set; }
        public string telephone { get; set; }
        public string imageUrl { get; set; }
        public int gender { get; set; }
        public DateTime? birthDate { get; set; }
        public bool hasNotification { get; set; }
        public Business? business { get; set; }
    }
}
