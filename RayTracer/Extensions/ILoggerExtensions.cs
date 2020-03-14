using RayTracer.Logging;
using System;

namespace RayTracer.Extensions
{
    public static class ILoggerExtensions
    {
        public static void LogInfo(this ILogger logger, string message, params string[] tags)
        {
            logger.Log(new Models.Log
            {
                Message = message,
                Severity = Models.Severity.Info,
                Time = DateTimeOffset.Now,
                Tags = tags
            });
        }

        public static void LogWarning(this ILogger logger, string message, params string[] tags)
        {
            logger.Log(new Models.Log
            {
                Message = message,
                Severity = Models.Severity.Warning,
                Time = DateTimeOffset.Now,
                Tags = tags
            });
        }

        public static void LogWarning(this ILogger logger, Exception exception, string message, params string[] tags)
        {
            logger.Log(new Models.Log
            {
                Exception = exception,
                Message = message,
                Severity = Models.Severity.Warning,
                Time = DateTimeOffset.Now,
                Tags = tags
            });
        }
    }
}
