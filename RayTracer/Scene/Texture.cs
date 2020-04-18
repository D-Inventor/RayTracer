using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using System;
using System.Drawing;

namespace RayTracer.Scene
{
    public class Texture : IDisposable
    {
        private readonly int texturePointer;
        private readonly int[] pixels;

        public int Width { get; }
        public int Height { get; }

        public Texture(int texturePointer, int width, int height, int[] pixels)
        {
            if (pixels.Length != width * height) { throw new ArgumentException("Width and Height do not correspond with the amount of given pixels"); }

            this.texturePointer = texturePointer;
            Width = width;
            Height = height;
            this.pixels = pixels;
        }

        public int this[int x, int y]
        {
            get => pixels[x + y * Width];
            set => pixels[x + y * Width] = value;
        }

        public Color4 this[float u, float v]
        {
            get
            {
                var un = (int)(((u % 1) + 1) % 1 * Width);
                var vn = (int)(((v % 1) + 1) % 1 * Height);

                var pixel = this[un, vn];

                byte[] bytes = BitConverter.GetBytes(pixel);
                return new Color4(bytes[2], bytes[1], bytes[0], bytes[3]);
            }
        }

        public void Clear(Color4 colour)
        {
            for (int i = 0; i < pixels.Length; i++) pixels[i] = colour.ToArgb();
        }

        public void SetChanges()
        {
            GL.BindTexture(TextureTarget.Texture2D, texturePointer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
        }

        public void Use(TextureUnit textureUnit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, texturePointer);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteTexture(texturePointer);

                disposedValue = true;
            }
        }

        ~Texture()
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
