using Autofac;

using Microsoft.Extensions.Configuration;

using NewRayTracer.Models.Configuration;

namespace NewRayTracer.Models.Composition
{
    public class CompositionContext
    {
        public CompositionContext(ContainerBuilder container, IGameEnvironment gameEnvironment, IConfigurationRoot configuration)
        {
            Container = container;
            GameEnvironment = gameEnvironment;
            Configuration = configuration;
        }

        public ContainerBuilder Container { get; }
        public IGameEnvironment GameEnvironment { get; }
        public IConfigurationRoot Configuration { get; }
    }
}
