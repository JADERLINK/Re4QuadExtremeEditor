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
    static class ModelListJsonFile
    {

        // lista com o nome dos arquivos json
        public static void writeModelListJsonFile(string filename, string[] list)
        {
            JObject o = new JObject();
            o["ModelList"] = JArray.Parse(JsonConvert.SerializeObject(list));
            File.WriteAllText(filename, o.ToString());
        }

        public static string[] parseModelListJsonFile(string filename)
        {
            string[] list = new string[0];

            if (File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                JObject o = JObject.Parse(json);

                if (o["ModelList"] != null)
                {
                    list = JsonConvert.DeserializeObject<string[]>(o["ModelList"].ToString());
                }
            }

            return list;
        }

    }
}
