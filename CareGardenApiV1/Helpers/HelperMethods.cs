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

        public static string GetMailTemplate()
        {
            return
            "<html>" +
            "<body style=\"margin: 20px auto; width:100%; padding: 0; display: flex; justify-content: center; align-self: center; font-family:monospace\">" +
            "    <div style=\"width: 50%; margin: 0 auto; min-width: 350px; height: auto; box-shadow: 4px 8px 20px 1px lightgray;  border-radius: 5px; box-sizing: border-box; background-image: linear-gradient(to right, #07c6b6, #6172F3);\">" +
            "        <div style=\"width: 100%; height: 110px; padding: 20px;box-sizing: border-box\">" +
            "            <div style=\"display: block; min-height: 64px; text-align: center;\">" +
            "                <img style=\"width: 64px; margin: 0 auto;\" src=\"https://dl.dropboxusercontent.com/s/7kli4nza26ot7sd/logo.PNG\" />" +
            "            </div>" +
            "            <p style=\"text-align: center; font-size: 27px; font-weight:bold; letter-spacing: 5px; color: #fff; margin-top: 6px;\">caregarden</p>" +
            "        </div>" +
            "        <div style=\"width: 100%; text-align: center; padding:85px 5px; font-size: 18px; line-height: 21px; font-weight: bold; color: #eee; height: auto; box-sizing: border-box;\">" +
            "            {content}" +
            "        </div>" +
            "        <footer style=\"width: 100%; padding: 4px; padding-bottom: 20px; clear:both; box-sizing: border-box; text-align: center; height: 50px;\">" +
            "            <a style=\"text-decoration:none; color:#cee;\" href=\"https://www.facebook.com/caregardenapp\" class=\"fa-brands\" target=\"_blank\">" +
            "                <img src=\"https://dl.dropboxusercontent.com/s/2xdk0pevhtb4d57/facebook-square-brands.png\">" +
            "            </a>&nbsp;" +
            "            <a style=\"text-decoration:none; color:#cee;\" href=\"https://www.twitter.com/caregardenapp\" class=\"fa-brands\" target=\"_blank\">" +
            "                <img src=\"https://dl.dropboxusercontent.com/s/xt8xvoo81h6ns7h/twitter-square-brands.png\">" +
            "            </a>&nbsp;" +
            "            <a style=\"text-decoration:none; color:#cee;\" href=\"https://www.instagram.com/caregardenapp\" class=\"fa-brands\" target=\"_blank\">" +
            "                <img src=\"https://dl.dropboxusercontent.com/s/6n11ruxdlly1psp/instagram-square-brands.png\">" +
            "            </a>&nbsp;" +
            "            <a style=\"text-decoration:none; color:#cee;\" href=\"https://www.linkedin.com/caregardenapp\" class=\"fa-brands\" target=\"_blank\">" +
            "                <img src=\"https://dl.dropboxusercontent.com/s/9hlwqojl68xhuc4/linkedin-brands.png\">" +
            "            </a>" +
            "</footer>" +
            "</div>" +
            "</body>" +
            "</html>";
        }
    }
}
