using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using RayTracer.Extensions;
using RayTracer.Logging;

namespace RayTracer.Services
{
    public interface IRenderService
    {
        void Initialise();
        void RenderScene(Scene.Scene scene);
        void Resize(int width, int height);
    }

    public class RenderService : IRenderService
    {
        private readonly string mesh = "RenderMesh";
        private readonly string texture = "RenderTexture";
        private readonly string shader = "DefaultProgram";
        private readonly ILogger<RenderService> logger;

        public RenderService(ILogger<RenderService> logger)
        {
            this.logger = logger;
        }

        public void Initialise()
        {

            LogDeviceInformation();
        }

        public void Resize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }

        public void RenderScene(Scene.Scene scene)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            scene.Shaders[shader].Use();
            scene.Textures[texture].Use();

            scene.Meshes[mesh].Draw();
        }

        private void LogDeviceInformation()
        {
            string OpenGLContext = GL.GetString(StringName.Vendor);
            string graphicsCard = GL.GetString(StringName.Renderer);
            string version = GL.GetString(StringName.Version);
            string slversion = GL.GetString(StringName.ShadingLanguageVersion);
            logger.LogInfo($"Using '{graphicsCard}' from '{OpenGLContext}' version '{version}'");
            logger.LogInfo($"Supports shader language version {slversion}");
        }
    }
}
