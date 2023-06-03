using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

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
    }
}
