using System;
using System.Collections.Generic;

namespace NewRayTracer.Services.JobManagement
{
    public class JobConstraint : IJobConstraint
    {
        public JobConstraint(Type Job, ISet<Type> Constraints)
        {
            this.Job = Job;
            this.Constraints = Constraints;
        }

        public Type Job { get; }
        public ISet<Type> Constraints { get; }
    }
}
