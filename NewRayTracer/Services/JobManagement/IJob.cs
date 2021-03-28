using System;
using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public interface IJob
    {
        string DisplayName { get; }
        Task DoAsync();
    }
}
