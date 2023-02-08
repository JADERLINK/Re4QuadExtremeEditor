using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src.JSON
{
    /// <summary>
    /// Representa o arquivo de configurações json, nas quais são replicadas na classe Globals;
    /// </summary>
    public class Configs
    {
        public string xscrDiretory { get; set; }

        public string xfileDiretory { get; set; }

        public Color SkyColor { get; set; }


        // colocar novas configurões aqui;

        // floats
        public ConfigFrationalSymbol FrationalSymbol { get; set; }
        public int FrationalAmount { get; set; }

        //items rotations
        public bool ItemDisableRotationAll { get; set; }
        public bool ItemDisableRotationIfXorYorZequalZero { get; set; }
        public bool ItemDisableRotationIfZisNotGreaterThanZero { get; set; }
        public ObjRotationOrder ItemRotationOrder { get; set; }
        public float ItemRotationCalculationMultiplier { get; set; }
        public float ItemRotationCalculationDivider { get; set; }

        // lang
        public bool LoadLangTranslation { get; set; }
        public string LangID { get; set; }

        // openGL mode Version
        public bool ForceUseOldOpenGL { get; set; }
        public bool ForceUseModernOpenGL { get; set; }
    }
}
