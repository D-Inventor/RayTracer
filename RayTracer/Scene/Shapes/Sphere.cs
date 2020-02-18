using OpenTK;
using RayTracer.Models.RayTracer;
using System;
using System.Diagnostics;

namespace RayTracer.Scene.Shapes
{
    public class Sphere : ShapeBase
    {
        public double Radius { get; set; }

        public override bool TryGetCollision(Ray ray, out Collision collision)
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

                if (s2 > 0)
                    sfinal = s2;
                else if (s1 > 0)
                    sfinal = s1;
                else
                {
                    collision = null;
                    return false;
                }

                var collisionPosition = ray.Position + ray.Direction * (float)sfinal;

                collision = new Collision
                {
                    InDirection = ray.Direction,
                    Material = this.Material,
                    Position = collisionPosition,
                    Normal = collisionPosition - position
                };
                return true;
            }

            collision = null;
            return false;
        }
    }
}
