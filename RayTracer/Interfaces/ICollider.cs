using RayTracer.Models.RayTracer;

namespace RayTracer.Interfaces
{
    public interface ICollider
    {
        bool TryGetCollision(Ray ray, out Collision collision);
    }
}
