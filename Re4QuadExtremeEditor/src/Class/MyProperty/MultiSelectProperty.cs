using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.Interfaces;
using Re4QuadExtremeEditor.src.Class.ObjMethods;
using Re4QuadExtremeEditor.src.Class.MyProperty.CustomAttribute;
using Re4QuadExtremeEditor.src.Class.MyProperty.CustomTypeConverter;
using Re4QuadExtremeEditor.src.Class.MyProperty.CustomUITypeEditor;
using Re4QuadExtremeEditor.src.Class.TreeNodeObj;
using Re4QuadExtremeEditor.src.Class.MyProperty.CustomCollection;

namespace Re4QuadExtremeEditor.src.Class.MyProperty
{
    public class MultiSelectProperty : GenericProperty
    {

        private void UpdateSetFloatType()
        {
            for (int i = 0; i < EtcModelETS.Count; i++)
            {
                ((EtcModelProperty)EtcModelETS[i]).UpdateSetFloatType();
            }
            for (int i = 0; i < SpecialITA.Count; i++)
            {
                ((SpecialProperty)SpecialITA[i]).UpdateSetFloatType();
            }
            for (int i = 0; i < SpecialAEV.Count; i++)
            {
                ((SpecialProperty)SpecialAEV[i]).UpdateSetFloatType();
            }
        }

        public MultiSelectProperty(List<TreeNode> Selecteds, UpdateMethods updateMethods) : base()
        {
            SetThis(this);

            EnemyESL = new GenericCollection();
            EtcModelETS = new GenericCollection();
            SpecialITA = new GenericCollection();
            SpecialAEV = new GenericCollection();
            MultiSelectInfo = new MultiSelectObjInfoToProperty(Lang.GetAttributeText(aLang.MultiSelectInfoValueText), updateMethods);

            foreach (var item in Selecteds)
            {
                if (item is Object3D obj)
                {
                    ushort Id = obj.ObjLineRef;
                    var group = obj.Group;
                    if (group == GroupType.ESL)
                    {
                        EnemyProperty p = new EnemyProperty(Id, updateMethods, ((EnemyNodeGroup)obj.Parent).PropertyMethods, true);
                        EnemyESL.Add(p);
                        MultiSelectInfo.Add(p);
                    }
                    else if (group == GroupType.ETS)
                    {
                        EtcModelProperty p = new EtcModelProperty(Id, updateMethods, ((EtcModelNodeGroup)obj.Parent).PropertyMethods, true);
                        p.UpdateSetFloatTypeEvent += UpdateSetFloatType;
                        EtcModelETS.Add(p);
                        MultiSelectInfo.Add(p);
                    }
                    else if (group == GroupType.ITA)
                    {
                        SpecialProperty p = new SpecialProperty(Id, updateMethods, ((SpecialNodeGroup)obj.Parent).PropertyMethods, false, true);
                        p.UpdateSetFloatTypeEvent += UpdateSetFloatType;
                        SpecialITA.Add(p);
                        MultiSelectInfo.Add(p);
                    }
                    else if (group == GroupType.AEV)
                    {
                        SpecialProperty p = new SpecialProperty(Id, updateMethods, ((SpecialNodeGroup)obj.Parent).PropertyMethods, false, true);
                        p.UpdateSetFloatTypeEvent += UpdateSetFloatType;
                        SpecialAEV.Add(p);
                        MultiSelectInfo.Add(p);
                    }
                
                }
            }

            //EnemyESL.OrderList();
            //EtcModelETS.OrderList();
            //SpecialITA.OrderList();
            //SpecialAEV.OrderList();

            ChangePropertyIsBrowsable("EnemyESL", EnemyESL.Count != 0);
            ChangePropertyIsBrowsable("EtcModelETS", EtcModelETS.Count != 0);
            ChangePropertyIsBrowsable("SpecialITA", SpecialITA.Count != 0);
            ChangePropertyIsBrowsable("SpecialAEV", SpecialAEV.Count != 0);
        }

        [CustomCategory(aLang.MultiSelectCategory)]
        [CustomDisplayName(aLang.MultiSelectInfoDisplayName)]
        [CustomDescription(aLang.MultiSelectInfoDescription)]
        [DefaultValue(null)]
        [ReadOnly(true)]
        [Browsable(true)]
        [Editor(typeof(MultiSelectUITypeEditor), typeof(UITypeEditor))]
        [DynamicTypeDescriptor.Id(0, 0)]
        public MultiSelectObjInfoToProperty MultiSelectInfo { get; }


        [CustomCategory(aLang.MultiSelectCategory)]
        [CustomDisplayName(aLang.MultiSelectEnemyDisplayName)]
        [Description("")]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(false)]
        [TypeConverter(typeof(GenericCollectionConverter))]
        [Editor(typeof(NoneUITypeEditor), typeof(UITypeEditor))]
        [DynamicTypeDescriptor.Id(1, 0)]
        public GenericCollection EnemyESL { get; private set; }



        [CustomCategory(aLang.MultiSelectCategory)]
        [CustomDisplayName(aLang.MultiSelectEtcmodelDisplayName)]
        [Description("")]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(false)]
        [TypeConverter(typeof(GenericCollectionConverter))]
        [Editor(typeof(NoneUITypeEditor), typeof(UITypeEditor))]
        [DynamicTypeDescriptor.Id(2, 0)]
        public GenericCollection EtcModelETS { get; private set; }



        [CustomCategory(aLang.MultiSelectCategory)]
        [CustomDisplayName(aLang.MultiSelectSpecialItaDisplayName)]
        [Description("")]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(false)]
        [TypeConverter(typeof(GenericCollectionConverter))]
        [Editor(typeof(NoneUITypeEditor), typeof(UITypeEditor))]
        [DynamicTypeDescriptor.Id(3, 0)]
        public GenericCollection SpecialITA { get; private set; }



        [CustomCategory(aLang.MultiSelectCategory)]
        [CustomDisplayName(aLang.MultiSelectSpecialAevDisplayName)]
        [Description("")]
        [DefaultValue(null)]
        [ReadOnly(false)]
        [Browsable(false)]
        [TypeConverter(typeof(GenericCollectionConverter))]
        [Editor(typeof(NoneUITypeEditor), typeof(UITypeEditor))]
        [DynamicTypeDescriptor.Id(4, 0)]
        public GenericCollection SpecialAEV { get; private set; }

    }

}
