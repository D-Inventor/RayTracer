using OpenTK;

namespace RayTracer.Models.RayTracer
{
    public class Ray
    {
        private Vector3 direction;

        public Vector3 Position { get; set; }
        public Vector3 Direction { get => direction; set => direction = value.Normalized(); }
    }
}
