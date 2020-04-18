using Newtonsoft.Json;

using RayTracer.Json;

namespace RayTracer.Models.Storage.Shapes
{
    [JsonConverter(typeof(ShapeConverter))]
    public abstract class ShapeModel
    {
        public string MaterialName { get; set; }
        public VectorModel Position { get; set; }
        public QuaternionModel Rotation { get; set; }
        public abstract string ShapeType { get; set; }
    }
}
