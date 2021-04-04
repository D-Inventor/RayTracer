using System;
using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public class OtherTestJob : IJob
    {
        public Task DoAsync()
        {
            return Task.Delay(3000);
        }
    }
}
