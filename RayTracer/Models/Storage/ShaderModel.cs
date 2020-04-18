using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using OpenTK.Graphics.OpenGL4;

namespace RayTracer.Models.Storage
{
    public class ShaderModel
    {
        public string Name { get; set; }
        public string Source { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ShaderType Type { get; set; }
    }
}
