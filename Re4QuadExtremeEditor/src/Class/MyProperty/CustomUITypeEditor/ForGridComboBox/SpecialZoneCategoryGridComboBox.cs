﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Re4QuadExtremeEditor.src.Class.MyProperty.CustomUITypeEditor
{
    //SpecialZoneCategoryGridComboBox
    public class SpecialZoneCategoryGridComboBox : GridComboBox
    {
        protected override void RetrieveDataList(ITypeDescriptorContext context)
        {
            DataList = ListBoxProperty.SpecialZoneCategoryList.Values.ToArray();
        }

        protected override object GetDataObjectSelected(ITypeDescriptorContext context)
        {
            return ListBox.SelectedItem;
        }

        protected override void onStart()
        {
            ListBox.HorizontalScrollbar = false;
        }

    }
}
