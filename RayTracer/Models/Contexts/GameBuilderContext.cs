using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RayTracer.Helpers;

namespace RayTracer.Models.Contexts
{
    public interface IGameBuilderContext
    {
        IConfigurationRoot Configuration { get; }
        IEnvironmentHelper EnvironmentHelper { get; }
        IServiceCollection Services { get; }
    }

    public class GameBuilderContext : IGameBuilderContext
    {
        public IConfigurationRoot Configuration { get; set; }
        public IEnvironmentHelper EnvironmentHelper { get; set; }
        public IServiceCollection Services { get; set; }
    }
}
