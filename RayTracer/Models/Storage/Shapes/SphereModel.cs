namespace RayTracer.Models.Storage.Shapes
{
    public class SphereModel : ShapeModel
    {
        public override string ShapeType { get => "Sphere"; set { } }

        public double Radius { get; set; }
    }
}
