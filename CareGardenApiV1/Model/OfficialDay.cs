namespace CareGardenApiV1.Model
{
    public class OfficialDay
    {
        public string name { get; set; }
        public string nameEn { get; set; }
        public string dayName { get; set; }
        public DateTime date { get; set; }

        public OfficialDay(string name, string nameEn, string dayName, DateTime date)
        {
            this.name = name;
            this.nameEn = nameEn;
            this.dayName = dayName;
            this.date = date;
        }
    }
}
