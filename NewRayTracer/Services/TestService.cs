using NewRayTracer.Models.Events;
using NewRayTracer.Services.Events;

using System.Threading.Tasks;

namespace NewRayTracer.Services
{
    public class TestService : IService
    {
        private readonly IEventPublisher<TestEvent> _testEventPublisher;

        public TestService(IEventPublisher<TestEvent> TestEventPublisher)
        {
            _testEventPublisher = TestEventPublisher;
        }

        public Task ExecuteAsync()
        {
            return _testEventPublisher.Publish(new TestEvent
            {
                Message = "This is a test event!"
            });
        }
    }
}
