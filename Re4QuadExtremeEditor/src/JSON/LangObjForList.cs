using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re4QuadExtremeEditor.src.JSON
{
    public class LangObjForList
    {
        public string LangID { get; }
        public string LangName { get; }
        public string LangFilePath { get; }

        public LangObjForList(string LangID, string LangName, string LangFilePath) 
        {
            this.LangID = LangID;
            this.LangFilePath = LangFilePath;
            this.LangName = LangName;
        }

        public override bool Equals(object obj)
        {
            return obj is LangObjForList lang && lang.LangID == LangID;
        }

        public override int GetHashCode()
        {
            return LangID.GetHashCode();
        }

        public override string ToString()
        {
            return LangName;
        }
    }
}
