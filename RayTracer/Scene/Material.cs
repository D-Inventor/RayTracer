using OpenTK.Graphics;

namespace RayTracer.Scene
{
    public class Material
    {
        public double Roughness { get; set; }
        public double Reflectiveness { get; set; }
        public Color4 Colour { get; set; }
    }
}
