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

        public async Task ExecuteAsync()
        {
            await Task.Delay(100);
            await _testEventPublisher.PublishAsync(new TestEvent
            {
                Message = "This is a test event!"
            });
        }
    }
}
