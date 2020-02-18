using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Scene.Lights
{
    public abstract class LightBase
    {
        protected Matrix4 transform;
        protected double brightness;

        public LightBase(Vector3 position, Quaternion rotation, double brightness)
        {
            this.brightness = brightness;

            transform = Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);
        }

        public abstract double GetBrightnessAtPosition(Vector3 position);
    }
}
