using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re4QuadExtremeEditor.src
{
    /// <summary>
    /// todas as constantes do programa
    /// </summary>
    public static class Consts
    {
        #region Program Directories
        // lista de de diretorios do programa
        // diretorio principal
        public const string dataDiretory = @"data\";
        // diretorio do arquivo de comfigurações
        public const string ConfigsFileDiretory = @"data\Configs.json";
        // diretorio da lista de Rooms
        public const string RoomListFileDiretory = @"data\RoomList.json";
        // diretorio de onde fica os arquivos RoomJson
        public const string RoomJsonFilesDiretory = @"data\RoomJson\";
        // diretorio da lista de model de itens
        public const string ItemsModelsListFileDiretory = @"data\ItemsModelsList.json";
        // diretorio da lista de model de etcmodel
        public const string EtcModelsListFileDiretory = @"data\EtcModelsList.json";
        // diretorio da lista de model dos inimigos
        public const string EnemiesModelsListFileDiretory = @"data\EnemiesModelsList.json";
        // diretorio da lista de model dos objetos internos
        public const string InternalModelsListFileDiretory = @"data\InternalModelsList.json";
        // diretorio de onde fica os arquivos ModelJson dos itens
        public const string ItemsModelsJsonFilesDiretory = @"data\ItemsModelsJson\";
        // diretorio de onde fica os arquivos ModelJson dos itens
        public const string EtcModelsJsonFilesDiretory = @"data\EtcModelsJson\";
        // diretorio de onde fica os arquivos ModelJson dos itens
        public const string EnemiesModelsJsonFilesDiretory = @"data\EnemiesModelsJson\";
        // diretorio de onde fica os arquivos ModelJson dos objetos internos 
        public const string InternalModelsJsonFilesDiretory = @"data\InternalModelsJson\";
        // diretorio da lista de ObjInfo de itens
        public const string ItemsObjInfoListFileDiretory = @"data\ItemsObjInfoList.json";
        // diretorio da lista de ObjInfo de etcmodel
        public const string EtcModelObjInfoListFileDiretory = @"data\EtcModelObjInfoList.json";
        // diretorio da lista de ObjInfo dos inimigos
        public const string EnemiesObjInfoListFileDiretory = @"data\EnemiesObjInfoList.json";

        // diretorio da lista de PromptMessages
        public const string PromptMessageListFileDiretory = @"data\PromptMessageList.json";

        // diretorio da pasta de linguagens
        public const string langDiretory = @"lang\";
        // diretorio da lista de linguagens, para selecionar
        public const string LangListFileDiretory = @"lang\LangList.json";

        #endregion

        // nome da textura transparente
        public const string TransparentTextureName = "TransparentTexture";

        // nome para os grupos
        public const string ItemsModelGroupName = "ItemsModelGroupName";
        public const string EtcModelGroupName = "EtcModelGroupName";
        public const string EnemiesModelGroupName = "EnemiesModelGroupName";
        public const string InternalModelGroupName = "InternalModelGroupName";

        // nomes dos nodes principais
        public const string NodeESL = "NodeESL";
        public const string NodeETS = "NodeETS";
        public const string NodeITA = "NodeITA";
        public const string NodeAEV = "NodeAEV";
        public const string NodeEXTRAS = "NodeEXTRAS";


        // nomes dos modelos internos usadas nos objetos extras
        public const string ModelKeyWarpPoint = "WarpArrow";
        public const string ModelKeyLadderPoint = "LadderX";
        public const string ModelKeyLadderObj = "Ladder";
        public const string ModelKeyLadderError = "LadderError";
        public const string ModelKeyAshleyPoint = "TextureX";
        public const string ModelKeyGrappleGunPoint = "GrappleGunArrow";
        public const string ModelKeyLocalTeleportationPoint = "LocalTeleportationArrow";


        //o meu maior float
        public const float MyFloatMax = 1000000000000000000000000000f; // 000 000
    }
}
