using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Scene.Lights
{
    public class PointLight : LightBase
    {
        public PointLight(Vector3 position, Quaternion rotation, double brightness, Color4 colour) : base(position, rotation, brightness, colour)
        { }

        public override double GetBrightnessAtPosition(Vector3 position)
        {
            var distanceSqr = (position - transform.ExtractTranslation()).LengthSquared;

            return brightness / distanceSqr;
        }
    }
}
