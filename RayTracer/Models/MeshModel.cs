using System.Collections.Generic;

namespace RayTracer.Models
{
    public class MeshModel
    {
        public string Name { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Elements { get; set; }
        public Dictionary<string, int> Parameters { get; set; }
    }
}
