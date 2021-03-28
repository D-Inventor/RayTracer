using NewRayTracer.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRayTracer.Services
{
    public class DecoratorIServiceLogging : IService
    {
        private readonly IService _decoratee;
        private readonly ILogger<IService> _logger;

        public DecoratorIServiceLogging(IService decoratee,
                                        ILogger<IService> logger)
        {
            _decoratee = decoratee;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.Info("Starting service: '{0}'", _decoratee.GetType().FullName);
            await _decoratee.ExecuteAsync();
            _logger.Info("Finished service: '{0}'", _decoratee.GetType().FullName);
        }
    }
}
