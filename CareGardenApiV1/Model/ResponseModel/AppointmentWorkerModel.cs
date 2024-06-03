using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class AppointmentWorkerModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string path { get; set; }
        public double rating { get; set; }
        public int countRating { get; set; }
        public double price { get; set; }
        public double discountPrice { get; set; }
        public double discountRate { get; set; }

        public bool isActive { get; set; }
        public string availableDateStr { get; set; }
        public DateTime? availableDate { get; set; }

        [JsonIgnore]
        public string? mondayWorkHours { get; set; }
        [JsonIgnore]
        public string? tuesdayWorkHours { get; set; }
        [JsonIgnore]
        public string? wednesdayWorkHours { get; set; }
        [JsonIgnore]
        public string? thursdayWorkHours { get; set; }
        [JsonIgnore]
        public string? fridayWorkHours { get; set; }
        [JsonIgnore]
        public string? saturdayWorkHours { get; set; }
        [JsonIgnore]
        public string? sundayWorkHours { get; set; }
        [JsonIgnore]
        public string? serviceIds { get; set; }

        public string GetWorkerWorkHours(DateTime businessStartDate)
        {
            switch (businessStartDate.DayOfWeek)
            {
                case DayOfWeek.Monday: return mondayWorkHours;
                case DayOfWeek.Tuesday: return tuesdayWorkHours;
                case DayOfWeek.Wednesday: return wednesdayWorkHours;
                case DayOfWeek.Thursday: return thursdayWorkHours;
                case DayOfWeek.Friday: return fridayWorkHours;
                case DayOfWeek.Saturday: return saturdayWorkHours;
                case DayOfWeek.Sunday: return sundayWorkHours;
                default: return null;
            }
        }
    }
}
