using OpenTK.Mathematics;

namespace KanaEngine
{
    public class GameObject
    {
        public Mesh Mesh { get; set; }
        public Transform Transform { get; set; }

        public GameObject(Mesh mesh)
        {
            Mesh = mesh;
            Transform = new Transform();
        }

        public virtual void Update(double deltaTime)
        {
            Transform.Update(deltaTime);
        }

        public virtual void Draw(Material material)
        {
            material.Use();
            material.SetMatrix4("model", Transform.ModelMatrix);
            Mesh.Draw();
            material.Unuse();
        }
    }
}