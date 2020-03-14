using OpenTK;

using RayTracer.Models.RayTracer;

using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Scene.Shapes
{
    public class Plane : ShapeBase
    {
        public override bool TryGetCollision(Ray ray, out IEnumerable<Collision> collision)
        {
            Vector3 normal = (new Vector4(Vector3.UnitY, 0) * Transform).Xyz;
            Vector3 position = Transform.ExtractTranslation();
            float d = Vector3.Dot(normal, position);

            if (Vector3.Dot(ray.Direction, normal) == 0)
            {
                collision = Enumerable.Empty<Collision>();
                return false;
            }

            float s = (-Vector3.Dot(ray.Position, normal) + d) / (Vector3.Dot(ray.Direction, normal));

            if (s < 0.001)
            {
                collision = Enumerable.Empty<Collision>();
                return false;
            }

            collision = new List<Collision>
            {
                new Collision
                {
                    InDirection = ray.Direction,
                    Material = Material,
                    Normal = normal,
                    Position = ray.Position + ray.Direction * s,
                    DistanceSqr = s * s
                }
            };
            return true;
        }
    }
}
