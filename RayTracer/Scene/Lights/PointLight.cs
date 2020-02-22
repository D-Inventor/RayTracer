using OpenTK;
using OpenTK.Graphics;

namespace RayTracer.Scene.Lights
{
    public class PointLight : LightBase
    {
        public PointLight(Vector3 position, Quaternion rotation, double brightness, Color4 colour) : base(position, rotation, brightness, colour)
        { }

        public override double GetBrightnessAtPosition(Vector3 position)
        {
            float distanceSqr = (position - transform.ExtractTranslation()).LengthSquared;

            return brightness / distanceSqr;
        }
    }
}
