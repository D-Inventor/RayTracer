using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using RayTracer.Factories;
using RayTracer.Interfaces;
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
        private readonly ILogger<App> logger;
        private readonly IServiceProvider services;

        public App(IConfigurationRoot configuration,
                   ILogger<App> logger,
                   IServiceProvider services)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.services = services;
        }

        new public void Run()
        {
            double fps = double.Parse(configuration["fps"]);
            double ups = double.Parse(configuration["ups"]);

            Title = configuration["window:title"];
            ClientSize = new Size(int.Parse(configuration["window:width"]), int.Parse(configuration["window:height"]));

            Run(ups, fps);
        }

        protected override void OnLoad(EventArgs e)
        {
            LogDeviceInformation();

            ClientModel client;
            using (StreamReader sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Json", "model.json")))
            {
                client = JsonConvert.DeserializeObject<ClientModel>(sr.ReadToEnd());
            }

            scene = SceneFactory.CreateScene(client.Scene);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            base.OnLoad(e);
        }

        private void LogDeviceInformation()
        {
            string OpenGLContext = GL.GetString(StringName.Vendor);
            string graphicsCard = GL.GetString(StringName.Renderer);
            string version = GL.GetString(StringName.Version);
            string slversion = GL.GetString(StringName.ShadingLanguageVersion);
            logger.LogInformation($"Using '{graphicsCard}' from '{OpenGLContext}' version '{version}'");
            logger.LogInformation($"Supports shader language version {slversion}");
            //Console.WriteLine($"Using '{graphicsCard}' from '{OpenGLContext}' version '{version}'");
            //Console.WriteLine($"Supports shader language version {slversion}");
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
