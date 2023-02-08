using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_API
{
    public class PMD
    {
        public string[] SkeletonNames;
        public string[] MeshNames;
        public string[] TextureNames;
        public int[] Parents;
        public float[][] Skeleton;
        public float[][] TextureData;
        public Dictionary<string, int> ObjBones;
        public PMDnode[] Nodes;
    }

    public class PMDnode 
    {
        public PMDbone[] Bones;
        public PMDmesh[] Meshs;
        public int SkeletonIndex;
        public int ObjId;
    }

    public class PMDmesh 
    {
        public PMDvertex[] Vertexs;
        public uint[] Orders;
        public int TextureIndex;
    }


    public class PMDvertex
    {
        public float x;
        public float z;
        public float y;
        public float w0;
        public float w1;
        public float i0;
        public float i1;
        public float nx;
        public float nz;
        public float ny;
        public float tu;
        public float tv;
        public float r;
        public float g;
        public float b;
        public float a;
    }

    public class PMDbone
    {
        public int boneId;
        public float[] unknown;
        public float x;
        public float z;
        public float y;
        public float unknown16;
    }
}