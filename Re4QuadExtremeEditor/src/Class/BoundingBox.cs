using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Re4QuadExtremeEditor.src.Class // baseado em https://github.com/DavidSM64/Quad64/blob/master/src/Viewer/BoundingBox.cs
{
    public static class BoundingBox
    {

        public static void draw_solid(Vector3 upper, Vector3 lower)
        {
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.PushMatrix();

            GL.Begin(PrimitiveType.Quads);

            GL.Vertex3(upper.X, upper.Y, lower.Z); // Top-right of top face
            GL.Vertex3(lower.X, upper.Y, lower.Z); // Top-left of top face
            GL.Vertex3(lower.X, upper.Y, upper.Z); // Bottom-left of top face
            GL.Vertex3(upper.X, upper.Y, upper.Z); // Bottom-right of top face

            GL.Vertex3(upper.X, lower.Y, lower.Z); // Top-right of bottom face
            GL.Vertex3(lower.X, lower.Y, lower.Z); // Top-left of bottom face
            GL.Vertex3(lower.X, lower.Y, upper.Z); // Bottom-left of bottom face
            GL.Vertex3(upper.X, lower.Y, upper.Z); // Bottom-right of bottom face

            GL.Vertex3(upper.X, upper.Y, upper.Z); // Top-Right of front face
            GL.Vertex3(lower.X, upper.Y, upper.Z); // Top-left of front face
            GL.Vertex3(lower.X, lower.Y, upper.Z); // Bottom-left of front face
            GL.Vertex3(upper.X, lower.Y, upper.Z); // Bottom-right of front face

            GL.Vertex3(upper.X, lower.Y, lower.Z); // Bottom-Left of back face
            GL.Vertex3(lower.X, lower.Y, lower.Z); // Bottom-Right of back face
            GL.Vertex3(lower.X, upper.Y, lower.Z); // Top-Right of back face
            GL.Vertex3(upper.X, upper.Y, lower.Z); // Top-Left of back face

            GL.Vertex3(lower.X, upper.Y, upper.Z); // Top-Right of left face
            GL.Vertex3(lower.X, upper.Y, lower.Z); // Top-Left of left face
            GL.Vertex3(lower.X, lower.Y, lower.Z); // Bottom-Left of left face
            GL.Vertex3(lower.X, lower.Y, upper.Z); // Bottom-Right of left face

            GL.Vertex3(upper.X, upper.Y, upper.Z); // Top-Right of left face
            GL.Vertex3(upper.X, upper.Y, lower.Z); // Top-Left of left face
            GL.Vertex3(upper.X, lower.Y, lower.Z); // Bottom-Left of left face
            GL.Vertex3(upper.X, lower.Y, upper.Z); // Bottom-Right of left face

            GL.End();
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);
        }

        public static void draw_TransparentSolid(Vector3 upper, Vector3 lower)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0f);
            GL.PushMatrix();

            GL.Begin(PrimitiveType.Quads);

            GL.Vertex3(upper.X, upper.Y, lower.Z); // Top-right of top face
            GL.Vertex3(lower.X, upper.Y, lower.Z); // Top-left of top face
            GL.Vertex3(lower.X, upper.Y, upper.Z); // Bottom-left of top face
            GL.Vertex3(upper.X, upper.Y, upper.Z); // Bottom-right of top face

            GL.Vertex3(upper.X, lower.Y, lower.Z); // Top-right of bottom face
            GL.Vertex3(lower.X, lower.Y, lower.Z); // Top-left of bottom face
            GL.Vertex3(lower.X, lower.Y, upper.Z); // Bottom-left of bottom face
            GL.Vertex3(upper.X, lower.Y, upper.Z); // Bottom-right of bottom face

            GL.Vertex3(upper.X, upper.Y, upper.Z); // Top-Right of front face
            GL.Vertex3(lower.X, upper.Y, upper.Z); // Top-left of front face
            GL.Vertex3(lower.X, lower.Y, upper.Z); // Bottom-left of front face
            GL.Vertex3(upper.X, lower.Y, upper.Z); // Bottom-right of front face

            GL.Vertex3(upper.X, lower.Y, lower.Z); // Bottom-Left of back face
            GL.Vertex3(lower.X, lower.Y, lower.Z); // Bottom-Right of back face
            GL.Vertex3(lower.X, upper.Y, lower.Z); // Top-Right of back face
            GL.Vertex3(upper.X, upper.Y, lower.Z); // Top-Left of back face

            GL.Vertex3(lower.X, upper.Y, upper.Z); // Top-Right of left face
            GL.Vertex3(lower.X, upper.Y, lower.Z); // Top-Left of left face
            GL.Vertex3(lower.X, lower.Y, lower.Z); // Bottom-Left of left face
            GL.Vertex3(lower.X, lower.Y, upper.Z); // Bottom-Right of left face

            GL.Vertex3(upper.X, upper.Y, upper.Z); // Top-Right of left face
            GL.Vertex3(upper.X, upper.Y, lower.Z); // Top-Left of left face
            GL.Vertex3(upper.X, lower.Y, lower.Z); // Bottom-Left of left face
            GL.Vertex3(upper.X, lower.Y, upper.Z); // Bottom-Right of left face

            GL.End();
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.AlphaTest);
        }

        public static void draw(Vector3 upper, Vector3 lower)
        {
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.PushMatrix();
          
            GL.Begin(PrimitiveType.LineLoop);

            GL.Vertex3(upper.X, upper.Y, lower.Z); // 1
            GL.Vertex3(lower.X, upper.Y, lower.Z); // 2
            GL.Vertex3(lower.X, upper.Y, upper.Z); // 3
            GL.Vertex3(upper.X, upper.Y, lower.Z); // 1
            GL.Vertex3(upper.X, upper.Y, upper.Z); // 4
            GL.Vertex3(lower.X, upper.Y, upper.Z); // 3

            GL.Vertex3(lower.X, lower.Y, upper.Z); // 7
            GL.Vertex3(lower.X, lower.Y, lower.Z); // 6
            GL.Vertex3(upper.X, lower.Y, lower.Z); // 5
            GL.Vertex3(lower.X, lower.Y, upper.Z); // 7
            GL.Vertex3(upper.X, lower.Y, upper.Z); // 8
            GL.Vertex3(upper.X, lower.Y, lower.Z); // 5

            GL.Vertex3(lower.X, upper.Y, lower.Z); // 2
            GL.Vertex3(lower.X, lower.Y, lower.Z); // 6
            GL.Vertex3(lower.X, upper.Y, upper.Z); // 3
            GL.Vertex3(upper.X, lower.Y, upper.Z); // 8
            GL.Vertex3(upper.X, upper.Y, upper.Z); // 4
            GL.Vertex3(upper.X, lower.Y, lower.Z); // 5

            GL.End();
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);
        }

        public static void drawTriggerZone(Vector2[] TriggerZone) 
        {
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.PushMatrix();

            GL.Begin(PrimitiveType.LineLoop);

            Vector2 p0 = TriggerZone[0];
            Vector2 p1 = TriggerZone[1];
            Vector2 p2 = TriggerZone[2];
            Vector2 p3 = TriggerZone[3];
              float h1 = TriggerZone[4].X; //HeightBoundary1
              float h2 = TriggerZone[4].Y; //HeightBoundary2
         
            GL.Vertex3(p1.X, h2, p1.Y); // p1 baixo
            GL.Vertex3(p2.X, h2, p2.Y); // p2 baixo
            GL.Vertex3(p3.X, h2, p3.Y); // p3 baixo
            GL.Vertex3(p0.X, h2, p0.Y); // p0 baixo
            GL.Vertex3(p1.X, h2, p1.Y); // p1 baixo
            GL.Vertex3(p3.X, h2, p3.Y); // p3 Cortemeio

            GL.Vertex3(p3.X, h1, p3.Y); // p3 sobe
            GL.Vertex3(p0.X, h2, p0.Y); // p0 baixo
            GL.Vertex3(p0.X, h1, p0.Y); // p0 sobe
            GL.Vertex3(p1.X, h2, p1.Y); // p1 baixo
            GL.Vertex3(p2.X, h1, p2.Y); // p2 sobe
            GL.Vertex3(p2.X, h2, p2.Y); // p2 baixo
            GL.Vertex3(p3.X, h1, p3.Y); // p3 sobe

            GL.Vertex3(p0.X, h1, p0.Y); // p0 cima
            GL.Vertex3(p1.X, h1, p1.Y); // p1 cima
            GL.Vertex3(p2.X, h1, p2.Y); // p2 cima
            GL.Vertex3(p3.X, h1, p3.Y); // p3 cima
            GL.Vertex3(p1.X, h1, p1.Y); // p1 Cortemeio
            GL.Vertex3(p1.X, h2, p1.Y); // p1 baixo

            GL.End();
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);
        }

        public static void drawTriggerZone_TransparentSolid(Vector2[] TriggerZone)
        {       
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0f);
            //GL.AlphaFunc(AlphaFunction.Equal, 0.1f);
            //GL.DepthMask(false);
          
            GL.PushMatrix();

            GL.Begin(PrimitiveType.Quads);

            Vector2 p0 = TriggerZone[0];
            Vector2 p1 = TriggerZone[1];
            Vector2 p2 = TriggerZone[2];
            Vector2 p3 = TriggerZone[3];
            float h1 = TriggerZone[4].X; //HeightBoundary1
            float h2 = TriggerZone[4].Y; //HeightBoundary2

            GL.Vertex3(p0.X, h1, p0.Y);
            GL.Vertex3(p1.X, h1, p1.Y);
            GL.Vertex3(p2.X, h1, p2.Y);
            GL.Vertex3(p3.X, h1, p3.Y);

            GL.Vertex3(p0.X, h1, p0.Y);
            GL.Vertex3(p1.X, h1, p1.Y);
            GL.Vertex3(p1.X, h2, p1.Y);
            GL.Vertex3(p0.X, h2, p0.Y);

            GL.Vertex3(p1.X, h1, p1.Y);
            GL.Vertex3(p2.X, h1, p2.Y);
            GL.Vertex3(p2.X, h2, p2.Y);
            GL.Vertex3(p1.X, h2, p1.Y);

            GL.Vertex3(p2.X, h1, p2.Y);
            GL.Vertex3(p3.X, h1, p3.Y);
            GL.Vertex3(p3.X, h2, p3.Y);
            GL.Vertex3(p2.X, h2, p2.Y);

            GL.Vertex3(p3.X, h1, p3.Y);
            GL.Vertex3(p0.X, h1, p0.Y);
            GL.Vertex3(p0.X, h2, p0.Y);
            GL.Vertex3(p3.X, h2, p3.Y);


            GL.Vertex3(p0.X, h2, p0.Y);
            GL.Vertex3(p1.X, h2, p1.Y);
            GL.Vertex3(p2.X, h2, p2.Y);
            GL.Vertex3(p3.X, h2, p3.Y);

            GL.End();
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.AlphaTest);
            //GL.DepthMask(true);
        }

        public static void drawTriggerZone_solid(Vector2[] TriggerZone)
        {
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.PushMatrix();

            GL.Begin(PrimitiveType.Quads);

            Vector2 p0 = TriggerZone[0];
            Vector2 p1 = TriggerZone[1];
            Vector2 p2 = TriggerZone[2];
            Vector2 p3 = TriggerZone[3];
            float h1 = TriggerZone[4].X; //HeightBoundary1
            float h2 = TriggerZone[4].Y; //HeightBoundary2

            GL.Vertex3(p0.X, h1, p0.Y);
            GL.Vertex3(p1.X, h1, p1.Y);
            GL.Vertex3(p2.X, h1, p2.Y);
            GL.Vertex3(p3.X, h1, p3.Y);

            GL.Vertex3(p0.X, h1, p0.Y);
            GL.Vertex3(p1.X, h1, p1.Y);
            GL.Vertex3(p1.X, h2, p1.Y);
            GL.Vertex3(p0.X, h2, p0.Y);

            GL.Vertex3(p1.X, h1, p1.Y);
            GL.Vertex3(p2.X, h1, p2.Y);
            GL.Vertex3(p2.X, h2, p2.Y);
            GL.Vertex3(p1.X, h2, p1.Y);

            GL.Vertex3(p2.X, h1, p2.Y);
            GL.Vertex3(p3.X, h1, p3.Y);
            GL.Vertex3(p3.X, h2, p3.Y);
            GL.Vertex3(p2.X, h2, p2.Y);

            GL.Vertex3(p3.X, h1, p3.Y);
            GL.Vertex3(p0.X, h1, p0.Y);
            GL.Vertex3(p0.X, h2, p0.Y);
            GL.Vertex3(p3.X, h2, p3.Y);


            GL.Vertex3(p0.X, h2, p0.Y);
            GL.Vertex3(p1.X, h2, p1.Y);
            GL.Vertex3(p2.X, h2, p2.Y);
            GL.Vertex3(p3.X, h2, p3.Y);

            GL.End();
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);
        }


        public static void drawCircleTriggerZone(Vector2 Center, Vector2 DownUp, float Radius) 
        {
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.PushMatrix();

            GL.Begin(PrimitiveType.LineLoop);

            int triangleAmount = 16;//(int)(Radius / 6);

            /*if (triangleAmount < 16)
            {
                triangleAmount = 16;
            }*/

            float h1 = DownUp.X; //HeightBoundary1
            float h2 = DownUp.Y; //HeightBoundary2

            // zig zig
            for (int i = 0; i < triangleAmount; i++)
            {
                float x = (float)(Center.X + (Radius * Math.Cos(i * MathHelper.TwoPi / triangleAmount)));
                float z = (float)(Center.Y + (Radius * Math.Sin(i * MathHelper.TwoPi / triangleAmount)));

                GL.Vertex3(x, h2, z);
                GL.Vertex3(x, h1, z);
            }

            GL.End();


            GL.Begin(PrimitiveType.LineLoop);

            // dowm
            for (int i = 0; i < triangleAmount; i++)
            {
                float x = (float)(Center.X + (Radius * Math.Cos(i * MathHelper.TwoPi / triangleAmount)));
                float z = (float)(Center.Y + (Radius * Math.Sin(i * MathHelper.TwoPi / triangleAmount)));

                GL.Vertex3(x, h1, z);
            }
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);

            // up
            for (int i = 0; i < triangleAmount; i++)
            {
                float x = (float)(Center.X + (Radius * Math.Cos(i * MathHelper.TwoPi / triangleAmount)));
                float z = (float)(Center.Y + (Radius * Math.Sin(i * MathHelper.TwoPi / triangleAmount)));

                GL.Vertex3(x, h2, z);
            }

            GL.End();

            GL.Begin(PrimitiveType.Lines);

            //int plus = triangleAmount / 8;

            int pluspos = 0;
            // to center
            for (int i = 0; i < 8; i++)
            {
                float x = (float)(Center.X + (Radius * Math.Cos(pluspos * MathHelper.TwoPi / triangleAmount)));
                float z = (float)(Center.Y + (Radius * Math.Sin(pluspos * MathHelper.TwoPi / triangleAmount)));

                GL.Vertex3(x, h2, z);
                GL.Vertex3(Center.X, h2, Center.Y);
                GL.Vertex3(Center.X, h1, Center.Y);
                GL.Vertex3(x, h1, z);

                pluspos += 2;//plus;
            }

            GL.End();
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);
        }

        public static void drawCircleTriggerZone_TransparentSolid(Vector2 Center, Vector2 DownUp, float Radius) 
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0f);
            //GL.AlphaFunc(AlphaFunction.Equal, 0.1f);
            //GL.DepthMask(false);

            GL.PushMatrix();

            GL.Begin(PrimitiveType.Triangles);

            int triangleAmount = 16;//(int)(Radius / 6);

            /*if (triangleAmount < 16)
            {
                triangleAmount = 16;
            }*/

            float h1 = DownUp.X; //HeightBoundary1
            float h2 = DownUp.Y; //HeightBoundary2

            float Oldx = (float)(Center.X + (Radius * Math.Cos(0 * MathHelper.TwoPi / triangleAmount)));
            float Oldz = (float)(Center.Y + (Radius * Math.Sin(0 * MathHelper.TwoPi / triangleAmount)));

            for (int i = 1; i < triangleAmount + 1; i++)
            {
                float x = (float)(Center.X + (Radius * Math.Cos(i * MathHelper.TwoPi / triangleAmount)));
                float z = (float)(Center.Y + (Radius * Math.Sin(i * MathHelper.TwoPi / triangleAmount)));

                // down
                GL.Vertex3(Center.X, h1, Center.Y);
                GL.Vertex3(Oldx, h1, Oldz);
                GL.Vertex3(x, h1, z);

                Oldx = x;
                Oldz = z;
            }

             Oldx = (float)(Center.X + (Radius * Math.Cos(0 * MathHelper.TwoPi / triangleAmount)));
             Oldz = (float)(Center.Y + (Radius * Math.Sin(0 * MathHelper.TwoPi / triangleAmount)));

            for (int i = 1; i < triangleAmount + 1; i++)
            {
                float x = (float)(Center.X + (Radius * Math.Cos(i * MathHelper.TwoPi / triangleAmount)));
                float z = (float)(Center.Y + (Radius * Math.Sin(i * MathHelper.TwoPi / triangleAmount)));

                // up
                GL.Vertex3(Center.X, h2, Center.Y);
                GL.Vertex3(Oldx, h2, Oldz);
                GL.Vertex3(x, h2, z);

                Oldx = x;
                Oldz = z;
            }

            Oldx = (float)(Center.X + (Radius * Math.Cos(0 * MathHelper.TwoPi / triangleAmount)));
            Oldz = (float)(Center.Y + (Radius * Math.Sin(0 * MathHelper.TwoPi / triangleAmount)));

            for (int i = 1; i < triangleAmount + 1; i++)
            {
                float x = (float)(Center.X + (Radius * Math.Cos(i * MathHelper.TwoPi / triangleAmount)));
                float z = (float)(Center.Y + (Radius * Math.Sin(i * MathHelper.TwoPi / triangleAmount)));

                // lados

                GL.Vertex3(Oldx, h1, Oldz);
                GL.Vertex3(Oldx, h2, Oldz);
                GL.Vertex3(x, h1, z);

                GL.Vertex3(x, h1, z);
                GL.Vertex3(x, h2, z);
                GL.Vertex3(Oldx, h2, Oldz);

                Oldx = x;
                Oldz = z;
            }


            GL.End();
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.AlphaTest);
            //GL.DepthMask(true);
        }

        public static void drawCircleTriggerZone_solid(Vector2 Center, Vector2 DownUp, float Radius) 
        {
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.PushMatrix();

            GL.Begin(PrimitiveType.Triangles);

            int triangleAmount = 16;//(int)(Radius / 6);

            /*if (triangleAmount < 16)
            {
                triangleAmount = 16;
            }*/

            float h1 = DownUp.X; //HeightBoundary1
            float h2 = DownUp.Y; //HeightBoundary2

            float Oldx = (float)(Center.X + (Radius * Math.Cos(0 * MathHelper.TwoPi / triangleAmount)));
            float Oldz = (float)(Center.Y + (Radius * Math.Sin(0 * MathHelper.TwoPi / triangleAmount)));

            for (int i = 1; i < triangleAmount + 1; i++)
            {
                float x = (float)(Center.X + (Radius * Math.Cos(i * MathHelper.TwoPi / triangleAmount)));
                float z = (float)(Center.Y + (Radius * Math.Sin(i * MathHelper.TwoPi / triangleAmount)));

                // down
                GL.Vertex3(Center.X, h1, Center.Y);
                GL.Vertex3(Oldx, h1, Oldz);
                GL.Vertex3(x, h1, z);

                // up
                GL.Vertex3(Center.X, h2, Center.Y);
                GL.Vertex3(Oldx, h2, Oldz);
                GL.Vertex3(x, h2, z);

                // lados

                GL.Vertex3(Oldx, h1, Oldz);
                GL.Vertex3(Oldx, h2, Oldz);
                GL.Vertex3(x, h1, z);

                GL.Vertex3(x, h1, z);
                GL.Vertex3(x, h2, z);
                GL.Vertex3(Oldx, h2, Oldz);

                Oldx = x;
                Oldz = z;
            }

            GL.End();
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);


        }


        public static void drawItemTrigggerRadius(float Radius) 
        {
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.PushMatrix();

            GL.Begin(PrimitiveType.LineLoop);

            int triangleAmount = 16;

            for (int i = 0; i < triangleAmount; i++)
            {
                float x = (float)(Radius * Math.Cos(i * MathHelper.TwoPi / triangleAmount));
                float z = (float)(Radius * Math.Sin(i * MathHelper.TwoPi / triangleAmount));

                GL.Vertex3(x, 0, z);
            }

            GL.End();
            GL.PopMatrix();
            GL.Enable(EnableCap.Texture2D);
        }

    }
}
