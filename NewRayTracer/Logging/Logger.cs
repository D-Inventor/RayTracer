using Serilog;

using System;

namespace NewRayTracer.Logging
{
    public class Logger<TSource> : ILogger<TSource>
    {
        private readonly ILogger _logger;

        public Logger(ILogger logger)
        {
            _logger = logger.ForContext<TSource>();
        }

        public void Error(Exception exception, string message, params object[] properties) => _logger.Error(exception, message, properties);
        public void Error(string message, params object[] properties) => _logger.Error(message, properties);
        public void Warning(Exception exception, string message, params object[] properties) => _logger.Warning(exception, message, properties);
        public void Warning(string message, params object[] properties) => _logger.Warning(message, properties);
        public void Info(Exception exception, string message, params object[] properties) => _logger.Information(exception, message, properties);
        public void Info(string message, params object[] properties) => _logger.Information(message, properties);
        public void Debug(Exception exception, string message, params object[] properties) => _logger.Debug(exception, message, properties);
        public void Debug(string message, params object[] properties) => _logger.Debug(message, properties);
    }
}
