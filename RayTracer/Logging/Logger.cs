using RayTracer.Models;
using RayTracer.Pipeline;

namespace RayTracer.Logging
{
    public interface ILogger
    {
        void Log(Log log);
    }

    public class Logger : ILogger
    {
        private readonly IPipelineInput<Log> pipeline;

        public Logger(IPipelineInput<Log> pipeline)
        {
            this.pipeline = pipeline;
        }

        public void Log(Log log)
        {
            pipeline.Execute(log);
        }
    }
}
