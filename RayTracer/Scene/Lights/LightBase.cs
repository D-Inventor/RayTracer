using OpenTK;
using OpenTK.Graphics;
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

        public LightBase(Vector3 position, Quaternion rotation, double brightness, Color4 colour)
        {
            this.brightness = brightness;
            Colour = colour;
            transform = Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);
        }

        public Vector3 Position => transform.ExtractTranslation();

        public Color4 Colour { get; set; }

        public abstract double GetBrightnessAtPosition(Vector3 position);
    }
}
