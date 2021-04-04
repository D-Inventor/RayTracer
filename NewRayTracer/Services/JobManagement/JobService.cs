using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public class JobService : IService
    {
        private readonly IJobManager _jobManager;

        public JobService(IJobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public Task ExecuteAsync()
        {
            return _jobManager.ExecuteAsync();
        }
    }
}
