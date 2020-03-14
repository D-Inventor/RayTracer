using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RayTracer.Pipeline
{
    public interface IPipelineInput<Input>
    {
        void Execute(Input value, IPipelineContext context = default);
    }

    public interface IPipelineOutput<Output>
    {
        Output GetOutput();
    }

    public interface IPipelineComponent<Input, Output> : IPipelineInput<Input>, IPipelineOutput<Output>
    {
        T AddStep<T>(T pipelineInput) where T : IPipelineInput<Output>;
    }

    public static class PipelineComponent
    {
        public static IPipelineComponent<I, I> CreateIdentity<I>()
        {
            return new PipelineComponent<I, I>(new PipelineStep<I, I>((i, c) => i));
        }

        public static IPipelineComponent<I, O> CreatePipeline<I, O>(IPipelineStep<I, O> step)
        {
            return new PipelineComponent<I, O>(step);
        }

        public static IPipelineComponent<I, O> CreatePipeline<I, O>(Func<I, IPipelineContext, O> step)
        {
            return new PipelineComponent<I, O>(new PipelineStep<I, O>(step));
        }

        public static IAsyncPipelineComponent<I, I> CreateAsyncIdentity<I>()
        {
            return new AsyncPipelineComponent<I, I>(new AsyncPipelineStep<I, I>((i, c) => Task.FromResult(i)));
        }

        public static IAsyncPipelineComponent<I, O> CreateAsyncPipeline<I, O>(IAsyncPipelineStep<I, O> step)
        {
            return new AsyncPipelineComponent<I, O>(step);
        }

        public static IAsyncPipelineComponent<I, O> CreateAsyncPipeline<I, O>(Func<I, IPipelineContext, Task<O>> step)
        {
            return new AsyncPipelineComponent<I, O>(new AsyncPipelineStep<I, O>(step));
        }
    }

    public class PipelineComponent<Input, Output> : IPipelineComponent<Input, Output>
    {

        private readonly IPipelineStep<Input, Output> step;

        internal PipelineComponent(IPipelineStep<Input, Output> step)
        {
            this.step = step;
        }

        public void Execute(Input input, IPipelineContext context = default)
        {
            if (context == default) context = new PipelineContext();

            _output = step.Execute(input, context);
            if (!(context.IsBroken || _next == default))
                foreach (IPipelineInput<Output> component in _next)
                    component.Execute(_output, context.Copy());
        }

        private Output _output = default;
        public Output GetOutput()
        {
            return _output;
        }

        private ICollection<IPipelineInput<Output>> _next = new List<IPipelineInput<Output>>();
        public T AddStep<T>(T pipelineInput) where T : IPipelineInput<Output>
        {
            _next.Add(pipelineInput);
            return pipelineInput;
        }
    }
}
