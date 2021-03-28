using Autofac;

using NewRayTracer.Logging;
using NewRayTracer.Models.Composition;
using NewRayTracer.Services;
using NewRayTracer.Services.JobManagement;

namespace NewRayTracer.Composing
{
    public class SystemComposer : IComposer
    {
        public void Compose(CompositionContext context)
        {
            context.Container.RegisterInstance(context.Configuration)
                             .SingleInstance();

            context.Container.RegisterInstance(context.GameEnvironment)
                             .SingleInstance();

            context.Container.RegisterInstance(context.Logger)
                             .SingleInstance();
            context.Container.RegisterGeneric(typeof(Logger<>))
                             .As(typeof(ILogger<>))
                             .InstancePerDependency();

            context.Container.RegisterDecorator<DecoratorIServiceLogging, IService>();
            context.Container.RegisterDecorator<DecoratorIJobPerformanceLogger, IJob>();
            context.Container.RegisterDecorator<DecoratorIJobEventPublishing, IJob>();
        }
    }
}
