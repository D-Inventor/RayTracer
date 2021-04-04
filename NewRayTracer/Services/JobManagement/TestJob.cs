﻿using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public class TestJob : IJob
    {
        public Task DoAsync()
        {
            return Task.Delay(5000);
        }
    }
}
