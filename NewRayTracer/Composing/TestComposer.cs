using NewRayTracer.Models.Composition;

using System;

namespace NewRayTracer.Composing
{
    public class TestComposer : IComposer
    {
        public void Compose(CompositionContext context)
        {
            Console.WriteLine("Test composer!");
        }
    }
}
