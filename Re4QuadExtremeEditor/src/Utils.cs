﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Re4QuadExtremeEditor.src.JSON;
using Re4QuadExtremeEditor.src.Class;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.TreeNodeObj;
using System.IO;
using OpenTK;

namespace Re4QuadExtremeEditor.src
{
    /// <summary>
    /// Metodos uteis para serem usados;
    /// </summary>
    public static class Utils
    {        
        /// <summary>
        /// define as configs padrões 
        /// </summary>
        /// <returns></returns>
        public static Configs GetDefaultConfigs()
        {
            Configs configs = new Configs();
            configs.xfileDiretory = @"xfile\";
            configs.xscrDiretory = @"xscr\";
            configs.SkyColor = Color.Azure;

            // colocar novas configurões aqui;
            configs.FrationalAmount = 9;
            configs.FrationalSymbol = ConfigFrationalSymbol.AcceptsCommaAndPeriod_OutputPeriod;

            configs.ItemDisableRotationAll = false;
            configs.ItemDisableRotationIfXorYorZequalZero = false;
            configs.ItemDisableRotationIfZisNotGreaterThanZero = true;
            configs.ItemRotationOrder = ObjRotationOrder.RotationXY;
            configs.ItemRotationCalculationMultiplier = 1;
            configs.ItemRotationCalculationDivider = 1;

            configs.ForceUseOldOpenGL = false;
            configs.ForceUseModernOpenGL = false;

            return configs;
        }

        /// <summary>
        /// metodo que tem como função carregar as cofigurações ao carregar;
        /// </summary>
        public static void StartLoadConfigs() 
        {
            if (File.Exists(Consts.ConfigsFileDiretory))
            {
                Configs configs = GetDefaultConfigs();
                // para caso o arquivo não consiga ser lido
                try { configs = ConfigsFile.parseConfigs(Consts.ConfigsFileDiretory); } catch (Exception) { }
              
                Globals.BackupConfigs = configs;
                Globals.xfileDiretory = configs.xfileDiretory;
                Globals.xscrDiretory = configs.xscrDiretory;
                Globals.SkyColor = configs.SkyColor;

                // colocar novas configurões aqui;
                Globals.FrationalAmount = configs.FrationalAmount;
                Globals.FrationalSymbol = configs.FrationalSymbol;

                Globals.ItemDisableRotationAll = configs.ItemDisableRotationAll;
                Globals.ItemDisableRotationIfXorYorZequalZero = configs.ItemDisableRotationIfXorYorZequalZero;
                Globals.ItemDisableRotationIfZisNotGreaterThanZero = configs.ItemDisableRotationIfZisNotGreaterThanZero;
                Globals.ItemRotationOrder = configs.ItemRotationOrder;
                Globals.ItemRotationCalculationMultiplier = configs.ItemRotationCalculationMultiplier;
                Globals.ItemRotationCalculationDivider = configs.ItemRotationCalculationDivider;
            }
            else 
            {
                if (!Directory.Exists(Consts.dataDiretory))
                {
                    Directory.CreateDirectory(Consts.dataDiretory);
                }

                // para caso o arquivo não consiga ser gravado
                try { ConfigsFile.writeConfigsFile(Consts.ConfigsFileDiretory, GetDefaultConfigs()); } catch (Exception) { } 

                Globals.BackupConfigs = GetDefaultConfigs();
            }
        
        }


