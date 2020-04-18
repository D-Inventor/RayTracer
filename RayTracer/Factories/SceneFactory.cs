using OpenTK;
using OpenTK.Graphics.OpenGL4;

using RayTracer.Extensions;
using RayTracer.Helpers;
using RayTracer.Interfaces;
using RayTracer.Logging;
using RayTracer.Models.Storage;
using RayTracer.Models.Storage.Lights;
using RayTracer.Models.Storage.Shapes;
using RayTracer.Models.Storage.Textures;
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
    public interface ISceneFactory
    {
        Scene.Scene CreateScene(SceneModel model);
    }

    public class SceneFactory : ISceneFactory
    {
        private readonly IMeshFactory meshFactory;
        private readonly ITextureFactory textureFactory;
        private readonly ILogger<SceneFactory> logger;

        public SceneFactory(IMeshFactory meshFactory, ITextureFactory textureFactory, ILogger<SceneFactory> logger)
        {
            this.meshFactory = meshFactory;
            this.textureFactory = textureFactory;
            this.logger = logger;
        }

        public Scene.Scene CreateScene(SceneModel model)
        {
            IDictionary<string, Texture> textures = CreateTextures(model);
            IDictionary<string, Material> materials = CreateMaterials(model, textures);
            ICollection<ShapeBase> shapes = CreateShapes(model, materials);
            ICollection<LightBase> lights = CreateLights(model);
            IDictionary<string, Shader> shaders = CreateShaderPrograms(model);
            IDictionary<string, Mesh> meshes = CreateMeshes(model);
            Camera camera = CreateCamera(model, textures, materials);

            Scene.Scene result = new Scene.Scene
            {
                Textures = textures,
                Materials = materials,
                Shapes = shapes,
                Shaders = shaders,
                Meshes = meshes,
                Camera = camera,
                Lights = lights
            };

            result.Camera.Collider = new ColliderCollection(result.Shapes.Cast<ICollider>());
            result.Camera.CollisionHelper = new CollisionHelper(result, result.Camera.Collider);
            result.Camera.SetRenderQueue(result.Camera.GetCollisions());

            return result;
        }

        private ICollection<LightBase> CreateLights(SceneModel model)
        {
            List<LightBase> result = new List<LightBase>();
            logger.LogDebug("Creating lights");
            foreach (LightModel light in model.LightSources)
            {
                switch (light.Type)
                {
                    case "Point":
                        result.Add(new PointLight(light.Position, light.Rotation, light.Brightness, light.Colour));
                        break;
                    default:
                        logger.LogDebug($"No conversion is available for a light of type '{light.Type}'");
                        break;
                }
            }

            return result;
        }

        private Camera CreateCamera(SceneModel model, IDictionary<string, Texture> textures, IDictionary<string, Material> materials)
        {
            logger.LogDebug("Creating camera...");
            CameraModel cameraModel = model.Camera;
            if (!textures.ContainsKey(cameraModel.TextureTarget))
            {
                logger.LogDebug("Unable to create the camera, because the target texture doesn't exist.");
                return null;
            }

            if (!materials.ContainsKey(cameraModel.Material))
            {
                logger.LogDebug($"Unable to create the camera, because the camera material '{cameraModel.Material}' does not exist");
                return null;
            }

            return new Camera(cameraModel.Width, cameraModel.Height, cameraModel.ViewDistance, cameraModel.Position, cameraModel.ViewDirection, textures[cameraModel.TextureTarget], materials[cameraModel.Material]);
        }

        private IDictionary<string, Mesh> CreateMeshes(SceneModel model)
        {
            Dictionary<string, Mesh> result = new Dictionary<string, Mesh>();
            logger.LogDebug($"Creating meshes...");
            foreach (MeshModel meshModel in model.Meshes)
            {
                logger.LogDebug($"Creating mesh '{meshModel.Name}'");
                if (result.ContainsKey(meshModel.Name))
                {
                    logger.LogDebug($"A mesh with the name '{meshModel.Name}' already exists. Skipping...");
                    continue;
                }

                Mesh mesh = meshFactory.CreateMesh(meshModel.Vertices, meshModel.Elements, meshModel.Parameters);
                result.Add(meshModel.Name, mesh);
            }

            return result;
        }

        private IDictionary<string, Texture> CreateTextures(SceneModel model)
        {
            Dictionary<string, Texture> result = new Dictionary<string, Texture>();
            logger.LogDebug("Creating textures...");
            foreach (TextureModel textureModel in model.Textures)
            {
                logger.LogDebug($"Creating texture '{textureModel.Name}'");
                if (result.ContainsKey(textureModel.Name))
                {
                    logger.LogDebug($"A texture with the name '{textureModel.Name}' already exists. Skipping...");
                    continue;
                }

                Texture texture = null;
                switch (textureModel)
                {
                    case ImageTextureModel imageTextureModel:
                        texture = textureFactory.CreateTexture(imageTextureModel.Path);
                        break;
                    case ColourTextureModel colourTextureModel:
                        texture = textureFactory.CreateTexture(colourTextureModel.Width, colourTextureModel.Height, colourTextureModel.Colour);
                        break;
                    default:
                        logger.LogDebug($"Texture '{textureModel.Name}' cannot be created, because no conversion for the given texture model has been defined.");
                        break;
                }

                if (texture != null)
                {
                    result.Add(textureModel.Name, texture);
                }
            }

            return result;
        }

        private IDictionary<string, Material> CreateMaterials(SceneModel model, IDictionary<string, Texture> textures)
        {
            Dictionary<string, Material> result = new Dictionary<string, Material>();
            logger.LogDebug("Creating materials...");
            foreach (MaterialModel material in model.Materials)
            {
                logger.LogDebug($"Creating material '{material.Name}'");
                if (result.ContainsKey(material.Name))
                {
                    logger.LogDebug($"A material with the name '{material.Name}' already exists. Skipping...");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(material.Texture) || !textures.ContainsKey(material.Texture))
                {
                    logger.LogWarning($"Texture with name '{material.Texture}' does not exist. Skipping...");
                    continue;
                }

                result.Add(material.Name, new Material
                {
                    Colour = material.Colour,
                    Reflectiveness = material.Reflectiveness,
                    Roughness = material.Roughness,
                    Texture = textures[material.Texture]
                });
            }

            return result;
        }

        private ICollection<ShapeBase> CreateShapes(SceneModel model, IDictionary<string, Material> materials)
        {
            List<ShapeBase> shapes = new List<ShapeBase>();

            logger.LogDebug("Creating shapes...");
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
                    logger.LogDebug($"No material with the name '{shapemodel.MaterialName}' was found.");
                    continue;
                }

                shape.Material = materials[shapemodel.MaterialName];
                shape.Transform = Matrix4.CreateFromQuaternion(shapemodel.Rotation) * Matrix4.CreateTranslation(shapemodel.Position);

                shapes.Add(shape);
            }

            return shapes;
        }

        private IDictionary<string, Shader> CreateShaderPrograms(SceneModel scene)
        {
            Dictionary<string, Shader> result = new Dictionary<string, Shader>();

            IDictionary<string, CompiledShader> shaders = CreateShaders(scene);

            logger.LogDebug("Creating shader programs...");
            foreach (ShaderProgramModel program in scene.ShaderPrograms)
            {
                logger.LogDebug($"Creating shader program '{program.Name}'");
                if (result.ContainsKey(program.Name))
                {
                    logger.LogDebug($"A shader program with the name '{program.Name}' already exists. Skipping...");
                    continue;
                }

                if (!shaders.TryGetValue(program.VertexShader, out CompiledShader vertexShader) || vertexShader.Type != ShaderType.VertexShader)
                {
                    logger.LogDebug($"Could not find the vertexshader for program {program.Name}");
                    continue;
                }

                if (!shaders.TryGetValue(program.FragmentShader, out CompiledShader fragmentShader) || fragmentShader.Type != ShaderType.FragmentShader)
                {
                    logger.LogDebug($"Could not find the fragmentshader for program {program.Name}");
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

        private IDictionary<string, CompiledShader> CreateShaders(SceneModel scene)
        {
            Dictionary<string, CompiledShader> result = new Dictionary<string, CompiledShader>();

            logger.LogDebug("Creating shaders...");
            foreach (ShaderModel shader in scene.Shaders)
            {
                logger.LogDebug($"Creating shader '{shader.Name}'...");
                if (result.ContainsKey(shader.Name))
                {
                    logger.LogDebug($"Shader '{shader.Name}' has already been defined. Skipping...");
                    continue;
                }

                if (!File.Exists(shader.Source))
                {
                    logger.LogDebug($"Could not find file for shader '{shader.Name}'. Skipping...");
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
                    logger.LogDebug($"Attempted to read the source of shader '{shader.Name}', but failed: {e.Message}\n{e.StackTrace}");
                    continue;
                }

                int shaderPointer = GL.CreateShader(shader.Type);
                GL.ShaderSource(shaderPointer, source);
                GL.CompileShader(shaderPointer);
                string compileMessage = GL.GetShaderInfoLog(shaderPointer);
                if (!string.IsNullOrEmpty(compileMessage))
                {
                    logger.LogDebug($"shader {shader.Name} was not created:");
                    logger.LogDebug(compileMessage);
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

        private void DeleteShaders(IDictionary<string, CompiledShader> shaders)
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
