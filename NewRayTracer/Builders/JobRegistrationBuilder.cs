using Autofac;

using NewRayTracer.Models.Composition;
using NewRayTracer.Services;
using NewRayTracer.Services.JobManagement;

using System;
using System.Collections.Generic;

namespace NewRayTracer.Builders
{
    public class JobRegistrationBuilder : IRegistrationBuilder
    {
        private readonly ISet<Type> _jobTypes;
        private Dictionary<Type, ISet<Type>> _jobConstraints;

        public JobRegistrationBuilder()
        {
            _jobTypes = new HashSet<Type>();
            _jobConstraints = new Dictionary<Type, ISet<Type>>();
        }

        public JobConstraintBuilder<TJob> AddJob<TJob>() where TJob : IJob
        {
            _jobTypes.Add(typeof(TJob));
            return new JobConstraintBuilder<TJob>(this);
        }

        public JobRegistrationBuilder AddConstraint(Type first, Type second)
        {
            var constraints = _jobConstraints[second] = _jobConstraints.ContainsKey(second) ? _jobConstraints[second] : new HashSet<Type>();
            constraints.Add(first);
            return this;
        }

        public JobRegistrationBuilder AddConstraint<TFirst, TSecond>() where TFirst : IJob where TSecond : IJob
        {
            return AddConstraint(typeof(TFirst), typeof(TSecond));
        }

        public void Register(CompositionContext context)
        {
            context.Container
                   .RegisterType<JobService>()
                   .As<IService>()
                   .InstancePerDependency();

            context.Container
                   .RegisterType<JobManager>()
                   .As<IJobManager>()
                   .SingleInstance();

            foreach (Type type in _jobTypes)
            {
                context.Container
                       .RegisterType(type)
                       .As<IJob>()
                       .InstancePerDependency();
            }

            foreach(var kvp in _jobConstraints)
            {
                var constraint = new JobConstraint(kvp.Key, kvp.Value);
                context.Container
                       .RegisterInstance(constraint)
                       .As<IJobConstraint>()
                       .SingleInstance();
            }
        }
    }
}
