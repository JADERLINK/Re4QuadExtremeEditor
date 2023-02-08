using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using OpenTK;

namespace Re4QuadExtremeEditor.src.JSON
{
    public class LangListFile
    {
        public static void writeLangListFile(string filename, LangObjForList[] langList)
        {
            JObject o = new JObject();
            o["LangList"] = JArray.Parse(JsonConvert.SerializeObject(langList));
            File.WriteAllText(filename, o.ToString());
        }

        public static LangObjForList[] parseLangList(string filename)
        {
            LangObjForList[] list = new LangObjForList[0];

            if (File.Exists(filename))
            {
                string json = null;
                JObject o = null;
                try { json = File.ReadAllText(filename); } catch (Exception) { }
                try { o = JObject.Parse(json); } catch (Exception) { }

                if (o != null && o["LangList"] != null)
                {
                    try
                    {
                        list = JsonConvert.DeserializeObject<LangObjForList[]>(o["LangList"].ToString());
                    }
                    catch (Exception)
                    {
                    }
                    
                }
            }
            return list;
        }


    }
}
