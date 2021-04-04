using Autofac;

using NewRayTracer.Builders;
using NewRayTracer.Extensions;
using NewRayTracer.Models.Composition;
using NewRayTracer.Models.Events;
using NewRayTracer.Services;
using NewRayTracer.Services.Events;
using NewRayTracer.Services.JobManagement;

namespace NewRayTracer.Composing
{
    public class TestComposer : IComposer
    {
        public void Compose(CompositionContext context)
        {
            context.Container.RegisterType<TestService>()
                             .As<IService>()
                             .InstancePerDependency();

            JobRegistrationBuilder jobsBuilder = context.AddBuilder<JobRegistrationBuilder>();
            jobsBuilder.AddJob<TestJob>();
            jobsBuilder.AddJob<OtherTestJob>();
            jobsBuilder.AddJob<JetAnotherJob>().Before<TestJob>().Before<OtherTestJob>();


            context.Container.SubscribeEvent<TestEventSubscriber, TestEvent>();
        }
    }
}
