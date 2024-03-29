﻿//using OpenTK.Graphics.OpenGL;
//using System;

//namespace HelloOpenTK
//{
//    public struct Vertex
//    {
//        public Vertex(float w, float x, float y)
//        {
//            _w = w;
//            _x = x;
//            _y = y;
//        }

//        public float _w;
//        public float _x;
//        public float _y;
//    }

//    public class Rectangle
//    {
//        int VertexBufferObject;

//        int VertexArrayObject;

//        Vertex[] vertices = new Vertex[4];

//        public Rectangle(Vertex v1, Vertex v2, Vertex v3, Vertex v4)
//        {
//            vertices[0] = v1;
//            vertices[1] = v2;
//            vertices[2] = v3;
//            vertices[3] = v4;

//            /// VBO (Vertex Buffer Object)
//            // 그래픽카드에 Buffer (저장공간)를 생성
//            VertexBufferObject = GL.GenBuffer();
//            // 생성한 Buffer를 어떤 특성(ArrayBuffer)으로 선택
//            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
//            // 선택한 Buffer에 데이터를 보내기
//            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float) * 3, vertices, BufferUsageHint.StaticDraw);


//            /// VAO (Vertex Array Object)
//            // 그래픽카드에 VertexArrayObject를 저장할 공간 할당
//            VertexArrayObject = GL.GenVertexArray();
//            // VertexArrayObject를 선택
//            GL.BindVertexArray(VertexArrayObject);
//            // 그래픽카드에 Bind된 Buffer의 해석 방법을 Bind된 VertexArrayObject에 저장
//            // 0번 속성, 데이터는 3개, float 타입, ...
//            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
//            // 0번 속성을 Enable
//            GL.EnableVertexAttribArray(0);

//            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
//            GL.BindVertexArray(0);
//        }

//        public void Draw()
//        {
//            GL.BindVertexArray(VertexArrayObject);
//            GL.DrawArrays(PrimitiveType.Triangles, 0, 4);
//        }
//    }
//}
