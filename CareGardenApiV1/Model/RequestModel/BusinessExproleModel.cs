using CareGardenApiV1.Helpers;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessExploreModel
    {
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public DateTime? availableDate { get; set; }
        public Guid? serviceId { get; set; }
        public DayZone? dayZone { get; set; }
        public List<int> offers { get; set; } = new List<int>();
        public int? page { get; set; } = 0;
        public int? take { get; set; } = 5;
        public int? isWithinKilometer { get; set; }
        public WorkingGenderType workingGenderType { get; set; }
        public SortByType sortByType { get; set; }
        [JsonIgnore]
        public List<Guid?> favoriteBusinessIds { get; set; } = new();
        public string? city { get; set; }
        public bool isStartPage { get; set; }
    }
}
