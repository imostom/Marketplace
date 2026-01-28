using Marketplace.Core.Interfaces;

namespace Marketplace.Infrastructure.Logging
{
    public class LoggerManager : ILoggerManager
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogDebug(object message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogError(object message)
        {
            logger.Error(message);
        }

        public void LogError(Exception ex, string message)
        {
            logger.Error(ex, message);
        }

        public void LogError(string message, Exception ex)
        {
            logger.Error(ex, message);
        }

        public void LogInformation(string message)
        {
            logger.Info(message);
        }

        public void LogInformation(object message)
        {
            logger.Info(message);
        }

        public void LogWarning(string message)
        {
            logger.Warn(message);
        }

        public void LogWarning(object message)
        {
            logger.Warn(message);
        }

    }
}
