using RayTracer.Models.RayTracer;

using System.Collections.Generic;

namespace RayTracer.Scene.Shapes
{
    public class Plane : ShapeBase
    {
        public override bool TryGetCollision(Ray ray, out IEnumerable<Collision> collision)
        {
            collision = null;
            return false;
        }
    }
}
