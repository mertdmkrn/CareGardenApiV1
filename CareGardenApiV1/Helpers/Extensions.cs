using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Helpers
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value?.Trim());
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string IsNull(this string value, string value2)
        {
            return value.IsNotNullOrEmpty() ? value : value2;
        }

        public static string HashString(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return null;
            }

            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(value);
            var hashedValue = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedValue);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> target)
        {
            return !(target != null && target.Count() > 0);
        }

        public static int ToInt(this string number, int defaultInt = 0)
        {
            int resultNum = defaultInt;
            try
            {
                if (!string.IsNullOrEmpty(number))
                    resultNum = Convert.ToInt32(number);
            }
            catch
            {
            }

            return resultNum;
        }

        public static double DifferenceBetweenDates(this DateTime? startDate, DateTime? endDate, DateType dateType = DateType.Minute)
        {
            if (startDate == null || endDate == null) { return 0; }

            TimeSpan timespan = endDate.Value.Subtract(startDate.Value);

            switch (dateType)
            {
                case DateType.Second: return Math.Round(timespan.TotalSeconds, 2);
                case DateType.Minute: return Math.Round(timespan.TotalMinutes, 2);
                case DateType.Hour: return Math.Round(timespan.TotalHours, 2);
                case DateType.Day: return Math.Round(timespan.TotalDays, 2);
                case DateType.Month: return endDate.Value.Month - startDate.Value.Month;
                case DateType.Year: return endDate.Value.Year - startDate.Value.Year;
            }

            return 0;
        }
    }
}
