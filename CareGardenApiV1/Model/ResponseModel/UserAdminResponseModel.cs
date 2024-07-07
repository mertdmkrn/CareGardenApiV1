using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class UserAdminResponseModel
    {
        public Guid id { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public string city { get; set; }
        public string imageUrl { get; set; }
        public string role { get; set; }
        public int gender { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? birthDate { get; set; }
        public bool isBan { get; set; }
        public ICollection<Complain> complains { get; set; }
        public int itemCount { get; set; }

    }
}
