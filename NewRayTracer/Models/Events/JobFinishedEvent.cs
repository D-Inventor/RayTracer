using NewRayTracer.Services.JobManagement;

namespace NewRayTracer.Models.Events
{
    public class JobFinishedEvent
    {
        public JobFinishedEvent(IJob job)
        {
            Job = job;
        }

        private IJob Job { get; }
    }
}
