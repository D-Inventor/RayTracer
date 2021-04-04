using Autofac;

using Microsoft.Extensions.Configuration;

using NewRayTracer.Builders;
using NewRayTracer.Models.Configuration;

using Serilog;

using System;
using System.Collections.Generic;

namespace NewRayTracer.Models.Composition
{
    public class CompositionContext : IDisposable
    {
        private readonly Dictionary<Type, IRegistrationBuilder> _builders;

        public CompositionContext(ContainerBuilder container, IGameEnvironment gameEnvironment, IConfigurationRoot configuration, ILogger logger)
        {
            Container = container;
            GameEnvironment = gameEnvironment;
            Configuration = configuration;
            Logger = logger;

            _builders = new Dictionary<Type, IRegistrationBuilder>();
        }

        public ContainerBuilder Container { get; }
        public IGameEnvironment GameEnvironment { get; }
        public IConfigurationRoot Configuration { get; }
        public ILogger Logger { get; }

        public TBuilder AddBuilder<TBuilder>() where TBuilder : class, IRegistrationBuilder, new()
        {
            Type builderType = typeof(TBuilder);
            _builders[builderType] =
                (_builders.ContainsKey(builderType) && _builders[builderType] is TBuilder builder)
                    ? builder
                    : (builder = new TBuilder());
            return builder;
        }

        #region IDisposable implementation
        private bool _disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var builder in _builders) builder.Value.Register(this);
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
