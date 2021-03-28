using NewRayTracer.Models.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
