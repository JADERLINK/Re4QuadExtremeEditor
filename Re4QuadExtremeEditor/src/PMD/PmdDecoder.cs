using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PMD_API
{
    public static class PmdDecoder
    {
        public static PMD GetPMD(string FilePath) 
        {
            PMD pmd = null;

            FileStream SRC = new FileStream(FilePath, FileMode.Open);

            SRC.Read(new byte[58], 0, 58);

            byte[] temp = new byte[4];
            SRC.Read(temp, 0, 4); // Quantidade de nome de Nodes
            int SkeletonName_length = BitConverter.ToInt32(temp, 0);

            string[] SkeletonNames = new string[SkeletonName_length];

            for (int i = 0; i < SkeletonName_length; i++)
            {
                string name;
                int namelen;

                temp = new byte[4];
                SRC.Read(temp, 0, 4); //aqui contem o valor que reprensenta o tamanho da string
                namelen = BitConverter.ToInt32(temp, 0);

                temp = new byte[namelen];
                SRC.Read(temp, 0, namelen); // nome
                name = Encoding.UTF8.GetString(temp, 0, namelen);

                temp = new byte[4];
                SRC.Read(temp, 0, 4);// # conteudo int, representa o id do nome (vem depois no nome)
                SkeletonNames[BitConverter.ToInt32(temp, 0)] = name;
            }

            temp = new byte[4];
            SRC.Read(temp, 0, 4);// # tamanho da quantidade de nome de Mesh
            int MeshName_length = BitConverter.ToInt32(temp, 0);

            string[] MeshNames = new string[MeshName_length];

            for (int i = 0; i < MeshName_length; i++) // o mesmo que o de cima so que para as mesh
            {
                string name;
                int namelen;

                temp = new byte[4];
                SRC.Read(temp, 0, 4);
                namelen = BitConverter.ToInt32(temp, 0);

                temp = new byte[namelen];
                SRC.Read(temp, 0, namelen); // nome
                name = Encoding.UTF8.GetString(temp, 0, namelen);

                temp = new byte[4];
                SRC.Read(temp, 0, 4);//
                MeshNames[BitConverter.ToInt32(temp, 0)] = name;
            }

            temp = new byte[4];
            SRC.Read(temp, 0, 4); //    # zeros
            SRC.Read(temp, 0, 4); // quantidade de skeletons
            int skeleton_length = BitConverter.ToInt32(temp, 0); // quantidade de skeletons

            float[][] Skeleton = new float[skeleton_length][];
            int[] Parents = new int[skeleton_length];

            for (int i = 0; i < skeleton_length; i++)
            {
                temp = new byte[4];
                SRC.Read(temp, 0, 4);
                int parent = BitConverter.ToInt32(temp, 0);

                Skeleton[i] = new float[26];

                for (int iv = 0; iv < 26; iv++)
                {
                    temp = new byte[4];
                    SRC.Read(temp, 0, 4);
                    Skeleton[i][iv] = BitConverter.ToSingle(temp, 0);

                }

                //Skeleton[i][9] = -Skeleton[i][9];
                Parents[i] = parent;


                // legenda
                //  Skeleton[skeleton][floats]
                //  Skeleton[i][7] = x
                //  Skeleton[i][9] = y
                //  Skeleton[i][8] = z
            }

            temp = new byte[4];
            SRC.Read(temp, 0, 4);
            int obj_length = BitConverter.ToInt32(temp, 0);

            PMDnode[] nodes = new PMDnode[obj_length];

            Dictionary<string, int> ObjBones = new Dictionary<string, int>();

            for (int obj_i = 0; obj_i < obj_length; obj_i++)
            {
                temp = new byte[4];
                SRC.Read(temp, 0, 4);
                int Skeleton_i = BitConverter.ToInt32(temp, 0);

                SRC.Read(new byte[32], 0, 32); //     # all zeros

                temp = new byte[4];
                SRC.Read(temp, 0, 4);
                int mesh_lenght = BitConverter.ToInt32(temp, 0);

                nodes[obj_i] = new PMDnode();
                nodes[obj_i].Meshs = new PMDmesh[mesh_lenght];

                for (int im = 0; im < mesh_lenght; im++)
                {
                    nodes[obj_i].Meshs[im] = new PMDmesh();
                    temp = new byte[4];
                    SRC.Read(temp, 0, 4);
                    nodes[obj_i].Meshs[im].TextureIndex = BitConverter.ToInt32(temp, 0);
                }

                temp = new byte[4];
                SRC.Read(temp, 0, 4); // # zeros
                SRC.Read(temp, 0, 4); // # number of meshes again 

                nodes[obj_i].SkeletonIndex = Skeleton_i;
                nodes[obj_i].ObjId = obj_i;

                for (int im = 0; im < mesh_lenght; im++)
                {
                    temp = new byte[4];
                    SRC.Read(temp, 0, 4); // # 40 00 00 00

                    if (BitConverter.ToInt32(temp, 0) != 64)
                    {
                        continue;
                    }

                    temp = new byte[4];
                    SRC.Read(temp, 0, 4);
                    int groupsize = BitConverter.ToInt32(temp, 0); 

                    temp = new byte[(groupsize + 1) * 2];
                    SRC.Read(temp, 0, (groupsize + 1) * 2); //  # predata

                    HashSet<ushort> IdExisting = new HashSet<ushort>();
                    uint[] Order = new uint[groupsize];

                    int offset = 0;
                    for (int i = 0; i < (groupsize); i++)
                    {
                        ushort id = BitConverter.ToUInt16(temp, offset);
                        IdExisting.Add(id);
                        Order[i] = id;
                        offset += 2;
                    }


                    temp = new byte[2];
                    SRC.Read(temp, 0, 2); // # 2 zeros

                    PMDvertex[] groupvertices = new PMDvertex[IdExisting.Count];

                    for (int i = 0; i < IdExisting.Count; i++) // o certo
                    {
                        byte[] vertices = new byte[64];
                        SRC.Read(vertices, 0, 64); //# create array of vertice data

                        PMDvertex v = new PMDvertex();
                        v.x = BitConverter.ToSingle(vertices, 0);
                        v.y = BitConverter.ToSingle(vertices, 4);
                        v.z = BitConverter.ToSingle(vertices, 8);
                        v.w0 = BitConverter.ToSingle(vertices, 12);
                        v.w1 = BitConverter.ToSingle(vertices, 16);
                        v.i0 = BitConverter.ToSingle(vertices, 20);
                        v.i1 = BitConverter.ToSingle(vertices, 24);
                        v.nx = BitConverter.ToSingle(vertices, 28);
                        v.ny = BitConverter.ToSingle(vertices, 32);
                        v.nz = BitConverter.ToSingle(vertices, 36);
                        v.tu = BitConverter.ToSingle(vertices, 40);
                        v.tv = BitConverter.ToSingle(vertices, 44);
                        v.r = BitConverter.ToSingle(vertices, 48);
                        v.g = BitConverter.ToSingle(vertices, 52);
                        v.b = BitConverter.ToSingle(vertices, 56);
                        v.a = BitConverter.ToSingle(vertices, 60);
                        //v.y = -v.y;
                        //v.ny = -v.ny;
                        groupvertices[i] = v;
                    }
                    nodes[obj_i].Meshs[im].Vertexs = groupvertices;
                    nodes[obj_i].Meshs[im].Orders = Order;

                }


                temp = new byte[4];
                SRC.Read(temp, 0, 4);
                int num_bones = BitConverter.ToInt32(temp, 0);

                nodes[obj_i].Bones = new PMDbone[num_bones];
                for (int i = 0; i < num_bones; i++)
                {
                    PMDbone b = new PMDbone();

                    temp = new byte[4];
                    SRC.Read(temp, 0, 4);
                    int boneid = BitConverter.ToInt32(temp, 0);
                    b.boneId = boneid;

                    temp = new byte[52];
                    SRC.Read(temp, 0, 52); // # unused

                    b.unknown = new float[13];
                    int offset = 0;
                    for (int ib = 0; ib < 13; ib++)
                    {
                        b.unknown[ib] = BitConverter.ToSingle(temp, offset);
                        offset += 4;
                    }

                    byte[] bone = new byte[16];
                    SRC.Read(bone, 0, 16); //   # bone's xyz, but redundant

                    b.x = BitConverter.ToSingle(bone, 0);
                    b.z = BitConverter.ToSingle(bone, 4);
                    b.y = BitConverter.ToSingle(bone, 8);
                    b.unknown16 = BitConverter.ToSingle(bone, 12);
                    //b.y = -b.y;
                    //bones[obj_i][i] = b;
                    nodes[obj_i].Bones[i] = b;

                    ObjBones.Add(obj_i + "_" + (3 * i), boneid);
                }

            }

            temp = new byte[4];
            SRC.Read(temp, 0, 4); // # number of textures
            int numberOfTextures = BitConverter.ToInt32(temp, 0);
            string[] TextureNames = new string[numberOfTextures];
            float[][] TextureData = new float[numberOfTextures][];
            for (int i = 0; i < numberOfTextures; i++)
            {
                temp = new byte[72];
                SRC.Read(temp, 0, 72); // # texture data + other

                TextureData[i] = new float[18];
                int offset = 0;
                for (int f = 0; f < 18; f++)
                {
                    TextureData[i][f] = BitConverter.ToSingle(temp, offset);
                    offset += 4;
                }

                temp = new byte[4];
                SRC.Read(temp, 0, 4); //  # filename size
                int texNameLenght = BitConverter.ToInt32(temp, 0);
                temp = new byte[texNameLenght];
                SRC.Read(temp, 0, texNameLenght);
                TextureNames[i] = Encoding.UTF8.GetString(temp, 0, texNameLenght);
            }

            SRC.Close();

            pmd = new PMD();
            pmd.MeshNames = MeshNames;
            pmd.SkeletonNames = SkeletonNames;
            pmd.Parents = Parents;
            pmd.Skeleton = Skeleton;
            pmd.ObjBones = ObjBones;
            pmd.TextureNames = TextureNames;
            pmd.Nodes = nodes;
            pmd.TextureData = TextureData;

            return pmd;
        }



    }
}
