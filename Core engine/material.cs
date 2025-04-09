namespace KanaEngine
{
    public class Material
    {
        public Shader Shader { get; private set; }
        // Add properties for different material parameters (e.g., Color, Texture)
        // For now, we'll just associate a shader

        public Material(Shader shader)
        {
            Shader = shader;
        }

        public void Use()
        {
            Shader.Use();
        }

        public void Unuse()
        {
            Shader.Unuse();
        }

        public void SetInt(string name, int value) => Shader.SetInt(name, value);
        public void SetFloat(string name, float value) => Shader.SetFloat(name, value);
        public void SetMatrix4(string name, OpenTK.Mathematics.Matrix4 matrix) => Shader.SetMatrix4(name, matrix);
        // Add more Set methods for other uniform types (Vector3, Texture, etc.)
    }
}