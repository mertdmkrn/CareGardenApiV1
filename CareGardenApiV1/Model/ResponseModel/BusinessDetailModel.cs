using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;
using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class BusinessDetailModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string telephone { get; set; }
        public string description { get; set; }
        public string descriptionEn { get; set; }
        public int discountRate { get; set; }
        public double averageRating { get; set; }
        public int countRating { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public bool isOpen { get; set; }
        public bool isFeatured { get; set; }
        public bool hasPromotion { get; set; }
        public WorkingGenderType workingGenderType { get; set; }
        public BusinessWorkingInfo businessWorkingInfo { get; set; } = new BusinessWorkingInfo();
        public ICollection<BusinessServicesInfo> businessServicesInfos { get; set; } = new HashSet<BusinessServicesInfo>();
        public ICollection<BusinessGallery> assets { get; set; } = new HashSet<BusinessGallery>();
        public ICollection<Worker> workers { get; set; } = new HashSet<Worker>();
        public ICollection<BusinessProperties> properties { get; set; } = new HashSet<BusinessProperties>();

        public ICollection<Discount> discounts { get; set; } = new HashSet<Discount>();


        [JsonIgnore]
        public ICollection<BusinessServiceModel> businessServices { get; set; } = new HashSet<BusinessServiceModel>();

        [JsonIgnore]
        public bool officialDayAvailable { get; set; }


    }
}
