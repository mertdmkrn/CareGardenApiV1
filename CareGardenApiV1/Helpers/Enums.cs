namespace CareGardenApiV1.Helpers
{
    public enum AppointmentStatus
    {

        Waiting,
        Approve,
        Rejected
    }

    public enum Gender
    {
        Women,
        Men,
        Unspecified,
        Nothing
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


    public enum SortByType
    {
        Recommended,
        MostPopular,
        Nearest,
        TopRated,
        Newest
    }

    public enum WorkingGenderType
    {
        Everyone,
        FemaleOnly,
        MaleOnly,
        All
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

    public enum DayZone
    {
        Morning,
        Afternoon,
        Evening
    }

    public enum DiscountType
    {
        AllDay,
        WeekDay,
        WeekEnd
    }

    public enum CommentFilterType
    {
        AllRates,
        Rate1,
        Rate2,
        Rate3,
        Rate4,
        Rate5
    }

    public enum CommentOrderType
    {
        Lastest,
        Oldest,
        Popular,
        Worst
    }

    public enum SettingType
    {
        String,
        ListString,
        Integer,
        ListInteger,
        Boolean
    }
}
