using System;
using System.Collections.Generic;

namespace RayTracer.Models
{
    public class Log
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public Severity Severity { get; set; }
        public ICollection<string> Tags { get; set; } = new List<string>();
        public DateTimeOffset Time { get; set; }
    }

    public enum Severity
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }
}
