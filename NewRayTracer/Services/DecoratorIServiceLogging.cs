using NewRayTracer.Extensions;
using NewRayTracer.Logging;

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
            _logger.Info("Starting service: {0}", _decoratee.GetType().GetFormattedName());
            await _decoratee.ExecuteAsync();
            _logger.Info("Finished service: {0}", _decoratee.GetType().GetFormattedName());
        }
    }
}
