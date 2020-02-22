using OpenTK;
using OpenTK.Graphics.OpenGL4;
using RayTracer.Interfaces;
using RayTracer.Models;
using RayTracer.Models.Shapes;
using RayTracer.Models.Textures;
using RayTracer.Scene;
using RayTracer.Scene.Lights;
using RayTracer.Scene.Shapes;
using RayTracer.Shaders;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RayTracer.Factories
{
    public static class SceneFactory
    {
        public static Scene.Scene CreateScene(SceneModel model)
        {
            IDictionary<string, Texture> textures = CreateTextures(model);
            IDictionary<string, Material> materials = CreateMaterials(model);
            ICollection<ShapeBase> shapes = CreateShapes(model, materials);
            ICollection<LightBase> lights = CreateLights(model);
            IDictionary<string, Shader> shaders = CreateShaderPrograms(model);
            IDictionary<string, Mesh> meshes = CreateMeshes(model);
            Camera camera = CreateCamera(model, textures, materials);
            camera.Collider = new ColliderCollection(shapes.Cast<ICollider>());

            return new Scene.Scene
            {
                Textures = textures,
                Materials = materials,
                Shapes = shapes,
                Shaders = shaders,
                Meshes = meshes,
                Camera = camera,
                Lights = lights
            };
        }

        private static ICollection<LightBase> CreateLights(SceneModel model)
        {
            List<LightBase> result = new List<LightBase>();
            Console.WriteLine("Creating lights");
            foreach(var light in model.LightSources)
            {
                switch (light.Type)
                {
                    case "Point":
                        result.Add(new PointLight(light.Position, light.Rotation, light.Brightness, light.Colour));
                        break;
                    default:
                        Console.WriteLine($"No conversion is available for a light of type '{light.Type}'");
                        break;
                }
            }

            return result;
        }

        private static Camera CreateCamera(SceneModel model, IDictionary<string, Texture> textures, IDictionary<string, Material> materials)
        {
            Console.WriteLine("Creating camera...");
            CameraModel cameraModel = model.Camera;
            if (!textures.ContainsKey(cameraModel.TextureTarget))
            {
                Console.WriteLine("Unable to create the camera, because the target texture doesn't exist.");
                return null;
            }

            if (!materials.ContainsKey(cameraModel.Material))
            {
                Console.WriteLine($"Unable to create the camera, because the camera material '{cameraModel.Material}' does not exist");
                return null;
            }

            return new Camera(cameraModel.Width, cameraModel.Height, cameraModel.ViewDistance, cameraModel.Position, cameraModel.ViewDirection, textures[cameraModel.TextureTarget], materials[cameraModel.Material]);
        }

        private static IDictionary<string, Mesh> CreateMeshes(SceneModel model)
        {
            Dictionary<string, Mesh> result = new Dictionary<string, Mesh>();
            Console.WriteLine($"Creating meshes...");
            foreach (MeshModel meshModel in model.Meshes)
            {
                Console.WriteLine($"Creating mesh '{meshModel.Name}'");
                if (result.ContainsKey(meshModel.Name))
                {
                    Console.WriteLine($"A mesh with the name '{meshModel.Name}' already exists. Skipping...");
                    continue;
                }

                Mesh mesh = MeshFactory.CreateMesh(meshModel.Vertices, meshModel.Elements, meshModel.Parameters);
                result.Add(meshModel.Name, mesh);
            }

            return result;
        }

        private static IDictionary<string, Texture> CreateTextures(SceneModel model)
        {
            Dictionary<string, Texture> result = new Dictionary<string, Texture>();
            Console.WriteLine("Creating textures...");
            foreach (TextureModel textureModel in model.Textures)
            {
                Console.WriteLine($"Creating texture '{textureModel.Name}'");
                if (result.ContainsKey(textureModel.Name))
                {
                    Console.WriteLine($"A texture with the name '{textureModel.Name}' already exists. Skipping...");
                    continue;
                }

                Texture texture = null;
                switch (textureModel)
                {
                    case ImageTextureModel imageTextureModel:
                        texture = TextureFactory.CreateTexture(imageTextureModel.Path);
                        break;
                    case ColourTextureModel colourTextureModel:
                        texture = TextureFactory.CreateTexture(colourTextureModel.Width, colourTextureModel.Height, colourTextureModel.Colour);
                        break;
                    default:
                        Console.WriteLine($"Texture '{textureModel.Name}' cannot be created, because no conversion for the given texture model has been defined.");
                        break;
                }

                if (texture != null)
                {
                    result.Add(textureModel.Name, texture);
                }
            }

            return result;
        }

        private static IDictionary<string, Material> CreateMaterials(SceneModel model)
        {
            Dictionary<string, Material> result = new Dictionary<string, Material>();
            Console.WriteLine("Creating materials...");
            foreach (MaterialModel material in model.Materials)
            {
                Console.WriteLine($"Creating material '{material.Name}'");
                if (result.ContainsKey(material.Name))
                {
                    Console.WriteLine($"A material with the name '{material.Name}' already exists. Skipping...");
                    continue;
                }

                result.Add(material.Name, new Material
                {
                    Colour = material.Colour,
                    Reflectiveness = material.Reflectiveness,
                    Roughness = material.Roughness
                });
            }

            return result;
        }

        private static ICollection<ShapeBase> CreateShapes(SceneModel model, IDictionary<string, Material> materials)
        {
            List<ShapeBase> shapes = new List<ShapeBase>();

            Console.WriteLine("Creating shapes...");
            foreach (ShapeModel shapemodel in model.Shapes)
            {
                ShapeBase shape;
                switch (shapemodel)
                {
                    case SphereModel sphere:
                        shape = new Sphere
                        {
                            Radius = sphere.Radius
                        };
                        break;
                    case PlaneModel _:
                        shape = new Plane();
                        break;
                    default:
                        continue;
                }

                if (!materials.ContainsKey(shapemodel.MaterialName))
                {
                    Console.WriteLine($"No material with the name '{shapemodel.MaterialName}' was found.");
                    continue;
                }

                shape.Material = materials[shapemodel.MaterialName];
                shape.Transform = Matrix4.CreateFromQuaternion(shapemodel.Rotation) * Matrix4.CreateTranslation(shapemodel.Position);

                shapes.Add(shape);
            }

            return shapes;
        }

        private static IDictionary<string, Shader> CreateShaderPrograms(SceneModel scene)
        {
            Dictionary<string, Shader> result = new Dictionary<string, Shader>();

            IDictionary<string, CompiledShader> shaders = CreateShaders(scene);

            Console.WriteLine("Creating shader programs...");
            foreach (ShaderProgramModel program in scene.ShaderPrograms)
            {
                Console.WriteLine($"Creating shader program '{program.Name}'");
                if (result.ContainsKey(program.Name))
                {
                    Console.WriteLine($"A shader program with the name '{program.Name}' already exists. Skipping...");
                    continue;
                }

                if (!shaders.TryGetValue(program.VertexShader, out CompiledShader vertexShader) || vertexShader.Type != ShaderType.VertexShader)
                {
                    Console.WriteLine($"Could not find the vertexshader for program {program.Name}");
                    continue;
                }

                if (!shaders.TryGetValue(program.FragmentShader, out CompiledShader fragmentShader) || fragmentShader.Type != ShaderType.FragmentShader)
                {
                    Console.WriteLine($"Could not find the fragmentshader for program {program.Name}");
                    continue;
                }

                int programPointer = GL.CreateProgram();

                GL.AttachShader(programPointer, vertexShader.ShaderPointer);
                GL.AttachShader(programPointer, fragmentShader.ShaderPointer);

                GL.LinkProgram(programPointer);

                result.Add(program.Name, new Shader(programPointer));
            }

            DeleteShaders(shaders);

            return result;
        }

        private static IDictionary<string, CompiledShader> CreateShaders(SceneModel scene)
        {
            Dictionary<string, CompiledShader> result = new Dictionary<string, CompiledShader>();

            Console.WriteLine("Creating shaders...");
            foreach (ShaderModel shader in scene.Shaders)
            {
                Console.WriteLine($"Creating shader '{shader.Name}'...");
                if (result.ContainsKey(shader.Name))
                {
                    Console.WriteLine($"Shader '{shader.Name}' has already been defined. Skipping...");
                    continue;
                }

                if (!File.Exists(shader.Source))
                {
                    Console.WriteLine($"Could not find file for shader '{shader.Name}'. Skipping...");
                    continue;
                }

                string source;
                try
                {
                    using (StreamReader sr = new StreamReader(shader.Source))
                    {
                        source = sr.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Attempted to read the source of shader '{shader.Name}', but failed: {e.Message}\n{e.StackTrace}");
                    continue;
                }

                int shaderPointer = GL.CreateShader(shader.Type);
                GL.ShaderSource(shaderPointer, source);
                GL.CompileShader(shaderPointer);
                string compileMessage = GL.GetShaderInfoLog(shaderPointer);
                if (!string.IsNullOrEmpty(compileMessage))
                {
                    Console.WriteLine($"shader {shader.Name} was not created:");
                    Console.WriteLine(compileMessage);
                    GL.DeleteShader(shaderPointer);
                    continue;
                }

                result.Add(shader.Name, new CompiledShader
                {
                    ShaderPointer = shaderPointer,
                    Type = shader.Type
                });
            }

            return result;
        }

        private static void DeleteShaders(IDictionary<string, CompiledShader> shaders)
        {
            foreach (KeyValuePair<string, CompiledShader> shader in shaders)
            {
                GL.DeleteShader(shader.Value.ShaderPointer);
            }
        }

        private class CompiledShader
        {
            public int ShaderPointer { get; set; }
            public ShaderType Type { get; set; }
        }
    }
}
