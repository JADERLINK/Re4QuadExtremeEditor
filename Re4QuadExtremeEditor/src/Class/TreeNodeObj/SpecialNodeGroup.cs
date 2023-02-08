﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.ObjMethods;
using Re4QuadExtremeEditor.src.Class.Interfaces;

namespace Re4QuadExtremeEditor.src.Class.TreeNodeObj
{
    public class SpecialNodeGroup : TreeNodeGroup, INodeChangeAmount
    {
        public SpecialNodeGroup() : base() { }
        public SpecialNodeGroup(string text) : base(text) { }
        public SpecialNodeGroup(string text, TreeNode[] children) : base(text, children) { }

        public SpecialMethods PropertyMethods { get; set; }

        public SpecialMethodsForGL MethodsForGL { get; set; }

        public ExtrasMethodsForGL ExtrasMethodsForGL { get; set; }

        public NodeChangeAmountMethods ChangeAmountMethods { get; set; }
    }
    
}
