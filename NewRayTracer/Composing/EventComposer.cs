using Autofac;

using NewRayTracer.Models.Composition;
using NewRayTracer.Services.Events;

namespace NewRayTracer.Composing
{
    public class EventComposer : IComposer
    {
        public void Compose(CompositionContext context)
        {
            context.Container.RegisterGeneric(typeof(EventPublisher<>)).As(typeof(IEventPublisher<>));
            context.Container.RegisterGenericDecorator(typeof(DecoratorIEventPublisherLogging<>), typeof(IEventPublisher<>));
        }
    }
}
