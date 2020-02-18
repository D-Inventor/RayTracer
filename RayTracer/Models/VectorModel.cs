using OpenTK;

namespace RayTracer.Models
{
    public class VectorModel
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static implicit operator Vector3(VectorModel model)
        {
            return new Vector3(model.X, model.Y, model.Z);
        }

        public static implicit operator VectorModel(Vector3 vector)
        {
            return new VectorModel
            {
                X = vector.X,
                Y = vector.Y,
                Z = vector.Z
            };
        }
    }
}
