using CareGardenApiV1.Handler.Abstract;

namespace CareGardenApiV1.Handler.Concrete
{
    public class LoggerHandler : ILoggerHandler
    {
        private readonly ILogger<LoggerHandler> _logger;

        public LoggerHandler(ILogger<LoggerHandler> logger)
        {
            _logger = logger;
        }

        public void LogMessage(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogMessage(Exception exception)
        {
            _logger.LogError($"Error: {exception.Message} \n StackTrace: {exception.StackTrace}");
        }
    }
}
