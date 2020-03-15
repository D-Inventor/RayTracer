using Autofac;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using RayTracer.Extensions;
using RayTracer.Factories;
using RayTracer.Interfaces;
using RayTracer.Logging;
using RayTracer.Models;

using System;
using System.Drawing;
using System.IO;

namespace RayTracer
{
    internal class App : GameWindow, IGameRunner
    {
        private Scene.Scene scene;

        private readonly string mesh = "RenderMesh";
        private readonly string texture = "RenderTexture";
        private readonly string shader = "DefaultProgram";
        private readonly IConfigurationRoot configuration;
        private readonly ILifetimeScope lifetimeScope;
        private readonly ILogger<App> logger;

        public App(IConfigurationRoot configuration, ILifetimeScope lifetimeScope, ILogger<App> logger)
        {
            this.configuration = configuration;
            this.lifetimeScope = lifetimeScope;
            this.logger = logger;
        }

        public new void Run()
        {
            double fps = double.Parse(configuration["fps"]);
            double ups = double.Parse(configuration["ups"]);

            Title = configuration["window:title"];
            ClientSize = new Size(int.Parse(configuration["window:width"]), int.Parse(configuration["window:height"]));

            Run(ups, fps);
        }

        protected override void OnLoad(EventArgs e)
        {
            using (ILifetimeScope scope = lifetimeScope.BeginLifetimeScope())
            {
                ISceneFactory sceneFactory = scope.Resolve<ISceneFactory>();

                LogDeviceInformation();

                ClientModel client;
                using (StreamReader sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Json", "model.json")))
                {
                    client = JsonConvert.DeserializeObject<ClientModel>(sr.ReadToEnd());
                }

                scene = sceneFactory.CreateScene(client.Scene);

                GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
                GL.Enable(EnableCap.Texture2D);
                GL.Disable(EnableCap.DepthTest);
                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            }

            base.OnLoad(e);
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

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            scene.Dispose();
            base.OnUnload(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            scene.Camera.Render();

            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            scene.Shaders[shader].Use();
            scene.Textures[texture].Use();

            scene.Meshes[mesh].Draw();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}
