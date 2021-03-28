﻿using Autofac;

using NewRayTracer.Extensions;
using NewRayTracer.Models.Composition;
using NewRayTracer.Models.Events;
using NewRayTracer.Services;
using NewRayTracer.Services.Events;

namespace NewRayTracer.Composing
{
    public class TestComposer : IComposer
    {
        public void Compose(CompositionContext context)
        {
            context.Container.RegisterType<TestService>()
                             .As<IService>()
                             .InstancePerDependency();

            context.Container.SubscribeEvent<TestEventSubscriber, TestEvent>();
        }
    }
}