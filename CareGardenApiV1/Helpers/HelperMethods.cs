using System.Net.Mail;
using System.Text.RegularExpressions;
using CareGardenApiV1.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Model.ResponseModel;
using System.Text;

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
                if (emailaddress.IsNullOrEmpty()) return false;
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
            if (string.IsNullOrEmpty(telephoneNumber)) return false;

            string regex = @"^\+(\d{1,4})[\s-]?\(?\d{1,4}?\)?[\s-]?\d{1,4}[\s-]?\d{1,4}[\s-]?\d{1,9}$";

            return Regex.IsMatch(telephoneNumber, regex, RegexOptions.IgnoreCase);
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

        public async static Task<UserResponseModel> GetSessionUserResponseModel(HttpRequest request, IUserService userService)
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

            return await userService.GetUserResponseModelById(userId.ToGuid());
        }

        public static string GetClaimInfo(HttpRequest request, string type)
        {
            var tokenString = request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (tokenString.IsNullOrEmpty()) return null;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

            if (token == null || token.Claims == null)
            {
                return null;
            }

            return token.Claims.FirstOrDefault(x => x.Type == type)?.Value?.ToString();
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
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>")
                   .AppendLine("<body style=\"margin: 20px auto; width:100%; padding: 0; display: flex; justify-content: center; align-self: center; font-family:monospace\">")
                   .AppendLine("    <div style=\"width: 50%; margin: 0 auto; min-width: 350px; height: auto; box-shadow: 4px 8px 20px 1px lightgray;  border-radius: 5px; box-sizing: border-box; background-image: linear-gradient(to right, #07c6b6, #6172F3);\">")
                   .AppendLine("        <div style=\"width: 100%; height: 110px; padding: 20px;box-sizing: border-box\">")
                   .AppendLine("            <div style=\"display: block; min-height: 64px; text-align: center;\">")
                   .AppendLine("                <img style=\"width: 64px; margin: 0 auto;\" src=\"https://dl.dropboxusercontent.com/s/7kli4nza26ot7sd/logo.PNG\" />")
                   .AppendLine("            </div>")
                   .AppendLine("            <p style=\"text-align: center; font-size: 27px; font-weight:bold; letter-spacing: 5px; color: #fff; margin-top: 6px;\">caregarden</p>")
                   .AppendLine("        </div>")
                   .AppendLine("        <div style=\"width: 100%; text-align: center; padding:85px 5px; font-size: 18px; line-height: 21px; font-weight: bold; color: #eee; height: auto; box-sizing: border-box;\">")
                   .AppendLine("            {content}")
                   .AppendLine("        </div>")
                   .AppendLine("        <footer style=\"width: 100%; padding: 4px; padding-bottom: 20px; clear:both; box-sizing: border-box; text-align: center; height: 50px;\">")
                   .AppendLine("            <a style=\"text-decoration:none; color:#cee;\" href=\"https://www.facebook.com/caregardenapp\" class=\"fa-brands\" target=\"_blank\">")
                   .AppendLine("                <img src=\"https://dl.dropboxusercontent.com/s/2xdk0pevhtb4d57/facebook-square-brands.png\">")
                   .AppendLine("            </a>&nbsp;")
                   .AppendLine("            <a style=\"text-decoration:none; color:#cee;\" href=\"https://www.twitter.com/caregardenapp\" class=\"fa-brands\" target=\"_blank\">")
                   .AppendLine("                <img src=\"https://dl.dropboxusercontent.com/s/xt8xvoo81h6ns7h/twitter-square-brands.png\">")
                   .AppendLine("            </a>&nbsp;")
                   .AppendLine("            <a style=\"text-decoration:none; color:#cee;\" href=\"https://www.instagram.com/caregardenapp\" class=\"fa-brands\" target=\"_blank\">")
                   .AppendLine("                <img src=\"https://dl.dropboxusercontent.com/s/6n11ruxdlly1psp/instagram-square-brands.png\">")
                   .AppendLine("            </a>&nbsp;")
                   .AppendLine("            <a style=\"text-decoration:none; color:#cee;\" href=\"https://www.linkedin.com/caregardenapp\" class=\"fa-brands\" target=\"_blank\">")
                   .AppendLine("                <img src=\"https://dl.dropboxusercontent.com/s/9hlwqojl68xhuc4/linkedin-brands.png\">")
                   .AppendLine("            </a>")
                   .AppendLine("</footer>")
                   .AppendLine("</div>")
                   .AppendLine("</body>")
                   .AppendLine("</html>");

            return builder.ToString();
        }

        public static int GetDay(this DateTime date)
        {
            switch (date.DayOfWeek)
            {

                case DayOfWeek.Monday: return 1;
                case DayOfWeek.Tuesday: return 2;
                case DayOfWeek.Wednesday: return 3;
                case DayOfWeek.Thursday: return 4;
                case DayOfWeek.Friday: return 5;
                case DayOfWeek.Saturday: return 6;
                case DayOfWeek.Sunday: return 7;
                default: return -1;
            }
        }

        public static bool GetBusinessOpen(BusinessWorkingInfo workingInfo, bool officialDayAvailable)
        {
            try
            {
                var now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Turkey Standard Time");
                var today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Today, "Turkey Standard Time");

                if (workingInfo == null)
                    return true;

                if (officialDayAvailable && Constants.OfficialDays.Exists(x => x.date.Equals(today)))
                    return false;

                var workHours = workingInfo.GetBusinessWorkInfoHours(today);

                if (workHours.IsNullOrEmpty())
                    return false;

                var startHours = workHours.Split('-').FirstOrDefault();
                var endHours = workHours.Split('-').LastOrDefault();

                if (startHours.IsNullOrEmpty() || endHours.IsNullOrEmpty())
                    return false;

                var startHour = startHours.Split(":")[0].ToInt();
                var startMinute = startHours.Split(":")[1].ToInt();
                var businessStartDate = new DateTime(today.Year, today.Month, today.Day, startHour, startMinute, 0);

                var endHour = endHours.Split(":")[0].ToInt();
                var endMinute = endHours.Split(":")[1].ToInt();

                if (endHour >= 24)
                {
                    endHour = 23;
                    endMinute = 59;
                }

                var businessEndDate = new DateTime(today.Year, today.Month, today.Day, endHour, endMinute, 0);

                if (now >= businessStartDate && now < businessEndDate)
                    return true;

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool GetBusinessOpenSpecialDate(BusinessWorkingInfo workingInfo, bool officialDayAvailable, DateTime? specialDate)
        {
            if (!specialDate.HasValue)
                return true;

            var date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(specialDate.Value, "Turkey Standard Time");

            if (workingInfo == null)
                return true;

            if (officialDayAvailable && Constants.OfficialDays.Exists(x => x.date.Equals(specialDate)))
                return false;

            var workHours = workingInfo.GetBusinessWorkInfoHours(date);

            if (workHours.IsNullOrEmpty())
                return false;

            return true;
        }


        public static string GetBusinessWorkInfoHours(this BusinessWorkingInfo workingInfo, DateTime date)
        {
            switch (date.DayOfWeek)
            {

                case DayOfWeek.Monday: return workingInfo.mondayWorkHours;
                case DayOfWeek.Tuesday: return workingInfo.tuesdayWorkHours;
                case DayOfWeek.Wednesday: return workingInfo.wednesdayWorkHours;
                case DayOfWeek.Thursday: return workingInfo.thursdayWorkHours;
                case DayOfWeek.Friday: return workingInfo.fridayWorkHours;
                case DayOfWeek.Saturday: return workingInfo.saturdayWorkHours;
                case DayOfWeek.Sunday: return workingInfo.sundayWorkHours;
                default: return null;
            }
        }

        public static bool IsAvailableAppointmentDay(List<Appointment> appointments, BusinessWorkingInfo workingInfo, bool officialDayAvailable, DateTime availableDate)
        {
            if (appointments == null)
                return true;

            var date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(availableDate, "Turkey Standard Time");

            if (workingInfo == null)
                return false;

            if (officialDayAvailable && Constants.OfficialDays.Exists(x => x.date.Equals(date)))
                return false;

            var workHours = workingInfo.GetBusinessWorkInfoHours(date);

            if (workHours.IsNullOrEmpty())
                return false;

            var startHours = workHours.Split('-').FirstOrDefault();
            var endHours = workHours.Split('-').LastOrDefault();

            if (startHours.IsNullOrEmpty() || endHours.IsNullOrEmpty())
                return false;

            var todayWorksMinutes = 0;

            var endStartTimeDifference = endHours.Replace(":", "").ToInt() - startHours.Replace(":", "").ToInt();

            var sumWorkMinutes = (endStartTimeDifference / 100) * 60;
            sumWorkMinutes += endStartTimeDifference % 100;

            return sumWorkMinutes - todayWorksMinutes >= 30;
        }

        public static List<WorkerAvailableTimeModel> GetWorkerAvailableTimes(List<Appointment> appointments, BusinessWorkingInfo businessWorkingInfo, BusinessServiceModel businessService, DateTime? startDate, DateTime? endDate)
        {
            List<WorkerAvailableTimeModel> workerAvailableTimeModels = new List<WorkerAvailableTimeModel>();

            var dateDifferenceDays = (int)startDate.DifferenceBetweenDates(endDate, DateType.Day);

            for (int i = 0; i < dateDifferenceDays; i++)
            {
                var date = startDate.Value.AddDays(i);

                if (!GetBusinessOpenSpecialDate(businessWorkingInfo, businessWorkingInfo.officialHolidayAvailable, date))
                    continue;

                var workingHours = businessWorkingInfo.GetBusinessWorkInfoHours(date);

                if (string.IsNullOrEmpty(workingHours))
                    continue;

                var workingHoursParts = workingHours.Split('-');
                if (workingHoursParts.Length != 2)
                    continue;

                if (!TimeSpan.TryParse(workingHoursParts[0], out var startWorkTime) ||
                    !TimeSpan.TryParse(workingHoursParts[1], out var endWorkTime))
                    continue;

                var dayStartDate = new DateTime(date.Year, date.Month, date.Day)
                    .Add(startWorkTime);

                var dayEndDate = new DateTime(date.Year, date.Month, date.Day)
                    .Add(endWorkTime);

                var serviceDurationMinutes = businessService.maxDuration.IsNull(businessService.minDuration);
                var numberOfAppointmentsPerDay = (int)Math.Ceiling(dayStartDate.DifferenceBetweenDates(dayEndDate, DateType.Minute) / serviceDurationMinutes);

                for (int j = 0; j < numberOfAppointmentsPerDay; j++)
                {
                    WorkerAvailableTimeModel workerAvailableTimeModel = new WorkerAvailableTimeModel();

                    workerAvailableTimeModel.startDate = dayStartDate.AddMinutes(j * serviceDurationMinutes);
                    workerAvailableTimeModel.endDate = dayStartDate.AddMinutes((j + 1) * serviceDurationMinutes);
                    workerAvailableTimeModel.dateText = dayStartDate.ToString("dd.MM.yyyy");
                    workerAvailableTimeModel.hourText = workerAvailableTimeModel.startDate.ToString("HH:mm") + "-" + workerAvailableTimeModel.endDate.ToString("HH:mm");

                    if (appointments.Exists(x => x.startDate >= workerAvailableTimeModel.startDate && x.startDate < workerAvailableTimeModel.endDate))
                        continue;

                    workerAvailableTimeModels.Add(workerAvailableTimeModel);
                }
            }

            return workerAvailableTimeModels;
        }
    }
}
