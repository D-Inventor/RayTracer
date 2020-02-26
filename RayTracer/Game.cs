using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class Game
    {
        private Type startupType;
        private IServiceCollection serviceCollection;

        private Game(string[] args)
        {
            startupType = null;
            serviceCollection = new ServiceCollection();

            AddConfiguration(args);
        }

        public Game CreateDefaultGame(string[] args)
        {
            return new Game(args);
        }

        public Game UseStartup<T>() where T : class
        {
            serviceCollection.AddSingleton<T>();
            startupType = typeof(T);
            return this;
        }

        private void AddConfiguration(string[] args)
        {
            serviceCollection.AddSingleton<IConfigurationRoot>(
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddCommandLine(args)
                    .AddJsonFile("appsettings.json")
                    .Build()
            );
        }
    }
}
