using Autofac;

using Microsoft.Extensions.Configuration;

using NewRayTracer.Models.Configuration;

using Serilog;
using Serilog.Core;

namespace NewRayTracer.Models.Composition
{
    public class CompositionContext
    {
        public CompositionContext(ContainerBuilder container, IGameEnvironment gameEnvironment, IConfigurationRoot configuration, ILogger logger)
        {
            Container = container;
            GameEnvironment = gameEnvironment;
            Configuration = configuration;
            Logger = logger;
        }

        public ContainerBuilder Container { get; }
        public IGameEnvironment GameEnvironment { get; }
        public IConfigurationRoot Configuration { get; }
        public ILogger Logger { get; }
    }
}
