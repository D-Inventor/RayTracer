using System;
using System.Threading;
using System.Threading.Tasks;

namespace NewRayTracer.Services
{
    public class TestService : IService
    {
        public Task ExecuteAsync(CancellationToken stopEvent)
        {
            Console.WriteLine("Test!");
            return Task.CompletedTask;
        }
    }
}
