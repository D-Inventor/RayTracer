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
using RayTracer.Models.Storage;
using RayTracer.Services;
using System;
using System.Drawing;
using System.IO;

namespace RayTracer
{
    internal class App : GameWindow, IGameRunner
    {
        private readonly IRenderService renderService;
        private readonly IConfigurationRoot configuration;
        private readonly ILifetimeScope lifetimeScope;
        private readonly ILogger<App> logger;

        private Scene.Scene scene;
        private readonly string mesh = "RenderMesh";
        private readonly string texture = "RenderTexture";
        private readonly string shader = "DefaultProgram";

        public App(IRenderService renderService, IConfigurationRoot configuration, ILifetimeScope lifetimeScope, ILogger<App> logger)
        {
            this.renderService = renderService;
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

                renderService.Initialise();
            }

            base.OnLoad(e);
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
            renderService.Resize(Width, Height);
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            renderService.RenderScene(scene);
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
    }
}
