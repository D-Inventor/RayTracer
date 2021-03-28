using Autofac;

using NewRayTracer.Services.Events;

namespace NewRayTracer.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder SubscribeEvent<TImplementation, TEvent>(this ContainerBuilder container) where TImplementation : IEventSubscriber<TEvent>
        {
            container.RegisterType<TImplementation>()
                     .As<IEventSubscriber<TEvent>>()
                     .InstancePerDependency();

            return container;
        }
    }
}
