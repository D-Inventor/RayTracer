using NewRayTracer.Extensions;
using NewRayTracer.Models.Events;
using NewRayTracer.Services.Events;

using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public class DecoratorIJobEventPublishing : IJob, IDecorator<IJob>
    {
        private readonly IEventPublisher<JobFinishedEvent> _jobFinishedEventPublisher;

        public DecoratorIJobEventPublishing(IJob decoratee,
                                            IEventPublisher<JobFinishedEvent> jobFinishedEventPublisher)
        {
            Decoratee = decoratee;
            _jobFinishedEventPublisher = jobFinishedEventPublisher;
        }

        public IJob Decoratee { get; }

        public async Task DoAsync()
        {
            await Decoratee.DoAsync();
            await _jobFinishedEventPublisher.PublishAsync(new JobFinishedEvent(Decoratee.UnwrapDecorators()));
        }
    }
}
