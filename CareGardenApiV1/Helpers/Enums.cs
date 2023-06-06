namespace CareGardenApiV1.Helpers
{
    public class Enums
    {
        public enum AppointmentStatus { 

            Beklemede,
            Onaylandi      
        }

        public enum Gender
        {
            Belirtilmemis,
            Erkek,
            Kadin
        }

        public enum ConfirmationType
        {
            Sms,
            Email
        }

        public enum DateType
        {
            Second,
            Minute,
            Hour,
            Day,
            Month,
            Year
        }
    }
}
