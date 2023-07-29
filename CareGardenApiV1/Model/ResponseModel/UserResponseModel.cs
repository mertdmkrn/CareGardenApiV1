namespace CareGardenApiV1.Model.ResponseModel
{
    public class UserResponseModel
    {
        public Guid id { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public string city { get; set; }
        public string imageUrl { get; set; }
        public HashSet<string> favoriteBusinessList { get; set; } = new HashSet<string>();
        public string services { get; set; }
        public int gender { get; set; }
        public DateTime? birthDate { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
