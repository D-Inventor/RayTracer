﻿using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public interface IJob
    {
        Task DoAsync();
    }
}
