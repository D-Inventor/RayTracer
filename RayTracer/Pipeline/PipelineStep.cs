using System;

namespace RayTracer.Pipeline
{
    public interface IPipelineStep<Input, Output>
    {
        Output Execute(Input input, IPipelineContext context);
    }

    public class PipelineStep<Input, Output> : IPipelineStep<Input, Output>
    {
        private readonly Func<Input, IPipelineContext, Output> action;

        public PipelineStep(Func<Input, IPipelineContext, Output> action)
        {
            this.action = action;
        }

        public Output Execute(Input input, IPipelineContext context)
        {
            return action.Invoke(input, context);
        }
    }
}
