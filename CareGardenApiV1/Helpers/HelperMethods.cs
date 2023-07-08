using System;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using CareGardenApiV1.Model;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CareGardenApiV1.Service.Concrete;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Repository.Abstract;

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
                if(emailaddress.IsNullOrEmpty()) return false;
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
                if (telephoneNumber.IsNullOrEmpty()) return false;

                telephoneNumber = telephoneNumber.Replace("+9", "");
                string regex = @"^(05(\d{9}))$";
                return Regex.IsMatch(telephoneNumber, regex, RegexOptions.IgnoreCase);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public async static Task<User> GetSessionUser(HttpRequest request, IUserService userService) 
        {
            var tokenString = request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

            if (token == null)
            {
                return null;
            }

            var userId = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value?.ToString();

            if (!userId.IsGuid())
            {  
                return null;
            }

            return await userService.GetUserById(userId.ToGuid());
        }


        public async static Task<Business> GetSessionBusiness(HttpRequest request, IBusinessService businessService)
        {
            var tokenString = request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

            if (token == null)
            {
                return null;
            }

            var businessId = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value?.ToString();

            if (!businessId.IsGuid())
            {
                return null;
            }

            return await businessService.GetBusinessByIdAsync(businessId.ToGuid());
        }

        public static string GetSessionUserRole(HttpRequest request)
        {
            var tokenString = request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

            if (token == null)
            {
                return null;
            }

            return token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value?.ToString();
        }
    }
}
