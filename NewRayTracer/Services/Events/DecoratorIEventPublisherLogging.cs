using NewRayTracer.Extensions;
using NewRayTracer.Logging;

using System.Threading.Tasks;

namespace NewRayTracer.Services.Events
{
    public class DecoratorIEventPublisherLogging<TEvent> : IEventPublisher<TEvent>, IDecorator<IEventPublisher<TEvent>>
    {
        private readonly ILogger<IEventPublisher<TEvent>> _logger;

        public DecoratorIEventPublisherLogging(IEventPublisher<TEvent> decoratee,
                                               ILogger<IEventPublisher<TEvent>> logger)
        {
            Decoratee = decoratee;
            _logger = logger;
        }

        public IEventPublisher<TEvent> Decoratee { get; }

        public Task PublishAsync(TEvent @event)
        {
            _logger.Debug("Event fired: {0}", typeof(TEvent).GetFormattedName());
            return Decoratee.PublishAsync(@event);
        }
    }
}
