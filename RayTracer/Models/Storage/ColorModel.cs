using OpenTK.Graphics;

namespace RayTracer.Models.Storage
{
    public class ColorModel
    {
        public float Red { get; set; }
        public float Green { get; set; }
        public float Blue { get; set; }
        public float Alpha { get; set; }

        public static implicit operator Color4(ColorModel model)
        {
            return new Color4(model.Red, model.Green, model.Blue, model.Alpha);
        }

        public static implicit operator ColorModel(Color4 color)
        {
            return new ColorModel
            {
                Red = color.R,
                Green = color.G,
                Blue = color.B,
                Alpha = color.A
            };
        }
    }
}
