using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re4QuadExtremeEditor.src.JSON
{
    public class RoomInfo
    {
        // key que será usado no Dicionary
        public string RoomKey { get; }
        // Id da room no jogo
        public int RoomId { get; }
        // lista dos nomes dos arquivos do tipo RoomJson
        public string[] RoomJsonFiles { get; }
        // nome da Room
        public string Name { get; set; }
        // Descrição da mesma
        public string Description { get; set; }
        
        public RoomInfo(string RoomKey, int RoomId, string[] RoomJsonFiles, string Name, string Description) 
        {
            this.RoomKey = RoomKey;
            this.RoomId = RoomId;
            this.RoomJsonFiles = RoomJsonFiles;
            this.Name = Name;
            this.Description = Description;
        }

        public override string ToString()
        {
            return Name + " = " + Description;
        }

        public override bool Equals(object obj)
        {
            return (obj is RoomInfo r && r.RoomKey == this.RoomKey);
        }
        public override int GetHashCode()
        {
            return RoomKey.GetHashCode();
        }
    }
}
