namespace RayTracer.Models.Storage
{
    public class CameraModel
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float ViewDistance { get; set; }
        public string TextureTarget { get; set; }
        public VectorModel Position { get; set; }
        public VectorModel ViewDirection { get; set; }
        public string Material { get; set; }
    }
}
