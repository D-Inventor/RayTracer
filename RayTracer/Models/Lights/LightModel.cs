using Newtonsoft.Json;
using OpenTK.Graphics;
using RayTracer.Json;

namespace RayTracer.Models.Lights
{
    [JsonConverter(typeof(LightConverter))]
    public abstract class LightModel
    {
        public VectorModel Position { get; set; }
        public QuaternionModel Rotation { get; set; }
        public double Brightness { get; set; }
        public Color4 Colour { get; set; }

        public abstract string Type { get; set; }
    }
}
