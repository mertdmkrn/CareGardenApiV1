namespace CareGardenApiV1.Handler.Abstract
{
    public interface ILoggerHandler
    {
        void LogMessage(string message);
        void LogMessage(Exception exception);
    }
}
