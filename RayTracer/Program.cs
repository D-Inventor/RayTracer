using Microsoft.Extensions.Configuration;
using RayTracer.Builders;

namespace RayTracer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //ClientModel templateClient = new ClientModel
            //{
            //    WindowSize = new Size(50, 60),
            //    Scene = new SceneModel
            //    {
            //        Materials = new MaterialModel[]
            //        {
            //            new MaterialModel
            //            {
            //                Name = "ExampleMaterial",
            //                Colour = Color4.AliceBlue,
            //                Reflectiveness = 0.5,
            //                Roughness = 0.7
            //            }
            //        },
            //        Shapes = new ShapeModel[]
            //        {
            //            new SphereModel
            //            {
            //                MaterialName = "ExampleMaterial",
            //                Position = new Vector3(0.1f, 0.2f, 0.3f),
            //                Radius = 2.0,
            //                Rotation = new Quaternion(0.3f, 0.4f, 0.5f),
            //            },
            //            new PlaneModel
            //            {
            //                MaterialName = "ExampleMaterial",
            //                Position = new Vector3(1.2f, 2.3f, 4.5f),
            //                Rotation = new Quaternion(0.6f, 0.7f, 0.8f),
            //            }
            //        },
            //        Shaders = new ShaderModel[]
            //        {
            //            new ShaderModel
            //            {
            //                Name = "DefaultVertex",
            //                Source = "Shaders/Default.vert",
            //                Type = ShaderType.VertexShader
            //            },
            //            new ShaderModel
            //            {
            //                Name = "DefaultFragment",
            //                Source = "Shaders/Default.frag",
            //                Type = ShaderType.FragmentShader
            //            }
            //        },
            //        ShaderPrograms = new ShaderProgramModel[]
            //        {
            //            new ShaderProgramModel
            //            {
            //                Name = "DefaultProgram",
            //                VertexShader = "DefaultVertex",
            //                FragmentShader = "DefaultFragment"
            //            }
            //        }
            //    }
            //};

            //using (StreamWriter sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "template_model.json")))
            //{
            //    sw.Write(JsonConvert.SerializeObject(templateClient, Formatting.Indented));
            //}

            //return;

            var game = new GameBuilder().Build();

            game.Run();
        }
    }
}
