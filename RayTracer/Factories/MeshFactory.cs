using OpenTK.Graphics.OpenGL4;

using RayTracer.Scene;

using System.Collections.Generic;

namespace RayTracer.Factories
{
    public interface IMeshFactory
    {
        Mesh CreateMesh(float[] data, uint[] elements, IDictionary<string, int> parameters);
    }

    public class MeshFactory : IMeshFactory
    {
        public MeshFactory()
        { }

        public Mesh CreateMesh(float[] data, uint[] elements, IDictionary<string, int> parameters)
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
