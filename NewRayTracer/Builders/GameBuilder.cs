using Microsoft.Extensions.Configuration;

using NewRayTracer.Composing;
using NewRayTracer.Models.Collections;
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

            BatchedCollectionBuilder<IComposer> composition = AddComposition();
            composition.Add(new RaytraceComposer()).After<TestComposer>();
            composition.Add(new TestComposer());

            return this;
        }

        public void Build()
        {
            IConfigurationRoot configuration = _configurationBuilder.Build();
            BatchedCollection<IComposer> compositionCollection = _compositionBuilder.Build();

            CompositionContext context = new CompositionContext();
            foreach(IComposer c in compositionCollection.SelectMany(c => c))
            {
                c.Compose(context);
            }
        }
    }
}
