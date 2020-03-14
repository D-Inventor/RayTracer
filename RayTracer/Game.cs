using Autofac;
using RayTracer.Interfaces;
using System;

namespace RayTracer
{
    public interface IGame : IDisposable
    {
        IContainer Services { get; }

        void Run();
    }

    public class Game : IGame
    {
        public Game(IContainer services)
        {
            Services = services;
        }

        public void Run()
        {
            using (var scope = Services.BeginLifetimeScope())
            {
                scope.Resolve<IGameRunner>().Run();
            }
        }

        public IContainer Services { get; }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Services.Dispose();

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
