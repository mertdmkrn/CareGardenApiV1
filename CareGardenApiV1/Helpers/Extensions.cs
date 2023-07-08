using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static CareGardenApiV1.Helpers.Enums;

namespace CareGardenApiV1.Helpers
{
    public static class Extensions
    {
        private static readonly Regex RegexGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

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

        public static double IsNull(this double value, double value2)
        {
            return value != 0 ? value : value2;
        }

        public static int IsNull(this int value, int value2)
        {
            return value != 0 ? value : value2;
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

        public static bool IsValidFullName(this string fullName)
        {
            if (fullName.IsNullOrEmpty()) return false;

            var fullNameArr = fullName.Trim().Split(" ");

            if(fullNameArr.Length < 2) return false;

            for (int i = 0; i < fullNameArr.Length; i++)
            {
                if(i == 0 && fullNameArr[i].Length < 3) return false;

                if(i > 0 && fullNameArr[i].Length < 2) return false;
            }

            return true;
        }

        public static Guid ToGuid(this string target, Guid? defaultValue = null)
        {
            if (string.IsNullOrEmpty(target)) return defaultValue.GetValueOrDefault(Guid.Empty);

            return target.IsGuid() ? new Guid(target) : defaultValue.GetValueOrDefault(Guid.Empty);
        }

        public static Guid? ToGuidNullable(this string target, Guid? defaultValue = null)
        {
            if (string.IsNullOrEmpty(target)) return defaultValue;

            return target.IsGuid() ? new Guid(target) : defaultValue;
        }

        public static bool IsGuid(this string value)
        {
            return !string.IsNullOrEmpty(value) && RegexGuid.IsMatch(value);
        }
        public static string TurkishChrToEnglishChr(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            Dictionary<char, char> TurkishChToEnglishChDic = new Dictionary<char, char>()
            {
                {'ç','c'},
                {'Ç','C'},
                {'ğ','g'},
                {'Ğ','G'},
                {'ı','i'},
                {'İ','I'},
                {'ş','s'},
                {'Ş','S'},
                {'ö','o'},
                {'Ö','O'},
                {'ü','u'},
                {'Ü','U'}
            };

            return text.Aggregate(new StringBuilder(), (sb, chr) =>
            {
                if (TurkishChToEnglishChDic.ContainsKey(chr))
                    sb.Append(TurkishChToEnglishChDic[chr]);
                else
                    sb.Append(chr);

                return sb;
            }).ToString();
        }

    }
}
