using NewRayTracer.Models.Composition;

using System;

namespace NewRayTracer.Composing
{
    public class RaytraceComposer : IComposer
    {
        public void Compose(CompositionContext context)
        {
            Console.WriteLine("Raytrace composer");
        }
    }
}
