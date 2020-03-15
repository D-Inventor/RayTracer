using Autofac;

using RayTracer.Logging;
using RayTracer.Models;
using RayTracer.Pipeline;

using System;

namespace RayTracer.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddLogging(this ContainerBuilder builder, Action<IPipelineComponent<Log, Log>, IComponentContext> configure)
        {
            builder.Register(context =>
            {
                IPipelineComponent<Log, Log> pipeline = PipelineComponent.CreateIdentity<Log>();

                configure?.Invoke(pipeline, context);

                return new Logger(pipeline);
            }).As<ILogger>().SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .InstancePerDependency();

            return builder;
        }
    }
}
