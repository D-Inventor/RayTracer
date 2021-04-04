using NewRayTracer.Models.Events;
using NewRayTracer.Services.Events;
using NewRayTracer.Services.JobManagement;

using System.Threading.Tasks;

namespace NewRayTracer.Services
{
    public class TestService : IService
    {
        private readonly IEventPublisher<TestEvent> _testEventPublisher;
        private readonly IJobManager _jobManager;

        public TestService(IEventPublisher<TestEvent> TestEventPublisher, IJobManager jobManager)
        {
            _testEventPublisher = TestEventPublisher;
            _jobManager = jobManager;
        }

        public async Task ExecuteAsync()
        {
            await _testEventPublisher.PublishAsync(new TestEvent
            {
                Message = "This is a test event!"
            });

            await _jobManager.ExecuteAsync();
        }
    }
}
