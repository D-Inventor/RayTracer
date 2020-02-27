using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Interfaces
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}
