using OpenTK;
using RayTracer.Models.RayTracer;
using RayTracer.Scene;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Helpers
{
    public interface IPixelCollisionCalculator
    {
        IEnumerable<Collision> GetInitialCollisions(Vector3 pixelHorizontal, Vector3 pixelVertical, Vector3 pixelCenter, Vector3 cameraPosition, int pixelX, int pixelY, Material material, Texture texture);
    }

    public class PixelCollisionCalculatorAA8 : IPixelCollisionCalculator
    {
        public IEnumerable<Collision> GetInitialCollisions(Vector3 pixelHorizontal, Vector3 pixelVertical, Vector3 pixelCenter, Vector3 cameraPosition, int pixelX, int pixelY, Material material, Texture texture)
        {
            pixelHorizontal /= 4;
            pixelVertical /= 4;

            Vector3 topleft = pixelCenter + pixelHorizontal * -1.5f + pixelVertical * 1.5f;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if ((x + y) % 2 == 0)
                    {
                        var position = topleft + x * pixelHorizontal + y * pixelVertical;
                        yield return new Collision
                        {
                            Depth = 0,
                            InDirection = cameraPosition - position,
                            Normal = position - cameraPosition,
                            Material = material,
                            Position = position,
                            Pixel = new PixelReference(0.125f, pixelX, pixelY, texture)
                        };
                    }
                }
            }
        }
    }
}
