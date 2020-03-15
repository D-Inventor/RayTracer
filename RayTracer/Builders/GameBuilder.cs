using Autofac;

using Microsoft.Extensions.Configuration;
using RayTracer.Extensions;
using RayTracer.Factories;
using RayTracer.Helpers;
using RayTracer.Interfaces;
using RayTracer.Logging;
using RayTracer.Models.Options;

using System;

namespace RayTracer.Builders
{
    public class GameBuilder : IBuilder<IGame>
    {
        public GameBuilder()
        { }

        public GameBuilder(EnvironmentOptions options)
        {
            EnvironmentHelper = new EnvironmentHelper(options);
        }

        public IEnvironmentHelper EnvironmentHelper { get; set; }
        private IConfigurationRoot Configuration { get; set; }

        public IGame Build()
        {
            ContainerBuilder builder = new ContainerBuilder();

            AddConfiguration(builder);

            builder.AddLogging((pipeline, context) =>
            {
                IConfigurationRoot configuration = context.Resolve<IConfigurationRoot>();
                var section = configuration.GetSection("Logging");

                pipeline
                .AddLogFilter(Models.Severity.Debug)
                .AddLogFormatter(new LogFormatter(section))
                .AddConsole();
            });

            builder.RegisterType<App>().As<IGameRunner>().SingleInstance();
            builder.RegisterType<SceneFactory>().As<ISceneFactory>().InstancePerLifetimeScope();
            builder.RegisterType<MeshFactory>().As<IMeshFactory>().InstancePerLifetimeScope();
            builder.RegisterType<TextureFactory>().As<ITextureFactory>().InstancePerLifetimeScope();

            return new Game(builder.Build());
        }

        private void AddConfiguration(ContainerBuilder builder)
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(EnvironmentHelper.ConfigurationPath)
                .AddJsonFile("Appsettings.json", false)
                .AddJsonFile($"Appsettings.{EnvironmentHelper.Environment}.json")
                .AddCommandLine(Environment.GetCommandLineArgs())
                .Build();

            builder.RegisterInstance(Configuration)
                .SingleInstance();
        }
    }
}
