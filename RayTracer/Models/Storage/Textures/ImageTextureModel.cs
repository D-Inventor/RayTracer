namespace RayTracer.Models.Storage.Textures
{
    public class ImageTextureModel : TextureModel
    {
        public override string TextureType { get => "Image"; set { } }
        public string Path { get; set; }
    }
}
