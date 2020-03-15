using RayTracer.Logging;
using RayTracer.Models;

using System;
using System.Collections.Generic;

namespace RayTracer.Extensions
{
    public static class ILoggerExtensions
    {
        public static void LogDebug(this ILogger logger, string message, params string[] tags)
        {
            logger.Log(null, message, Severity.Debug, tags);
        }

        public static void LogInfo(this ILogger logger, string message, params string[] tags)
        {
            logger.Log(null, message, Severity.Info, tags);
        }

        public static void LogWarning(this ILogger logger, string message, params string[] tags)
        {
            logger.Log(null, message, Severity.Warning, tags);
        }

        public static void LogWarning(this ILogger logger, Exception exception, string message, params string[] tags)
        {
            logger.Log(exception, message, Severity.Warning, tags);
        }

        public static void LogError(this ILogger logger, string message, params string[] tags)
        {
            logger.Log(null, message, Severity.Error, tags);
        }

        public static void LogError(this ILogger logger, Exception exception, string message, params string[] tags)
        {
            logger.Log(exception, message, Severity.Error, tags);
        }

        public static void LogCritical(this ILogger logger, string message, params string[] tags)
        {
            logger.Log(null, message, Severity.Critical, tags);
        }

        public static void LogCritical(this ILogger logger, Exception exception, string message, params string[] tags)
        {
            logger.Log(exception, message, Severity.Critical, tags);
        }

        public static void Log(this ILogger logger, Exception exception = null, string message = null, Severity severity = Severity.Debug, params string[] tags)
        {
            logger.Log(new Log
            {
                Exception = exception,
                Message = message,
                Severity = severity,
                Time = DateTimeOffset.Now,
                Tags = new List<string>(tags)
            });
        }
    }
}
