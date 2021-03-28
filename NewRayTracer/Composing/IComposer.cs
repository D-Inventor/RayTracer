using NewRayTracer.Models.Composition;

namespace NewRayTracer.Composing
{
    public interface IComposer
    {
        void Compose(CompositionContext context);
    }
}
