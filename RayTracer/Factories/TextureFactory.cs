using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using RayTracer.Extensions;
using RayTracer.Logging;
using RayTracer.Scene;

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace RayTracer.Factories
{
    public interface ITextureFactory
    {
        Texture CreateTexture(int width, int height, Color4 colour);
        Texture CreateTexture(string file);
    }

    public class TextureFactory : ITextureFactory
    {
        private readonly ILogger<TextureFactory> logger;

        public TextureFactory(ILogger<TextureFactory> logger)
        {
            this.logger = logger;
        }

        public Texture CreateTexture(string file)
        {
            Bitmap bmp = new Bitmap(file);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            int width = bmp.Width;
            int height = bmp.Height;
            int[] pixels = new int[width * height];
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixels, 0, width * height);
            bmp.UnlockBits(data);

            int texturePointer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texturePointer);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new float[] { 1.0f, 1.0f, 0.0f, 1.0f });
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
            try
            {
                return new Texture(texturePointer, width, height, pixels);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, $"Attempted to create a texture, but failed.");
                GL.DeleteTexture(texturePointer);

                return null;
            }
        }

        public Texture CreateTexture(int width, int height, Color4 colour)
        {
            int[] pixels = new int[width * height];

            int texturePointer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texturePointer);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new float[] { 1.0f, 1.0f, 0.0f, 1.0f });
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, pixels);

            try
            {
                Texture texture = new Texture(texturePointer, width, height, pixels);
                texture.Clear(colour);
                texture.SetChanges();
                return texture;
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, $"Attempted to create a texture, but failed.");
                GL.DeleteTexture(texturePointer);

                return null;
            }
        }
    }
}
