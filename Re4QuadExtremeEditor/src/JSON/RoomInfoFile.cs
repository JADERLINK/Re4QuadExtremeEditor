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
    static class RoomInfoFile
    {
        public static void writeRoomInfoFile(string filename, List<RoomInfo> RoomList)
        {
            JArray array = new JArray();
            foreach (var item in RoomList)
            {
                JObject entry = new JObject();
                entry["RoomKey"] = item.RoomKey;
                entry["RoomId"] = item.RoomId.ToString("X4");
                entry["RoomJsonFiles"] = JArray.Parse(JsonConvert.SerializeObject(item.RoomJsonFiles));
                entry["Name"] = item.Name;
                entry["Description"] = item.Description;
                array.Add(entry);
            }

            JObject o = new JObject();
            o["RoomInfos"] = array;

            File.WriteAllText(filename, o.ToString());
        }

        private static bool checkValidEntry(JObject entry)
        {
            return (entry["RoomKey"] != null && entry["RoomJsonFiles"] != null && entry["RoomId"] != null
                 && entry["Name"] != null && entry["Description"] != null);
        }

        public static List<RoomInfo> parseRoomList(string filename)
        {
            List<RoomInfo> RoomList = new List<RoomInfo>();

            if (File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                JObject o = JObject.Parse(json);
                if (o["RoomInfos"] != null)
                {
                    JArray array = (JArray)o["RoomInfos"];
                    foreach (JToken token in array.Children())
                    {
                        JObject entry = (JObject)token;
                        if (checkValidEntry(entry))
                        {
                            string RoomKey = entry["RoomKey"].ToString();
                            int RoomId = int.Parse(entry["RoomId"].ToString(), System.Globalization.NumberStyles.HexNumber);
                            string[] RoomJsonFiles = JsonConvert.DeserializeObject<string[]>(entry["RoomJsonFiles"].ToString());
                            string Name = entry["Name"].ToString();
                            string Description = entry["Description"].ToString();
                            RoomInfo r = new RoomInfo(RoomKey, RoomId, RoomJsonFiles, Name, Description);
                            RoomList.Add(r);
                        }

                    }

                }

            }

            return RoomList;
        }




    }
}