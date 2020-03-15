using RayTracer.Models;
using RayTracer.Pipeline;

namespace RayTracer.Logging
{
    public interface ILogger<in Context> : ILogger
    { }

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

    public class Logger<Context> : ILogger<Context>
    {
        private readonly ILogger logger;

        public Logger(ILogger logger)
        {
            this.logger = logger;
        }

        public void Log(Log log)
        {
            log.Tags.Add(typeof(Context).FullName);
            logger.Log(log);
        }
    }
}
