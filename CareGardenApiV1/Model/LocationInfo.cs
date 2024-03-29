namespace CareGardenApiV1.Model
{
    public class LocationInfo
    {
        public string name { get; set; }
        public string baseName { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public LocationInfo(string name, string baseName, double latitude, double longitude)
        {
            this.name = name;
            this.baseName = baseName;
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }
}
