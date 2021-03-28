
using Autofac;

using NewRayTracer.Models.Composition;
using NewRayTracer.Services.JobManagement;

namespace NewRayTracer.Composing
{
    public class RaytraceComposer : IComposer
    {
        public void Compose(CompositionContext context)
        {
            context.Container.RegisterType<TestJob>().As<IJob>().InstancePerDependency();
        }
    }
}
