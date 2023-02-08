using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re4QuadExtremeEditor.src.JSON
{

    public class ModelJson
    {
        // nome do model, para referencia
        public string Modelkey { get; }

        // se ativa  GL.Enable(EnableCap.Blend);
        public bool EnableBlend { get; set; }

        // se ativa GL.Enable(EnableCap.AlphaTest);
        public bool EnableAlphaTest { get; set; }

        // define valor de GL.AlphaFunc(AlphaFunction.Gequal, value);
        public float AlphaValue { get; set; }

        // lista dos nomes dos arquivos pmd a serem carregados
        // diretorio, nome do arquivo  || foi feito assim para poder pegar o diretorio para a textura
        public List<KeyValuePair<string, string>> PmdList { get; set; }
        // unload texture, lista de texturas a não serem carregadas
        public List<string> BlackListTextures { get; set; }
        //pmd file patch, FixPmd class // para mover, girar e escalonar o pmd
        public Dictionary<string, FixPmd> FixPmds { get; set; }

        public ModelJson(string Modelkey)
        {
            this.Modelkey = Modelkey;
            EnableBlend = false;
            EnableAlphaTest = false;
            AlphaValue = 0;
            PmdList = new List<KeyValuePair<string, string>>();
            BlackListTextures = new List<string>();
            FixPmds = new Dictionary<string, FixPmd>();
        }

    }
}
