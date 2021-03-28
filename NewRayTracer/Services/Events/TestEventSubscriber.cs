using NewRayTracer.Logging;
using NewRayTracer.Models.Events;

using System.Threading.Tasks;

namespace NewRayTracer.Services.Events
{
    public class TestEventSubscriber : IEventSubscriber<TestEvent>
    {
        private readonly ILogger<TestEventSubscriber> _logger;

        public TestEventSubscriber(ILogger<TestEventSubscriber> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync(TestEvent @event)
        {
            _logger.Info("Event received: {0}", @event.Message);
            return Task.CompletedTask;
        }
    }
}
