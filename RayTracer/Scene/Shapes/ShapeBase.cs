﻿using OpenTK;
using RayTracer.Interfaces;
using RayTracer.Models.RayTracer;

namespace RayTracer.Scene.Shapes
{
    public abstract class ShapeBase : ICollider
    {
        public Material Material { get; set; }
        public Matrix4 Transform { get; set; }

        public abstract bool TryGetCollision(Ray ray, out Collision collision);
    }
}
