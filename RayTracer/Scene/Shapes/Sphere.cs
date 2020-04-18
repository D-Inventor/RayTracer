using OpenTK;
using OpenTK.Graphics;
using RayTracer.Models.RayTracer;

using System;
using System.Collections.Generic;

namespace RayTracer.Scene.Shapes
{
    public class Sphere : ShapeBase
    {
        public double Radius { get; set; }

        public override Color4 GetColourAt(Vector3 position)
        {
            position -= Transform.ExtractTranslation();
            position /= (float)Radius;

            var up = (new Vector4(Vector3.UnitY, 0) * Transform).Xyz;
            var right = (new Vector4(Vector3.UnitX, 0) * Transform).Xyz;
            var forward = (new Vector4(Vector3.UnitZ, 0) * Transform).Xyz;

            var equator = Vector3.Cross(Vector3.Cross(up, position), up).Normalized();
            var ar = Vector3.Dot(right, equator);
            var af = Vector3.Dot(forward, equator);

            var u = (((1 - ar) * (af < 0 ? -1 : 1)) + 2) * 0.25f;
            var v = (Vector3.Dot(up, position) + 1) * 0.5f;

            return Material.Texture[u, v];
        }

        public override bool TryGetCollision(Ray ray, out IEnumerable<Collision> collision)
        {
            Vector3 position = Transform.ExtractTranslation();

            Vector3 dif = position - ray.Position;
            float scalar = Vector3.Dot(dif, ray.Direction);
            Vector3 projection = ray.Direction * scalar;
            dif -= projection;
            if (dif.LengthSquared <= (Radius * Radius))
            {

                double b = 2 * Vector3.Dot(ray.Direction, ray.Position - position);
                double c = -2 * Vector3.Dot(ray.Position, position) + position.LengthSquared + ray.Position.LengthSquared - (Radius * Radius);

                double dsqrt = Math.Sqrt(b * b - 4 * c);

                double s1 = (-b + dsqrt) * 0.5;
                double s2 = (-b - dsqrt) * 0.5;

                double sfinal;

                if (s2 > 0.001)
                    sfinal = s2;
                else if (s1 > 0.001)
                    sfinal = s1;
                else
                {
                    collision = null;
                    return false;
                }

                Vector3 collisionPosition = ray.Position + ray.Direction * (float)sfinal;

                collision = new List<Collision>
                {
                    new Collision
                    {
                        InDirection = ray.Direction,
                        Material = Material,
                        Position = collisionPosition,
                        Normal = collisionPosition - position,
                        DistanceSqr = (collisionPosition - ray.Position).LengthSquared,
                        Shape = this
                    }
                };
                return true;
            }

            collision = null;
            return false;
        }
    }
}
