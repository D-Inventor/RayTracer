using Microsoft.VisualStudio.TestTools.UnitTesting;
using RayTracer.Extensions;
using RayTracer.Pipeline;
using System.Threading.Tasks;

namespace UnitTests.Logging
{
    [TestClass]
    public class PipelineTests
    {
        [TestMethod]
        public void Execute_FullPipeline_ExecutesToTheEnd()
        {
            var initialComponent = PipelineComponent.CreatePipeline<string, int>((x, c) => x.Length);

            IPipelineOutput<bool> pipelineOutput = initialComponent
                .AddStep((x, c) => x > 10);

            initialComponent.Execute("12345678901");

            Assert.IsTrue(pipelineOutput.GetOutput());
        }

        [TestMethod]
        public void Execute_BrokenPipeline_DoesNotFinish()
        {
            bool triggered = false;
            var initialComponent = PipelineComponent.CreatePipeline<int, bool>((x, c) =>
            {
                c.Break();
                return true;
            });

            IPipelineOutput<bool> pipelineOutput = initialComponent.AddStep((x, c) =>
            {
                triggered = true;
                return x;
            });

            var context = new PipelineContext();

            initialComponent.Execute(10, context);

            Assert.IsTrue(context.IsBroken);
            Assert.IsFalse(triggered);
        }

        [TestMethod]
        public async Task ExecuteAsync_FullPipeline_ExecutesToTheEnd()
        {
            var initialComponent = PipelineComponent.CreateAsyncPipeline<string, int>((x, c) => Task.FromResult(x.Length));

            IPipelineOutput<bool> pipelineOutput = initialComponent
                .AddStep((x, c) => Task.FromResult(x > 10));

            await initialComponent.ExecuteAsync("dit is een test string");

            Assert.IsTrue(pipelineOutput.GetOutput());
        }

        [TestMethod]
        public async Task ExecuteAsync_BrokenPipeline_DoesNotFinish()
        {
            bool triggered = false;
            var initialComponent = PipelineComponent.CreateAsyncPipeline<int, bool>((x, c) =>
            {
                c.Break();
                return Task.FromResult(true);
            });

            IPipelineOutput<bool> pipelineOutput = initialComponent.AddStep((x, c) =>
            {
                triggered = true;
                return Task.FromResult(x);
            });

            var context = new PipelineContext();

            await initialComponent.ExecuteAsync(10, context);

            Assert.IsTrue(context.IsBroken);
            Assert.IsFalse(triggered);
        }
    }
}
