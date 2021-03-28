using Autofac;

using Microsoft.Extensions.Configuration;

using NewRayTracer.Composing;
using NewRayTracer.Extensions;
using NewRayTracer.Logging;
using NewRayTracer.Models.Collections;
using NewRayTracer.Models.Composition;
using NewRayTracer.Models.Configuration;
using NewRayTracer.Services;

using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

using System.Linq;

namespace NewRayTracer.Builders
{
    public class GameBuilder
    {
        private IConfigurationBuilder _configurationBuilder;
        private BatchedCollectionBuilder<IComposer> _compositionBuilder;
        private LoggerConfiguration _loggerConfiguration;

        public IConfigurationBuilder AddConfiguration()
            => _configurationBuilder = _configurationBuilder ?? new ConfigurationBuilder();

        public BatchedCollectionBuilder<IComposer> AddComposition()
            => _compositionBuilder = _compositionBuilder ?? new BatchedCollectionBuilder<IComposer>();

        public LoggerConfiguration AddLogging()
            => _loggerConfiguration = _loggerConfiguration ?? new LoggerConfiguration();


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

            AddLogging()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u}]({SourceContext}){NewLine}\t{Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Literate);

            return this;
        }

        public Game Build()
        {
            IConfigurationRoot configuration = _configurationBuilder.Build();
            BatchedCollection<IComposer> compositionCollection = _compositionBuilder.Build();
            Logger Serilogger = _loggerConfiguration.CreateLogger();
            ILogger<GameBuilder> logger = new Logger<GameBuilder>(Serilogger);

            ContainerBuilder containerBuilder = new ContainerBuilder();

            CompositionContext context = new CompositionContext(containerBuilder, GameEnvironment.Instance, configuration, Serilogger);
            foreach(IComposer c in compositionCollection.SelectMany(c => c))
            {
                logger.Debug("Compose: {0}", c.GetType().GetFormattedName());
                c.Compose(context);
            }

            IContainer container = containerBuilder.Build();
            return new Game(container);
        }
    }
}
