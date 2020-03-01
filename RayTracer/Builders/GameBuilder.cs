using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RayTracer.Helpers;
using RayTracer.Interfaces;
using RayTracer.Models.Options;
using System;
using System.IO;

namespace RayTracer.Builders
{
    public class GameBuilder : IBuilder<IGame>
    {
        public IGame Build()
        {
            IEnvironmentHelper environmentHelper = GetEnvironmentHelper();
            IConfigurationRoot configuration = GetConfiguration(environmentHelper);
            IServiceProvider services = ConfigureServices(environmentHelper, configuration);

            return new Game(services);
        }

        private IConfigurationRoot GetConfiguration(IEnvironmentHelper environmentHelper)
        {
            return new ConfigurationBuilder()
                .SetBasePath(environmentHelper.ConfigurationPath)
                .AddJsonFile("Appsettings.json", false)
                .AddJsonFile($"Appsettings.{environmentHelper.Environment}.json")
                .AddCommandLine(Environment.GetCommandLineArgs())
                .Build();
        }

        private IServiceProvider ConfigureServices(IEnvironmentHelper environmentHelper, IConfigurationRoot configuration)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(environmentHelper);
            serviceCollection.AddSingleton(configuration);



            return serviceCollection.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });
        }

        private IEnvironmentHelper GetEnvironmentHelper()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Environment.json")
                .Build();

            EnvironmentOptions options = new EnvironmentOptions();
            configuration.Bind(options);

            return new EnvironmentHelper(options);
        }
    }
}
