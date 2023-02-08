using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Re4QuadExtremeEditor.src.JSON
{
    public class RoomJson
    {
        // Nome do arquivo Json
        public string JsonName { get; }
        // em qual pasta esta os arquivos
        public string RoomFolder { get; set; }
        // lista dos nomes dos arquivos pmd a serem carregados
        public List<string> PmdList { get; set; }
        // unload texture // texturas que não são carregadas
        public List<string> BlackListTextures { get; set; }
        //pmd file patch, FixPmd class
        // corrige a posição, escala e rotação;
        public Dictionary<string, FixPmd> FixPmds { get; set; }

        public RoomJson(string JsonName)
        {
            this.JsonName = JsonName;
            PmdList = new List<string>();
            BlackListTextures = new List<string>();
            FixPmds = new Dictionary<string, FixPmd>();
        }
    }

    public struct FixPmd
    {
        public Vector3 Rotation { get; }
        public Vector3 Position { get; }
        public Vector3 Scale { get; }

        public FixPmd(Vector3 Rotation, Vector3 Position, Vector3 Scale) 
        {
            this.Rotation = Rotation;
            this.Position = Position;
            this.Scale = Scale;
        }
    }
}
