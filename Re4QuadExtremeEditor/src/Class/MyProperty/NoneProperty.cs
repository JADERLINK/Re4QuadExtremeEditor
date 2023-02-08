using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;

namespace Re4QuadExtremeEditor.src.Class.MyProperty
{
    //[DefaultPropertyAttribute("None")]
    public class NoneProperty : GenericProperty
    {
        /*
        [CategoryAttribute("None")]
        [DescriptionAttribute("None")]
        [DisplayNameAttribute("None")]
        [DefaultValueAttribute(null)]
        [ReadOnlyAttribute(true)]
        [BrowsableAttribute(true)]
        public string None { get => "None"; }
        */

        [CategoryAttribute("Info")]
        [DescriptionAttribute("")]
        [DisplayNameAttribute("Re4 Quad Extreme Editor")]
        [DefaultValueAttribute(null)]
        [ReadOnlyAttribute(true)]
        [BrowsableAttribute(true)]
        public string Version { get => "Version: 1.0.1"; }

        [CategoryAttribute("Info")]
        [DescriptionAttribute("")]
        [DisplayNameAttribute("OpenGL")]
        [DefaultValueAttribute(null)]
        [ReadOnlyAttribute(true)]
        [BrowsableAttribute(true)]
        public string OpenGLVersion { get => "Version: " + Globals.OpenGLVersion; }


        [CategoryAttribute("Info")]
        [DescriptionAttribute("")]
        [DisplayNameAttribute("OpenGL Mode")]
        [DefaultValueAttribute(null)]
        [ReadOnlyAttribute(true)]
        [BrowsableAttribute(true)]
        public string GL_Mode { get 
            {
                if (Globals.UseOldGL)
                {
                    return "Using Old OpenGL";
                }
                else 
                {
                    return "Using Modern OpenGL";
                }
            
            }}



    }
}
