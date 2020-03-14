using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RayTracer.Pipeline
{
    public interface IAsyncPipelineInput<Input>
    {
        Task ExecuteAsync(Input value, IPipelineContext context = default);
    }

    public interface IAsyncPipelineComponent<Input, Output> : IAsyncPipelineInput<Input>, IPipelineOutput<Output>
    {
        T AddStep<T>(T pipelineInput) where T : IAsyncPipelineInput<Output>;
    }

    public class AsyncPipelineComponent<Input, Output> : IAsyncPipelineComponent<Input, Output>
    {

        private readonly IAsyncPipelineStep<Input, Output> step;

        internal AsyncPipelineComponent(IAsyncPipelineStep<Input, Output> step)
        {
            this.step = step;
        }

        public async Task ExecuteAsync(Input input, IPipelineContext context = default)
        {
            if (context == default) context = new PipelineContext();

            _output = await step.ExecuteAsync(input, context);
            if (!(context.IsBroken || _next == default))
                Task.WaitAll(_next.Select(x => x.ExecuteAsync(_output, context.Copy())).ToArray());
        }

        private Output _output = default;
        public Output GetOutput()
        {
            return _output;
        }

        private ICollection<IAsyncPipelineInput<Output>> _next = new List<IAsyncPipelineInput<Output>>();
        public T AddStep<T>(T pipelineInput) where T : IAsyncPipelineInput<Output>
        {
            _next.Add(pipelineInput);
            return pipelineInput;
        }
    }
}
