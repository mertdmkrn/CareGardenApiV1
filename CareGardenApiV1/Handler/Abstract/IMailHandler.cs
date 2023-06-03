using CareGardenApiV1.Handler.Model;
using System.Security.Claims;

namespace CareGardenApiV1.Handler.Abstract
{
    public interface IMailHandler
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
