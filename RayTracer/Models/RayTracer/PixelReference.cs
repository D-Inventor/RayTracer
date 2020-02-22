using OpenTK.Graphics;

using RayTracer.Scene;

using System;

namespace RayTracer.Models.RayTracer
{
    public class PixelReference
    {
        public PixelReference(float contribution, int x, int y, Texture texture)
        {
            Contribution = contribution;
            X = x;
            Y = y;
            Texture = texture;
        }

        public Texture Texture { get; }

        public int Y { get; }

        public int X { get; }

        public float Contribution { get; }

        private bool used = false;
        public void Use(Color4 colour)
        {
            if (used) { return; }

            byte r = (byte)Math.Floor(colour.R * Contribution * colour.A * 255),
                 g = (byte)Math.Floor(colour.G * Contribution * colour.A * 255),
                 b = (byte)Math.Floor(colour.B * Contribution * colour.A * 255);

            int result = (r << 16) + (g << 8) + (b);
            Texture[X, Y] += result;
            used = true;
        }
    }
}
