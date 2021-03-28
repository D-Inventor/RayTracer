using NewRayTracer.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public class DecoratorIJobPerformanceLogger : IJob
    {
        private readonly IJob _decoratee;
        private readonly ILogger<IJob> _logger;

        public DecoratorIJobPerformanceLogger(IJob decoratee,
                                   ILogger<IJob> logger)
        {
            _decoratee = decoratee;
            _logger = logger;
        }

        public string DisplayName => _decoratee.DisplayName;

        public async Task DoAsync()
        {
            _logger.Debug("Starting job {0}", DisplayName);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await _decoratee.DoAsync();
            stopwatch.Stop();
            _logger.Debug("Finished job {0} in {1}", DisplayName, stopwatch.Elapsed);
        }
    }
}
