using OpenTK;
using OpenTK.Graphics;

using RayTracer.Interfaces;
using RayTracer.Models.RayTracer;

using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Helpers
{
    public class CollisionHelper
    {
        private readonly Scene.Scene scene;
        private readonly ICollider collider;

        public CollisionHelper(Scene.Scene scene, ICollider collider)
        {
            this.scene = scene;
            this.collider = collider;
        }

        public Color4 FindColourAtCollision(Collision collision)
        {
            Color4 result = Color4.Black;
            foreach (Scene.Lights.LightBase light in scene.Lights.Where(l => Vector3.Dot(l.Position - collision.Position, collision.Normal) > 0))
            {
                Vector3 lightposition = light.Position;
                Vector3 lightdirection = lightposition - collision.Position;
                if (Vector3.Dot(lightdirection, collision.Normal) <= 0)
                {
                    continue;
                }

                // test if the light source is obstructed
                Ray lightRay = new Ray()
                {
                    Direction = lightposition - collision.Position,
                    Position = collision.Position
                };
                if (!collider.TryGetCollision(lightRay, out IEnumerable<Collision> lightCollisions) || lightCollisions.Any(c => (c.Position - collision.Position).LengthSquared > lightdirection.LengthSquared))
                {
                    lightdirection.Normalize();
                    float effectiveness = Vector3.Dot(lightdirection, collision.Normal);

                    double brightness = light.GetBrightnessAtPosition(collision.Position);
                    Color4 collisionColour = collision.Shape.GetColourAt(collision.Position);
                    result.R = (float)MathHelper.Clamp(collisionColour.R * brightness * effectiveness * light.Colour.R + result.R, 0, 1);
                    result.G = (float)MathHelper.Clamp(collisionColour.G * brightness * effectiveness * light.Colour.G + result.G, 0, 1);
                    result.B = (float)MathHelper.Clamp(collisionColour.B * brightness * effectiveness * light.Colour.B + result.B, 0, 1);
                }
            }

            result.A = (float)(1 - collision.Material.Reflectiveness);
            return result;
        }

        public IEnumerable<Ray> GetResultRays(Collision collision)
        {
            IEnumerable<Ray> result = Enumerable.Empty<Ray>();

            if (collision.Material.Reflectiveness != 0)
            {
                result = result.Concat(GetReflectionRays(collision));
            }

            return result;
        }

        private static IEnumerable<Ray> GetReflectionRays(Collision collision)
        {
            return new List<Ray>
            {
                new Ray
                {
                    Direction = collision.GetReflection(),
                    Position = collision.Position
                }
            };
        }
    }
}
