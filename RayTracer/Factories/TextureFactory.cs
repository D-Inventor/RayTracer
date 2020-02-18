using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using RayTracer.Scene;

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace RayTracer.Factories
{
    public static class TextureFactory
    {
        public static Texture CreateTexture(string file)
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
                Console.WriteLine($"Attempted to create a texture, but failed: {ex.Message}\n{ex.StackTrace}");
                GL.DeleteTexture(texturePointer);

                return null;
            }
        }

        public static Texture CreateTexture(int width, int height, Color4 colour)
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
                Console.WriteLine($"Attempted to create a texture, but failed: {ex.Message}\n{ex.StackTrace}");
                GL.DeleteTexture(texturePointer);

                return null;
            }
        }
    }
}
