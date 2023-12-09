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

        
        float[] vertices = {
              0.0f,  0.5f, 0.0f, //Bottom-left vertex
              0.0f,  0.0f, 0.0f, //Bottom-right vertex
              0.5f,  0.0f, 0.0f  //Top vertex
              };
        
        float[] vertices_2 = {
              0.0f,  0.5f, 0.0f, //Bottom-left vertex
              0.5f,  0.5f, 0.0f, //Bottom-right vertex
              0.5f,  0.0f, 0.0f  //Top vertex
              };
        

        Shader shader;

        int VertexArrayObject;
        int VertexArrayObject_2;

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
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);//Code goes here

            ///////////// VBO (ver1)
            int VertexBufferObject;
            // 그래픽카드에 Buffer(저장공간) 을 생성
            VertexBufferObject = GL.GenBuffer();
            // 생성한 Buffer를 어떤 특성(ArrayBuffer)으로 선택
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // 선택한 Buffer에 데이터를 보내기
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            ///////////// VBO (ver2)
            int VertexBufferObject_2;
            // 그래픽카드에 Buffer(저장공간) 을 생성
            VertexBufferObject_2 = GL.GenBuffer();
            // 생성한 Buffer를 어떤 특성(ArrayBuffer)으로 선택
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject_2);
            // 선택한 Buffer에 데이터를 보내기
            GL.BufferData(BufferTarget.ArrayBuffer, vertices_2.Length * sizeof(float), vertices_2, BufferUsageHint.StaticDraw);


            ///////////// SHADER
            string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string vertexPath = Path.Join(ProjectDirectory, "Vertex.glsl").ToString();
            string fragmentPath = Path.Join(ProjectDirectory, "Fragment.glsl").ToString();

            shader = new Shader(vertexPath, fragmentPath);

            ///////////// VAO (ver1)
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            VertexArrayObject = GL.GenVertexArray();
            // 그래픽카드에 VertexArrayobject를 저장할 공간 할당
            GL.BindVertexArray(VertexArrayObject);
            // VertexArrayobject를 선택
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            // 그래픽카드에 Bind 된 Buffer의 해석 방법을 Bind된 VertexArrayobject에 저장
            // 0번 속성 , 데이터3개 , float 타입
            GL.EnableVertexAttribArray(0);
            // 0번 속성을 Enable

            ///////////// VAO (ver2)
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject_2);
            VertexArrayObject_2 = GL.GenVertexArray();
            // 그래픽카드에 VertexArrayobject를 저장할 공간 할당
            GL.BindVertexArray(VertexArrayObject_2);
            // VertexArrayobject를 선택
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            // 그래픽카드에 Bind 된 Buffer의 해석 방법을 Bind된 VertexArrayobject에 저장
            // 0번 속성 , 데이터3개 , float 타입
            GL.EnableVertexAttribArray(0);
            // 0번 속성을 Enable
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);//Code goes here.

            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            GL.BindVertexArray(VertexArrayObject_2);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

    }
}

