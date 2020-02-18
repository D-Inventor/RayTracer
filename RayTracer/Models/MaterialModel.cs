using OpenTK.Graphics;

namespace RayTracer.Models
{
    public class MaterialModel
    {
        public string Name { get; set; }
        public double Roughness { get; set; }
        public double Reflectiveness { get; set; }
        public Color4 Colour { get; set; }
    }
}
