using OpenTK.Mathematics;

namespace KanaEngine
{
    public class Transform
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;

        public Matrix4 ModelMatrix
        {
            get
            {
                Matrix4 translationMatrix = Matrix4.CreateTranslation(Position);
                Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(Rotation);
                Matrix4 scaleMatrix = Matrix4.CreateScale(Scale);
                return scaleMatrix * rotationMatrix * translationMatrix;
            }
        }
    }
}