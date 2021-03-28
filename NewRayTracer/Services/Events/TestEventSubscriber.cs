using NewRayTracer.Models.Events;

using System;
using System.Threading.Tasks;

namespace NewRayTracer.Services.Events
{
    public class TestEventSubscriber : IEventSubscriber<TestEvent>
    {
        public Task Publish(TestEvent @event)
        {
            Console.WriteLine("Event received: {0}", @event.Message);
            return Task.CompletedTask;
        }
    }
}
