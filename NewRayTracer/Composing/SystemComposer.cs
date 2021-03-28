using Autofac;

using NewRayTracer.Models.Composition;

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
        }
    }
}
