using RayTracer.Interfaces;
using RayTracer.Models.RayTracer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Scene.Shapes
{
    public class ColliderCollection : ICollider
    {
        private readonly IEnumerable<ICollider> colliders;

        public ColliderCollection(IEnumerable<ICollider> colliders)
        {
            this.colliders = colliders;
        }

        public bool TryGetCollision(Ray ray, out Collision collision)
        {
            collision = null;
            float distanceSqr = float.MaxValue;
            foreach(var collider in colliders)
            {
                if(collider.TryGetCollision(ray, out var result))
                {
                    var resultDistance = (result.Position - ray.Position).LengthSquared;
                    if(resultDistance < distanceSqr)
                    {
                        collision = result;
                        distanceSqr = resultDistance;
                    }
                }
            }

            return collision != null;
        }
    }
}
