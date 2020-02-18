using Newtonsoft.Json;
using RayTracer.Json;

namespace RayTracer.Models.Lights
{
    [JsonConverter(typeof(LightConverter))]
    public abstract class LightModel
    {
        public VectorModel Position { get; set; }
        public QuaternionModel Rotation { get; set; }
        public double Brightness { get; set; }

        public abstract string Type { get; set; }
    }
}
