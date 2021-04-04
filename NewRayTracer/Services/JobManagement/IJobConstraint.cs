using System;
using System.Collections.Generic;

namespace NewRayTracer.Services.JobManagement
{
    public interface IJobConstraint
    {
        ISet<Type> Constraints { get; }
        Type Job { get; }
    }
}