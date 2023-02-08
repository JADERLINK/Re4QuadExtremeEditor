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
    public class Model3D
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

        Vector3 center = new Vector3(0, 0, 0);
        Vector3 upper = new Vector3(0, 0, 0);
        Vector3 lower = new Vector3(0, 0, 0);
        public Vector3 UpperBoundary { get { return upper; } }
        public Vector3 LowerBoundary { get { return lower; } }
        public Vector3 CenterBoundary { get { return center; } }


        public List<MeshData> meshes;

        public string ModelKey {get;}

        /// <summary>
        /// nome da textura, diretorio da propria
        /// </summary>     
        public Dictionary<string, string> TexturesNamesList;


        public bool EnableBlend { get; }
        public bool EnableAlphaTest { get; }
        public float AlphaValue { get; }


        private void calculateCenter()
        {
            float max_x = 0, min_x = 0, max_y = 0, min_y = 0, max_z = 0, min_z = 0;
            uint count = 0;
            
            foreach (MeshData md in meshes)
            {
                uint offset = 0;
                for (int i = 0; i < md.vertices.Length / 5; i++)
                {
                    Vector3 vec = new Vector3(md.vertices[offset+0], md.vertices[offset+1], md.vertices[offset+2]);
                    if (count == 0)
                    {
                        min_x = vec.X; max_x = vec.X;
                        min_y = vec.Y; max_y = vec.Y;
                        min_z = vec.Z; max_z = vec.Z;
                    }
                    else
                    {
                        if (vec.X < min_x)
                          { min_x = vec.X; }
                        if (vec.X > max_x)
                          { max_x = vec.X; }
                        if (vec.Y < min_y)
                          { min_y = vec.Y; }
                        if (vec.Y > max_y)
                          { max_y = vec.Y; }
                        if (vec.Z < min_z)
                          { min_z = vec.Z; }
                        if (vec.Z > max_z)
                          { max_z = vec.Z; }
                    }
                    count++;
                    offset += 5;
                }
            }
            center = new Vector3((max_x+min_x) / 2, (max_y + min_y) / 2, (max_z + min_z) / 2);
            upper = new Vector3(max_x, max_y, max_z);
            lower = new Vector3(min_x, min_y, min_z);
        }

        public Model3D(ModelJson modeljson, string BaseDiretory)
        {
            ModelKey = modeljson.Modelkey;
            EnableBlend = modeljson.EnableBlend;
            EnableAlphaTest = modeljson.EnableAlphaTest;
            AlphaValue = modeljson.AlphaValue;
            meshes = new List<MeshData>();
            TexturesNamesList = new Dictionary<string, string>();

            // arquivos pmds
            Dictionary<string, PMD> pmds = new Dictionary<string, PMD>();
            // diretorios dos arquivos
            Dictionary<string, string> pmdDiretory = new Dictionary<string, string>();

            // carrega os pmds
            foreach (var item in modeljson.PmdList)
            {

                // obtem o caminho completo do diretorio do arquivo PMD
                string PmdDiretory = BaseDiretory + item.Key;

                // verifica se o diretorio dos arquivos PMD existe
                if (Directory.Exists(PmdDiretory))
                {
                    
                    // obtem o caminho do arquivo
                    string PmdPath = PmdDiretory + item.Value;

                    if (File.Exists(PmdPath))
                    {
                        try
                        {
                            pmds.Add(item.Value, PmdDecoder.GetPMD(PmdPath));
                            pmdDiretory.Add(item.Value, item.Key);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

            }

            var PmdKeys = pmds.Keys.ToArray();

            // parte das texturas
            // diretory, texture name
            HashSet<KeyValuePair<string, string>> TGAs = new HashSet<KeyValuePair<string, string>>();
            for (int i = 0; i < PmdKeys.Length; i++)
            {
                for (int it = 0; it < pmds[PmdKeys[i]].TextureNames.Length; it++)
                {
                    if (!modeljson.BlackListTextures.Contains(pmds[PmdKeys[i]].TextureNames[it]) && pmds[PmdKeys[i]].TextureNames[it].Length != 0)
                    {
                        TGAs.Add(new KeyValuePair<string, string>(pmdDiretory[PmdKeys[i]], pmds[PmdKeys[i]].TextureNames[it]));
                    }
                }
            }

            var TGAnames = TGAs.ToArray();
            for (int i = 0; i < TGAnames.Length; i++)
            {
                string TgaPath = BaseDiretory + TGAnames[i].Key + TGAnames[i].Value;
                if (File.Exists(TgaPath) && !TexturesNamesList.ContainsKey(TGAnames[i].Value))
                {
                    TexturesNamesList.Add(TGAnames[i].Value, TGAnames[i].Key);
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

                if (modeljson.FixPmds.ContainsKey(Pmdname))
                {
                    fix = modeljson.FixPmds[Pmdname];
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
                            point = point * ry * rx * rz;

                            int iS = pmds[Pmdname].Nodes[iN].SkeletonIndex;

                            vertices[vOffset + 0] = point.X
                                * pmds[Pmdname].Skeleton[iS][0]
                                * fix.Scale.X
                                + pmds[Pmdname].Skeleton[iS][7]
                                + fix.Position.X;
                            vertices[vOffset + 1] = point.Y
                                * pmds[Pmdname].Skeleton[iS][1]
                                * fix.Scale.Y
                                + pmds[Pmdname].Skeleton[iS][8]
                                + fix.Position.Y;
                            vertices[vOffset + 2] = point.Z
                                * pmds[Pmdname].Skeleton[iS][2]
                                * fix.Scale.Z
                                + pmds[Pmdname].Skeleton[iS][9]
                                + fix.Position.Z;
                            vertices[vOffset + 3] = pmds[Pmdname].Nodes[iN].Meshs[iM].Vertexs[iv].tu;
                            vertices[vOffset + 4] = pmds[Pmdname].Nodes[iN].Meshs[iM].Vertexs[iv].tv;
                            vOffset += 5;
                        }

                        MeshData m = new MeshData();
                        m.vertices = vertices;
                        m.indices = indices;
                        m.texturename = null;
                        if (TexturesNamesList.ContainsKey(pmds[Pmdname].TextureNames[pmds[Pmdname].Nodes[iN].Meshs[iM].TextureIndex]))
                        {
                            m.texturename = pmds[Pmdname].TextureNames[pmds[Pmdname].Nodes[iN].Meshs[iM].TextureIndex];
                        }
                        if (modeljson.BlackListTextures.Contains(pmds[Pmdname].TextureNames[pmds[Pmdname].Nodes[iN].Meshs[iM].TextureIndex])
                            || pmds[Pmdname].TextureNames[pmds[Pmdname].Nodes[iN].Meshs[iM].TextureIndex].Length == 0)
                        {
                            m.texturename = Consts.TransparentTextureName;
                        }
                        meshes.Add(m);
                    }

                }
            }

            // fim da parte de converter

            // verifica o centro
            calculateCenter();

            //carrega para o GL
            DataBase.ShaderObjs.Use();
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

            meshes.Clear();
            TexturesNamesList.Clear();
        }


        public override bool Equals(object obj)
        {
            return (obj is Model3D m && m.ModelKey == ModelKey);
        }
        public override int GetHashCode()
        {
            return ModelKey.GetHashCode();
        }
        public override string ToString()
        {
            return ModelKey;
        }
    }
}
