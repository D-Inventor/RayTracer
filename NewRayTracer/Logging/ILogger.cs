using System;

namespace NewRayTracer.Logging
{
    public interface ILogger<TSource>
    {
        void Debug(Exception exception, string message, params object[] properties);
        void Debug(string message, params object[] properties);
        void Error(Exception exception, string message, params object[] properties);
        void Error(string message, params object[] properties);
        void Info(Exception exception, string message, params object[] properties);
        void Info(string message, params object[] properties);
        void Warning(Exception exception, string message, params object[] properties);
        void Warning(string message, params object[] properties);
    }
}
