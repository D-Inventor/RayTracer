using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public class JetAnotherJob : IJob
    {
        public Task DoAsync()
        {
            return Task.Delay(7000);
        }
    }
}
