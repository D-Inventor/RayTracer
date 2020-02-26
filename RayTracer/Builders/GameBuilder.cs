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
        private Type startupType;
        private IServiceCollection serviceCollection;

        public GameBuilder()
        {
            startupType = null;
            serviceCollection = new ServiceCollection();
        }

        public GameBuilder AddConfiguration(Action<IConfigurationBuilder> configure = null)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Appsettings.json")
                .AddCommandLine(Environment.GetCommandLineArgs());

            configure?.Invoke(configurationBuilder);

            serviceCollection.AddSingleton<IConfigurationRoot>(configurationBuilder.Build());
            return this;
        }

        public GameBuilder AddStartup<T>() where T : class
        {
            startupType = typeof(T);
            serviceCollection.AddSingleton<T>();
            return this;
        }

        public IGame Build()
        {
            throw new NotImplementedException();
        }
    }
}
