using NewRayTracer.Models.Events;
using NewRayTracer.Services.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public class DecoratorIJobEventPublishing : IJob
    {
        private readonly IJob _decoratee;
        private readonly IEventPublisher<JobFinishedEvent> _jobFinishedEventPublisher;

        public DecoratorIJobEventPublishing(IJob decoratee,
                                            IEventPublisher<JobFinishedEvent> jobFinishedEventPublisher)
        {
            _decoratee = decoratee;
            _jobFinishedEventPublisher = jobFinishedEventPublisher;
        }

        public string DisplayName => _decoratee.DisplayName;

        public async Task DoAsync()
        {
            await _decoratee.DoAsync();
            await _jobFinishedEventPublisher.PublishAsync(new JobFinishedEvent(_decoratee));
        }
    }
}
