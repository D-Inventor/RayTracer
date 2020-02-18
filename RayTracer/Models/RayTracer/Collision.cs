using OpenTK;

using RayTracer.Scene;

namespace RayTracer.Models.RayTracer
{
    public class Collision
    {
        private Vector3 normal;
        private Vector3 inDirection;

        public Vector3 Normal { get => normal; set => normal = value.Normalized(); }
        public Vector3 Position { get; set; }
        public Vector3 InDirection { get => inDirection; set => inDirection = value.Normalized(); }
        public int Depth { get; set; }
        public Material Material { get; set; }
        public PixelReference Pixel { get; set; }

        public Vector3 GetReflection() => InDirection - 2 * Vector3.Dot(InDirection, Normal) * Normal;
    }
}
