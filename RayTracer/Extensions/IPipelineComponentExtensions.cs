using RayTracer.Models;
using RayTracer.Pipeline;

using System;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Extensions
{
    public static class IPipelineComponentExtensions
    {
        public static IPipelineComponent<POut, NOut> AddStep<PIn, POut, NOut>(this IPipelineComponent<PIn, POut> component, IPipelineStep<POut, NOut> step)
        {
            return component.AddStep(PipelineComponent.CreatePipeline(step));
        }

        public static IPipelineComponent<POut, NOut> AddStep<PIn, POut, NOut>(this IPipelineComponent<PIn, POut> component, Func<POut, IPipelineContext, NOut> step)
        {
            return component.AddStep(PipelineComponent.CreatePipeline(step));
        }

        public static IAsyncPipelineComponent<POut, NOut> AddStep<PIn, POut, NOut>(this IAsyncPipelineComponent<PIn, POut> component, IAsyncPipelineStep<POut, NOut> step)
        {
            return component.AddStep(PipelineComponent.CreateAsyncPipeline(step));
        }

        public static IAsyncPipelineComponent<POut, NOut> AddStep<PIn, POut, NOut>(this IAsyncPipelineComponent<PIn, POut> component, Func<POut, IPipelineContext, Task<NOut>> step)
        {
            return component.AddStep(PipelineComponent.CreateAsyncPipeline(step));
        }

        public static IPipelineComponent<Log, Log> AddLogFilter<PIn>(this IPipelineComponent<PIn, Log> component, Severity severity)
        {
            return component.AddStep((log, context) =>
            {
                if(log.Severity < severity)
                {
                    context.Break();
                }
                return log;
            });
        }

        public static IPipelineComponent<Log, string> AddLogFormatter<PIn>(this IPipelineComponent<PIn, Log> component)
        {
            return component.AddStep((log, context) =>
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[{log.Time}]");
                sb.Append($" <{log.Severity.ToString().PadLeft(10)}>");

                foreach(var tag in log.Tags)
                {
                    sb.Append($" ({tag})");
                }

                sb.Append($" {log.Message}");

                if(log.Exception != null)
                {
                    sb.Append($"{Environment.NewLine}{log.Exception.Message}{Environment.NewLine}{log.Exception.StackTrace}");
                }

                return sb.ToString();
            });
        }

        public static IPipelineComponent<string, string> AddConsole<PIn>(this IPipelineComponent<PIn, string> component)
        {
            return component.AddStep((s, context) =>
            {
                Console.WriteLine(s);
                return s;
            });
        }
    }
}
