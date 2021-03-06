﻿using OpenTK;
using OpenTK.Graphics;

using RayTracer.Extensions;
using RayTracer.Helpers;
using RayTracer.Interfaces;
using RayTracer.Models.RayTracer;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RayTracer.Scene
{
    public class Camera
    {
        private ConcurrentQueue<Collision> collisionQueue = null;

        private readonly float width;
        private readonly float height;
        private readonly float viewDistance;
        private readonly IPixelCollisionCalculator pixelCollisionCalculator;
        private Matrix4 transform;
        private readonly Texture texture;
        private readonly Material material;
        private int maxItems = 500000;

        private static Vector3 Up => Vector3.UnitY;
        private static Vector3 Forward => Vector3.UnitZ;
        private static Vector3 Right => Vector3.UnitX;

        public Camera(float width, float height, float viewDistance, Vector3 position, Vector3 viewDirection, Texture texture, Material material)
        {
            this.width = width;
            this.height = height;
            this.viewDistance = viewDistance;
            this.texture = texture;
            this.material = material;
            float yaw = (float)Math.Atan(viewDirection.X / viewDirection.Z);
            float pitch = (float)Math.Atan(viewDirection.Y / viewDirection.Z);
            pixelCollisionCalculator = new PixelCollisionCalculatorAA8();

            transform = Matrix4.CreateTranslation(position) *
                        Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(pitch, yaw, 0));
        }

        public ICollider Collider { get; set; }
        public CollisionHelper CollisionHelper { get; set; }

        public ConcurrentQueue<Collision> GetCollisions()
        {
            ConcurrentQueue<Collision> result = new ConcurrentQueue<Collision>();
            Vector3 cameraPosition = transform.ExtractTranslation();
            Vector3 right = (new Vector4(Right) * transform).Xyz;
            Vector3 up = (new Vector4(Up) * transform).Xyz;
            Vector3 center = ((new Vector4(Forward) * transform).Xyz * viewDistance) + cameraPosition;
            Vector3 bottomLeft = center + (width * -0.5f * right) - (height * 0.5f * up);
            Vector3 nextHorizontalPixel = (width / texture.Width) * right;
            Vector3 nextVerticalPixel = (height / texture.Height) * up;
            bottomLeft += 0.5f * (nextHorizontalPixel + nextVerticalPixel);

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    Vector3 position = bottomLeft + x * nextHorizontalPixel + y * nextVerticalPixel;

                    foreach(var pc in pixelCollisionCalculator.GetInitialCollisions(nextHorizontalPixel, nextVerticalPixel, position, cameraPosition, x, y, material, texture))
                    {
                        result.Enqueue(pc);
                    }
                }
            }

            return result;
        }

        public void SetRenderQueue(ConcurrentQueue<Collision> collisions)
        {
            // empty the current queue
            while (collisionQueue?.TryDequeue(out Collision _) ?? false) ;

            collisionQueue = collisions;
        }

        public void Render()
        {
            bool setChanges = !collisionQueue.IsEmpty;

            Parallel.For(0, maxItems, (index) =>
            {
                if (collisionQueue.TryDequeue(out Collision collision))
                {
                    if (collision.Material.Reflectiveness != 1)
                    {
                        Color4 result = CollisionHelper.FindColourAtCollision(collision);

                        collision.Pixel.Use(result);
                    }

                    if (collision.Depth < 4)
                    {
                        foreach (Ray ray in CollisionHelper.GetResultRays(collision))
                        {
                            if (Collider.TryGetCollision(ray, out IEnumerable<Collision> rayCollisions))
                            {
                                Collision rayCollision = rayCollisions.Min(new CollisionDistanceComparer());
                                rayCollision.Depth = collision.Depth + 1;
                                rayCollision.Pixel = new PixelReference((float)collision.Material.Reflectiveness * collision.Pixel.Contribution, collision.Pixel.X, collision.Pixel.Y, collision.Pixel.Texture);
                                collisionQueue.Enqueue(rayCollision);
                            }
                        }
                    }
                }
            });

            if (setChanges)
            {
                texture.SetChanges();
            }
        }
    }
}
