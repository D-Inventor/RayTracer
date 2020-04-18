using RayTracer.Models.Storage.Lights;
using RayTracer.Models.Storage.Shapes;
using RayTracer.Models.Storage.Textures;

namespace RayTracer.Models.Storage
{
    public class SceneModel
    {
        public MaterialModel[] Materials { get; set; }
        public ShaderModel[] Shaders { get; set; }
        public ShaderProgramModel[] ShaderPrograms { get; set; }
        public ShapeModel[] Shapes { get; set; }
        public TextureModel[] Textures { get; set; }
        public MeshModel[] Meshes { get; set; }
        public CameraModel Camera { get; set; }
        public LightModel[] LightSources { get; set; }
    }
}
