using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace KanaEngine
{
    public class Shader
    {
        public int Handle { get; private set; }

        public Shader(string vertexPath, string fragmentPath)
        {
            int vertexShader = CompileShader(vertexPath, ShaderType.VertexShader);
            int fragmentShader = CompileShader(fragmentPath, ShaderType.FragmentShader);

            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            string infoLog = GL.GetProgramInfoLog(Handle);
            if (!string.IsNullOrEmpty(infoLog))
            {
                Console.WriteLine($"Shader Program Linking Error: {infoLog}");
            }
        }

        private int CompileShader(string path, ShaderType type)
        {
            string source = File.ReadAllText(path);
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            string infoLog = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(infoLog))
            {
                Console.WriteLine($"Shader Compilation Error ({type}): {infoLog}");
            }

            return shader;
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void Unuse()
        {
            GL.UseProgram(0);
        }

        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            if (location != -1)
            {
                GL.Uniform1(location, value);
            }
        }

        public void SetFloat(string name, float value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            if (location != -1)
            {
                GL.Uniform1(location, value);
            }
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(Handle, name);
            if (location != -1)
            {
                GL.UniformMatrix4(location, false, ref matrix);
            }
        }

        public void Dispose()
        {
            GL.DeleteProgram(Handle);
        }
    }

private Shader _ssrShader;

protected override void OnLoad()
{
    base.OnLoad();

    _ssrShader = new Shader("Assets/Shaders/fullscreen.vert", "Assets/Shaders/ssr.frag");
}

protected override void OnRenderFrame(FrameEventArgs args)
{
    base.OnRenderFrame(args);

    _ssrShader.Use();
    GL.ActiveTexture(TextureUnit.Texture0);
    GL.BindTexture(TextureTarget.Texture2D, colorTextureId); 
    _ssrShader.SetInt("colorBuffer", 0);

    GL.ActiveTexture(TextureUnit.Texture1);
    GL.BindTexture(TextureTarget.Texture2D, depthTextureId); 
    _ssrShader.SetInt("depthBuffer", 1);

    GL.ActiveTexture(TextureUnit.Texture2);
    GL.BindTexture(TextureTarget.Texture2D, normalTextureId);
    _ssrShader.SetInt("normalBuffer", 2);

    Matrix4 projectionInverse = camera.ProjectionMatrix.Inverted(); 
    Matrix4 viewInverse = camera.ViewMatrix.Inverted();
    _ssrShader.SetMatrix4("projectionInverse", projectionInverse);
    _ssrShader.SetMatrix4("viewInverse", viewInverse);

   
    _ssrShader.SetVector3("cameraPosition", camera.Position); 
    _ssrShader.SetFloat("stepSize", 0.1f); 
    _ssrShader.SetInt("maxSteps", 50);   
    _ssrShader.SetFloat("depthThreshold", 0.01f);
    _ssrShader.SetFloat("materialReflectionFactor", 0.5f); 
    _fullscreenQuadMesh.Draw();
    _ssrShader.Unuse();



    SwapBuffers();
}

protected override void OnUnload()
{
    base.OnUnload();
    _ssrShader.Dispose();

}
}