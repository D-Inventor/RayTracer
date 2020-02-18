using OpenTK.Graphics.OpenGL4;

using RayTracer.Scene;

using System.Collections.Generic;

namespace RayTracer.Factories
{
    public static class MeshFactory
    {
        public static Mesh CreateMesh(float[] data, uint[] elements, IDictionary<string, int> parameters)
        {
            int vertexBuffer = GL.GenBuffer(),
                elementBuffer = GL.GenBuffer(),
                vertexArray = GL.GenVertexArray();

            Mesh mesh = new Mesh(vertexBuffer, elementBuffer, vertexArray, data, elements, parameters);
            mesh.WriteMesh();

            return mesh;
        }
    }
}
