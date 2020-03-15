using Microsoft.Extensions.Configuration;

using RayTracer.Models;
using RayTracer.Models.Options;
using RayTracer.Pipeline;

using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Logging
{
    public interface ILogFormatter : IPipelineStep<Log, string>
    { }

    public class LogFormatter : ILogFormatter
    {
        private LogFormatterOptions options;

        public LogFormatter(IConfiguration configuration)
        {
            options = new LogFormatterOptions
            {
                ExceptionFormat = configuration["ExceptionFormat"],
                LogFormat = configuration["LogFormat"],
                TagFormat = configuration["TagFormat"]
            };
        }

        public string Execute(Log input, IPipelineContext context)
        {
            return options.LogFormat
                .Replace("{{Date}}", input.Time.ToString())
                .Replace("{{Severity}}", input.Severity.ToString().PadRight(8))
                .Replace("{{Tags}}", FormatTags(input.Tags))
                .Replace("{{Message}}", input.Message)
                .Replace("{{Exception}}", FormatException(input.Exception));
        }


        private string FormatTags(IEnumerable<string> tags)
        {
            return tags.Select(x => options.TagFormat.Replace("{{Tag}}", x)).Aggregate((l, r) => $"{l} {r}");
        }

        private string FormatException(Exception exception)
        {
            if (exception is null) return string.Empty;

            return options.ExceptionFormat
                .Replace("{{Message}}", exception.Message)
                .Replace("{{StackTrace}}", exception.StackTrace);
        }
    }
}
