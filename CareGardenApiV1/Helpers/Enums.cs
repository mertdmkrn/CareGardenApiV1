namespace CareGardenApiV1.Helpers
{
    public class Enums
    {
        public enum AppointmentStatus {

            Waiting,
            Approve
        }

        public enum Gender
        {
            Women,
            Men,
            Unspecified
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

        public enum WorkingGenderType
        {
            Everyone,
            FemaleOnly,
            MaleOnly
        }

        public enum WorkingDayType
        {
            MondayFriday,
            MondaySaturday,
            AllDay
        }

        public enum CommentType
        {
            User,
            Business
        }

        public enum PaidType
        {
            Subscribe,
            Campaign,
            Sorting
        }

    }
}
