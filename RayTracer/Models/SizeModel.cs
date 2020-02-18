using System.Drawing;

namespace RayTracer.Models
{
    public class SizeModel
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public static implicit operator Size(SizeModel model)
        {
            return new Size(model.Width, model.Height);
        }

        public static implicit operator SizeModel(Size size)
        {
            return new SizeModel
            {
                Width = size.Width,
                Height = size.Height
            };
        }
    }
}
