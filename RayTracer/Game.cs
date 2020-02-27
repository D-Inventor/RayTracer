using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RayTracer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public interface IGame: IDisposable
    {
        IServiceProvider Services { get; }

        void Run();
    }

    public class Game : IGame
    {
        public Game(IServiceProvider services)
        {
            Services = services;
        }

        public void Run()
        {
            Services.GetRequiredService<IGameRunner>().Run();
        }

        public IServiceProvider Services { get; }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if(Services is IDisposable disposableServices)
                {
                    disposableServices.Dispose();
                }

                disposedValue = true;
            }
        }

        ~Game()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
