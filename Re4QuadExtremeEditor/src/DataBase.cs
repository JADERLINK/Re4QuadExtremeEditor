using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Re4QuadExtremeEditor.src.JSON;
using Re4QuadExtremeEditor.src.Class;
using Re4QuadExtremeEditor.src.Class.TreeNodeObj;
using Re4QuadExtremeEditor.src.Class.Files;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.Shaders;

namespace Re4QuadExtremeEditor.src
{
    /// <summary>
    /// Contem todo o conteudo da modelagem(3d), os conteudos dos arquivos AEV, ESL, ETS e ITA, e as definições dos objetos;
    /// </summary>
    public static class DataBase
    {
        // representa a lista de mapas para serem selecionados
        // poderia carregar na classe SelectRoom, porem deixo aqui para carregar a tradução somente uma vez.
        public static List<RoomInfo> RoomList = new List<RoomInfo>();

        //representa a Room (cenario) selecionada, e a ser renderizada
        public static Room SelectedRoom = null;

        //shaders
        public static IShader ShaderRoom = null;
        public static IShader ShaderObjs = null;
        public static IShader ShaderBoundingBox = null;

        // texturas padrões
        public static int NoTextureIdGL;
        public static int TransparentTextureIdGL;
        public static int SolidTextureIdGL;

        // os grupos de objetos presentes no programa
        public static ModelGroup EnemiesModels; // modelos dos inimigos
        public static ModelGroup ItemsModels; // modelos dos itens
        public static ModelGroup EtcModels; // modelos da pasta "etcmodel"
        public static ModelGroup InternalModels; // modelos proprios pra o programa



        // Dicionarios com os ids dos objetos no jogo
        public static Dictionary<ushort, ObjInfo> EnemiesIDs;
        public static Dictionary<ushort, ObjInfo> ItemsIDs;
        public static Dictionary<ushort, ObjInfo> EtcModelIDs;


        // aqui são os objetos que representa os arquivos no programa
        public static FileEnemyEslGroup FileESL;
        public static FileEtcModelEtsGroup FileETS;
        public static FileSpecialGroup FileITA;
        public static FileSpecialGroup FileAEV;
        public static ExtraGroup Extras;

        //conteudo do treeview
        public static EnemyNodeGroup NodeESL;
        public static EtcModelNodeGroup NodeETS;
        public static SpecialNodeGroup NodeITA;
        public static SpecialNodeGroup NodeAEV;
        public static ExtraNodeGroup NodeEXTRAS;

        // lista de objetos selecionados na treeview
        public static List<TreeNode> SelectedNodes;
        // o ultimo node/objeto selecionado
        public static TreeNode LastSelectNode = null;

    }
}
