using NewRayTracer.Models.Composition;

namespace NewRayTracer.Builders
{
    public interface IRegistrationBuilder
    {
        void Register(CompositionContext context);
    }
}
