using System;

namespace RayTracer.Models.Contexts
{
    public interface IAppContext
    {
        Scene.Scene CurrentScene { get; set; }
    }

    public class AppContext : IAppContext, IDisposable
    {
        private Scene.Scene currentScene;
        public Scene.Scene CurrentScene
        {
            get => currentScene;
            set
            {
                currentScene?.Dispose();
                currentScene = value;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                CurrentScene?.Dispose();

                disposedValue = true;
            }
        }

        ~AppContext()
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
