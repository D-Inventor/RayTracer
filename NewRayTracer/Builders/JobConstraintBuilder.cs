using NewRayTracer.Services.JobManagement;

namespace NewRayTracer.Builders
{
    public class JobConstraintBuilder<TJob> where TJob : IJob
    {
        private readonly JobRegistrationBuilder _registrationBuilder;

        public JobConstraintBuilder(JobRegistrationBuilder registrationBuilder)
        {
            _registrationBuilder = registrationBuilder;
        }

        public JobConstraintBuilder<TJob> Before<TOtherJob>() where TOtherJob : IJob
        {
            _registrationBuilder.AddConstraint<TJob, TOtherJob>();
            return this;
        }

        public JobConstraintBuilder<TJob> After<TOtherJob>() where TOtherJob : IJob
        {
            _registrationBuilder.AddConstraint<TOtherJob, TJob>();
            return this;
        }
    }
}
