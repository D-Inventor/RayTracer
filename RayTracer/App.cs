using Newtonsoft.Json;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

using RayTracer.Factories;
using RayTracer.Models;

using System;
using System.IO;

namespace RayTracer
{
    internal class App : GameWindow
    {
        private Scene.Scene scene;

        private string mesh = "RenderMesh";
        private string texture = "RenderTexture";
        private string shader = "DefaultProgram";

        protected override void OnLoad(EventArgs e)
        {
            LogDeviceInformation();

            ClientModel client;
            using (StreamReader sr = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Json", "model.json")))
            {
                client = JsonConvert.DeserializeObject<ClientModel>(sr.ReadToEnd());
            }

            ClientSize = client.WindowSize;

            scene = SceneFactory.CreateScene(client.Scene);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.DepthTest);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            scene.Camera.SetRenderQueue(scene.Camera.GetCollisions());
            scene.Camera.Render();

            base.OnLoad(e);
        }

        private static void LogDeviceInformation()
        {
            string OpenGLContext = GL.GetString(StringName.Vendor);
            string graphicsCard = GL.GetString(StringName.Renderer);
            string version = GL.GetString(StringName.Version);
            string slversion = GL.GetString(StringName.ShadingLanguageVersion);
            Console.WriteLine($"Using '{graphicsCard}' from '{OpenGLContext}' version '{version}'");
            Console.WriteLine($"Supports shader language version {slversion}");
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
