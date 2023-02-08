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
    static class ModelJsonFile
    {
        static string parseFromVector3(Vector3 v)
        {
            List<float> xyz = new List<float>() { v.X, v.Y, v.Z };
            return JsonConvert.SerializeObject(xyz);
        }

        static Vector3 parseToVector3(string json)
        {
            List<float> xyz = JsonConvert.DeserializeObject<List<float>>(json);
            return new Vector3(xyz[0], xyz[1], xyz[2]);
        }

        public static void writeModelJsonFile(string filename, ModelJson model)
        {
            JObject entry = new JObject();
            entry["Modelkey"] = model.Modelkey;

            entry["EnableBlend"] = model.EnableBlend;
            entry["EnableAlphaTest"] = model.EnableAlphaTest;
            entry["AlphaValue"] = model.AlphaValue;
           
            entry["PmdList"] = JArray.Parse(JsonConvert.SerializeObject(model.PmdList));

            if (model.BlackListTextures.Count != 0)
            {
                entry["BlackListTextures"] = JArray.Parse(JsonConvert.SerializeObject(model.BlackListTextures));
            }

            if (model.FixPmds.Count != 0)
            {
                JArray arrayFixPmds = new JArray();
                foreach (var item in model.FixPmds)
                {
                    JObject FixPmds = new JObject();
                    FixPmds["Key"] = item.Key;
                    FixPmds["Rotation"] = parseFromVector3(item.Value.Rotation);
                    FixPmds["Position"] = parseFromVector3(item.Value.Position);
                    FixPmds["Scale"] = parseFromVector3(item.Value.Scale);
                    arrayFixPmds.Add(FixPmds);
                }
                entry["FixPmds"] = arrayFixPmds;         
            }

            JObject o = new JObject();
            o["ModelJson"] = entry;

            File.WriteAllText(filename, o.ToString());
        }
        private static bool checkValidEntryFixPmds(JObject entry)
        {
            return (entry["Key"] != null && entry["Rotation"] != null
                 && entry["Position"] != null && entry["Scale"] != null);
        }


        public static ModelJson parseModelJson(string filename)
        {
            ModelJson m = null;
            if (File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                JObject o = JObject.Parse(json);

                ModelJson newmodel = null;
                if (o["ModelJson"] != null)
                {
                    JObject oModelJson = (JObject)o["ModelJson"];
                    string Modelkey = oModelJson["Modelkey"].ToString();
                    newmodel = new ModelJson(Modelkey);

                    if (oModelJson["EnableBlend"] != null)
                    {
                        newmodel.EnableBlend = bool.Parse(oModelJson["EnableBlend"].ToString());
                    }
                    if (oModelJson["EnableAlphaTest"] != null)
                    {
                        newmodel.EnableAlphaTest = bool.Parse(oModelJson["EnableAlphaTest"].ToString());
                    }
                    if (oModelJson["AlphaValue"] != null)
                    {
                        newmodel.AlphaValue = float.Parse(oModelJson["AlphaValue"].ToString());
                    }

                    newmodel.PmdList = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(oModelJson["PmdList"].ToString());

                    if (oModelJson["BlackListTextures"] != null)
                    {
                        newmodel.BlackListTextures = JsonConvert.DeserializeObject<List<string>>(oModelJson["BlackListTextures"].ToString());
                    }

                    Dictionary<string, FixPmd> FixPmds = new Dictionary<string, FixPmd>();
                    if (oModelJson["FixPmds"] != null)
                    {
                        JArray arrayFixPmds = (JArray)oModelJson["FixPmds"];
                        foreach (JToken token in arrayFixPmds.Children())
                        {
                            JObject entry = (JObject)token;
                            if (checkValidEntryFixPmds(entry))
                            {
                                string Key = entry["Key"].ToString();
                                Vector3 Rotation = parseToVector3(entry["Rotation"].ToString());
                                Vector3 Position = parseToVector3(entry["Position"].ToString());
                                Vector3 Scale = parseToVector3(entry["Scale"].ToString());
                                FixPmd fixPmd = new FixPmd(Rotation, Position, Scale);
                                FixPmds.Add(Key, fixPmd);
                            }
                        }
                    }
                    newmodel.FixPmds = FixPmds;
                    m = newmodel;

                }
            }
            return m;
        }

    }
}
