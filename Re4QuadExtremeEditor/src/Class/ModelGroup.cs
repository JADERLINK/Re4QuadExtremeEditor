using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Re4QuadExtremeEditor.src.JSON;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace Re4QuadExtremeEditor.src.Class
{
    public class ModelGroup
    {
        public string ModelGroupName { get; }

        public Dictionary<string, Model3D> Models { get;}

        //texturas dos modelos
        private Dictionary<string, Bitmap> textures;
        private Dictionary<string, int> texturesGL;

        public ModelGroup(string ModelGroupName, string ModelsDiretory, string ModelListJsonPath, string BaseDiretory) 
        {
            this.ModelGroupName = ModelGroupName;

            Models = new Dictionary<string, Model3D>();
            textures = new Dictionary<string, Bitmap>();
            texturesGL = new Dictionary<string, int>();

            if (File.Exists(ModelListJsonPath) && Directory.Exists(ModelsDiretory))
            {
             
                string[] list = new string[0];
                try{ list = ModelListJsonFile.parseModelListJsonFile(ModelListJsonPath);}catch (Exception){ }

                // carrega os modelos
                foreach (var item in list)
                {
                    string ModelPath = ModelsDiretory + item;

                    if (File.Exists(ModelPath))
                    {
                        ModelJson modelJson = null;
                        Model3D m3d = null;
                        try { modelJson = ModelJsonFile.parseModelJson(ModelPath); } catch (Exception) { }
                        if (modelJson != null)
                        {
                            m3d = new Model3D(modelJson, BaseDiretory);
                        }
                        if (m3d != null && m3d.meshes.Count != 0 && !Models.ContainsKey(m3d.ModelKey))
                        {
                            Models.Add(m3d.ModelKey, m3d);
                        } 
                    }
                }

                // carrega as texturas
                foreach (var item in Models)
                {
                    foreach (var tex in item.Value.TexturesNamesList)
                    {
                        string TgaPath = BaseDiretory + tex.Value + tex.Key;
                        if (File.Exists(TgaPath))
                        {
                            try
                            {
                                TGASharpLib.TGA nTGA = new TGASharpLib.TGA(TgaPath);
                                if (!textures.ContainsKey(tex.Key))
                                {
                                    textures.Add(tex.Key, nTGA.ToBitmap());
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }

                // coloca as texturas no GL
                var textureNames = textures.Keys.ToArray();
                for (int i = 0; i < textureNames.Length; i++)
                {
                    texturesGL.Add(textureNames[i], Texture.GetTextureIdGL(textures[textureNames[i]]));
                }

            }

        }

        public void RenderModel(string ModelKey)
        {
            if (Models.ContainsKey(ModelKey))
            {
                Model3D model = Models[ModelKey];

                if (model.EnableBlend)
                {
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                }
                if (model.EnableAlphaTest)
                {
                    GL.Enable(EnableCap.AlphaTest);
                    GL.AlphaFunc(AlphaFunction.Gequal, model.AlphaValue); //Gequal
                }
                
                GL.ActiveTexture(TextureUnit.Texture0);

                if (Globals.UseOldGL)
                {
                    GL.EnableClientState(ArrayCap.VertexArray);
                    GL.EnableClientState(ArrayCap.TextureCoordArray);
                }


                for (int i = 0; i < model.meshes.Count; i++)
                {
                    if (!Globals.UseOldGL)
                    {
                        GL.BindVertexArray(model.meshes[i].vertexArrayObject);
                    }

                    if (model.meshes[i].texturename != null && texturesGL.ContainsKey(model.meshes[i].texturename))
                    {
                        GL.BindTexture(TextureTarget.Texture2D, texturesGL[model.meshes[i].texturename]);
                    }
                    else if (model.meshes[i].texturename == Consts.TransparentTextureName)
                    {
                        GL.BindTexture(TextureTarget.Texture2D, DataBase.TransparentTextureIdGL);
                    }
                    else
                    {
                        GL.BindTexture(TextureTarget.Texture2D, DataBase.NoTextureIdGL);
                    }

                    if (Globals.UseOldGL)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, model.meshes[i].vertexBufferObject);
                        GL.VertexPointer(3, VertexPointerType.Float, 5 * sizeof(float), 0);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, model.meshes[i].vertexBufferObject);
                        GL.TexCoordPointer(2, TexCoordPointerType.Float, 5 * sizeof(float), 3 * sizeof(float));
                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, model.meshes[i].elementBufferObject);
                    }


                    GL.DrawElements(PrimitiveType.Triangles, model.meshes[i].indices.Length, DrawElementsType.UnsignedInt, 0);
                }

                if (Globals.UseOldGL)
                {
                    GL.DisableClientState(ArrayCap.VertexArray);
                    GL.DisableClientState(ArrayCap.TextureCoordArray);
                }

                GL.Disable(EnableCap.AlphaTest);
                GL.Disable(EnableCap.Blend);
            }
        }

        public void ClearGL() 
        {
            foreach (var item in texturesGL)
            {
                GL.DeleteBuffer(item.Value);
            }

            foreach (var item in Models)
            {
                item.Value.ClearGL();
            }

            textures.Clear();
            texturesGL.Clear();
            Models.Clear();
        }


        public override bool Equals(object obj)
        {
            return (obj is ModelGroup mg && mg.ModelGroupName == ModelGroupName);
        }
        public override int GetHashCode()
        {
            return ModelGroupName.GetHashCode();
        }
        public override string ToString()
        {
            return ModelGroupName;
        }

    }
}
