using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Scene
{
    public class Mesh : IDisposable
    {
        private readonly int vertexBuffer;
        private readonly int vertexArray;
        private readonly int elementBuffer;

        private float[] data;
        private uint[] elements;

        private IDictionary<string, int> parameters;

        public Mesh(int vertexBuffer, int elementBuffer, int vertexArray, float[] data, uint[] elements, IDictionary<string, int> parameters)
        {
            this.vertexBuffer = vertexBuffer;
            this.elementBuffer = elementBuffer;
            this.vertexArray = vertexArray;
            this.data = data;
            this.elements = elements;
            this.parameters = parameters;
        }

        public void WriteMesh()
        {
            GL.BindVertexArray(vertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, elements.Length * sizeof(uint), elements, BufferUsageHint.StaticDraw);

            int index = 0,
                offset = 0,
                size = parameters.Values.Sum() * sizeof(float);
            foreach (KeyValuePair<string, int> kvp in parameters.OrderBy(x => x.Key))
            {
                GL.VertexAttribPointer(index, kvp.Value, VertexAttribPointerType.Float, false, size, offset);
                GL.EnableVertexAttribArray(index);

                offset += kvp.Value * sizeof(float);
                index++;
            }
        }

        public void Draw()
        {
            GL.BindVertexArray(vertexArray);
            GL.DrawElements(PrimitiveType.Triangles, elements.Length, DrawElementsType.UnsignedInt, 0);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteBuffer(vertexBuffer);
                GL.DeleteBuffer(elementBuffer);
                GL.DeleteVertexArray(vertexArray);

                disposedValue = true;
            }
        }

        ~Mesh()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
