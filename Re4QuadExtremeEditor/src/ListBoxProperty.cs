using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Re4QuadExtremeEditor.src.Class;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src
{
    /// <summary>
    /// Conteudo usado nas Listbox/ComboBox
    /// </summary>
    public static class ListBoxProperty
    {
        // listas de objetos a serem selecionados nos list box

        public static Dictionary<ushort, UshortObjForListBox> EnemiesList;
        public static Dictionary<ushort, UshortObjForListBox> ItemsList;
        public static Dictionary<ushort, UshortObjForListBox> EtcmodelsList;

        // EnemyEnableList
        public static Dictionary<byte, ByteObjForListBox> EnemyEnableList;
        // FloatTypeList
        public static Dictionary<bool, BoolObjForListBox> FloatTypeList;
        // SpecialTypeList
        public static Dictionary<SpecialType, ByteObjForListBox> SpecialTypeList;
        // SpecialZoneCategory
        public static Dictionary<byte, ByteObjForListBox> SpecialZoneCategoryList;
        // ItemAuraType
        public static Dictionary<ushort, UshortObjForListBox> ItemAuraTypeList;
        // RefInteractionType
        public static Dictionary<byte, ByteObjForListBox> RefInteractionTypeList;
        // PromptMessage
        public static Dictionary<byte, ByteObjForListBox> PromptMessageList;
    }
}
