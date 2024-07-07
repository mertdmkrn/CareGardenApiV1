namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessFileInfoRequestModel
    {
        public Guid? businessId{ get; set; } = Guid.Empty;
        public bool isSliderPhoto{ get; set; }
        public bool isProfilePhoto{ get; set; }
        public int sortOrder{ get; set; }
        public IFormFile file{ get; set; }
    }
}
