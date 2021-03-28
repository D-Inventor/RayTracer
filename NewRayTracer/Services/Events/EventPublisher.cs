using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewRayTracer.Services.Events
{
    public class EventPublisher<TEvent> : IEventPublisher<TEvent>
    {
        private readonly IEnumerable<IEventSubscriber<TEvent>> _subscribers;

        public EventPublisher(IEnumerable<IEventSubscriber<TEvent>> subscribers)
        {
            _subscribers = subscribers;
        }

        public Task PublishAsync(TEvent @event)
            => Task.WhenAll(_subscribers.Select(s => s.PublishAsync(@event)));
    }
}
