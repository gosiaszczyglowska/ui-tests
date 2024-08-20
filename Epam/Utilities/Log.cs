using log4net;
using System;
using System.Reflection;

namespace PageObject.Utilities
{
    public static class Log
    {
        // Static logger instance
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Method to log info messages
        public static void LogInfo(string message)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info(message);
            }
        }

        // Method to log error messages with exception
        public static void LogError(string message, Exception ex)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error(message, ex);
            }
        }

        // Method to log debug messages
        public static void LogDebug(string message)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(message);
            }
        }

        // Add other logging methods as needed (e.g., Warn, Fatal)
        public static void LogWarn(string message)
        {
            if (Logger.IsWarnEnabled)
            {
                Logger.Warn(message);
            }
        }

        public static void LogFatal(string message, Exception ex)
        {
            if (Logger.IsFatalEnabled)
            {
                Logger.Fatal(message, ex);
            }
        }
    }
}