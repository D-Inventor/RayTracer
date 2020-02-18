using RayTracer.Scene.Lights;
using RayTracer.Scene.Shapes;
using RayTracer.Shaders;

using System;
using System.Collections.Generic;

namespace RayTracer.Scene
{
    public class Scene : IDisposable
    {
        public IDictionary<string, Material> Materials { get; set; }
        public ICollection<ShapeBase> Shapes { get; set; }
        public IDictionary<string, Shader> Shaders { get; set; }
        public IDictionary<string, Texture> Textures { get; set; }
        public IDictionary<string, Mesh> Meshes { get; set; }
        public Camera Camera { get; set; }
        public ICollection<LightBase> Lights { get; set; }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                foreach (KeyValuePair<string, Shader> shader in Shaders)
                {
                    shader.Value.Dispose();
                }

                foreach (KeyValuePair<string, Texture> texture in Textures)
                {
                    texture.Value.Dispose();
                }

                foreach (KeyValuePair<string, Mesh> mesh in Meshes)
                {
                    mesh.Value.Dispose();
                }

                disposedValue = true;
            }
        }

        ~Scene()
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
