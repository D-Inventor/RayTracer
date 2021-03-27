using Microsoft.Extensions.Configuration;

using NewRayTracer.Composing;
using NewRayTracer.Models.Composition;
using NewRayTracer.Models.Configuration;

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

            var composition = AddComposition();
            composition.Add(new RaytraceComposer()).After<TestComposer>();
            composition.Add(new TestComposer());

            return this;
        }

        public void Build()
        {
            var configuration = _configurationBuilder.Build();
            var compositionCollection = _compositionBuilder.Build();

            var context = new CompositionContext();
            foreach(var c in compositionCollection.SelectMany(c => c))
            {
                c.Compose(context);
            }
        }
    }
}
