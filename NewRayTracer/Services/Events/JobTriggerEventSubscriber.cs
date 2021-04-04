using NewRayTracer.Models.Events;

using System;
using System.Threading.Tasks;

namespace NewRayTracer.Services.Events
{
    public class JobTriggerEventSubscriber : IEventSubscriber<JobFinishedEvent>
    {
        public Task PublishAsync(JobFinishedEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
