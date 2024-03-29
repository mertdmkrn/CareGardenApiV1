namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessFileInfoModel
    {
        public Guid? businessId{ get; set; } = Guid.Empty;
        public bool isSliderPhoto{ get; set; }
        public bool isProfilePhoto{ get; set; }
        public int sortOrder{ get; set; }
        public IFormFile file{ get; set; }
    }
}
