using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public interface IJobManager
    {
        Task ExecuteAsync();
    }
}