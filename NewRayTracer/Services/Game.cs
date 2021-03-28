using Autofac;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewRayTracer.Services
{
    public class Game : IAsyncDisposable
    {
        private readonly IContainer _container;

        public Game(IContainer container)
        {
            _container = container;
        }

        public ValueTask DisposeAsync()
        {
            return _container.DisposeAsync();
        }

        public async Task Run()
        {
            using(ILifetimeScope scope = _container.BeginLifetimeScope())
            {
                IEnumerable<IService> services = scope.Resolve<IEnumerable<IService>>();
                await Task.WhenAll(services.Select(s => s.ExecuteAsync())).ConfigureAwait(false);
            }
        }
    }
}
