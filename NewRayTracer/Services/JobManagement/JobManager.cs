﻿using NewRayTracer.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewRayTracer.Services.JobManagement
{
    public class JobManager : IJobManager
    {
        private readonly Dictionary<Type, IJob> _jobs;
        private readonly ICollection<IJobConstraint> _constraints;

        public JobManager(ICollection<IJob> jobs, ICollection<IJobConstraint> constraints)
        {
            _jobs = jobs.ToDictionary((j) => j.UnwrapDecorators().GetType());
            _constraints = constraints;
        }

        public async Task ExecuteAsync()
        {
            ISet<Type> unstarted = new HashSet<Type>(_jobs.Keys);
            IDictionary<Type, Task> started = new Dictionary<Type, Task>();
            ISet<Type> finished = new HashSet<Type>();

            do
            {
                if(started.Any())
                    await Task.WhenAny(started.Values);

                var newlyFinished = started.Where(kvp => kvp.Value.IsCompleted).Select(kvp => kvp.Key).ToList();
                finished.UnionWith(newlyFinished);
                foreach (var f in newlyFinished) started.Remove(f);

                var constrainedJobs = _constraints.Where(c => !c.Constraints.IsSubsetOf(finished)).Select(c => c.Job);
                var startableJobs = unstarted.Except(constrainedJobs).ToList();
                unstarted.ExceptWith(startableJobs);
                foreach (var job in startableJobs)
                {
                    started[job] = _jobs[job].DoAsync();
                }
            } while (started.Count > 0);
        }
    }
}
