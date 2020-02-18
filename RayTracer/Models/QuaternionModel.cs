using OpenTK;

namespace RayTracer.Models
{
    public class QuaternionModel
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public static implicit operator Quaternion(QuaternionModel model)
        {
            return new Quaternion(model.X, model.Y, model.Z, model.W);
        }

        public static implicit operator QuaternionModel(Quaternion quaternion)
        {
            return new QuaternionModel
            {
                X = quaternion.X,
                Y = quaternion.Y,
                Z = quaternion.Z,
                W = quaternion.W
            };
        }
    }
}
