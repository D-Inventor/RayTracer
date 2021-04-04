using NewRayTracer.Extensions;
using NewRayTracer.Logging;

using System.Threading.Tasks;

namespace NewRayTracer.Services
{
    public class DecoratorIServiceLogging : IService, IDecorator<IService>
    {
        private readonly ILogger<IService> _logger;

        public DecoratorIServiceLogging(IService decoratee,
                                        ILogger<IService> logger)
        {
            Decoratee = decoratee;
            _logger = logger;
        }

        public IService Decoratee { get; }

        public async Task ExecuteAsync()
        {
            var displayName = Decoratee.UnwrapDecorators().GetType().GetFormattedName();
            _logger.Info("Starting service: {0}", displayName);
            await Decoratee.ExecuteAsync();
            _logger.Info("Finished service: {0}", displayName);
        }
    }
}
