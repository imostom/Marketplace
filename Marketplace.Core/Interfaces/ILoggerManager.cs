namespace Marketplace.Core.Interfaces
{
    public interface ILoggerManager
    {
        void LogDebug(object message);
        void LogDebug(string message);
        void LogError(object message);
        void LogError(string message);
        void LogError(Exception ex, string message);
        void LogError(string message, Exception ex);
        void LogInformation(string message);
        void LogInformation(object message);
        void LogWarning(string message);
        void LogWarning(object message);
    }
}
