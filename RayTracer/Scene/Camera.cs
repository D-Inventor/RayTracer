using OpenTK;
using OpenTK.Graphics;
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
        private Matrix4 transform;
        private readonly Texture texture;
        private readonly Material material;
        int maxItems = 50000;

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

            transform = Matrix4.CreateTranslation(position) *
                        Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(pitch, yaw, 0));
        }

        public ICollider Collider { get; set; }
        public Scene Scene { get; set; }

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

                    Collision collision = new Collision
                    {
                        Depth = 0,
                        InDirection = cameraPosition - position,
                        Normal = position - cameraPosition,
                        Material = material,
                        Position = position,
                        Pixel = new PixelReference(1.0f, x, y, texture)
                    };

                    result.Enqueue(collision);
                }
            }

            return result;
        }

        public void SetRenderQueue(ConcurrentQueue<Collision> collisions)
        {
            // empty the current queue
            while (collisionQueue?.TryDequeue(out var _) ?? false) ;

            collisionQueue = collisions;
        }

        public void Render()
        {
            bool setChanges = !collisionQueue.IsEmpty;
            bool improveTiming = setChanges;

            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Parallel.For(0, maxItems, (index) =>
            for (int index = 0; index < maxItems; index++)
            {
                if (collisionQueue.TryDequeue(out var collision))
                {
                    UseColour(collision);

                    Ray ray = new Ray
                    {
                        Direction = collision.GetReflection(),
                        Position  = collision.Position
                    };

                    if (collision.Depth < 1 && Collider.TryGetCollision(ray, out var reflectCollision))
                    {
                        reflectCollision.Depth = collision.Depth + 1;
                        reflectCollision.Pixel = new PixelReference((float)collision.Material.Reflectiveness * collision.Pixel.Contribution, collision.Pixel.X, collision.Pixel.Y, collision.Pixel.Texture);
                        collisionQueue.Enqueue(reflectCollision);
                    }
                }
                else
                {
                    improveTiming = false;
                }
                //});
            }
            timer.Stop();

            if (setChanges)
            {
                texture.SetChanges();
            }

            //if (improveTiming)
            //{
            //    var target = (1000f / 80f);
            //    maxItems = Math.Max((int)Math.Round(maxItems * (target / timer.ElapsedMilliseconds)), 200);
            //    Console.WriteLine(maxItems);
            //}
        }

        private void UseColour(Collision collision)
        {
            if (collision.Material.Reflectiveness != 1)
            {
                Color4 result = Color4.Black;
                foreach(var light in Scene.Lights.Where(l => Vector3.Dot(l.Position - collision.Position, collision.Normal) > 0))
                {
                    var lightdirection = light.Position - collision.Position;
                    if(Vector3.Dot(lightdirection, collision.Normal) <= 0)
                    {
                        continue;
                    }
                    lightdirection.Normalize();
                    var effectiveness = Vector3.Dot(lightdirection, collision.Normal);

                    var brightness = light.GetBrightnessAtPosition(collision.Position);
                    var collisionColour = collision.Material.Colour;
                    result.R = (float)MathHelper.Clamp(collisionColour.R * brightness * effectiveness + result.R, 0, 1);
                    result.G += (float)MathHelper.Clamp(collisionColour.G * brightness * effectiveness + result.G, 0, 1);
                    result.B += (float)MathHelper.Clamp(collisionColour.B * brightness * effectiveness + result.B, 0, 1);
                }

                result.A = (float)(1 - collision.Material.Reflectiveness);
                collision.Pixel.Use(result);
            }
        }
    }
}
