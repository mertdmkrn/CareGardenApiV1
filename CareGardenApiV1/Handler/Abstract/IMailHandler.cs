using CareGardenApiV1.Handler.Model;

namespace CareGardenApiV1.Handler.Abstract
{
    public interface IMailHandler
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
