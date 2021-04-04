using NewRayTracer.Extensions;
using NewRayTracer.Logging;

using System.Diagnostics;
using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public class DecoratorIJobPerformanceLogger : IJob, IDecorator<IJob>
    {
        private readonly ILogger<IJob> _logger;

        public DecoratorIJobPerformanceLogger(IJob decoratee,
                                   ILogger<IJob> logger)
        {
            Decoratee = decoratee;
            _logger = logger;
        }

        public IJob Decoratee { get; }

        public async Task DoAsync()
        {
            var displayName = Decoratee.UnwrapDecorators().GetType().GetFormattedName();
            _logger.Debug("Starting job {0}", displayName);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await Decoratee.DoAsync();
            stopwatch.Stop();
            _logger.Debug("Finished job {0} in {1}", displayName, stopwatch.Elapsed);
        }
    }
}
