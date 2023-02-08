using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Re4QuadExtremeEditor.src.JSON;
using System.Drawing;
using System.IO;
using PMD_API;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Re4QuadExtremeEditor.src.Class
{
    /// <summary>
    /// Classe que reprenta a estrutura 3d de uma room (não contem seus objetos)
    /// </summary>
    public class Room
    {
        public class MeshData
        {
            // etapa 1;
            public float[] vertices;
            public uint[] indices;
            public string texturename;

            // etapa 2;
            public int vertexBufferObject; // vertices  VBO
            public int elementBufferObject; // indices  EBO
            public int vertexArrayObject; // VAO
        }

        // todas as meshs
        private List<MeshData> meshes;
        //todas as imagens
        // nome da imagem, conteudo
        private Dictionary<string, Bitmap> textures;
        // nome da imagem, id no GL
        private Dictionary<string, int> texturesGL;

        public RoomInfo GetRoomInfo { get; }

        const float ExtraScale = 10f;

        public Room(RoomInfo roomInfo)
        {
            GetRoomInfo = roomInfo;

            meshes = new List<MeshData>();
            textures = new Dictionary<string, Bitmap>();
            texturesGL = new Dictionary<string, int>();

            for (int iJson = 0; iJson < roomInfo.RoomJsonFiles.Length; iJson++)
            {
                // obtem o caminho completo
                string JsonPath = Consts.RoomJsonFilesDiretory + roomInfo.RoomJsonFiles[iJson];

                //verifica se ele existe
                if (File.Exists(JsonPath))
                {
                    Dictionary<string, PMD> pmds = new Dictionary<string, PMD>();

                    // obtem o RoomJson class
                    RoomJson rj = new RoomJson(null);
                    try { rj = RoomJsonFile.parseRoomJson(JsonPath); } catch (Exception) { }

                    // obtem o caminho completo do diretorio dos arquivos PMD
                    string PmdDiretory = Globals.xscrDiretory + rj.RoomFolder;

                    // verifica se o diretorio dos arquivos PMD existe
                    if (Directory.Exists(PmdDiretory))
                    {
                        // lista de pmds
                        for (int ipmd = 0; ipmd < rj.PmdList.Count; ipmd++)
                        {
                            string PmdPath = PmdDiretory + rj.PmdList[ipmd];

                            if (File.Exists(PmdPath))
                            {
                                try
                                {
                                    pmds.Add(rj.PmdList[ipmd], PmdDecoder.GetPMD(PmdPath));
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }

                        var PmdKeys = pmds.Keys.ToArray();

                        // parte das texturas
                        HashSet<string> TGAs = new HashSet<string>();
                        for (int i = 0; i < PmdKeys.Length; i++)
                        {
                            for (int it = 0; it < pmds[PmdKeys[i]].TextureNames.Length; it++)
                            {
                                if (!rj.BlackListTextures.Contains(pmds[PmdKeys[i]].TextureNames[it]))
                                {
                                    TGAs.Add(pmds[PmdKeys[i]].TextureNames[it]);
                                }
                            }
                        }

                        var TGAnames = TGAs.ToArray();
                        for (int i = 0; i < TGAnames.Length; i++)
                        {
                            string TgaPath = PmdDiretory + TGAnames[i];
                            if (File.Exists(TgaPath))
                            {
                                try
                                {
                                    TGASharpLib.TGA nTGA = new TGASharpLib.TGA(TgaPath);
                                    if (!textures.ContainsKey(TGAnames[i]))
                                    {
                                        textures.Add(TGAnames[i], nTGA.ToBitmap());
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }

                        // convert

                        for (int ipmd = 0; ipmd < PmdKeys.Length; ipmd++)
                        {
                            string Pmdname = PmdKeys[ipmd];

                            FixPmd fix = new FixPmd(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1));
                            var rx = Matrix4.CreateRotationX(fix.Rotation.X);
                            var ry = Matrix4.CreateRotationY(fix.Rotation.Y);
                            var rz = Matrix4.CreateRotationZ(fix.Rotation.Z);

                            if (rj.FixPmds.ContainsKey(Pmdname))
                            {
                                fix = rj.FixPmds[Pmdname];
                                rx = Matrix4.CreateRotationX(fix.Rotation.X);
                                ry = Matrix4.CreateRotationY(fix.Rotation.Y);
                                rz = Matrix4.CreateRotationZ(fix.Rotation.Z);
                            }

                            for (int iN = 0; iN < pmds[Pmdname].Nodes.Length; iN++)
                            {
                                for (int iM = 0; iM < pmds[Pmdname].Nodes[iN].Meshs.Length; iM++)
                                {
                                    float[] vertices = new float[pmds[Pmdname].Nodes[iN].Meshs[iM].Vertexs.Length * 5];
                                    uint[] indices = pmds[Pmdname].Nodes[iN].Meshs[iM].Orders;

                                    int vOffset = 0;
                                    for (int iv = 0; iv < pmds[Pmdname].Nodes[iN].Meshs[iM].Vertexs.Length; iv++)
                                    {

                                        var point = new Vector4(pmds[Pmdname].Nodes[iN].Meshs[iM].Vertexs[iv].x, pmds[Pmdname].Nodes[iN].Meshs[iM].Vertexs[iv].y, pmds[Pmdname].Nodes[iN].Meshs[iM].Vertexs[iv].z, 1);
                                        point = point * rx * ry * rz;

                                        int iS = pmds[Pmdname].Nodes[iN].SkeletonIndex;

                                        vertices[vOffset + 0] = (point.X
                                            * pmds[Pmdname].Skeleton[iS][0]
                                            * fix.Scale.X
                                            + pmds[Pmdname].Skeleton[iS][7]
                                            + fix.Position.X)
                                            * ExtraScale;
                                        vertices[vOffset + 1] = (point.Y
                                            * pmds[Pmdname].Skeleton[iS][1]
                                            * fix.Scale.Y
                                            + pmds[Pmdname].Skeleton[iS][8]
                                            + fix.Position.Y)
                                            * ExtraScale;
                                        vertices[vOffset + 2] = (point.Z
                                            * pmds[Pmdname].Skeleton[iS][2]
                                            * fix.Scale.Z
                                            + pmds[Pmdname].Skeleton[iS][9]
                                            + fix.Position.Z)
                                            * ExtraScale;
                                        vertices[vOffset + 3] = pmds[Pmdname].Nodes[iN].Meshs[iM].Vertexs[iv].tu;
                                        vertices[vOffset + 4] = pmds[Pmdname].Nodes[iN].Meshs[iM].Vertexs[iv].tv;
                                        vOffset += 5;
                                    }

                                    MeshData m = new MeshData();
                                    m.vertices = vertices;
                                    m.indices = indices;
                                    m.texturename = null;
                                    if (textures.ContainsKey(pmds[Pmdname].TextureNames[pmds[Pmdname].Nodes[iN].Meshs[iM].TextureIndex]))
                                    {
                                        m.texturename = pmds[Pmdname].TextureNames[pmds[Pmdname].Nodes[iN].Meshs[iM].TextureIndex];
                                    }
                                    if (rj.BlackListTextures.Contains(pmds[Pmdname].TextureNames[pmds[Pmdname].Nodes[iN].Meshs[iM].TextureIndex]) 
                                        || pmds[Pmdname].TextureNames[pmds[Pmdname].Nodes[iN].Meshs[iM].TextureIndex].Length == 0)
                                    {
                                        m.texturename = Consts.TransparentTextureName;
                                    }
                                    meshes.Add(m);
                                }

                            }
                        }

                    }

                }

            }

            // carrega para o GL
            var textureNames = textures.Keys.ToArray();
            for (int i = 0; i < textureNames.Length; i++)
            {
                texturesGL.Add(textureNames[i], Texture.GetTextureIdGL(textures[textureNames[i]]));
            }

            DataBase.ShaderRoom.Use();
            int vertexLocation = DataBase.ShaderRoom.GetAttribLocation("aPosition");
            int texCoordLocation = DataBase.ShaderRoom.GetAttribLocation("aTexCoord");


            for (int i = 0; i < meshes.Count; i++)
            {
                if (!Globals.UseOldGL)
                {
                    meshes[i].vertexArrayObject = GL.GenVertexArray();
                    GL.BindVertexArray(meshes[i].vertexArrayObject);
                }
             
                meshes[i].vertexBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, meshes[i].vertexBufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, meshes[i].vertices.Length * sizeof(float), meshes[i].vertices, BufferUsageHint.StaticDraw);

                meshes[i].elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, meshes[i].elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, meshes[i].indices.Length * sizeof(uint), meshes[i].indices, BufferUsageHint.StaticDraw);

                if (!Globals.UseOldGL)
                {
                    //aPosition
                    GL.EnableVertexAttribArray(vertexLocation);
                    GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                    //aTexCoord
                    GL.EnableVertexAttribArray(texCoordLocation);
                    GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
                }
            }

           //int finish = 0;

        }

        public void ClearGL()
        {
            
            for (int i = 0; i < meshes.Count; i++)
            {
                if (!Globals.UseOldGL)
                {
                    GL.BindVertexArray(meshes[i].vertexArrayObject);
                    GL.DeleteBuffer(meshes[i].vertexArrayObject);
                }

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.DeleteBuffer(meshes[i].vertexBufferObject);
                GL.DeleteBuffer(meshes[i].elementBufferObject);

            }

            foreach (var item in texturesGL)
            {
                GL.DeleteBuffer(item.Value);
            }
            
            meshes.Clear();
            textures.Clear();
            texturesGL.Clear();
        }


        public void Render()
        {

            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0.5f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.ActiveTexture(TextureUnit.Texture0);

            if (Globals.UseOldGL)
            {
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.EnableClientState(ArrayCap.TextureCoordArray);
            }

            for (int i = 0; i < meshes.Count; i++)
            {
                if (!Globals.UseOldGL)
                {
                    GL.BindVertexArray(meshes[i].vertexArrayObject);
                }    

                if (meshes[i].texturename != null && texturesGL.ContainsKey(meshes[i].texturename))
                {
                    GL.BindTexture(TextureTarget.Texture2D, texturesGL[meshes[i].texturename]);
                }
                else if (meshes[i].texturename == Consts.TransparentTextureName)
                {
                    GL.BindTexture(TextureTarget.Texture2D, DataBase.TransparentTextureIdGL);
                }
                else
                {
                    GL.BindTexture(TextureTarget.Texture2D, DataBase.NoTextureIdGL);
                }

                if (Globals.UseOldGL)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, meshes[i].vertexBufferObject);
                    GL.VertexPointer(3, VertexPointerType.Float, 5 * sizeof(float), 0);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, meshes[i].vertexBufferObject);
                    GL.TexCoordPointer(2, TexCoordPointerType.Float, 5 * sizeof(float), 3 * sizeof(float));
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, meshes[i].elementBufferObject);
                }

                GL.DrawElements(PrimitiveType.Triangles, meshes[i].indices.Length, DrawElementsType.UnsignedInt, 0);
            }

            if (Globals.UseOldGL)
            {
                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            }

            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);

        }

        public void Render_Solid()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            for (int i = 0; i < meshes.Count; i++)
            {
                if (!Globals.UseOldGL)
                {
                    GL.BindVertexArray(meshes[i].vertexArrayObject);
                }
                GL.BindTexture(TextureTarget.Texture2D, DataBase.SolidTextureIdGL);

                if (Globals.UseOldGL)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, meshes[i].vertexBufferObject);
                    GL.VertexPointer(3, VertexPointerType.Float, 5 * sizeof(float), 0);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, meshes[i].vertexBufferObject);
                    GL.TexCoordPointer(2, TexCoordPointerType.Float, 5 * sizeof(float), 3 * sizeof(float));
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, meshes[i].elementBufferObject);
                }

                GL.DrawElements(PrimitiveType.Triangles, meshes[i].indices.Length, DrawElementsType.UnsignedInt, 0);
            }

        }


        #region DropToGround

        /// <summary>
        ///  retorna o novo Y (coloca o objeto rente ao chão mais proximo)
        /// </summary>
        /// <param name="pos">posição do objeto</param>
        /// <returns></returns>
        public float DropToGround(Vector3 pos)
        {
            List<float> found = new List<float>();

            for (int im = 0; im < meshes.Count; im++)
            {
                for (int j = 0; j < meshes[im].indices.Length; j += 3)
                {
                    tempTriangle temp;
                    uint index1 = 5 * meshes[im].indices[j];
                    uint index2 = 5 * meshes[im].indices[j + 1];
                    uint index3 = 5 * meshes[im].indices[j + 2];
                    int numVertices = meshes[im].vertices.Length;
                    if (index1 >= numVertices || index2 >= numVertices || index3 >= numVertices)
                    { continue; }
                    temp.a = new Vector3(meshes[im].vertices[index1] * 100f, meshes[im].vertices[index1 + 1] * 100f, meshes[im].vertices[index1 + 2] * 100f);
                    temp.b = new Vector3(meshes[im].vertices[index2] * 100f, meshes[im].vertices[index2 + 1] * 100f, meshes[im].vertices[index2 + 2] * 100f);
                    temp.c = new Vector3(meshes[im].vertices[index3] * 100f, meshes[im].vertices[index3 + 1] * 100f, meshes[im].vertices[index3 + 2] * 100f);
                    if (PointInTriangle(pos.Xz, temp.a.Xz, temp.b.Xz, temp.c.Xz))
                    {
                        found.Add(barryCentric(temp.a, temp.b, temp.c, pos));
                    }

                }
            }


            if (found.Count == 0)
               {return pos.Y; }

            int closest_index = 0;
            float closest_abs = 9999999.0f;
            // Console.WriteLine("Found " + found.Count + " triangles under position");
            for (int i = 0; i < found.Count; i++)
            {
                float abs = Math.Abs(pos.Y - found[i]);
                if (abs < closest_abs)
                {
                    closest_abs = abs;
                    closest_index = i;
                }
            }
            return found[closest_index];
        }

        private struct tempTriangle
        {
            public Vector3 a, b, c;
        }

        private static bool PointInTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            var s = p0.Y * p2.X - p0.X * p2.Y + (p2.Y - p0.Y) * p.X + (p0.X - p2.X) * p.Y;
            var t = p0.X * p1.Y - p0.Y * p1.X + (p0.Y - p1.Y) * p.X + (p1.X - p0.X) * p.Y;

            if ((s < 0) != (t < 0))
                return false;

            var A = -p1.Y * p2.X + p0.Y * (p2.X - p1.X) + p0.X * (p1.Y - p2.Y) + p1.X * p2.Y;
            if (A < 0.0)
            {
                s = -s;
                t = -t;
                A = -A;
            }
            return s > 0 && t > 0 && (s + t) <= A;
        }

        private static float barryCentric(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 pos)
        {
            float det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            float l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Z - p3.Z)) / det;
            float l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Z - p3.Z)) / det;
            float l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
        }

        #endregion
    }


}
