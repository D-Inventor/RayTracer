using OpenTK.Graphics.OpenGL4;

using System;
using System.IO;

namespace RayTracer.Shaders
{
    public class Shader : IDisposable
    {
        private readonly int program;

        public Shader(string vertexPath, string fragmentPath)
        {
            // read shaders from source
            string vertexSource, fragmentSource;
            int vertexShader, fragmentShader;
            using (StreamReader sr = new StreamReader(vertexPath))
            {
                vertexSource = sr.ReadToEnd();
            }
            using (StreamReader sr = new StreamReader(fragmentPath))
            {
                fragmentSource = sr.ReadToEnd();
            }

            // bind source to shaders
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);

            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);

            // compile shaders
            GL.CompileShader(vertexShader);
            string infoLog = GL.GetShaderInfoLog(vertexShader);
            if (!string.IsNullOrEmpty(infoLog))
            {
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(fragmentShader);
            infoLog = GL.GetShaderInfoLog(fragmentShader);
            if (!string.IsNullOrEmpty(infoLog))
            {
                Console.WriteLine(infoLog);
            }

            // bind shaders to program
            program = GL.CreateProgram();

            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);

            GL.LinkProgram(program);

            // individual shaders are no longer required. Delete them
            GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public Shader(int programPointer)
        {
            program = programPointer;
        }

        public void Use()
        {
            GL.UseProgram(program);
        }
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(program, attribName);
        }

        #region IDisposable implementation
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(program);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