        /// <summary>
        /// carrega a lista de Room ao iniciar o programa
        /// </summary>
        public static void StartLoadRoomInfoList()
        {
            try
            {
                DataBase.RoomList.Clear();
                DataBase.RoomList = RoomInfoFile.parseRoomList(Consts.RoomListFileDiretory);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// carrega os shader ao iniciar o programa
        /// </summary>
        public static void StartLoadShader() 
        {
            DataBase.ShaderRoom = new Class.Shaders.Shader(Encoding.UTF8.GetString(Properties.Resources.RoomShaderVert), Encoding.UTF8.GetString(Properties.Resources.RoomShaderFrag));
            DataBase.ShaderRoom.Use();
            DataBase.ShaderRoom.SetInt("texture0", 0);

            DataBase.ShaderObjs = new Class.Shaders.Shader(Encoding.UTF8.GetString(Properties.Resources.ObjShaderVert), Encoding.UTF8.GetString(Properties.Resources.ObjShaderFrag));
            DataBase.ShaderObjs.Use();
            DataBase.ShaderObjs.SetInt("texture0", 0);

            DataBase.ShaderBoundingBox = new Class.Shaders.Shader(Encoding.UTF8.GetString(Properties.Resources.BoundingBoxShaderVert), Encoding.UTF8.GetString(Properties.Resources.BoundingBoxShaderFrag));
            DataBase.ShaderBoundingBox.Use();
        }

        /// <summary>
        /// não carrega shader, usa oldGL, codigo adaptado
        /// </summary>
        public static void StartLoadNoShader_OldGL()
        {
            DataBase.ShaderRoom = new Class.Shaders.NoShaderRoom();
            DataBase.ShaderObjs = new Class.Shaders.NoShaderObjs();
            DataBase.ShaderBoundingBox = new Class.Shaders.NoShaderBoundingBox();
        }


        /// <summary>
        /// carrega os modelos 3d dos objetos ao iniciar o programa
        /// </summary>
        public static void StartLoadObjsModels()
        {
            DataBase.InternalModels = new ModelGroup(Consts.InternalModelGroupName, Consts.InternalModelsJsonFilesDiretory, Consts.InternalModelsListFileDiretory, Directory.GetCurrentDirectory() + "\\");
            DataBase.ItemsModels = new ModelGroup(Consts.ItemsModelGroupName, Consts.ItemsModelsJsonFilesDiretory, Consts.ItemsModelsListFileDiretory, Globals.xfileDiretory);
            DataBase.EtcModels = new ModelGroup(Consts.EtcModelGroupName, Consts.EtcModelsJsonFilesDiretory, Consts.EtcModelsListFileDiretory, Globals.xfileDiretory);
            DataBase.EnemiesModels = new ModelGroup(Consts.EnemiesModelGroupName, Consts.EnemiesModelsJsonFilesDiretory, Consts.EnemiesModelsListFileDiretory, Globals.xfileDiretory);           
            //int finish = 0;
        }


        /// <summary>
        /// carrega as lista de ObjInfo ao iniciar o programa
        /// </summary>
        public static void StartLoadObjsInfoLists() 
        {
            DataBase.ItemsIDs = new Dictionary<ushort, ObjInfo>();
            DataBase.EtcModelIDs = new Dictionary<ushort, ObjInfo>();
            DataBase.EnemiesIDs = new Dictionary<ushort, ObjInfo>();
            try { DataBase.ItemsIDs = ObjInfoFile.parseObjInfoList(Consts.ItemsObjInfoListFileDiretory); } catch (Exception){}
            try { DataBase.EtcModelIDs = ObjInfoFile.parseObjInfoList(Consts.EtcModelObjInfoListFileDiretory); } catch (Exception){}
            try { DataBase.EnemiesIDs = ObjInfoFile.parseObjInfoList(Consts.EnemiesObjInfoListFileDiretory); } catch (Exception){}
        }

        /// <summary>
        /// na lista de enimigos os ids vão de 0x00 a 0x4F, depois disso se repete a ordem dos inimigos, porem eles não emitem som
        /// </summary>
        public static void StartEnemyExtraSegmentList() 
        {
            if (Globals.CreateEnemyExtraSegmentList)
            {
                // segund ExtraSegment
                for (ushort i = 0; i < 0x50; i++)
                {
                    ushort originalId = (ushort)(i * 0x100);

                    var list = (from obj in DataBase.EnemiesIDs
                                where obj.Key >= originalId && obj.Key <= (originalId + 0xFF)
                                select obj.Key).ToArray();

                    if (list.Length != 0)
                    {
                        foreach (var Key in list)
                        {
                            ushort newId = (ushort)(Key + 0x5000);
                            ObjInfo obj;
                            if (!DataBase.EnemiesIDs.ContainsKey(newId) && DataBase.EnemiesIDs.TryGetValue(Key, out obj))
                            {
                                ObjInfo newObj = new ObjInfo(newId,
                                    obj.ModelKey,
                                    obj.UseInternalModel,
                                    obj.Name + " " + Lang.GetText(eLang.EnemyExtraSegmentSegund),
                                    obj.Description + " " + Lang.GetText(eLang.EnemyExtraSegmentSegund) + " " + Lang.GetText(eLang.EnemyExtraSegmentNoSound));
                                DataBase.EnemiesIDs.Add(newId, newObj);
                            }

                        }
                    }
                }

                //third ExtraSegment
                for (ushort i = 0; i < 0x50; i++)
                {
                    ushort originalId = (ushort)(i * 0x100);

                    var list = (from obj in DataBase.EnemiesIDs
                                where obj.Key >= originalId && obj.Key <= (originalId + 0xFF)
                                select obj.Key).ToArray();

                    if (list.Length != 0)
                    {
                        foreach (var Key in list)
                        {
                            ushort newId = (ushort)(Key + 0xA000);
                            ObjInfo obj;
                            if (!DataBase.EnemiesIDs.ContainsKey(newId) && DataBase.EnemiesIDs.TryGetValue(Key, out obj))
                            {
                                ObjInfo newObj = new ObjInfo(newId,
                                    obj.ModelKey,
                                    obj.UseInternalModel,
                                    obj.Name + " " + Lang.GetText(eLang.EnemyExtraSegmentThird),
                                    obj.Description + " " + Lang.GetText(eLang.EnemyExtraSegmentThird) + " " + Lang.GetText(eLang.EnemyExtraSegmentNoSound));
                                DataBase.EnemiesIDs.Add(newId, newObj);
                            }

                        }
                    }
                }

            }
        
        }


        /// <summary>
        /// carrega a lista de PromptMessage, em  ListBoxProperty.PromptMessageList
        /// </summary>
        public static void StartLoadPromptMessageList() 
        {
            try
            {
                ListBoxProperty.PromptMessageList = PromptMessageListFile.parsePromptMessageList(Consts.PromptMessageListFileDiretory);
            }
            catch (Exception)
            {
                ListBoxProperty.PromptMessageList = new Dictionary<byte, ByteObjForListBox>();
            }
           
        }

        /// <summary>
        /// prenche ListBoxProperty
        /// </summary>
        public static void StartSetListBoxsProperty()
        {
            //ListBoxProperty.FloatTypeLis
            Dictionary<bool, BoolObjForListBox> FloatType = new Dictionary<bool, BoolObjForListBox>();
            FloatType.Add(false, new BoolObjForListBox(false, Lang.GetAttributeText(aLang.ListBoxFloatTypeDisable)));
            FloatType.Add(true, new BoolObjForListBox(true, Lang.GetAttributeText(aLang.ListBoxFloatTypeEnable)));
            ListBoxProperty.FloatTypeList = FloatType;


            //ListBoxProperty.EnemyEnableList
            Dictionary<byte, ByteObjForListBox> Enable = new Dictionary<byte, ByteObjForListBox>();
            Enable.Add(0x00, new ByteObjForListBox(0x00, "00: " + Lang.GetAttributeText(aLang.ListBoxDisable)));
            Enable.Add(0x01, new ByteObjForListBox(0x01, "01: " + Lang.GetAttributeText(aLang.ListBoxEnable)));
            ListBoxProperty.EnemyEnableList = Enable;


            //ListBoxProperty.SpecialZoneCategoryList
            Dictionary<byte, ByteObjForListBox> SpecialZoneCategory = new Dictionary<byte, ByteObjForListBox>();
            SpecialZoneCategory.Add(0x00, new ByteObjForListBox(0x00, "00: " + Lang.GetAttributeText(aLang.ListBoxSpecialZoneCategory00)));
            SpecialZoneCategory.Add(0x01, new ByteObjForListBox(0x01, "01: " + Lang.GetAttributeText(aLang.ListBoxSpecialZoneCategory01)));
            SpecialZoneCategory.Add(0x02, new ByteObjForListBox(0x02, "02: " + Lang.GetAttributeText(aLang.ListBoxSpecialZoneCategory02)));
            ListBoxProperty.SpecialZoneCategoryList = SpecialZoneCategory;

            //ListBoxProperty.Ref
            //TypeList
            Dictionary<byte, ByteObjForListBox> RefInteractionTypeList = new Dictionary<byte, ByteObjForListBox>();
            RefInteractionTypeList.Add(0x00, new ByteObjForListBox(0x00, "00: " + Lang.GetAttributeText(aLang.ListBoxRefInteractionType00)));
            RefInteractionTypeList.Add(0x01, new ByteObjForListBox(0x01, "01: " + Lang.GetAttributeText(aLang.ListBoxRefInteractionType01Enemy)));
            RefInteractionTypeList.Add(0x02, new ByteObjForListBox(0x02, "02: " + Lang.GetAttributeText(aLang.ListBoxRefInteractionType02EtcModel)));
            ListBoxProperty.RefInteractionTypeList = RefInteractionTypeList;


            //ListBoxProperty.ItemAuraTypeList
            Dictionary<ushort, UshortObjForListBox> ItemAuraType = new Dictionary<ushort, UshortObjForListBox>();
            ItemAuraType.Add(0x00, new UshortObjForListBox(0x00, "00: " + Lang.GetAttributeText(aLang.ListBoxItemAuraType00)));
            ItemAuraType.Add(0x01, new UshortObjForListBox(0x01, "01: " + Lang.GetAttributeText(aLang.ListBoxItemAuraType01)));
            ItemAuraType.Add(0x02, new UshortObjForListBox(0x02, "02: " + Lang.GetAttributeText(aLang.ListBoxItemAuraType02)));
            ItemAuraType.Add(0x03, new UshortObjForListBox(0x03, "03: " + Lang.GetAttributeText(aLang.ListBoxItemAuraType03)));
            ItemAuraType.Add(0x04, new UshortObjForListBox(0x04, "04: " + Lang.GetAttributeText(aLang.ListBoxItemAuraType04)));
            ItemAuraType.Add(0x05, new UshortObjForListBox(0x05, "05: " + Lang.GetAttributeText(aLang.ListBoxItemAuraType05)));
            ItemAuraType.Add(0x06, new UshortObjForListBox(0x06, "06: " + Lang.GetAttributeText(aLang.ListBoxItemAuraType06)));
            ItemAuraType.Add(0x07, new UshortObjForListBox(0x07, "07: " + Lang.GetAttributeText(aLang.ListBoxItemAuraType07)));
            ItemAuraType.Add(0x08, new UshortObjForListBox(0x08, "08: " + Lang.GetAttributeText(aLang.ListBoxItemAuraType08)));
            ItemAuraType.Add(0x09, new UshortObjForListBox(0x09, "09: " + Lang.GetAttributeText(aLang.ListBoxItemAuraType09)));
            ListBoxProperty.ItemAuraTypeList = ItemAuraType;


            //ListBoxProperty.SpecialTypeList
            Dictionary<SpecialType, ByteObjForListBox> SpecialTypeList = new Dictionary<SpecialType, ByteObjForListBox>();
            SpecialTypeList.Add(SpecialType.T00_GeneralPurpose, new ByteObjForListBox(0x00, Lang.GetAttributeText(aLang.SpecialType00_GeneralPurpose)));
            SpecialTypeList.Add(SpecialType.T01_WarpDoor, new ByteObjForListBox(0x01, Lang.GetAttributeText(aLang.SpecialType01_WarpDoor)));
            SpecialTypeList.Add(SpecialType.T02_CutSceneEvents, new ByteObjForListBox(0x02, Lang.GetAttributeText(aLang.SpecialType02_CutSceneEvents)));
            SpecialTypeList.Add(SpecialType.T03_Items, new ByteObjForListBox(0x03, Lang.GetAttributeText(aLang.SpecialType03_Items)));
            SpecialTypeList.Add(SpecialType.T04_GroupedEnemyTrigger, new ByteObjForListBox(0x04, Lang.GetAttributeText(aLang.SpecialType04_GroupedEnemyTrigger)));
            SpecialTypeList.Add(SpecialType.T05_Message, new ByteObjForListBox(0x05, Lang.GetAttributeText(aLang.SpecialType05_Message)));
            //SpecialTypeList.Add(SpecialType.T06_Unused, new ByteObjForListBox(0x06, Lang.GetAttributeText(aLang.SpecialType06_Unused)));
            //SpecialTypeList.Add(SpecialType.T07_Unused, new ByteObjForListBox(0x07, Lang.GetAttributeText(aLang.SpecialType07_Unused)));
            SpecialTypeList.Add(SpecialType.T08_TypeWriter, new ByteObjForListBox(0x08, Lang.GetAttributeText(aLang.SpecialType08_TypeWriter)));
            //SpecialTypeList.Add(SpecialType.T09_Unused, new ByteObjForListBox(0x09, Lang.GetAttributeText(aLang.SpecialType09_Unused)));
            SpecialTypeList.Add(SpecialType.T0A_DamagesThePlayer, new ByteObjForListBox(0x0A, Lang.GetAttributeText(aLang.SpecialType0A_DamagesThePlayer)));
            SpecialTypeList.Add(SpecialType.T0B_FalseCollision, new ByteObjForListBox(0x0B, Lang.GetAttributeText(aLang.SpecialType0B_FalseCollision)));
            //SpecialTypeList.Add(SpecialType.T0C_Unused, new ByteObjForListBox(0x0C, Lang.GetAttributeText(aLang.SpecialType0C_Unused)));
            SpecialTypeList.Add(SpecialType.T0D_Unknown, new ByteObjForListBox(0x0D, Lang.GetAttributeText(aLang.SpecialType0D_Unknown)));
            SpecialTypeList.Add(SpecialType.T0E_Crouch, new ByteObjForListBox(0x0E, Lang.GetAttributeText(aLang.SpecialType0E_Crouch)));
            //SpecialTypeList.Add(SpecialType.T0F_Unused, new ByteObjForListBox(0x0F, Lang.GetAttributeText(aLang.SpecialType0F_Unused)));
            SpecialTypeList.Add(SpecialType.T10_FixedLadderClimbUp, new ByteObjForListBox(0x10, Lang.GetAttributeText(aLang.SpecialType10_FixedLadderClimbUp)));
            SpecialTypeList.Add(SpecialType.T11_ItemDependentEvents, new ByteObjForListBox(0x11, Lang.GetAttributeText(aLang.SpecialType11_ItemDependentEvents)));
            SpecialTypeList.Add(SpecialType.T12_AshleyHideCommand, new ByteObjForListBox(0x12, Lang.GetAttributeText(aLang.SpecialType12_AshleyHideCommand)));
            SpecialTypeList.Add(SpecialType.T13_LocalTeleportation, new ByteObjForListBox(0x13, Lang.GetAttributeText(aLang.SpecialType13_LocalTeleportation)));
            SpecialTypeList.Add(SpecialType.T14_UsedForElevators, new ByteObjForListBox(0x14, Lang.GetAttributeText(aLang.SpecialType14_UsedForElevators)));
            SpecialTypeList.Add(SpecialType.T15_AdaGrappleGun, new ByteObjForListBox(0x15, Lang.GetAttributeText(aLang.SpecialType15_AdaGrappleGun)));
            ListBoxProperty.SpecialTypeList = SpecialTypeList;

        }

        /// <summary>
        /// prenche EnemiesList, EtcmodelsList, ItemsList em ListBoxProperty class
        /// </summary>
        public static void StartSetListBoxsPropertybjsInfoLists()
        {

            //ListBoxProperty.EnemiesList
            Dictionary<ushort, UshortObjForListBox> Enemies = new Dictionary<ushort, UshortObjForListBox>();
            foreach (var item in DataBase.EnemiesIDs)
            {
                string ID = item.Value.GameId.ToString("X4");
                if (ID[2] == 'F' && ID[3] == 'F'){ continue;}
                if (ID == "FFFF") { continue; }
                Enemies.Add(item.Value.GameId, new UshortObjForListBox(item.Value.GameId, item.Value.GameId.ToString("X4") + ": " + item.Value.Description));
            }
            Enemies = Enemies.OrderBy(o => o.Key).ToDictionary(p => p.Key, p => p.Value);
            ListBoxProperty.EnemiesList = Enemies;



            //ListBoxProperty.EtcmodelsList
            Dictionary<ushort, UshortObjForListBox> EtcModels = new Dictionary<ushort, UshortObjForListBox>();
            foreach (var item in DataBase.EtcModelIDs)
            {
                string ID = item.Value.GameId.ToString("X4");
                if (ID[2] == 'F' && ID[3] == 'F') { continue; }
                if (ID == "FFFF") { continue; }
                EtcModels.Add(item.Value.GameId, new UshortObjForListBox(item.Value.GameId, item.Value.GameId.ToString("X4") + ": " + item.Value.Description));
            }
            EtcModels = EtcModels.OrderBy(o => o.Key).ToDictionary(p => p.Key, p => p.Value);
            ListBoxProperty.EtcmodelsList = EtcModels;



            //ListBoxProperty.ItemsList
            Dictionary<ushort, UshortObjForListBox> Itens = new Dictionary<ushort, UshortObjForListBox>();
            foreach (var item in DataBase.ItemsIDs)
            {
                if (item.Value.GameId == ushort.MaxValue) { continue; }
                Itens.Add(item.Value.GameId, new UshortObjForListBox(item.Value.GameId, item.Value.GameId.ToString("X4") + ": " + item.Value.Description));
            }
            Itens = Itens.OrderBy(o => o.Key).ToDictionary(p => p.Key, p => p.Value);
            ListBoxProperty.ItemsList = Itens;
        }

        /// <summary>
        /// metodo destinado a instanciar os Nodes de Grupos;
        /// </summary>
        public static void StartCreateNodes() 
        {
            EnemyNodeGroup esl = new EnemyNodeGroup();
            esl.Group = GroupType.ESL;
            esl.Text = Lang.GetText(eLang.NodeESL);
            esl.Name = Consts.NodeESL;
            esl.ForeColor = Globals.NodeColorESL;
            esl.NodeFont = Globals.TreeNodeFontText;

            EtcModelNodeGroup ets = new EtcModelNodeGroup();
            ets.Group = GroupType.ETS;
            ets.Text = Lang.GetText(eLang.NodeETS);
            ets.Name = Consts.NodeETS;
            ets.ForeColor = Globals.NodeColorETS;
            ets.NodeFont = Globals.TreeNodeFontText;

            SpecialNodeGroup ita = new SpecialNodeGroup();
            ita.Group = GroupType.ITA;
            ita.Text = Lang.GetText(eLang.NodeITA);
            ita.Name = Consts.NodeITA;
            ita.ForeColor = Globals.NodeColorITA;
            ita.NodeFont = Globals.TreeNodeFontText;

            SpecialNodeGroup aev = new SpecialNodeGroup();
            aev.Group = GroupType.AEV;
            aev.Text = Lang.GetText(eLang.NodeAEV);
            aev.Name = Consts.NodeAEV;
            aev.ForeColor = Globals.NodeColorAEV;
            aev.NodeFont = Globals.TreeNodeFontText;

            ExtraNodeGroup extras = new ExtraNodeGroup();
            extras.Group = GroupType.EXTRAS;
            extras.Text = Lang.GetText(eLang.NodeEXTRAS);
            extras.Name = Consts.NodeEXTRAS;
            extras.ForeColor = Globals.NodeColorEXTRAS;
            extras.NodeFont = Globals.TreeNodeFontText;

            DataBase.NodeESL = esl;
            DataBase.NodeETS = ets;
            DataBase.NodeITA = ita;
            DataBase.NodeAEV = aev;
            DataBase.NodeEXTRAS = extras;
        }

        /// <summary>
        /// metodo destinado a instanciar o node do ExtraGroup;
        /// </summary>
        public static void StartExtraGroup() 
        {
            DataBase.Extras = new Class.Files.ExtraGroup();
            DataBase.NodeEXTRAS.DisplayMethods = DataBase.Extras.DisplayMethods;
            DataBase.NodeEXTRAS.MoveMethods = DataBase.Extras.MoveMethods;
        }

        public static List<ushort> AllUshots() 
        {
            List<ushort> list = new List<ushort>();
            for (ushort i = 0; i < ushort.MaxValue; i++)
            {
                list.Add(i);
            }
            return list;
        }

        public static OpenTK.Vector4 ColorToVector4(Color color) 
        {
            return new OpenTK.Vector4((float)color.R / byte.MaxValue, (float)color.G / byte.MaxValue, (float)color.B / byte.MaxValue, (float)color.A / byte.MaxValue);
        }


        public static Color Vector4ToColor(OpenTK.Vector4 color)
        {
            return Color.FromArgb((int)Math.Round(color.W * byte.MaxValue), (int)Math.Round(color.X * byte.MaxValue), (int)Math.Round(color.Y * byte.MaxValue), (int)Math.Round(color.Z * byte.MaxValue));
        }

        public static float EnemyAngleToRad(short EnemyAngle)
        {
            //return ((2 * (float)Math.PI) * EnemyAngle) / 32768;
            return (MathHelper.TwoPi * EnemyAngle) / 32768;
        }

        public static short RadToEnemyAngle(float RadAngle)
        {
            //float temp = (32768 * RadAngle) / (2 * (float)Math.PI);
            float temp = (32768 * RadAngle) / MathHelper.TwoPi;
            if (temp > short.MaxValue) { temp = short.MaxValue; }
            if (temp < short.MinValue) { temp = short.MinValue; }
            return (short)temp;
        }

        public static float RadAngle1Scale(float RadAngle) 
        {
            float rad = RadAngle;
            while (rad > MathHelper.TwoPi)
            {
                rad -= MathHelper.TwoPi;
            }
            while (rad < -MathHelper.TwoPi)
            {
                rad += MathHelper.TwoPi;
            }
            return rad;
        }

        public static SpecialFileFormat GroupTypeToSpecialFileFormat(GroupType group) 
        {
            switch (group)
            {
                case GroupType.ITA:
                    return SpecialFileFormat.ITA;
                case GroupType.AEV:
                    return SpecialFileFormat.AEV;
            }
            return SpecialFileFormat.NULL;
        }

        public static SpecialType ToSpecialType(byte specialType)
        {
            if (specialType < 0x16)
            {
                return (SpecialType)specialType;
            }
            return SpecialType.UnspecifiedType;
        }

        public static void ToMoveCheckLimits(ref Vector3[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Vector3 vec = value[i];
                if (float.IsNaN(vec.X) || float.IsInfinity(vec.X)) { vec.X = 0; }
                if (float.IsNaN(vec.Y) || float.IsInfinity(vec.Y)) { vec.Y = 0; }
                if (float.IsNaN(vec.Z) || float.IsInfinity(vec.Z)) { vec.Z = 0; }
                if (vec.X > Consts.MyFloatMax) { vec.X = Consts.MyFloatMax; }
                if (vec.X < -Consts.MyFloatMax) { vec.X = -Consts.MyFloatMax; }
                if (vec.Y > Consts.MyFloatMax) { vec.Y = Consts.MyFloatMax; }
                if (vec.Y < -Consts.MyFloatMax) { vec.Y = -Consts.MyFloatMax; }
                if (vec.Z > Consts.MyFloatMax) { vec.Z = Consts.MyFloatMax; }
                if (vec.Z < -Consts.MyFloatMax) { vec.Z = -Consts.MyFloatMax; }
                value[i] = vec;
            }
        }

        public static Vector3[] GetObjScale_ToMove_Null(ushort ID) { return null; }
        public static void SetObjScale_ToMove_Null(ushort ID, Vector3[] value) { }

        /// <summary>
        /// Recarrega os arquivos Json e seus textos;
        /// </summary>
        public static void ReloadJsonFiles() 
        {
            StartLoadRoomInfoList();
            StartLoadObjsInfoLists();
            StartSetTextTranslationLists();
            StartEnemyExtraSegmentList();
            StartSetListBoxsPropertybjsInfoLists();
        }

        /// <summary>
        /// Recarrega os modelos 3d;
        /// </summary>
        public static void ReloadModels()
        {
            DataBase.InternalModels.ClearGL();
            DataBase.ItemsModels.ClearGL();
            DataBase.EtcModels.ClearGL();
            DataBase.EnemiesModels.ClearGL();
            StartLoadObjsModels();
            if (DataBase.SelectedRoom != null)
            {
                DataBase.SelectedRoom.ClearGL();
                DataBase.SelectedRoom = null;
            }
            GC.Collect();
        }


        public static UshortObjForListBox[] ItemRotationOrderForListBox() 
        {
            UshortObjForListBox[] list = new UshortObjForListBox[15];
            list[0] = new UshortObjForListBox(0, Lang.GetText(eLang.RotationXYZ));
            list[1] = new UshortObjForListBox(1, Lang.GetText(eLang.RotationXZY));
            list[2] = new UshortObjForListBox(2, Lang.GetText(eLang.RotationYXZ));
            list[3] = new UshortObjForListBox(3, Lang.GetText(eLang.RotationYZX));
            list[4] = new UshortObjForListBox(4, Lang.GetText(eLang.RotationZYX));
            list[5] = new UshortObjForListBox(5, Lang.GetText(eLang.RotationZXY));
            list[6] = new UshortObjForListBox(6, Lang.GetText(eLang.RotationXY));
            list[7] = new UshortObjForListBox(7, Lang.GetText(eLang.RotationXZ));
            list[8] = new UshortObjForListBox(8, Lang.GetText(eLang.RotationYX));
            list[9] = new UshortObjForListBox(9, Lang.GetText(eLang.RotationYZ));
            list[10] = new UshortObjForListBox(10, Lang.GetText(eLang.RotationZX));
            list[11] = new UshortObjForListBox(11, Lang.GetText(eLang.RotationZY));
            list[12] = new UshortObjForListBox(12, Lang.GetText(eLang.RotationX));
            list[13] = new UshortObjForListBox(13, Lang.GetText(eLang.RotationY));
            list[14] = new UshortObjForListBox(14, Lang.GetText(eLang.RotationZ));

            return list;
        }

        /// <summary>
        /// carrega a lista de idiomas disponiveis para selecionar no programa
        /// </summary>
        public static void StartLoadLangList()
        {
            if (Directory.Exists(Consts.langDiretory) && File.Exists(Consts.LangListFileDiretory))
            {
                Globals.Langs.AddRange(JSON.LangListFile.parseLangList(Consts.LangListFileDiretory));
            }
        
        }

        /// <summary>
        /// carrega o conteudo da tradução selecionada.
        /// </summary>
        public static void StartLoadLangFile() 
        {
            if (Globals.BackupConfigs.LoadLangTranslation)
            {
                LangObjForList lang = Globals.Langs.Find(l => l.LangID == Globals.BackupConfigs.LangID);
                if (lang != null && File.Exists(Consts.langDiretory + lang.LangFilePath))
                {
                    Lang.LoadedTranslation = true;
                    Lang.StartOthersTexts();
                    LangFile.parseLang(Consts.langDiretory + lang.LangFilePath);
                }
            }
       
        }

        /// <summary>
        /// preenche as listagens de EnemiesIDs, EtcModelIDs, ItemsIDs, RoomList; com a tradução
        /// </summary>
        public static void StartSetTextTranslationLists() 
        {
            if (Lang.LoadedTranslation == true)
            {
                foreach (var item in DataBase.EnemiesIDs)
                {
                    string Key = item.Key.ToString("X4");
                    if (Lang.Lists.Enemy.ContainsKey(Key))
                    {
                        if (Lang.Lists.Enemy[Key].Key != null)
                        {
                            item.Value.Name = Lang.Lists.Enemy[Key].Key;
                        }
                        if (Lang.Lists.Enemy[Key].Value != null)
                        {
                            item.Value.Description = Lang.Lists.Enemy[Key].Value;
                        }
                    }
                }


                foreach (var item in DataBase.EtcModelIDs)
                {
                    string Key = item.Key.ToString("X4");
                    if (Lang.Lists.EtcModel.ContainsKey(Key))
                    {
                        if (Lang.Lists.EtcModel[Key].Key != null)
                        {
                            item.Value.Name = Lang.Lists.EtcModel[Key].Key;
                        }
                        if (Lang.Lists.EtcModel[Key].Value != null)
                        {
                            item.Value.Description = Lang.Lists.EtcModel[Key].Value;
                        }
                    }
                }


                foreach (var item in DataBase.ItemsIDs)
                {
                    string Key = item.Key.ToString("X4");
                    if (Lang.Lists.Item.ContainsKey(Key))
                    {
                        if (Lang.Lists.Item[Key].Key != null)
                        {
                            item.Value.Name = Lang.Lists.Item[Key].Key;
                        }
                        if (Lang.Lists.Item[Key].Value != null)
                        {
                            item.Value.Description = Lang.Lists.Item[Key].Value;
                        }
                    }
                }


                foreach (var item in DataBase.RoomList)
                {
                    //item.RoomKey
                    if (Lang.Lists.Room.ContainsKey(item.RoomKey))
                    {
                        if (Lang.Lists.Room[item.RoomKey].Key != null)
                        {
                            item.Name = Lang.Lists.Room[item.RoomKey].Key;
                        }
                        if (Lang.Lists.Room[item.RoomKey].Value != null)
                        {
                            item.Description = Lang.Lists.Room[item.RoomKey].Value;
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// define o OpenGL usado, se vai ser OldOpenGL (2.0 a 3.3), ou ModernOpenGL (a partir de 4.0)
        /// </summary>
        public static void Defines_The_OpenGL_Used()
        {
            //Globals.UseOldGL
            //Globals.OpenGLVersion
            try
            {
                string glString = Globals.OpenGLVersion.Trim();

                if (glString.StartsWith("1.") || glString.StartsWith("2.") || glString.StartsWith("3."))
                {
                    Globals.UseOldGL = true;
                }

                if (glString.StartsWith("1."))
                {
                    System.Windows.Forms.MessageBox.Show(
                        "Error: You have an outdated version of OpenGL, which is not supported by this program." +
                        " The program will now exit.\n\n" +
                        "OpenGL version: [" + Globals.OpenGLVersion + "]\n",
                        "OpenGL version error:",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                      "Error: " +
                      ex.Message,
                      "Error detecting OpenGL version:",
                      System.Windows.Forms.MessageBoxButtons.OK,
                      System.Windows.Forms.MessageBoxIcon.Error);
            }

            if (Globals.BackupConfigs.ForceUseModernOpenGL)
            {
                Globals.UseOldGL = false;
            }
            if (Globals.BackupConfigs.ForceUseOldOpenGL)
            {
                Globals.UseOldGL = true;
            }
        }





    }


}
