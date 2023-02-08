using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Re4QuadExtremeEditor.src.Class.Shaders
{

    public class NoShaderRoom : IShader
    {

        public NoShaderRoom() 
        {
            // nothing
        }

        public int GetAttribLocation(string attribName)
        {
            return -1;
        }

        public void SetFloat(string name, float data)
        {
            // nothing
        }

        public void SetInt(string name, int data)
        {
            // nothing
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            if (name == "view")
            {
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref data);
            }
            else if (name == "projection")
            {
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref data);
            }
        }

        public void SetVector3(string name, Vector3 data)
        {
            // nothing
        }

        public void SetVector4(string name, Vector4 data)
        {
            // nothing
        }

        public void Use()
        {
            // nothing
        }

        public void Start() 
        {
            GL.Translate(0,0,0);
            GL.Rotate(0,0,0,0);
            GL.Scale(1,1,1);
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public void SetAltRotation(OldRotation angles)
        {
            // nothing
        }
    }

    public class NoShaderObjs : IShader
    {
        private OldRotation OldRotation = OldRotation.Identity;
        private Matrix4 view = Matrix4.Identity;
        private Matrix4 projection = Matrix4.Identity;
        private Vector3 mScale = Vector3.One;
        private Vector3 mPosition = Vector3.Zero;

        public NoShaderObjs() 
        {
            // nothing
        }

        public int GetAttribLocation(string attribName)
        {
            return -1;
        }

        public void SetFloat(string name, float data)
        {
            // nothing
        }

        public void SetInt(string name, int data)
        {
            // nothing
        }

        public void SetMatrix4(string name, Matrix4 data)
        {   
            if (name == "view")
            {
                view = data;
            }
            else if (name == "projection")
            {
                projection = data;
            }
        }

        public void SetVector3(string name, Vector3 data)
        { 
            if (name == "mScale")
            {
                mScale = data;
            }
            else if (name == "mPosition")
            {
                mPosition = data;
            }            
        }

        public void SetVector4(string name, Vector4 data)
        {
            // nothing
        }

        public void Use()
        {
            // nothing
        }

        public void Start()
        {
          
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref view);
            

            GL.Translate(mPosition);

            // aqui a ordem dos GL.Rotate é invertida ao que esta sendo dito, pois é invertido em relação as multipicações das matriz de rotação.
            switch (OldRotation.GetObjRotationOrder)
            {
                case Enums.ObjRotationOrder.RotationXYZ:
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    break;
                case Enums.ObjRotationOrder.RotationXZY:
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    break;
                case Enums.ObjRotationOrder.RotationYXZ:
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    break;
                case Enums.ObjRotationOrder.RotationYZX:
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    break;
                case Enums.ObjRotationOrder.RotationZYX:
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    break;
                case Enums.ObjRotationOrder.RotationZXY:
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    break;
                case Enums.ObjRotationOrder.RotationXY:
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    break;
                case Enums.ObjRotationOrder.RotationXZ:
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    break;
                case Enums.ObjRotationOrder.RotationYX:
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    break;
                case Enums.ObjRotationOrder.RotationYZ:
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    break;
                case Enums.ObjRotationOrder.RotationZX:
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    break;
                case Enums.ObjRotationOrder.RotationZY:
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    break;
                case Enums.ObjRotationOrder.RotationX:
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    break;
                case Enums.ObjRotationOrder.RotationY:
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    break;
                case Enums.ObjRotationOrder.RotationZ:
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    break;
                case Enums.ObjRotationOrder.NoRotation:
                default:
                    GL.Rotate(0, 0, 0, 0);
                    break;
            }

            GL.Scale(mScale);
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
        }


        public void SetAltRotation(OldRotation angles)
        {
            OldRotation = angles;
        }
    }

    public class NoShaderBoundingBox : IShader
    {
        private OldRotation OldRotation = OldRotation.Identity;
        private Matrix4 view = Matrix4.Identity;
        private Matrix4 projection = Matrix4.Identity;
        private Vector3 mScale = Vector3.One;
        private Vector3 mPosition = Vector3.Zero;
        private Vector4 mColor = Vector4.One;

        public NoShaderBoundingBox()
        {
            // nothing
        }

        public int GetAttribLocation(string attribName)
        {
            return -1;
        }

        public void SetFloat(string name, float data)
        {
            // nothing
        }

        public void SetInt(string name, int data)
        {
            // nothing
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            if (name == "view")
            {
               view = data;
            }
            else if (name == "projection")
            {
                projection = data;
            }
        }

        public void SetVector3(string name, Vector3 data)
        {
            if (name == "mScale")
            {
                mScale = data;
            }
            else if (name == "mPosition")
            {
                mPosition = data;
            }
        }

        public void SetVector4(string name, Vector4 data)
        {
            if (name == "mColor")
            {
                mColor = data;
            }
        }

        public void Use()
        {
        }
        public void Start()
        {

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref view);
            

            GL.Translate(mPosition);

            switch (OldRotation.GetObjRotationOrder)
            {
                case Enums.ObjRotationOrder.RotationXYZ:
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    break;
                case Enums.ObjRotationOrder.RotationXZY:
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    break;
                case Enums.ObjRotationOrder.RotationYXZ:
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    break;
                case Enums.ObjRotationOrder.RotationYZX:
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    break;
                case Enums.ObjRotationOrder.RotationZYX:
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    break;
                case Enums.ObjRotationOrder.RotationZXY:
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    break;
                case Enums.ObjRotationOrder.RotationXY:
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    break;
                case Enums.ObjRotationOrder.RotationXZ:
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    break;
                case Enums.ObjRotationOrder.RotationYX:
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    break;
                case Enums.ObjRotationOrder.RotationYZ:
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    break;
                case Enums.ObjRotationOrder.RotationZX:
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    break;
                case Enums.ObjRotationOrder.RotationZY:
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    break;
                case Enums.ObjRotationOrder.RotationX:
                    GL.Rotate(OldRotation.XangleDegrees, 1, 0, 0);
                    break;
                case Enums.ObjRotationOrder.RotationY:
                    GL.Rotate(OldRotation.YangleDegrees, 0, 1, 0);
                    break;
                case Enums.ObjRotationOrder.RotationZ:
                    GL.Rotate(OldRotation.ZangleDegrees, 0, 0, 1);
                    break;
                case Enums.ObjRotationOrder.NoRotation:
                default:
                    GL.Rotate(0, 0, 0, 0);
                    break;
            }

            GL.Scale(mScale);
            GL.Color4(mColor);
        }


        public void SetAltRotation(OldRotation angles)
        {
            OldRotation = angles;
        }
    }
}
