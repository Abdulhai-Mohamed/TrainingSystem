using Contraacts;
using NLog;
using NLog.Web;

namespace LoggingService
{
    public class LoggerManger : ILoggerManger
    {

        public static NLog.Logger logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

     
        public void LogInfo(string message)
        {
            logger.Info(message);
        }


        public void LogWarning(string message)
        {
            logger.Warn(message);
        }
        public void LogError(string message)
        {
            logger.Error(message);
        }

    }
}