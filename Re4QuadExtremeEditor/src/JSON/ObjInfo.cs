using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re4QuadExtremeEditor.src.JSON
{

    public class ObjInfo
    {
        // id do objeto no jogo
        public ushort GameId { get; }
        // key do modelo para ser procurado a lista de modelos
        public string ModelKey { get; }
        // informa se esta usando um modelo carregado, ou um modelo 'interno'
        public bool UseInternalModel { get; }
        // nome para do objeto
        public string Name { get; set; }
        // descrição para o objeto
        public string Description { get; set; }

        public ObjInfo(ushort GameId, string ModelKey, bool UseInternalModel, string Name, string Description)
        {
            this.GameId = GameId;
            this.ModelKey = ModelKey;
            this.UseInternalModel = UseInternalModel;
            this.Name = Name;
            this.Description = Description;
        }

        public override string ToString()
        {
            return GameId.ToString("X4") + ": " + Name;
        }

        public override bool Equals(object obj)
        {
            return (obj is ObjInfo r && r.GameId == this.GameId);
        }
        public override int GetHashCode()
        {
            return GameId.GetHashCode();
        }

    }
}
