using CareGardenApiV1.Helpers;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.RequestModel
{
    public class BusinessExproleModel
    {
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public DateTime? availableDate { get; set; }
        public Guid? serviceId { get; set; }
        public DayZone dayZone { get; set; }
        public int offers { get; set; }
        public int? page { get; set; }
        public int? take { get; set; }
        public int isWithinKilometer { get; set; } = 200;
        public WorkingGenderType workingGenderType { get; set; }
        public SortByType sortByType { get; set; }
    }
}
