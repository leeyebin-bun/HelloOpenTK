using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HelloOpenTK
{
	public class MyGameWindow : GameWindow
	{
        public MyGameWindow(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }


        Vertex[] vertices = {
            new Vertex(-0.5f, -0.5f, 0.0f), //Bottom-left vertex
            new Vertex(0.5f, -0.5f, 0.0f), //Bottom-right vertex
            new Vertex(-0.5f,  0.5f, 0.0f),  //Top-left vertex
        };

        Vertex[] vertices_2 = {
            new Vertex(-0.5f, 0.5f, 0.0f), //top-left vertex
            new Vertex(0.5f, 0.5f, 0.0f), //top-right vertex
            new Vertex(0.5f, -0.5f, 0.0f), //bottom-right vertex
        };
        

        Shader shader;

        Triangle t1;
        Triangle t2;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);//Code goes here

            t1 = new Triangle(vertices[0], vertices[1], vertices[2]);
            t2 = new Triangle(vertices_2[0], vertices_2[1], vertices_2[2]);

            ///////////// SHADER
            string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string vertexPath = Path.Join(ProjectDirectory, "Vertex.glsl").ToString();
            string fragmentPath = Path.Join(ProjectDirectory, "Fragment.glsl").ToString();

            shader = new Shader(vertexPath, fragmentPath);

            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);//Code goes here.

            shader.Use();

            t1.Draw();
            t2.Draw();

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

    }
}

