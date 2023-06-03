using System;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace CareGardenApiV1.Helpers
{
    public static class HelperMethods
    {
        public static IConfiguration GetConfiguration()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { @"bin\" }, StringSplitOptions.None)[0];

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration;

        }

        public static bool IsValidEmail(this string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }       
        
        public static bool IsValidTelephoneNumber(this string telephoneNumber)
        {
            try
            {
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
