using RayTracer.Models.RayTracer;

namespace RayTracer.Scene.Shapes
{
    public class Plane : ShapeBase
    {
        public override bool TryGetCollision(Ray ray, out Collision collision)
        {
            collision = null;
            return false;
        }
    }
}
