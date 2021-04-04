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

            var jobsBuilder = context.AddBuilder<JobRegistrationBuilder>();
            jobsBuilder.AddJob<TestJob>().After<OtherTestJob>().After<JetAnotherJob>();
            jobsBuilder.AddJob<OtherTestJob>();
            jobsBuilder.AddJob<JetAnotherJob>();


            context.Container.SubscribeEvent<TestEventSubscriber, TestEvent>();
        }
    }
}
