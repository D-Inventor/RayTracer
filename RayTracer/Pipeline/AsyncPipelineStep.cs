using System;
using System.Threading.Tasks;

namespace RayTracer.Pipeline
{
    public interface IAsyncPipelineStep<Input, Output>
    {
        Task<Output> ExecuteAsync(Input input, IPipelineContext context);
    }

    public class AsyncPipelineStep<Input, Output> : IAsyncPipelineStep<Input, Output>
    {
        private readonly Func<Input, IPipelineContext, Task<Output>> action;

        public AsyncPipelineStep(Func<Input, IPipelineContext, Task<Output>> action)
        {
            this.action = action;
        }

        public Task<Output> ExecuteAsync(Input input, IPipelineContext context)
        {
            return action.Invoke(input, context);
        }
    }
}
