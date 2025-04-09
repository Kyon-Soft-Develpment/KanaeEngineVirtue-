using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4; 
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace KanaEngine
{
    public class KanaEngine : GameWindow
    {
        private List<GameObject> _gameObjects = new List<GameObject>();
        private Shader _defaultShader; 
        private Material _defaultMaterial;

        public KanaEngine(int width, int height, string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            Size = new Vector2i(width, height);
            Title = title;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            _defaultShader = new Shader("Shaders/basic.vert", "Shaders/basic.frag");
            _defaultMaterial = new Material(_defaultShader); 
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var gameObject in _gameObjects)
            {
                gameObject.Draw(_defaultMaterial);
            }

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var input = KeyboardState;
            if (input.IsKeyDown(OpenTK.Windowing.Common.Key.Escape))
            {
                Close();
            }
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update(args.Time);
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            _defaultShader.Dispose();
        }

        public void AddGameObject(GameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        static void Main(string[] args)
        {
            using (var game = new KanaEngine(800, 600, "Kana Engine"))
            {
                game.Run();
            }
        }
    }
}