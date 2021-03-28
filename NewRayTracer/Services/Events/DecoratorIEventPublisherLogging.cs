using NewRayTracer.Extensions;
using NewRayTracer.Logging;

using System.Threading.Tasks;

namespace NewRayTracer.Services.Events
{
    public class DecoratorIEventPublisherLogging<TEvent> : IEventPublisher<TEvent>
    {
        private readonly IEventPublisher<TEvent> _decoratee;
        private readonly ILogger<IEventPublisher<TEvent>> _logger;

        public DecoratorIEventPublisherLogging(IEventPublisher<TEvent> decoratee,
                                               ILogger<IEventPublisher<TEvent>> logger)
        {
            _decoratee = decoratee;
            _logger = logger;
        }

        public Task PublishAsync(TEvent @event)
        {
            _logger.Debug("Event fired: {0}", typeof(TEvent).GetFormattedName());
            return _decoratee.PublishAsync(@event);
        }
    }
}
