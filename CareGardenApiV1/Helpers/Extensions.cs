using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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

        public static bool IsNotNullOrEmpty(this Guid? value)
        {
            return value.HasValue && value != Guid.Empty;
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

        public static int ToInt(this double number, int defaultInt = 0)
        {
            int resultNum = defaultInt;
            try
            {
                resultNum = Convert.ToInt32(number);
            }
            catch
            {
            }

            return resultNum;
        }

        public static bool ToBoolean(this string value)
        {
            return value.IsNull("").ToLower() == "true";
        }


        public static bool Between(this int number, int start, int end)
        {
            return number >= start && number <= end;
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

        public static double DifferenceBetweenDates(this DateTime startDate, DateTime endDate, DateType dateType = DateType.Minute)
        {
            if (startDate == null || endDate == null) { return 0; }

            TimeSpan timespan = endDate.Subtract(startDate);

            switch (dateType)
            {
                case DateType.Second: return Math.Round(timespan.TotalSeconds, 2);
                case DateType.Minute: return Math.Round(timespan.TotalMinutes, 2);
                case DateType.Hour: return Math.Round(timespan.TotalHours, 2);
                case DateType.Day: return Math.Round(timespan.TotalDays, 2);
                case DateType.Month: return endDate.Month - startDate.Month;
                case DateType.Year: return endDate.Year - startDate.Year;
            }

            return 0;
        }

        public static bool IsValidFullName(this string fullName)
        {
            if (fullName.IsNullOrEmpty()) return false;

            var fullNameArr = fullName.Trim().Split(" ");

            if (fullNameArr.Length < 1) return false;

            if (fullNameArr.FirstOrDefault().Length < 3) return false;

            if (fullNameArr.LastOrDefault().Length < 2) return false;

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

        public static string GetLangugeValue(string language, string trValue, string enValue)
        {
            return language.Equals("tr", StringComparison.OrdinalIgnoreCase) ? trValue.IsNull(enValue) : enValue.IsNull(trValue);
        }

        public static string GetRelativeDate(this DateTime date, string language)
        {
            TimeSpan timeDifference = DateTime.Now - date;

            string todayStr = GetLangugeValue(language, "Bugün", "Today");
            string yesterdayStr = GetLangugeValue(language, "Dün", "Yesterday");
            string tomorrowStr = GetLangugeValue(language, "Yarın", "Tomorrow");
            string minuteStr = GetLangugeValue(language, "dakika", "minute");
            string hourStr = GetLangugeValue(language, "saat", "hour");
            string dayStr = GetLangugeValue(language, "gün", "day");
            string weekStr = GetLangugeValue(language, "hafta", "week");
            string monthStr = GetLangugeValue(language, "ay", "month");
            string yearStr = GetLangugeValue(language, "yıl", "year");
            string agoStr = GetLangugeValue(language, "önce", "ago");
            string leftStr = GetLangugeValue(language, "kaldı", "left");
            string lastMonthStr = GetLangugeValue(language, "Geçen ay", "Last month");
            string lastWeekStr = GetLangugeValue(language, "Geçen hafta", "Last week");

            var isOldDate = timeDifference.TotalNanoseconds < 0;
            var lastWord = isOldDate ? leftStr : agoStr;

            if (Math.Abs(timeDifference.TotalDays) < 1)
            {
                if (Math.Abs(timeDifference.TotalDays) < 0)
                    return todayStr;

                if (Math.Abs(timeDifference.TotalHours) < 1)
                    return $"{Math.Abs(Math.Floor(timeDifference.TotalMinutes)).ToInt()} {minuteStr}{timeDifference.TotalMinutes.addSuffix(language)} {lastWord}";

                return $"{Math.Abs(Math.Floor(timeDifference.TotalHours)).ToInt()} {hourStr}{timeDifference.TotalHours.addSuffix(language)} {lastWord}";
            }

            if (Math.Abs(timeDifference.TotalDays) < 2)
                return isOldDate ? tomorrowStr : yesterdayStr;

            if (Math.Abs(timeDifference.TotalDays) < 7)
                return $"{Math.Abs(Math.Floor(timeDifference.TotalDays)).ToInt()} {dayStr}{timeDifference.TotalDays.addSuffix(language)} {lastWord}";

            if (timeDifference.TotalDays < 14)
                return isOldDate ? $"{Math.Abs(Math.Floor(timeDifference.TotalDays)).ToInt()} {dayStr}{timeDifference.TotalDays.addSuffix(language)} {lastWord}" : lastWeekStr;

            if (timeDifference.TotalDays < 30)
                return $"{Math.Floor(timeDifference.TotalDays / 7).ToInt()} {weekStr}{(timeDifference.TotalDays / 7).addSuffix(language)} {lastWord}";

            if (timeDifference.TotalDays < 60)
                return isOldDate ? $"1 {monthStr} {leftStr}" : lastMonthStr;

            if (timeDifference.TotalDays < 365)
                return $"{Math.Floor(timeDifference.TotalDays / 30).ToInt()} {monthStr}{(timeDifference.TotalDays / 30).addSuffix(language)} {lastWord}";

            return $"{Math.Floor(timeDifference.TotalDays / 7).ToInt()} {yearStr}{(timeDifference.TotalDays / 365).addSuffix(language)} {lastWord}";
        }

        public static string GetRelativeDate(this DateTime? date, string language)
        {
            if (!date.HasValue) return string.Empty;

            return date.Value.GetRelativeDate(language);
        }

        private static string addSuffix(this double number, string language = "en")
        {
            return !language.Equals("tr", StringComparison.OrdinalIgnoreCase) && Math.Abs(number) >= 2 ? "s" : string.Empty;
        }
    }
}
