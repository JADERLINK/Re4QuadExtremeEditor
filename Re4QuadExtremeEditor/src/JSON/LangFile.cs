using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src.JSON
{
    public class LangFile
    {
        public static void writeLangFile(string filename)
        {
            JObject eLang = new JObject();
            foreach (var item in Lang.Text)
            {
                eLang[Enum.GetName(typeof(eLang),item.Key)] = item.Value;
            }

            JObject aLang = new JObject();
            foreach (var item in Lang.AttributeText)
            {
                aLang[Enum.GetName(typeof(aLang), item.Key)] = item.Value;
            }

            JObject Enemy = new JObject();
            JObject EnemyName = new JObject();
            JObject EnemyDescription = new JObject();

            Enemy["Name"] = EnemyName;
            Enemy["Description"] = EnemyDescription;

            foreach (var item in DataBase.EnemiesIDs)
            {
                EnemyName[item.Key.ToString("X4")] = item.Value.Name;
                EnemyDescription[item.Key.ToString("X4")] = item.Value.Description;
            }

            JObject EtcModel = new JObject();
            JObject EtcModelName = new JObject();
            JObject EtcModelDescription = new JObject();

            EtcModel["Name"] = EtcModelName;
            EtcModel["Description"] = EtcModelDescription;

            foreach (var item in DataBase.EtcModelIDs)
            {
                EtcModelName[item.Key.ToString("X4")] = item.Value.Name;
                EtcModelDescription[item.Key.ToString("X4")] = item.Value.Description;
            }

            JObject Item = new JObject();
            JObject ItemName = new JObject();
            JObject ItemDescription = new JObject();

            Item["Name"] = ItemName;
            Item["Description"] = ItemDescription;

            foreach (var item in DataBase.ItemsIDs)
            {
                ItemName[item.Key.ToString("X4")] = item.Value.Name;
                ItemDescription[item.Key.ToString("X4")] = item.Value.Description;
            }

            JObject Room = new JObject();
            JObject RoomName = new JObject();
            JObject RoomDescription = new JObject();

            Room["Name"] = RoomName;
            Room["Description"] = RoomDescription;

            foreach (var item in DataBase.RoomList)
            {
                RoomName[item.RoomKey] = item.Name;
                RoomDescription[item.RoomKey] = item.Description;
            }

            JObject entry = new JObject();
            entry["eLang"] = eLang;
            entry["aLang"] = aLang;
            entry["Enemy"] = Enemy;
            entry["EtcModel"] = EtcModel;
            entry["Item"] = Item;
            entry["Room"] = Room;

            JObject o = new JObject();
            o["Lang"] = entry;
            try { File.WriteAllText(filename, o.ToString()); } catch (Exception) { }
        }

        public static void parseLang(string filename)
        {
            if (File.Exists(filename))
            {
                string json = null;
                JObject o = null;
                try { json = File.ReadAllText(filename); } catch (Exception) { }
                try { o = JObject.Parse(json); } catch (Exception) { }

                if (o != null && o["Lang"] != null)
                {
                    JObject oLang = (JObject)o["Lang"];
                    
                    if (oLang["eLang"] != null)
                    {
                        JObject eLangJSON = (JObject)oLang["eLang"];
                        foreach (var item in eLangJSON)
                        {
                            eLang e = eLang.Null;
                            try { e = (eLang)Enum.Parse((typeof(eLang)), item.Key); } catch (Exception) { }
                            if (e != eLang.Null)
                            {
                                Lang.SetText(e, item.Value.ToString());
                            }
                        }
                    }

                    if (oLang["aLang"] != null)
                    {
                        JObject aLangJSON = (JObject)oLang["aLang"];
                        foreach (var item in aLangJSON)
                        {
                            aLang a = aLang.Null;
                            try { a = (aLang)Enum.Parse((typeof(aLang)), item.Key); } catch (Exception) { }
                            if (a != aLang.Null)
                            {
                                Lang.SetAttributeText(a, item.Value.ToString());
                            }
                        }
                    }

                    if (oLang["Enemy"] != null)
                    {
                        JObject Enemy = (JObject)oLang["Enemy"];
                        JObject Name = (JObject)Enemy["Name"];
                        JObject Description = (JObject)Enemy["Description"];
                        if (Name != null)
                        {
                            foreach (var item in Name)
                            {
                                if (!Lang.Lists.Enemy.ContainsKey(item.Key))
                                {
                                    try
                                    {
                                        Lang.Lists.Enemy.Add(item.Key, new KeyValuePair<string, string>(item.Value.ToString(), null));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else // ContainsKey == true
                                {
                                    try
                                    {
                                        var v = Lang.Lists.Enemy[item.Key];
                                        Lang.Lists.Enemy[item.Key] = new KeyValuePair<string, string>(item.Value.ToString(), v.Key);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                        }
                        if (Description != null)
                        {

                            foreach (var item in Description)
                            {
                                if (!Lang.Lists.Enemy.ContainsKey(item.Key))
                                {
                                    try
                                    {
                                        Lang.Lists.Enemy.Add(item.Key, new KeyValuePair<string, string>(null, item.Value.ToString()));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else  // ContainsKey == true
                                {
                                    try
                                    {
                                        var v = Lang.Lists.Enemy[item.Key];
                                        Lang.Lists.Enemy[item.Key] = new KeyValuePair<string, string>(v.Key, item.Value.ToString());
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                            }

                        }
                    }

                    if (oLang["EtcModel"] != null)
                    {
                        JObject EtcModel = (JObject)oLang["EtcModel"];
                        JObject Name = (JObject)EtcModel["Name"];
                        JObject Description = (JObject)EtcModel["Description"];
                        if (Name != null)
                        {
                            foreach (var item in Name)
                            {
                                if (!Lang.Lists.EtcModel.ContainsKey(item.Key))
                                {
                                    try
                                    {
                                        Lang.Lists.EtcModel.Add(item.Key, new KeyValuePair<string, string>(item.Value.ToString(), null));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else // ContainsKey == true
                                {
                                    try
                                    {
                                        var v = Lang.Lists.EtcModel[item.Key];
                                        Lang.Lists.EtcModel[item.Key] = new KeyValuePair<string, string>(item.Value.ToString(), v.Key);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }


                        }
                        if (Description != null)
                        {

                            foreach (var item in Description)
                            {
                                if (!Lang.Lists.EtcModel.ContainsKey(item.Key))
                                {
                                    try
                                    {
                                        Lang.Lists.EtcModel.Add(item.Key, new KeyValuePair<string, string>(null, item.Value.ToString()));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else  // ContainsKey == true
                                {
                                    try
                                    {
                                        var v = Lang.Lists.EtcModel[item.Key];
                                        Lang.Lists.EtcModel[item.Key] = new KeyValuePair<string, string>(v.Key, item.Value.ToString());
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                            }

                        }
                    }

                    if (oLang["Item"] != null)
                    {
                        JObject Item = (JObject)oLang["Item"];
                        JObject Name = (JObject)Item["Name"];
                        JObject Description = (JObject)Item["Description"];
                        if (Name != null)
                        {
                            foreach (var item in Name)
                            {
                                if (!Lang.Lists.Item.ContainsKey(item.Key))
                                {
                                    try
                                    {
                                        Lang.Lists.Item.Add(item.Key, new KeyValuePair<string, string>(item.Value.ToString(), null));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else // ContainsKey == true
                                {
                                    try
                                    {
                                        var v = Lang.Lists.Item[item.Key];
                                        Lang.Lists.Item[item.Key] = new KeyValuePair<string, string>(item.Value.ToString(), v.Key);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }


                        }
                        if (Description != null)
                        {

                            foreach (var item in Description)
                            {
                                if (!Lang.Lists.Item.ContainsKey(item.Key))
                                {
                                    try
                                    {
                                        Lang.Lists.Item.Add(item.Key, new KeyValuePair<string, string>(null, item.Value.ToString()));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else  // ContainsKey == true
                                {
                                    try
                                    {
                                        var v = Lang.Lists.Item[item.Key];
                                        Lang.Lists.Item[item.Key] = new KeyValuePair<string, string>(v.Key, item.Value.ToString());
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                            }

                        }
                    }

                    if (oLang["Room"] != null)
                    {
                        JObject Room = (JObject)oLang["Room"];
                        JObject Name = (JObject)Room["Name"];
                        JObject Description = (JObject)Room["Description"];

                        if (Name != null)
                        {
                            foreach (var item in Name)
                            {
                                if (!Lang.Lists.Room.ContainsKey(item.Key))
                                {
                                    try
                                    {
                                        Lang.Lists.Room.Add(item.Key, new KeyValuePair<string, string>(item.Value.ToString(), null));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else // ContainsKey == true
                                {
                                    try
                                    {
                                        var v = Lang.Lists.Room[item.Key];
                                        Lang.Lists.Room[item.Key] = new KeyValuePair<string, string>(item.Value.ToString(), v.Key);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                        }

                        if (Description != null)
                        {
                            foreach (var item in Description)
                            {
                                if (!Lang.Lists.Room.ContainsKey(item.Key))
                                {
                                    try
                                    {
                                        Lang.Lists.Room.Add(item.Key, new KeyValuePair<string, string>(null, item.Value.ToString()));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else  // ContainsKey == true
                                {
                                    try
                                    {
                                        var v = Lang.Lists.Room[item.Key];
                                        Lang.Lists.Room[item.Key] = new KeyValuePair<string, string>(v.Key, item.Value.ToString());
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                            }
                        }

                    }
                }

            }
        }

    }
}
