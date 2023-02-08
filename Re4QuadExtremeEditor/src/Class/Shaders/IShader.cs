using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Re4QuadExtremeEditor.src.Class.Shaders
{
    public interface IShader
    {
        /// <summary>
        /// seleciona o shader, somente para o modern OpenGL
        /// </summary>
        void Use();

        int GetAttribLocation(string attribName);

        void SetInt(string name, int data);

        void SetFloat(string name, float data);

        void SetMatrix4(string name, Matrix4 data);

        void SetVector3(string name, Vector3 data);

        void SetVector4(string name, Vector4 data);

        void SetAltRotation(OldRotation angles);

        /// <summary>
        /// define as variais para a renderização do Old OpenGL, para o Modern é o mesmo que Use();
        /// </summary>
        void Start();
    }
}
