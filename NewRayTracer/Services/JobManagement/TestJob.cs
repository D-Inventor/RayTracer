using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public class TestJob : IJob
    {
        public string DisplayName => "Test job";

        public Task DoAsync()
        {
            return Task.Delay(5000);
        }
    }
}
