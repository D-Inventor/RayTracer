namespace RayTracer.Models.Textures
{
    public class ImageTextureModel : TextureModel
    {
        public override string TextureType { get => "Image"; set { } }
        public string Path { get; set; }
    }
}
