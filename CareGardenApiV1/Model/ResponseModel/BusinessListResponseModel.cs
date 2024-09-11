using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.TableModel;
using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class BusinessListResponseModel
    {
        public BusinessListResponseModel()
        {
        }

        public Guid id { get; set; }
        public string name { get; set; }
        public string nameForUrl { get; set; }
        public string logoUrl { get; set; }
        public double distance { get; set; }
        public double averageRating { get; set; }
        public int countRating { get; set; }
        public double discountRate { get; set; }
        public int workingGenderType { get; set; }
        public string? imageUrl { get; set; }
        public bool isFeatured { get; set; }
        public bool hasPromotion { get; set; }
        public bool isOpen { get; set; }
        public byte isRecommended { get; set; }
        public DateTime? createDate { get; set; }
        public int resultCount { get; set; }

        [JsonIgnore]
        public Point? location { get; set; }

        [JsonIgnore]
        public BusinessWorkingInfo workingInfo { get; set; }

        [JsonIgnore]
        public bool officialDayAvailable { get; set; }

        [JsonIgnore]
        public int appointmentTimeInterval { get; set; }

        [JsonIgnore]
        public List<Appointment> appointments { get; set; }

        [JsonIgnore]
        public List<Discount> discounts { get; set; }

        [JsonIgnore]
        public List<Guid?> serviceIds { get; set; }

        [JsonIgnore]
        public string city { get; set; }
    }
}
