using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Re4QuadExtremeEditor.src.JSON
{
    static class ObjInfoFile
    {
        public static void writeObjInfoFile(string filename, ObjInfo[] list)
        {
            JArray array = new JArray();
            foreach (var item in list)
            {
                JObject entry = new JObject();
                entry["GameId"] = item.GameId.ToString("X4");
                entry["ModelKey"] = item.ModelKey;
                entry["UseInternalModel"] = item.UseInternalModel;
                entry["Name"] = item.Name;
                entry["Description"] = item.Description;
                array.Add(entry);
            }

            JObject o = new JObject();
            o["ObjInfos"] = array;

            File.WriteAllText(filename, o.ToString());
        }

        private static bool checkValidEntry(JObject entry)
        {
            return (entry["GameId"] != null && entry["ModelKey"] != null && entry["UseInternalModel"] != null
                 && entry["Name"] != null && entry["Description"] != null);
        }

        public static Dictionary<ushort, ObjInfo> parseObjInfoList(string filename)
        {
            Dictionary<ushort, ObjInfo> ObjInfoList = new Dictionary<ushort, ObjInfo>();

            if (File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                JObject o = JObject.Parse(json);
                if (o["ObjInfos"] != null)
                {
                    JArray array = (JArray)o["ObjInfos"];
                    foreach (JToken token in array.Children())
                    {
                        JObject entry = (JObject)token;
                        if (checkValidEntry(entry))
                        {
                            ushort GameId = ushort.Parse(entry["GameId"].ToString(), System.Globalization.NumberStyles.HexNumber);
                            string ModelKey = entry["ModelKey"].ToString();

                            bool UseInternalModel = bool.Parse(entry["UseInternalModel"].ToString());

                            string Name = entry["Name"].ToString();
                            string Description = entry["Description"].ToString();

                            ObjInfo obj = new ObjInfo(GameId, ModelKey, UseInternalModel, Name, Description);
                            if (!ObjInfoList.ContainsKey(GameId))
                            {
                                ObjInfoList.Add(GameId, obj);
                            }  
                        }
                    
                    }

                }

            }

            return ObjInfoList;
        }



    }
}
