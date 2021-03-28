using NewRayTracer.Models.Events;
using NewRayTracer.Services.Events;
using NewRayTracer.Services.JobManagement;

using System.Threading.Tasks;

namespace NewRayTracer.Services
{
    public class TestService : IService
    {
        private readonly IEventPublisher<TestEvent> _testEventPublisher;
        private readonly IJob _job;

        public TestService(IEventPublisher<TestEvent> TestEventPublisher, IJob job)
        {
            _testEventPublisher = TestEventPublisher;
            _job = job;
        }

        public async Task ExecuteAsync()
        {
            await _testEventPublisher.PublishAsync(new TestEvent
            {
                Message = "This is a test event!"
            });

            await _job.DoAsync();
        }
    }
}
