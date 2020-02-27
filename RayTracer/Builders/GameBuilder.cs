using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RayTracer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Builders
{
    public class GameBuilder : IBuilder<IGame>
    {
        private IServiceCollection serviceCollection;

        public GameBuilder()
        {
            serviceCollection = new ServiceCollection();
        }

        public GameBuilder AddConfiguration(Action<IConfigurationBuilder> configure = null)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Appsettings.json");

            configure?.Invoke(configurationBuilder);

            serviceCollection.AddSingleton<IConfigurationRoot>(configurationBuilder.Build());
            return this;
        }

        public GameBuilder AddStartup<T>() where T : class, IStartup
        {
            serviceCollection.AddSingleton<IStartup, T>();
            return this;
        }

        public IGame Build()
        {
            var services = BuildGameServices();
            return new Game(services);
        }

        private IServiceProvider BuildGameServices()
        {
            IServiceProvider configurationServices = serviceCollection.BuildServiceProvider();


            IServiceCollection result = new ServiceCollection();

            // add system services

            // add user services
            var startupTypes = configurationServices.GetServices<IStartup>();
            foreach(var startup in startupTypes)
            {
                startup.ConfigureServices(result);
            }

            return result.BuildServiceProvider();
        }
    }
}
