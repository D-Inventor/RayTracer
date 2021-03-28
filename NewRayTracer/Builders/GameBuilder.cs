using Autofac;

using Microsoft.Extensions.Configuration;

using NewRayTracer.Composing;
using NewRayTracer.Models.Collections;
using NewRayTracer.Models.Composition;
using NewRayTracer.Models.Configuration;
using NewRayTracer.Services;

using System.Linq;

namespace NewRayTracer.Builders
{
    public class GameBuilder
    {
        private IConfigurationBuilder _configurationBuilder;
        private BatchedCollectionBuilder<IComposer> _compositionBuilder;

        public IConfigurationBuilder AddConfiguration()
            => _configurationBuilder = _configurationBuilder ?? new ConfigurationBuilder();

        public BatchedCollectionBuilder<IComposer> AddComposition()
            => _compositionBuilder = _compositionBuilder ?? new BatchedCollectionBuilder<IComposer>();


        public GameBuilder CreateDefault()
        {
            AddConfiguration()
                .AddEnvironmentVariables()
                .SetBasePath(GameEnvironment.Instance.ConfigDirectory)
                .AddJsonFile("Appsettings.json")
                .AddJsonFile($"Appsettings.{GameEnvironment.Instance.Environent}.json");

            BatchedCollectionBuilder<IComposer> composition = AddComposition();
            composition.Add(new RaytraceComposer()).After<TestComposer>();
            composition.Add(new TestComposer());
            composition.Add(new EventComposer());
            composition.Add(new SystemComposer());

            return this;
        }

        public Game Build()
        {
            IConfigurationRoot configuration = _configurationBuilder.Build();
            BatchedCollection<IComposer> compositionCollection = _compositionBuilder.Build();

            ContainerBuilder containerBuilder = new ContainerBuilder();

            CompositionContext context = new CompositionContext(containerBuilder, GameEnvironment.Instance, configuration);
            foreach(IComposer c in compositionCollection.SelectMany(c => c))
            {
                c.Compose(context);
            }

            var container = containerBuilder.Build();
            return new Game(container);
        }
    }
}
