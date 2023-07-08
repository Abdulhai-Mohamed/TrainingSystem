namespace Contraacts
{
    public interface ILoggerManger
    {
        void LogInfo(string message);
        void LogDebug(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}