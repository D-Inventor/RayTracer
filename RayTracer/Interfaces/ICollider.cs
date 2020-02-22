using RayTracer.Models.RayTracer;

using System.Collections.Generic;

namespace RayTracer.Interfaces
{
    public interface ICollider
    {
        bool TryGetCollision(Ray ray, out IEnumerable<Collision> collision);
    }
}
