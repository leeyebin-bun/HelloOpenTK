﻿using System;
using System.Reflection;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Net.WebRequestMethods;

namespace HelloOpenTK
{
	public class MyGameWindow : GameWindow
	{
        public MyGameWindow(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }

        Vertex[] vertices = {

            new Vertex(-1.0f, 1.0f, 0.0f), //top-left vertex
            new Vertex(1.0f, 1.0f, 0.0f), //top-right vertex
            new Vertex(1.0f, -1.0f, 0.0f), //bottom-right vertex
            new Vertex(-1.0f, -1.0f, 0.0f), //Bottom-left vertex      
        };

        uint[] indices =
        {
            0,1,2,
            0,2,3,
            2,3,1,
        };

        Shader shader;

        int ElementBufferObject;
        int VertexBufferObject;
        int VertexArrayObject;
        float speed = 1.5f;
        float pitch = 0.0f;
        float yaw = 90.0f;
        float cameraSensitivity = 0.1f;

        Vector3 position = new Vector3(0.0f, 0.0f, -3.0f);
        Vector3 front = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        Vector2 lastMousePos;
        bool FirstMove = true;


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused) // check to see if the window is focused
            {
                return;
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            KeyboardState input = KeyboardState;//...

            if (input.IsKeyDown(Keys.W))
            {
                position += front * speed * (float)e.Time; //Forward 
            }
            if (input.IsKeyDown(Keys.S))
            {
                position -= front * speed * (float)e.Time; //Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time; //Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time; //Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                position += up * speed * (float)e.Time; //Up 
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                position -= up * speed * (float)e.Time; //Down
            }

           
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if(!MouseState.IsButtonDown(MouseButton.Right))
            {
                return;
            }

            float deltaX = e.DeltaX;
            float deltaY = e.DeltaY;

            if (pitch > 89.0f)
            {
                pitch = 89.0f;
            }
            else if (pitch < -89.0f)
            {
                pitch = -89.0f;
            }
            else
            {
                pitch -= deltaY * cameraSensitivity;
            }

            yaw += deltaX * cameraSensitivity;

            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);

        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.2f, 0.2f, 2.0f, 1.0f);//Code goes here


            //////////////////////////////////////////////////////////
            /// VBO (Vertex Buffer Object)
            // 그래픽카드에 Buffer (저장공간)를 생성
            VertexBufferObject = GL.GenBuffer();
            // 생성한 Buffer를 어떤 특성(ArrayBuffer)으로 선택
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // 선택한 Buffer에 데이터를 보내기
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float) * 3, vertices, BufferUsageHint.StaticDraw);

            /////////////////////////////////////////////////////////
            /// VAO (Vertex Array Object)
            // 그래픽카드에 VertexArrayObject를 저장할 공간 할당
            VertexArrayObject = GL.GenVertexArray();
            // VertexArrayObject를 선택
            GL.BindVertexArray(VertexArrayObject);
            // 그래픽카드에 Bind된 Buffer의 해석 방법을 Bind된 VertexArrayObject에 저장
            // 0번 속성, 데이터는 3개, float 타입, ...
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            // 0번 속성을 Enable
            GL.EnableVertexAttribArray(0);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            
            ///////////// SHADER
            string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string vertexPath = Path.Join(ProjectDirectory, "Vertex.glsl").ToString();
            string fragmentPath = Path.Join(ProjectDirectory, "Fragment.glsl").ToString();

            shader = new Shader(vertexPath, fragmentPath);

           
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);//Code goes here.

            shader.Use();

            Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(0.0f));
            Matrix4 view = Matrix4.LookAt(position, position + front, up);//카메라 위치
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Size.X / (float)Size.Y, 0.1f, 100.0f);

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.BindVertexArray(VertexArrayObject);
        
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

    }
}

