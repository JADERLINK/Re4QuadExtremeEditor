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
    static class RoomJsonFile
    {
        static string parseFromVector3(Vector3 v) 
        {
            List<float> xyz = new List<float>() {v.X, v.Y, v.Z};
            return JsonConvert.SerializeObject(xyz);
        }

        static Vector3 parseToVector3(string json) 
        {
            List<float> xyz = JsonConvert.DeserializeObject<List<float>>(json);
            return new Vector3(xyz[0],xyz[1], xyz[2]);
        }

        public static void writeRoomJsonFile(string filename, RoomJson room)
        {
            JObject entry = new JObject();
            entry["JsonName"] = room.JsonName;
            entry["RoomFolder"] = room.RoomFolder;
            entry["PmdList"] = JArray.Parse(JsonConvert.SerializeObject(room.PmdList));
            entry["BlackListTextures"] = JArray.Parse(JsonConvert.SerializeObject(room.BlackListTextures));
            JArray arrayFixPmds = new JArray();
            foreach (var item in room.FixPmds)
            {
                JObject FixPmds = new JObject();
                FixPmds["Key"] = item.Key;
                FixPmds["Rotation"] = parseFromVector3(item.Value.Rotation);
                FixPmds["Position"] = parseFromVector3(item.Value.Position);
                FixPmds["Scale"] = parseFromVector3(item.Value.Scale);
                arrayFixPmds.Add(FixPmds);
            }
            entry["FixPmds"] = arrayFixPmds;

            JObject o = new JObject();
            o["RoomJson"] = entry;

            File.WriteAllText(filename, o.ToString());
        }
        private static bool checkValidEntryFixPmds(JObject entry)
        {
            return (entry["Key"] != null && entry["Rotation"] != null
                 && entry["Position"] != null && entry["Scale"] != null);
        }


        public static RoomJson parseRoomJson(string filename)
        {
            RoomJson r = null;
            if (File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                JObject o = JObject.Parse(json);

                RoomJson newroom = null;
                if (o["RoomJson"] != null)
                {
                    JObject oRoomJson = (JObject)o["RoomJson"];
                    string JsonName = oRoomJson["JsonName"].ToString();
                    newroom = new RoomJson(JsonName);
                    newroom.RoomFolder = oRoomJson["RoomFolder"].ToString();
                    newroom.PmdList = JsonConvert.DeserializeObject<List<string>>(oRoomJson["PmdList"].ToString());
                    newroom.BlackListTextures = JsonConvert.DeserializeObject<List<string>>(oRoomJson["BlackListTextures"].ToString());

                    Dictionary<string, FixPmd> FixPmds = new Dictionary<string, FixPmd>(); 
                    if (oRoomJson["FixPmds"] != null)
                    {
                        JArray arrayFixPmds = (JArray)oRoomJson["FixPmds"];
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
                    newroom.FixPmds = FixPmds;
                    r = newroom;

                }
            }
            return r;
        }
    }
}
