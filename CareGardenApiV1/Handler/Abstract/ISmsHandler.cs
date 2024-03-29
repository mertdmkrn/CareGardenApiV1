namespace CareGardenApiV1.Handler.Abstract
{
    public interface ISmsHandler
    {
        Task<bool> SendSmsAsync(string message, string telephoneNumber, string senderType = "GetConfirmationCode");
    }
}
