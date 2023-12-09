using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.RequestModel;
using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class BusinessListModel
    {
        public BusinessListModel()
        {
        }

        public BusinessListModel(BusinessDetailModel businessDetailModel, GeometryFactory? gf, Point? userLocation)
        {
            Point? businessLocation = null;

            if (businessDetailModel.latitude > 0 && businessDetailModel.longitude > 0)
            {
                businessLocation = gf.CreatePoint(new Coordinate(businessDetailModel.latitude, businessDetailModel.longitude));
            }

            id = businessDetailModel.id;
            name = businessDetailModel.name;
            averageRating = businessDetailModel.averageRating;
            countRating = businessDetailModel.countRating;
            workingGenderType = (int)businessDetailModel.workingGenderType;
            imageUrl = businessDetailModel.assets.FirstOrDefault(x => x.isProfilePhoto)?.imageUrl;
            isFeatured = businessDetailModel.isFeatured;
            hasPromotion = businessDetailModel.hasPromotion;
            workingInfo = businessDetailModel.businessWorkingInfo;
            distance = userLocation != null && businessLocation != null 
                        ? Math.Round(businessLocation.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue, 1)
                        : 0;
            isOpen = HelperMethods.GetBusinessOpen(workingInfo, businessDetailModel.officialDayAvailable);
        }

        public Guid id { get; set; }
        public string name { get; set; }
        public double distance { get; set; }
        public double averageRating { get; set; }
        public int countRating { get; set; }
        public double discountRate { get; set; }
        public int workingGenderType { get; set; }
        public string imageUrl { get; set; }
        public bool isFeatured { get; set; }
        public bool hasPromotion { get; set; }
        public bool isOpen { get; set; }
        public DateTime? createDate { get; set; }

        [JsonIgnore]
        public BusinessWorkingInfo workingInfo { get; set; }

        [JsonIgnore]
        public bool officialDayAvailable { get; set; }

        [JsonIgnore]
        public List<Appointment> appointments { get; set; }

        [JsonIgnore]
        public int appointmentTimeInterval { get; set; }

        [JsonIgnore]
        public int appointmentPeopleCount { get; set; }
    }
}
