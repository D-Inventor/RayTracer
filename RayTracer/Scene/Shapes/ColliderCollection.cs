using RayTracer.Interfaces;
using RayTracer.Models.RayTracer;

using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Scene.Shapes
{
    public class ColliderCollection : ICollider
    {
        private readonly IEnumerable<ICollider> colliders;

        public ColliderCollection(IEnumerable<ICollider> colliders)
        {
            this.colliders = colliders;
        }

        public bool TryGetCollision(Ray ray, out IEnumerable<Collision> collision)
        {
            collision = TryGetCollision(ray);

            return collision != null && collision.Any();
        }

        private IEnumerable<Collision> TryGetCollision(Ray ray)
        {
            return colliders.SelectMany(collider =>
            {
                if (collider.TryGetCollision(ray, out IEnumerable<Collision> collisions))
                {
                    return collisions;
                }
                return Enumerable.Empty<Collision>();
            });
        }
    }
}
