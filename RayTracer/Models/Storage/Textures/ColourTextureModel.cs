using OpenTK.Graphics;

namespace RayTracer.Models.Storage.Textures
{
    public class ColourTextureModel : TextureModel
    {
        public override string TextureType { get => "Colour"; set { } }

        public int Width { get; set; }
        public int Height { get; set; }
        public Color4 Colour { get; set; }
    }
}
