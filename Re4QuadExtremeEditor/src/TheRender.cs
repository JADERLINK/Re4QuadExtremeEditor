using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Re4QuadExtremeEditor.src.Class;
using Re4QuadExtremeEditor.src.Class.TreeNodeObj;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src
{
    /// <summary>
    /// Classe destinada a renderizar tudo no cenario (No ambiente GL);
    /// </summary>
    public static class TheRender
    {
        #region info
        /*
        Para rederização Normal usar:
        TheRender.Render(ref camMtx, ref ProjMatrix, float);

        Para seleção de objeto usa-se a renderização "ToSelect":
        TheRender.RenderToSelect(ref camMtx, ref ProjMatrix);

        -------
        Referente a escala das coordenadas:
            Toda a escala do que esta sendo renderizado é 100 vezes menor que a escala de coordenadas dos arquivos AEV, ITA e ETS;

        Sendo para os arquivos PMD:
            * pmds de itens, inimigos e etcmodel, estão na scala 1 pra 1 no oque esta sendo renderizado,
            * os pmds dos cenarios são 10 vezes menor que os pmds citados acima.

        Para as coordenas do ESL:
            * os valores shorts das coordenadas devem ser multiplicado por 10 para estar na mesma escala que o AEV.

        Para outros arquivos:
            * caso na edição de novos arquivos, dividir as coordenadas por 100, pois estão na mesma escala que o AEV.

        -----
        Referente a renderização para a seleção:
        Todo o fundo é renderizado na cor branca (white) int: 0xFFFFFFFF
        Todo o model 3d do cenario (Room) é renderizado na cor preta (black) int: 0x000000FF

        Todos os objtos são rederizados por cores, nas quais são definidas pelo id e grupo do objeto, Sendo

        O primeiro e segundo byte da cor o ID (lineID) do objeto
        E o terceiro byte o Grupo do objeto
        E o quarto byte é 0xFF pois tem que ser uma cor solida.

        Nota: no openGL as cores vão de 0 a 1, então todos os valores devem ser divididos por 255;

        Os numeros dos grupos não podem ser 0 e 255, os outros valores podem ser usado, sendo os ja usados:
        1 = ESL
        2 = ETS
        3 = ITA
        4 = AEV
        5 = EXTRAS

        */
        #endregion


        private static readonly Vector3 boundOff = new Vector3(1f, 1f, 1f);
        private static readonly Vector3 boundNoneEnemy = new Vector3(3f, 4f, 3f);
        private static readonly Vector3 boundNoneEtcModel = new Vector3(3f, 3f, 3f);
        private static readonly Vector3 boundNoneItem = new Vector3(1.5f, 1.5f, 1.5f);
        private static readonly Vector3 boundNoneExtras = new Vector3(2f, 2f, 2f);

        #region normal Render

        private static void drawGrid(float objY)
        {
            DataBase.ShaderBoundingBox.Use();
            DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
            DataBase.ShaderBoundingBox.SetMatrix4("mRotation", Matrix4.Identity);
            DataBase.ShaderBoundingBox.SetAltRotation(OldRotation.Identity);
            DataBase.ShaderBoundingBox.SetVector3("mPosition", new Vector3(0, objY, 0));
            DataBase.ShaderBoundingBox.SetVector4("mColor", Globals.GL_ColorGrid);
            DataBase.ShaderBoundingBox.Start();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            if (Globals.CamGridvalue != 0)
            {
                float spaceBetweenTheLines = Globals.CamGridvalue / 10f;
                int lenght = 6560;
                int numberLines = (int)(lenght / spaceBetweenTheLines);
                int numberLines2 = numberLines / 2;
                float endPoint = (numberLines2 * spaceBetweenTheLines);
                float positionX = 0;
                float positionZ = 0;
                GL.PushMatrix();
                GL.Begin(PrimitiveType.Lines);
                for (int i = 0; i <= numberLines2; i++)
                {
                    // draw vertical line
                    GL.Vertex3(positionX, 0, -endPoint);
                    GL.Vertex3(positionX, 0, endPoint);

                    // draw horizontal line
                    GL.Vertex3(-endPoint, 0, positionZ);
                    GL.Vertex3(endPoint, 0, positionZ);

                    positionX -= spaceBetweenTheLines;
                    positionZ -= spaceBetweenTheLines;
                }
                positionX = 0;
                positionZ = 0;
                for (int i = 0; i <= numberLines2; i++)
                {
                    // draw vertical line
                    GL.Vertex3(positionX, 0, -endPoint);
                    GL.Vertex3(positionX, 0, endPoint);

                    // draw horizontal line
                    GL.Vertex3(-endPoint, 0, positionZ);
                    GL.Vertex3(endPoint, 0, positionZ);

                    positionX += spaceBetweenTheLines;
                    positionZ += spaceBetweenTheLines;
                }
                GL.End();
                GL.PopMatrix();
            }
            GL.Enable(EnableCap.Texture2D);
        }

        public static void Render(ref Matrix4 camMtx, ref Matrix4 ProjMatrix, float objY) 
        {
            GL.ClearColor(Globals.SkyColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (Globals.RenderRoom && DataBase.SelectedRoom != null)
            {
                DataBase.ShaderRoom.Use();
                DataBase.ShaderRoom.SetMatrix4("view", camMtx);
                DataBase.ShaderRoom.SetMatrix4("projection", ProjMatrix);
                DataBase.ShaderRoom.Start();
                DataBase.SelectedRoom.Render();
            }

            DataBase.ShaderObjs.Use();
            DataBase.ShaderObjs.SetMatrix4("view", camMtx);
            DataBase.ShaderObjs.SetMatrix4("projection", ProjMatrix);
            DataBase.ShaderBoundingBox.Use();
            DataBase.ShaderBoundingBox.SetMatrix4("view", camMtx);
            DataBase.ShaderBoundingBox.SetMatrix4("projection", ProjMatrix);

            if (Globals.RenderEnemyESL)
            {
                RenderEnemyESL();
            }

            if (Globals.RenderExtraObjs)
            {
                RenderExtras();
            }

            if (Globals.RenderEventsAEV)
            {
                RenderAEV();
            }

            if (Globals.RenderItemsITA)
            {
                RenderITA();
            }

            if (Globals.RenderEtcmodelETS)
            {
                RenderEtcModelETS();
            }

            if (Globals.CamGridEnable)
            {
                drawGrid(objY);
            }

            RenderPosTriggerZoneBox();

            
            GL.Finish();

        }
       
        private static void RenderEnemyESL()
        {
            Vector4 mColor = Globals.GL_ColorESL;
            Vector4 mColorSelected = Globals.GL_ColorSelected;

            DataBase.ShaderObjs.Use();
            DataBase.ShaderObjs.SetVector3("mScale", Vector3.One);
            DataBase.ShaderBoundingBox.Use();      
            DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);

            foreach (TreeNode item in DataBase.NodeESL.Nodes)
            {
                ushort ID = ((Object3D)item).ObjLineRef;
                ushort EnemiesID = DataBase.NodeESL.MethodsForGL.GetEnemyModelID(ID);
                ushort EnemyRoom = DataBase.NodeESL.MethodsForGL.GetEnemyRoom(ID);
                byte EnableState = DataBase.NodeESL.MethodsForGL.GetEnableState(ID);

                if (Globals.RenderDisabledEnemy || EnableState != 0)
                {
                    if (Globals.RenderDontShowOnlyDefinedRoom || EnemyRoom == Globals.RenderEnemyFromDefinedRoom)
                    {

                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetMatrix4("mRotation", DataBase.NodeESL.MethodsForGL.GetRotation(ID));
                        DataBase.ShaderBoundingBox.SetAltRotation(DataBase.NodeESL.MethodsForGL.GetOldRotation(ID));
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", DataBase.NodeESL.MethodsForGL.GetPosition(ID));
                        DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);
                        if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode()))
                        {
                            DataBase.ShaderBoundingBox.SetVector4("mColor", mColorSelected);
                        }

                        DataBase.ShaderObjs.Use();
                        DataBase.ShaderObjs.SetMatrix4("mRotation", DataBase.NodeESL.MethodsForGL.GetRotation(ID));
                        DataBase.ShaderObjs.SetAltRotation(DataBase.NodeESL.MethodsForGL.GetOldRotation(ID));
                        DataBase.ShaderObjs.SetVector3("mPosition", DataBase.NodeESL.MethodsForGL.GetPosition(ID));

                        if (!DataBase.EnemiesIDs.ContainsKey(EnemiesID))
                        {
                            string eId = EnemiesID.ToString("X4");
                            eId = eId[0].ToString() + eId[1].ToString() + "FF";
                            EnemiesID = ushort.Parse(eId, System.Globalization.NumberStyles.HexNumber);
                        }

                        if (DataBase.EnemiesIDs.ContainsKey(EnemiesID) && !DataBase.EnemiesIDs[EnemiesID].UseInternalModel && DataBase.EnemiesModels.Models.ContainsKey(DataBase.EnemiesIDs[EnemiesID].ModelKey))
                        {
                            DataBase.ShaderObjs.Start();
                            DataBase.EnemiesModels.RenderModel(DataBase.EnemiesIDs[EnemiesID].ModelKey);

                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(DataBase.EnemiesModels.Models[DataBase.EnemiesIDs[EnemiesID].ModelKey].UpperBoundary + boundOff, DataBase.EnemiesModels.Models[DataBase.EnemiesIDs[EnemiesID].ModelKey].LowerBoundary - boundOff);
                        }
                        else if (DataBase.EnemiesIDs.ContainsKey(EnemiesID) && DataBase.EnemiesIDs[EnemiesID].UseInternalModel && DataBase.InternalModels.Models.ContainsKey(DataBase.EnemiesIDs[EnemiesID].ModelKey))
                        {
                            DataBase.ShaderObjs.Start();
                            DataBase.InternalModels.RenderModel(DataBase.EnemiesIDs[EnemiesID].ModelKey);

                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(DataBase.InternalModels.Models[DataBase.EnemiesIDs[EnemiesID].ModelKey].UpperBoundary + boundOff, DataBase.InternalModels.Models[DataBase.EnemiesIDs[EnemiesID].ModelKey].LowerBoundary - boundOff);
                        }
                        else
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(boundNoneEnemy, -boundNoneEnemy);
                        }


                    }
                }

            }

        }

        private static void RenderEtcModelETS()
        {         
            Vector4 mColor = Globals.GL_ColorETS;
            Vector4 mColorSelected = Globals.GL_ColorSelected;

            foreach (TreeNode item in DataBase.NodeETS.Nodes)
            {
                ushort ID = ((Object3D)item).ObjLineRef;

                Vector3 boundOffFix = boundOff;
                Vector3 scale = DataBase.NodeETS.MethodsForGL.GetScale(ID);
                if (scale.X < 0) { boundOffFix.X *= -1; }
                if (scale.Y < 0) { boundOffFix.Y *= -1; }
                if (scale.Z < 0) { boundOffFix.Z *= -1; }

                DataBase.ShaderBoundingBox.Use();
                DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                DataBase.ShaderBoundingBox.SetMatrix4("mRotation", DataBase.NodeETS.MethodsForGL.GetAngle(ID));
                DataBase.ShaderBoundingBox.SetAltRotation(DataBase.NodeETS.MethodsForGL.GetOldRotation(ID));
                DataBase.ShaderBoundingBox.SetVector3("mPosition", DataBase.NodeETS.MethodsForGL.GetPosition(ID));
                DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);
                if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode()))
                {
                    DataBase.ShaderBoundingBox.SetVector4("mColor", mColorSelected);
                }

                DataBase.ShaderObjs.Use();
                DataBase.ShaderObjs.SetVector3("mScale", DataBase.NodeETS.MethodsForGL.GetScale(ID));
                DataBase.ShaderObjs.SetMatrix4("mRotation", DataBase.NodeETS.MethodsForGL.GetAngle(ID));
                DataBase.ShaderObjs.SetAltRotation(DataBase.NodeETS.MethodsForGL.GetOldRotation(ID));
                DataBase.ShaderObjs.SetVector3("mPosition", DataBase.NodeETS.MethodsForGL.GetPosition(ID));

                ushort EtcModelID = DataBase.NodeETS.MethodsForGL.GetEtcModelID(ID);

                if (DataBase.EtcModelIDs.ContainsKey(EtcModelID) && !DataBase.EtcModelIDs[EtcModelID].UseInternalModel && DataBase.EtcModels.Models.ContainsKey(DataBase.EtcModelIDs[EtcModelID].ModelKey))
                {
                    DataBase.ShaderObjs.Start();
                    DataBase.EtcModels.RenderModel(DataBase.EtcModelIDs[EtcModelID].ModelKey);

                    //DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.Start();
                    BoundingBox.draw((DataBase.EtcModels.Models[DataBase.EtcModelIDs[EtcModelID].ModelKey].UpperBoundary * DataBase.NodeETS.MethodsForGL.GetScale(ID)) + boundOffFix, (DataBase.EtcModels.Models[DataBase.EtcModelIDs[EtcModelID].ModelKey].LowerBoundary * DataBase.NodeETS.MethodsForGL.GetScale(ID)) - boundOffFix);
                }
                else if (DataBase.EtcModelIDs.ContainsKey(EtcModelID) && DataBase.EtcModelIDs[EtcModelID].UseInternalModel && DataBase.InternalModels.Models.ContainsKey(DataBase.EtcModelIDs[EtcModelID].ModelKey))
                {
                    DataBase.ShaderObjs.Start();
                    DataBase.InternalModels.RenderModel(DataBase.EtcModelIDs[EtcModelID].ModelKey);

                    //DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.Start();
                    BoundingBox.draw((DataBase.InternalModels.Models[DataBase.EtcModelIDs[EtcModelID].ModelKey].UpperBoundary * DataBase.NodeETS.MethodsForGL.GetScale(ID)) + boundOffFix, (DataBase.InternalModels.Models[DataBase.EtcModelIDs[EtcModelID].ModelKey].LowerBoundary * DataBase.NodeETS.MethodsForGL.GetScale(ID)) - boundOffFix);
                }
                else
                {
                    //DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.Start();
                    BoundingBox.draw(boundNoneEtcModel, -boundNoneEtcModel);
                }

            }

        }

        private static void RenderITA() 
        {
            foreach (TreeNode item in DataBase.NodeITA.Nodes)
            {
                RenderSpecial((Object3D)item, DataBase.NodeITA.MethodsForGL);
            }
        }

        private static void RenderAEV()
        {
            foreach (TreeNode item in DataBase.NodeAEV.Nodes)
            {
                RenderSpecial((Object3D)item, DataBase.NodeAEV.MethodsForGL);
            }
        }

        private static void RenderSpecial(Object3D item, Class.ObjMethods.SpecialMethodsForGL MethodsForGL) 
        {
            Vector4 mColor = new Vector4(0, 0, 0, 1f);

            ushort ID = item.ObjLineRef;
            GroupType Group = item.Group;

            if (Group == GroupType.ITA)
            {
                mColor = Globals.GL_ColorITA;
            }
            else if (Group == GroupType.AEV)
            {
                mColor = Globals.GL_ColorAEV;
            }

            if (Globals.UseMoreSpecialColors)
            {
                mColor = ReturnMoreSpecialColor(MethodsForGL.GetSpecialType(ID), mColor);
            }

            if (MethodsForGL.GetSpecialType(ID) == SpecialType.T03_Items)
            {
                // do trigger zone
                if (Globals.RenderItemTriggerZone)
                {
                    DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                    DataBase.ShaderBoundingBox.SetMatrix4("mRotation", Matrix4.Identity);
                    DataBase.ShaderBoundingBox.SetAltRotation(OldRotation.Identity);
                    DataBase.ShaderBoundingBox.SetVector3("mPosition", Vector3.Zero);
                    Vector4 TriggerZoneColor = Globals.GL_ColorItemTriggerZone;
                    if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode()))
                    {
                        TriggerZoneColor = Globals.GL_ColorItemTriggerZoneSelected;
                    }
                    DataBase.ShaderBoundingBox.SetVector4("mColor", TriggerZoneColor);
                    DataBase.ShaderBoundingBox.Start();
                    if (MethodsForGL.GetZoneCategory(ID) == SpecialZoneCategory.Category01)
                    {
                        BoundingBox.drawTriggerZone(MethodsForGL.GetTriggerZone(ID));
                    }
                    else if (MethodsForGL.GetZoneCategory(ID) == SpecialZoneCategory.Category02)
                    {
                        Vector2[] v = MethodsForGL.GetCircleTriggerZone(ID);
                        BoundingBox.drawCircleTriggerZone(v[0], v[1], v[2].X);
                    }

                }


                // do objeto item
                DataBase.ShaderBoundingBox.Use();
                DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                DataBase.ShaderBoundingBox.SetMatrix4("mRotation", MethodsForGL.GetItemRotation(ID));
                DataBase.ShaderBoundingBox.SetAltRotation(MethodsForGL.GetItemAltRotation(ID));
                DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetItemPosition(ID));
                if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode()))
                {
                    mColor = Globals.GL_ColorSelected;
                }
                DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                DataBase.ShaderObjs.Use();
                DataBase.ShaderObjs.SetVector3("mScale", Vector3.One);
                DataBase.ShaderObjs.SetMatrix4("mRotation", MethodsForGL.GetItemRotation(ID));
                DataBase.ShaderObjs.SetAltRotation(MethodsForGL.GetItemAltRotation(ID));
                DataBase.ShaderObjs.SetVector3("mPosition", MethodsForGL.GetItemPosition(ID));

                ushort item_ID = MethodsForGL.GetItemModelID(ID);

                if (DataBase.ItemsIDs.ContainsKey(item_ID) && !DataBase.ItemsIDs[item_ID].UseInternalModel && DataBase.ItemsModels.Models.ContainsKey(DataBase.ItemsIDs[item_ID].ModelKey))
                {
                    DataBase.ShaderObjs.Start();
                    DataBase.ItemsModels.RenderModel(DataBase.ItemsIDs[item_ID].ModelKey);

                    //DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.Start();
                    BoundingBox.draw(DataBase.ItemsModels.Models[DataBase.ItemsIDs[item_ID].ModelKey].UpperBoundary + boundOff, DataBase.ItemsModels.Models[DataBase.ItemsIDs[item_ID].ModelKey].LowerBoundary - boundOff);
                }
                else if (DataBase.ItemsIDs.ContainsKey(item_ID) && DataBase.ItemsIDs[item_ID].UseInternalModel && DataBase.InternalModels.Models.ContainsKey(DataBase.ItemsIDs[item_ID].ModelKey))
                {
                    DataBase.ShaderObjs.Start();
                    DataBase.InternalModels.RenderModel(DataBase.ItemsIDs[item_ID].ModelKey);

                    //DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.Start();
                    BoundingBox.draw(DataBase.InternalModels.Models[DataBase.ItemsIDs[item_ID].ModelKey].UpperBoundary + boundOff, DataBase.InternalModels.Models[DataBase.ItemsIDs[item_ID].ModelKey].LowerBoundary - boundOff);
                }
                else
                {
                    //DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.Start();
                    BoundingBox.draw(boundNoneItem, -boundNoneItem);
                }

                //RenderItemTriggerRadius
                float ItemTrigggerRadius = MethodsForGL.GetItemTrigggerRadius(ID);
                if (Globals.RenderItemTriggerRadius && ItemTrigggerRadius != 0)
                {
                    DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                    DataBase.ShaderBoundingBox.SetMatrix4("mRotation", Matrix4.Identity);
                    DataBase.ShaderBoundingBox.SetAltRotation(OldRotation.Identity);
                    DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetItemPosition(ID));
                    Vector4 RadiusColor = Globals.GL_ColorItemTrigggerRadius;
                    if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode()))
                    {
                        RadiusColor = Globals.GL_ColorItemTrigggerRadiusSelected;
                    }
                    DataBase.ShaderBoundingBox.SetVector4("mColor", RadiusColor);
                    DataBase.ShaderBoundingBox.Start();
                    BoundingBox.drawItemTrigggerRadius(ItemTrigggerRadius);
                }

            }
            else
            {
                if (Globals.RenderSpecialTriggerZone)
                {
                    DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                    DataBase.ShaderBoundingBox.SetMatrix4("mRotation", Matrix4.Identity);
                    DataBase.ShaderBoundingBox.SetAltRotation(OldRotation.Identity);
                    DataBase.ShaderBoundingBox.SetVector3("mPosition", Vector3.Zero);
                    if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode())) { mColor = Globals.GL_ColorSelected; }
                    DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);
                    DataBase.ShaderBoundingBox.Start();
                    if (MethodsForGL.GetZoneCategory(ID) == SpecialZoneCategory.Category01)
                    {
                        BoundingBox.drawTriggerZone(MethodsForGL.GetTriggerZone(ID));
                    }
                    else if (MethodsForGL.GetZoneCategory(ID) == SpecialZoneCategory.Category02)
                    {
                        Vector2[] v = MethodsForGL.GetCircleTriggerZone(ID);
                        BoundingBox.drawCircleTriggerZone(v[0], v[1], v[2].X);
                    }
                }
            }
        }

        private static void RenderExtras() 
        {
            foreach (TreeNode item in DataBase.NodeEXTRAS.Nodes)
            {
                ushort ID = ((Object3D)item).ObjLineRef;
                var association = DataBase.Extras.AssociationList[ID];
                if (association.FileFormat == SpecialFileFormat.AEV && Globals.RenderEventsAEV)
                {
                    RenderExtrasSubPart((Object3D)item, DataBase.FileAEV.ExtrasMethodsForGL, association.LineID, association.SubObjID, SpecialFileFormat.AEV);
                }
                else if (association.FileFormat == SpecialFileFormat.ITA && Globals.RenderItemsITA)
                {
                    RenderExtrasSubPart((Object3D)item, DataBase.FileITA.ExtrasMethodsForGL, association.LineID, association.SubObjID, SpecialFileFormat.ITA);
                }

            }
        }

        private static void RenderExtrasSubPart(Object3D item, Class.ObjMethods.ExtrasMethodsForGL MethodsForGL, ushort ID, byte SubId, SpecialFileFormat FileFormat) 
        {
            SpecialType specialType = MethodsForGL.GetSpecialType(ID);
            Vector4 mColor = Globals.GL_ColorEXTRAS;

            switch (specialType)
            {
                case SpecialType.T01_WarpDoor:
                    if (Globals.RenderExtraWarpDoor)
                    {
                        if (Globals.UseMoreSpecialColors)
                        {
                            mColor = Globals.GL_MoreColor_T01_DoorWarp;
                        }

                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderBoundingBox.SetMatrix4("mRotation", MethodsForGL.GetWarpRotation(ID));
                        DataBase.ShaderBoundingBox.SetAltRotation(MethodsForGL.GetWarpAltRotation(ID));
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetFirtPosition(ID));
                        if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode()))
                        {
                            mColor = Globals.GL_ColorSelected;
                        }
                        DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                        DataBase.ShaderObjs.Use();
                        DataBase.ShaderObjs.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderObjs.SetMatrix4("mRotation", MethodsForGL.GetWarpRotation(ID));
                        DataBase.ShaderObjs.SetAltRotation(MethodsForGL.GetWarpAltRotation(ID));
                        DataBase.ShaderObjs.SetVector3("mPosition", MethodsForGL.GetFirtPosition(ID));

                        if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyWarpPoint))
                        {
                            DataBase.ShaderObjs.Start();
                            DataBase.InternalModels.RenderModel(Consts.ModelKeyWarpPoint);

                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(DataBase.InternalModels.Models[Consts.ModelKeyWarpPoint].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyWarpPoint].LowerBoundary - boundOff);
                        }
                        else
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(boundNoneExtras, -boundNoneExtras);
                        }
                    }
                    break;
                case SpecialType.T13_LocalTeleportation:
                    if (!Globals.HideExtraExceptWarpDoor)
                    {
                        if (Globals.UseMoreSpecialColors)
                        {
                            mColor = Globals.GL_MoreColor_T13_LocalTeleportation;
                        }

                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderBoundingBox.SetMatrix4("mRotation", MethodsForGL.GetLocationAndLadderRotation(ID));
                        DataBase.ShaderBoundingBox.SetAltRotation(MethodsForGL.GetLocationAndLadderAltRotation(ID));
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetFirtPosition(ID));
                        if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode()))
                        {
                            mColor = Globals.GL_ColorSelected;
                        }
                        DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                        DataBase.ShaderObjs.Use();
                        DataBase.ShaderObjs.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderObjs.SetMatrix4("mRotation", MethodsForGL.GetLocationAndLadderRotation(ID));
                        DataBase.ShaderObjs.SetAltRotation(MethodsForGL.GetLocationAndLadderAltRotation(ID));
                        DataBase.ShaderObjs.SetVector3("mPosition", MethodsForGL.GetFirtPosition(ID));

                        if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyLocalTeleportationPoint))
                        {
                            DataBase.ShaderObjs.Start();
                            DataBase.InternalModels.RenderModel(Consts.ModelKeyLocalTeleportationPoint);

                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(DataBase.InternalModels.Models[Consts.ModelKeyLocalTeleportationPoint].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyLocalTeleportationPoint].LowerBoundary - boundOff);
                        }
                        else
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(boundNoneExtras, -boundNoneExtras);
                        }
                    }
                    break;
                case SpecialType.T10_FixedLadderClimbUp:
                    if (!Globals.HideExtraExceptWarpDoor)
                    {
                        if (Globals.UseMoreSpecialColors)
                        {
                            mColor = Globals.GL_MoreColor_T10_FixedLadderClimbUp;
                        }

                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderBoundingBox.SetMatrix4("mRotation", MethodsForGL.GetLocationAndLadderRotation(ID));
                        DataBase.ShaderBoundingBox.SetAltRotation(MethodsForGL.GetLocationAndLadderAltRotation(ID));
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetFirtPosition(ID));
                        if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode()))
                        {
                            mColor = Globals.GL_ColorSelected;
                        }
                        DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                        DataBase.ShaderObjs.Use();
                        DataBase.ShaderObjs.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderObjs.SetMatrix4("mRotation", MethodsForGL.GetLocationAndLadderRotation(ID));
                        DataBase.ShaderObjs.SetAltRotation(MethodsForGL.GetLocationAndLadderAltRotation(ID));
                        DataBase.ShaderObjs.SetVector3("mPosition", MethodsForGL.GetFirtPosition(ID));

                        if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyLadderPoint)
                         && DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyLadderObj))
                        {
                            //DataBase.ShaderObjs.Use();
                            DataBase.ShaderObjs.Start();
                            DataBase.InternalModels.RenderModel(Consts.ModelKeyLadderPoint);

                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(DataBase.InternalModels.Models[Consts.ModelKeyLadderPoint].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyLadderPoint].LowerBoundary - boundOff);

                            sbyte stepCount = MethodsForGL.GetLadderStepCount(ID);
                            if (stepCount >= 2)
                            {
                                float maxHeight = DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.Y;
                                //DataBase.ShaderObjs.Use();
                                DataBase.ShaderObjs.Start();
                                DataBase.InternalModels.RenderModel(Consts.ModelKeyLadderObj);

                                for (int i = 1; i < stepCount; i++)
                                {
                                    Vector3 position = new Vector3(MethodsForGL.GetFirtPosition(ID).X,
                                        MethodsForGL.GetFirtPosition(ID).Y + maxHeight,
                                        MethodsForGL.GetFirtPosition(ID).Z);

                                    DataBase.ShaderObjs.Use();
                                    DataBase.ShaderObjs.SetVector3("mPosition", position);
                                    DataBase.ShaderObjs.Start();
                                    DataBase.InternalModels.RenderModel(Consts.ModelKeyLadderObj);

                                    maxHeight += DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.Y;
                                }

                                Vector3 UpperBoundary = new Vector3(DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.X,
                                   maxHeight,
                                   DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.Z);

                                //DataBase.ShaderBoundingBox.Use();
                                DataBase.ShaderBoundingBox.Start();
                                BoundingBox.draw(UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].LowerBoundary - boundOff);
                            }
                            else if (stepCount <= -2)
                            {
                                float minHeight = DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.Y;
                                Vector3 position1 = new Vector3(
                                      MethodsForGL.GetFirtPosition(ID).X,
                                      MethodsForGL.GetFirtPosition(ID).Y - minHeight,
                                      MethodsForGL.GetFirtPosition(ID).Z);

                                DataBase.ShaderObjs.Use();
                                DataBase.ShaderObjs.SetVector3("mPosition", position1);
                                DataBase.ShaderObjs.Start();
                                DataBase.InternalModels.RenderModel(Consts.ModelKeyLadderObj);

                                for (int i = 1; i < -stepCount; i++)
                                {
                                    minHeight += DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.Y;

                                    Vector3 position = new Vector3(
                                        MethodsForGL.GetFirtPosition(ID).X,
                                        MethodsForGL.GetFirtPosition(ID).Y - minHeight,
                                        MethodsForGL.GetFirtPosition(ID).Z);

                                    DataBase.ShaderObjs.Use();
                                    DataBase.ShaderObjs.SetVector3("mPosition", position);
                                    DataBase.ShaderObjs.Start();
                                    DataBase.InternalModels.RenderModel(Consts.ModelKeyLadderObj);
                                }

                                Vector3 UpperBoundary = new Vector3(
                                    DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.X,
                               DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].LowerBoundary.Y,
                               DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.Z);

                                Vector3 LowerBoundary = new Vector3(
                                    DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].LowerBoundary.X,
                                   -minHeight,
                                   DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].LowerBoundary.Z);

                                //DataBase.ShaderBoundingBox.Use();
                                DataBase.ShaderBoundingBox.Start();
                                BoundingBox.draw(UpperBoundary + boundOff, LowerBoundary - boundOff);
                            }
                            else 
                            {
                                if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyLadderError))
                                {
                                    //DataBase.ShaderObjs.Use();
                                    DataBase.ShaderObjs.Start();
                                    DataBase.InternalModels.RenderModel(Consts.ModelKeyLadderError);

                                    //DataBase.ShaderBoundingBox.Use();
                                    DataBase.ShaderBoundingBox.Start();
                                    BoundingBox.draw(DataBase.InternalModels.Models[Consts.ModelKeyLadderError].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyLadderError].LowerBoundary - boundOff);
                                }
                            
                            }
                        }
                        else
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(boundNoneExtras, -boundNoneExtras);
                        }
                    }
                    break;
                case SpecialType.T12_AshleyHideCommand:
                    if (!Globals.HideExtraExceptWarpDoor)
                    {
                        if (Globals.UseMoreSpecialColors)
                        {
                            mColor = Globals.GL_MoreColor_T12_AshleyHideCommand;
                        }

                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderBoundingBox.SetMatrix4("mRotation", Matrix4.Identity);
                        DataBase.ShaderBoundingBox.SetAltRotation(OldRotation.Identity);
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetAshleyPoint(ID));
                        if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode()))
                        {
                            mColor = Globals.GL_ColorSelected;
                        }
                        DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                        DataBase.ShaderObjs.Use();
                        DataBase.ShaderObjs.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderObjs.SetMatrix4("mRotation", Matrix4.Identity);
                        DataBase.ShaderObjs.SetAltRotation(OldRotation.Identity);
                        DataBase.ShaderObjs.SetVector3("mPosition", MethodsForGL.GetAshleyPoint(ID));

                        if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyAshleyPoint))
                        {
                            DataBase.ShaderObjs.Start();
                            DataBase.InternalModels.RenderModel(Consts.ModelKeyAshleyPoint);

                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(DataBase.InternalModels.Models[Consts.ModelKeyAshleyPoint].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyAshleyPoint].LowerBoundary - boundOff);
                        }
                        else
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw(boundNoneExtras, -boundNoneExtras);
                        }

                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", Vector3.Zero);
                        DataBase.ShaderBoundingBox.Start();
                        BoundingBox.drawTriggerZone(MethodsForGL.GetAshleyHidingZoneCorner(ID));
                    }
                    break;
                case SpecialType.T15_AdaGrappleGun:
                    if (!Globals.HideExtraExceptWarpDoor)
                    {
                        if (SubId == 0)
                        {
                            RenderGrappleGun(item, MethodsForGL, ID, SubId, FileFormat, MethodsForGL.GetFirtPosition(ID));
                        }
                        else if (SubId == 1)
                        {
                            RenderGrappleGun(item, MethodsForGL, ID, SubId, FileFormat, MethodsForGL.GetGrappleGunEndPosition(ID));
                        }
                        else if (SubId == 2 && MethodsForGL.GetGrappleGunParameter3(ID) != 0)
                        {
                            RenderGrappleGun(item, MethodsForGL, ID, SubId, FileFormat, MethodsForGL.GetGrappleGunThirdPosition(ID));
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private static void RenderGrappleGun(Object3D item, Class.ObjMethods.ExtrasMethodsForGL MethodsForGL, ushort ID, byte SubId, SpecialFileFormat FileFormat, Vector3 position) 
        {
            Vector4 mColor = Globals.GL_ColorEXTRAS;
            if (Globals.UseMoreSpecialColors)
            {
                mColor = Globals.GL_MoreColor_T15_AdaGrappleGun;
            }

            DataBase.ShaderBoundingBox.Use();
            DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
            DataBase.ShaderBoundingBox.SetMatrix4("mRotation", MethodsForGL.GetGrappleGunFacingAngleRotation(ID));
            DataBase.ShaderBoundingBox.SetAltRotation(MethodsForGL.GetGrappleGunFacingAngleAltRotation(ID));
            DataBase.ShaderBoundingBox.SetVector3("mPosition", position);
            if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode()))
            {
                mColor = Globals.GL_ColorSelected;
            }
            DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

            DataBase.ShaderObjs.Use();
            DataBase.ShaderObjs.SetVector3("mScale", Vector3.One);
            DataBase.ShaderObjs.SetMatrix4("mRotation", MethodsForGL.GetGrappleGunFacingAngleRotation(ID));
            DataBase.ShaderObjs.SetAltRotation(MethodsForGL.GetGrappleGunFacingAngleAltRotation(ID));
            DataBase.ShaderObjs.SetVector3("mPosition", position);

            if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyGrappleGunPoint))
            {
                DataBase.ShaderObjs.Start();
                DataBase.InternalModels.RenderModel(Consts.ModelKeyGrappleGunPoint);

                //DataBase.ShaderBoundingBox.Use();
                DataBase.ShaderBoundingBox.Start();
                BoundingBox.draw(DataBase.InternalModels.Models[Consts.ModelKeyGrappleGunPoint].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyGrappleGunPoint].LowerBoundary - boundOff);
            }
            else
            {
                //DataBase.ShaderBoundingBox.Use();
                DataBase.ShaderBoundingBox.Start();
                BoundingBox.draw(boundNoneExtras, -boundNoneExtras);
            }


        }

        private static void RenderPosTriggerZoneBox()
        {
            if (Globals.RenderItemsITA)
            {
                foreach (TreeNode item in DataBase.NodeITA.Nodes)
                {
                    RenderPosTriggerZoneBoxSubPart((Object3D)item, DataBase.NodeITA.MethodsForGL);
                }
            }

            if (Globals.RenderEventsAEV)
            {
                foreach (TreeNode item in DataBase.NodeAEV.Nodes)
                {
                    RenderPosTriggerZoneBoxSubPart((Object3D)item, DataBase.NodeAEV.MethodsForGL);
                }
            }
        }

        private static void RenderPosTriggerZoneBoxSubPart(Object3D item, Class.ObjMethods.SpecialMethodsForGL MethodsForGL) 
        {
            Vector4 mColor = new Vector4(0, 0, 0, 0);

            ushort ID = item.ObjLineRef;
            GroupType Group = item.Group;

            if (Group == GroupType.ITA)
            {
                mColor = Globals.GL_ColorITA;
            }
            else if (Group == GroupType.AEV)
            {
                mColor = Globals.GL_ColorAEV;
            }

            if (Globals.UseMoreSpecialColors)
            {
                mColor = ReturnMoreSpecialColor(MethodsForGL.GetSpecialType(ID), mColor);
            }

            if (MethodsForGL.GetSpecialType(ID) != SpecialType.T03_Items && Globals.RenderSpecialTriggerZone)
            {
                DataBase.ShaderBoundingBox.Use();
                DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                DataBase.ShaderBoundingBox.SetMatrix4("mRotation", Matrix4.Identity);
                DataBase.ShaderBoundingBox.SetAltRotation(OldRotation.Identity);
                DataBase.ShaderBoundingBox.SetVector3("mPosition", Vector3.Zero);
                if (DataBase.SelectedNodes.ContainsKey(item.GetHashCode())) { mColor = Globals.GL_ColorSelected; }
                mColor.W = 0.1f;
                DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);
                DataBase.ShaderBoundingBox.Start();
                if (MethodsForGL.GetZoneCategory(ID) == SpecialZoneCategory.Category01)
                {
                    BoundingBox.drawTriggerZone_TransparentSolid(MethodsForGL.GetTriggerZone(ID));
                }
                else if (MethodsForGL.GetZoneCategory(ID) == SpecialZoneCategory.Category02)
                {
                    Vector2[] v = MethodsForGL.GetCircleTriggerZone(ID);
                    BoundingBox.drawCircleTriggerZone_TransparentSolid(v[0], v[1], v[2].X);
                }
            }
        }

        private static Vector4 ReturnMoreSpecialColor(SpecialType specialType, Vector4 color) 
        {
            switch (specialType)
            {
                case SpecialType.T00_GeneralPurpose: return Globals.GL_MoreColor_T00_GeneralPurpose;
                case SpecialType.T01_WarpDoor: return Globals.GL_MoreColor_T01_DoorWarp;
                case SpecialType.T02_CutSceneEvents: return Globals.GL_MoreColor_T02_CutSceneEvents;
                case SpecialType.T04_GroupedEnemyTrigger: return Globals.GL_MoreColor_T04_GroupedEnemyTrigger;
                case SpecialType.T05_Message: return Globals.GL_MoreColor_T05_Message;
                case SpecialType.T08_TypeWriter: return Globals.GL_MoreColor_T08_TypeWriter;
                case SpecialType.T0A_DamagesThePlayer: return Globals.GL_MoreColor_T0A_DamagesThePlayer;
                case SpecialType.T0B_FalseCollision: return Globals.GL_MoreColor_T0B_FalseCollision;
                case SpecialType.T0D_Unknown: return Globals.GL_MoreColor_T0D_Unknown;
                case SpecialType.T0E_Crouch: return Globals.GL_MoreColor_T0E_Crouch;
                case SpecialType.T10_FixedLadderClimbUp: return Globals.GL_MoreColor_T10_FixedLadderClimbUp;
                case SpecialType.T11_ItemDependentEvents: return Globals.GL_MoreColor_T11_ItemDependentEvents;
                case SpecialType.T12_AshleyHideCommand: return Globals.GL_MoreColor_T12_AshleyHideCommand;
                case SpecialType.T13_LocalTeleportation: return Globals.GL_MoreColor_T13_LocalTeleportation;
                case SpecialType.T14_UsedForElevators: return Globals.GL_MoreColor_T14_UsedForElevators;
                case SpecialType.T15_AdaGrappleGun: return Globals.GL_MoreColor_T15_AdaGrappleGun;
            }
            return color;
        }

        #endregion


        #region Select mode Render
        /// <summary>
        /// deve renderizar a mesma coisa do normal, porem somente as BoundingBox com cores solidas, ignorando a modelagem 3d;
        /// </summary>
        public static void RenderToSelect(ref Matrix4 camMtx, ref Matrix4 ProjMatrix) 
        {
            GL.ClearColor(System.Drawing.Color.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (Globals.RenderRoom && DataBase.SelectedRoom != null)
            {
                DataBase.ShaderRoom.Use();
                DataBase.ShaderRoom.SetMatrix4("view", camMtx);
                DataBase.ShaderRoom.SetMatrix4("projection", ProjMatrix);
                DataBase.ShaderRoom.Start();
                DataBase.SelectedRoom.Render_Solid(); // o cenario sera renderizado todo com a cor preta;
            }

            DataBase.ShaderBoundingBox.Use();
            DataBase.ShaderBoundingBox.SetMatrix4("view", camMtx);
            DataBase.ShaderBoundingBox.SetMatrix4("projection", ProjMatrix);

            if (Globals.RenderEnemyESL)
            {
                RenderEnemyESL_ToSelect();
            }

            if (Globals.RenderExtraObjs)
            {
                RenderExtras_ToSelect();
            }

            if (Globals.RenderEventsAEV)
            {
                RenderAEV_ToSelect();
            }

            if (Globals.RenderItemsITA)
            {
                RenderITA_ToSelect();
            }

            if (Globals.RenderEtcmodelETS)
            {
                RenderEtcModelETS_ToSelect();
            }


        }

        private static void RenderEnemyESL_ToSelect()
        {
            DataBase.ShaderBoundingBox.Use();
            DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);

            foreach (TreeNode item in DataBase.NodeESL.Nodes)
            {
                ushort ID = ((Object3D)item).ObjLineRef;
                ushort EnemiesID = DataBase.NodeESL.MethodsForGL.GetEnemyModelID(ID);
                ushort EnemyRoom = DataBase.NodeESL.MethodsForGL.GetEnemyRoom(ID);
                byte EnableState = DataBase.NodeESL.MethodsForGL.GetEnableState(ID);

                byte[] partColor = BitConverter.GetBytes(ID);
                Vector4 mColor = new Vector4((float)partColor[0] / 255, (float)partColor[1] / 255, (float)1 / 255, 1f);

                if (Globals.RenderDisabledEnemy || EnableState != 0)
                {
                    if (Globals.RenderDontShowOnlyDefinedRoom || EnemyRoom == Globals.RenderEnemyFromDefinedRoom)
                    {

                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetMatrix4("mRotation", DataBase.NodeESL.MethodsForGL.GetRotation(ID));
                        DataBase.ShaderBoundingBox.SetAltRotation(DataBase.NodeESL.MethodsForGL.GetOldRotation(ID));
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", DataBase.NodeESL.MethodsForGL.GetPosition(ID));
                        DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                        if (!DataBase.EnemiesIDs.ContainsKey(EnemiesID))
                        {
                            string eId = EnemiesID.ToString("X4");
                            eId = eId[0].ToString() + eId[1].ToString() + "FF";
                            EnemiesID = ushort.Parse(eId, System.Globalization.NumberStyles.HexNumber);
                        }

                        //DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.Start();
                        if (DataBase.EnemiesIDs.ContainsKey(EnemiesID) && !DataBase.EnemiesIDs[EnemiesID].UseInternalModel && DataBase.EnemiesModels.Models.ContainsKey(DataBase.EnemiesIDs[EnemiesID].ModelKey))
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            BoundingBox.draw_solid(DataBase.EnemiesModels.Models[DataBase.EnemiesIDs[EnemiesID].ModelKey].UpperBoundary + boundOff, DataBase.EnemiesModels.Models[DataBase.EnemiesIDs[EnemiesID].ModelKey].LowerBoundary - boundOff);
                        }
                        else if (DataBase.EnemiesIDs.ContainsKey(EnemiesID) && DataBase.EnemiesIDs[EnemiesID].UseInternalModel && DataBase.InternalModels.Models.ContainsKey(DataBase.EnemiesIDs[EnemiesID].ModelKey))
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            BoundingBox.draw_solid(DataBase.InternalModels.Models[DataBase.EnemiesIDs[EnemiesID].ModelKey].UpperBoundary + boundOff, DataBase.InternalModels.Models[DataBase.EnemiesIDs[EnemiesID].ModelKey].LowerBoundary - boundOff);
                        }
                        else
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            BoundingBox.draw_solid(boundNoneEnemy, -boundNoneEnemy);
                        }

                    }
                }

            }

        }

        private static void RenderEtcModelETS_ToSelect()
        {
            foreach (TreeNode item in DataBase.NodeETS.Nodes)
            {
                ushort ID = ((Object3D)item).ObjLineRef;

                byte[] partColor = BitConverter.GetBytes(ID);
                Vector4 mColor = new Vector4((float)partColor[0] / 255, (float)partColor[1] / 255, (float)2 / 255, 1f);

                Vector3 boundOffFix = boundOff;
                Vector3 scale = DataBase.NodeETS.MethodsForGL.GetScale(ID);
                if (scale.X < 0) { boundOffFix.X *= -1; }
                if (scale.Y < 0) { boundOffFix.Y *= -1; }
                if (scale.Z < 0) { boundOffFix.Z *= -1; }

                DataBase.ShaderBoundingBox.Use();
                DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                DataBase.ShaderBoundingBox.SetMatrix4("mRotation", DataBase.NodeETS.MethodsForGL.GetAngle(ID));
                DataBase.ShaderBoundingBox.SetAltRotation(DataBase.NodeETS.MethodsForGL.GetOldRotation(ID));
                DataBase.ShaderBoundingBox.SetVector3("mPosition", DataBase.NodeETS.MethodsForGL.GetPosition(ID));
                DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                ushort EtcModelID = DataBase.NodeETS.MethodsForGL.GetEtcModelID(ID);

                //DataBase.ShaderBoundingBox.Use();
                DataBase.ShaderBoundingBox.Start();
                if (DataBase.EtcModelIDs.ContainsKey(EtcModelID) && !DataBase.EtcModelIDs[EtcModelID].UseInternalModel && DataBase.EtcModels.Models.ContainsKey(DataBase.EtcModelIDs[EtcModelID].ModelKey))
                {
                    //DataBase.ShaderBoundingBox.Use();
                    BoundingBox.draw_solid((DataBase.EtcModels.Models[DataBase.EtcModelIDs[EtcModelID].ModelKey].UpperBoundary * DataBase.NodeETS.MethodsForGL.GetScale(ID)) + boundOffFix, (DataBase.EtcModels.Models[DataBase.EtcModelIDs[EtcModelID].ModelKey].LowerBoundary * DataBase.NodeETS.MethodsForGL.GetScale(ID)) - boundOffFix);
                }
                else if (DataBase.EtcModelIDs.ContainsKey(EtcModelID) && DataBase.EtcModelIDs[EtcModelID].UseInternalModel && DataBase.InternalModels.Models.ContainsKey(DataBase.EtcModelIDs[EtcModelID].ModelKey))
                {
                    //DataBase.ShaderBoundingBox.Use();
                    BoundingBox.draw_solid((DataBase.InternalModels.Models[DataBase.EtcModelIDs[EtcModelID].ModelKey].UpperBoundary * DataBase.NodeETS.MethodsForGL.GetScale(ID)) + boundOffFix, (DataBase.InternalModels.Models[DataBase.EtcModelIDs[EtcModelID].ModelKey].LowerBoundary * DataBase.NodeETS.MethodsForGL.GetScale(ID)) - boundOffFix);
                }
                else
                {
                    //DataBase.ShaderBoundingBox.Use();
                    BoundingBox.draw_solid(boundNoneEtcModel, -boundNoneEtcModel);
                }

            }

        }

        private static void RenderITA_ToSelect()
        {
            foreach (TreeNode item in DataBase.NodeITA.Nodes)
            {
                RenderSpecial_ToSelect((Object3D)item, DataBase.NodeITA.MethodsForGL);
            }
        }

        private static void RenderAEV_ToSelect()
        {
            foreach (TreeNode item in DataBase.NodeAEV.Nodes)
            {
                RenderSpecial_ToSelect((Object3D)item, DataBase.NodeAEV.MethodsForGL);
            }
        }

        private static void RenderSpecial_ToSelect(Object3D item, Class.ObjMethods.SpecialMethodsForGL MethodsForGL)
        {
            ushort ID = item.ObjLineRef;
            GroupType Group = item.Group;

            byte[] partColor = BitConverter.GetBytes(ID);

            Vector4 mColor = new Vector4(0, 0, 0, 1f);

            if (Group == GroupType.ITA)
            {
                mColor = new Vector4((float)partColor[0] / 255, (float)partColor[1] / 255, (float)3 / 255, 1f);
            }
            else if (Group == GroupType.AEV)
            {
                mColor = new Vector4((float)partColor[0] / 255, (float)partColor[1] / 255, (float)4 / 255, 1f);
            }

            if (MethodsForGL.GetSpecialType(ID) == SpecialType.T03_Items)
            {
                // do trigger zone
                if (Globals.RenderItemTriggerZone)
                {
                    DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                    DataBase.ShaderBoundingBox.SetMatrix4("mRotation", Matrix4.Identity);
                    DataBase.ShaderBoundingBox.SetAltRotation(OldRotation.Identity);
                    DataBase.ShaderBoundingBox.SetVector3("mPosition", Vector3.Zero);
                    DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);
                    DataBase.ShaderBoundingBox.Start();
                    if (MethodsForGL.GetZoneCategory(ID) == SpecialZoneCategory.Category01)
                    {
                        BoundingBox.drawTriggerZone_solid(MethodsForGL.GetTriggerZone(ID));
                    }
                    else if (MethodsForGL.GetZoneCategory(ID) == SpecialZoneCategory.Category02)
                    {
                        Vector2[] v = MethodsForGL.GetCircleTriggerZone(ID);
                        BoundingBox.drawCircleTriggerZone_solid(v[0], v[1], v[2].X);
                    }

                }


                // do objeto item
                DataBase.ShaderBoundingBox.Use();
                DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                DataBase.ShaderBoundingBox.SetMatrix4("mRotation", MethodsForGL.GetItemRotation(ID));
                DataBase.ShaderBoundingBox.SetAltRotation(MethodsForGL.GetItemAltRotation(ID));
                DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetItemPosition(ID));
                DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                ushort item_ID = MethodsForGL.GetItemModelID(ID);

                //DataBase.ShaderBoundingBox.Use();
                DataBase.ShaderBoundingBox.Start();
                if (DataBase.ItemsIDs.ContainsKey(item_ID) && !DataBase.ItemsIDs[item_ID].UseInternalModel && DataBase.ItemsModels.Models.ContainsKey(DataBase.ItemsIDs[item_ID].ModelKey))
                {
                    //DataBase.ShaderBoundingBox.Use();
                    BoundingBox.draw_solid(DataBase.ItemsModels.Models[DataBase.ItemsIDs[item_ID].ModelKey].UpperBoundary + boundOff, DataBase.ItemsModels.Models[DataBase.ItemsIDs[item_ID].ModelKey].LowerBoundary - boundOff);
                }
                else if (DataBase.ItemsIDs.ContainsKey(item_ID) && DataBase.ItemsIDs[item_ID].UseInternalModel && DataBase.InternalModels.Models.ContainsKey(DataBase.ItemsIDs[item_ID].ModelKey))
                {
                    //DataBase.ShaderBoundingBox.Use();
                    BoundingBox.draw_solid(DataBase.InternalModels.Models[DataBase.ItemsIDs[item_ID].ModelKey].UpperBoundary + boundOff, DataBase.InternalModels.Models[DataBase.ItemsIDs[item_ID].ModelKey].LowerBoundary - boundOff);
                }
                else
                {
                    //DataBase.ShaderBoundingBox.Use();
                    BoundingBox.draw_solid(boundNoneItem, -boundNoneItem);
                }


            }
            else
            {
                if (Globals.RenderSpecialTriggerZone)
                {
                    DataBase.ShaderBoundingBox.Use();
                    DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                    DataBase.ShaderBoundingBox.SetMatrix4("mRotation", Matrix4.Identity);
                    DataBase.ShaderBoundingBox.SetAltRotation(OldRotation.Identity);
                    DataBase.ShaderBoundingBox.SetVector3("mPosition", Vector3.Zero);
                    DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);
                    DataBase.ShaderBoundingBox.Start();
                    if (MethodsForGL.GetZoneCategory(ID) == SpecialZoneCategory.Category01)
                    {
                        BoundingBox.drawTriggerZone_solid(MethodsForGL.GetTriggerZone(ID));
                    }
                    else if (MethodsForGL.GetZoneCategory(ID) == SpecialZoneCategory.Category02)
                    {
                        Vector2[] v = MethodsForGL.GetCircleTriggerZone(ID);
                        BoundingBox.drawCircleTriggerZone_solid(v[0], v[1], v[2].X);
                    }

                }
            }
        }

        private static void RenderExtras_ToSelect()
        {
            foreach (TreeNode item in DataBase.NodeEXTRAS.Nodes)
            {
                ushort ID = ((Object3D)item).ObjLineRef;
                var association = DataBase.Extras.AssociationList[ID];
                if (association.FileFormat == SpecialFileFormat.AEV && Globals.RenderEventsAEV)
                {
                    RenderExtrasSubPart_ToSelect((Object3D)item, DataBase.FileAEV.ExtrasMethodsForGL, association.LineID, association.SubObjID, SpecialFileFormat.AEV);
                }
                else if (association.FileFormat == SpecialFileFormat.ITA && Globals.RenderItemsITA)
                {
                    RenderExtrasSubPart_ToSelect((Object3D)item, DataBase.FileITA.ExtrasMethodsForGL, association.LineID, association.SubObjID, SpecialFileFormat.ITA);
                }

            }
        }

        private static void RenderExtrasSubPart_ToSelect(Object3D item, Class.ObjMethods.ExtrasMethodsForGL MethodsForGL, ushort ID, byte SubId, SpecialFileFormat FileFormat)
        {
            SpecialType specialType = MethodsForGL.GetSpecialType(ID);

            ushort ExtraID = item.ObjLineRef;

            byte[] partColor = BitConverter.GetBytes(ExtraID);

            Vector4 mColor = new Vector4((float)partColor[0] / 255, (float)partColor[1] / 255, (float)5 / 255, 1f);

            switch (specialType)
            {
                case SpecialType.T01_WarpDoor:
                    if (Globals.RenderExtraWarpDoor)
                    {
                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderBoundingBox.SetMatrix4("mRotation", MethodsForGL.GetWarpRotation(ID));
                        DataBase.ShaderBoundingBox.SetAltRotation(MethodsForGL.GetWarpAltRotation(ID));
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetFirtPosition(ID));
                        DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                        DataBase.ShaderBoundingBox.Start();
                        if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyWarpPoint))
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            BoundingBox.draw_solid(DataBase.InternalModels.Models[Consts.ModelKeyWarpPoint].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyWarpPoint].LowerBoundary - boundOff);
                        }
                        else
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            BoundingBox.draw_solid(boundNoneExtras, -boundNoneExtras);
                        }


                    }
                    break;
                case SpecialType.T13_LocalTeleportation:
                    if (!Globals.HideExtraExceptWarpDoor)
                    {
                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderBoundingBox.SetMatrix4("mRotation", MethodsForGL.GetLocationAndLadderRotation(ID));
                        DataBase.ShaderBoundingBox.SetAltRotation(MethodsForGL.GetLocationAndLadderAltRotation(ID));
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetFirtPosition(ID));
                        DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                        DataBase.ShaderBoundingBox.Start();
                        if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyLocalTeleportationPoint))
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            BoundingBox.draw_solid(DataBase.InternalModels.Models[Consts.ModelKeyLocalTeleportationPoint].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyLocalTeleportationPoint].LowerBoundary - boundOff);
                        }
                        else
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            BoundingBox.draw_solid(boundNoneExtras, -boundNoneExtras);
                        }

                    }
                    break;
                case SpecialType.T10_FixedLadderClimbUp:
                    if (!Globals.HideExtraExceptWarpDoor)
                    {
                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderBoundingBox.SetMatrix4("mRotation", MethodsForGL.GetLocationAndLadderRotation(ID));
                        DataBase.ShaderBoundingBox.SetAltRotation(MethodsForGL.GetLocationAndLadderAltRotation(ID));
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetFirtPosition(ID));
                        DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                        if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyLadderPoint)
                         && DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyLadderObj))
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw_solid(DataBase.InternalModels.Models[Consts.ModelKeyLadderPoint].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyLadderPoint].LowerBoundary - boundOff);

                            sbyte stepCount = MethodsForGL.GetLadderStepCount(ID);

                            if (stepCount >= 2)
                            {
                                float maxHeight = DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.Y * stepCount;
                                Vector3 UpperBoundary = new Vector3(DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.X,
                                   maxHeight,
                                   DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.Z);

                                //DataBase.ShaderBoundingBox.Use();
                                DataBase.ShaderBoundingBox.Start();
                                BoundingBox.draw_solid(UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].LowerBoundary - boundOff);
                            }
                            else if (stepCount <= -2)
                            {
                                Vector3 UpperBoundary = new Vector3(
                                   DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.X,
                                   DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].LowerBoundary.Y,
                                   DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.Z);


                                float minHeight = DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].UpperBoundary.Y * (-stepCount);
                                Vector3 LowerBoundary = new Vector3(
                                    DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].LowerBoundary.X,
                                   (-minHeight),
                                   DataBase.InternalModels.Models[Consts.ModelKeyLadderObj].LowerBoundary.Z);

                                //DataBase.ShaderBoundingBox.Use();
                                DataBase.ShaderBoundingBox.Start();
                                BoundingBox.draw_solid(UpperBoundary + boundOff, LowerBoundary - boundOff);
                            }
                            else 
                            {
                                if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyLadderError))
                                {
                                    //DataBase.ShaderBoundingBox.Use();
                                    DataBase.ShaderBoundingBox.Start();
                                    BoundingBox.draw_solid(DataBase.InternalModels.Models[Consts.ModelKeyLadderError].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyLadderError].LowerBoundary - boundOff);
                                }

                            }
                        }
                        else
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            DataBase.ShaderBoundingBox.Start();
                            BoundingBox.draw_solid(boundNoneExtras, -boundNoneExtras);
                        }
                    }
                    break;
                case SpecialType.T12_AshleyHideCommand:
                    if (!Globals.HideExtraExceptWarpDoor)
                    {
                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
                        DataBase.ShaderBoundingBox.SetMatrix4("mRotation", Matrix4.Identity);
                        DataBase.ShaderBoundingBox.SetAltRotation(OldRotation.Identity);
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", MethodsForGL.GetAshleyPoint(ID));
                        DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

                        DataBase.ShaderBoundingBox.Start();
                        if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyAshleyPoint))
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            BoundingBox.draw_solid(DataBase.InternalModels.Models[Consts.ModelKeyAshleyPoint].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyAshleyPoint].LowerBoundary - boundOff);
                        }
                        else
                        {
                            //DataBase.ShaderBoundingBox.Use();
                            BoundingBox.draw_solid(boundNoneExtras, -boundNoneExtras);
                        };

                        DataBase.ShaderBoundingBox.Use();
                        DataBase.ShaderBoundingBox.SetVector3("mPosition", Vector3.Zero);
                        DataBase.ShaderBoundingBox.Start();
                        BoundingBox.drawTriggerZone_solid(MethodsForGL.GetAshleyHidingZoneCorner(ID));
                    }
                    break;
                case SpecialType.T15_AdaGrappleGun:
                    if (!Globals.HideExtraExceptWarpDoor)
                    {
                        if (SubId == 0)
                        {
                            RenderGrappleGun_ToSelect(item, MethodsForGL, ID, SubId, FileFormat, MethodsForGL.GetFirtPosition(ID));
                        }
                        else if (SubId == 1)
                        {
                            RenderGrappleGun_ToSelect(item, MethodsForGL, ID, SubId, FileFormat, MethodsForGL.GetGrappleGunEndPosition(ID));
                        }
                        else if (SubId == 2 && MethodsForGL.GetGrappleGunParameter3(ID) != 0)
                        {
                            RenderGrappleGun_ToSelect(item, MethodsForGL, ID, SubId, FileFormat, MethodsForGL.GetGrappleGunThirdPosition(ID));
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private static void RenderGrappleGun_ToSelect(Object3D item, Class.ObjMethods.ExtrasMethodsForGL MethodsForGL, ushort ID, byte SubId, SpecialFileFormat FileFormat, Vector3 position)
        {
            ushort ExtraID = item.ObjLineRef;

            byte[] partColor = BitConverter.GetBytes(ExtraID);

            Vector4 mColor = new Vector4((float)partColor[0] / 255, (float)partColor[1] / 255, (float)5 / 255, 1f);

            DataBase.ShaderBoundingBox.Use();
            DataBase.ShaderBoundingBox.SetVector3("mScale", Vector3.One);
            DataBase.ShaderBoundingBox.SetMatrix4("mRotation", MethodsForGL.GetGrappleGunFacingAngleRotation(ID));
            DataBase.ShaderBoundingBox.SetAltRotation(MethodsForGL.GetGrappleGunFacingAngleAltRotation(ID));
            DataBase.ShaderBoundingBox.SetVector3("mPosition", position);
            DataBase.ShaderBoundingBox.SetVector4("mColor", mColor);

            DataBase.ShaderBoundingBox.Start();
            if (DataBase.InternalModels.Models.ContainsKey(Consts.ModelKeyGrappleGunPoint))
            {
                //DataBase.ShaderBoundingBox.Use();
                BoundingBox.draw_solid(DataBase.InternalModels.Models[Consts.ModelKeyGrappleGunPoint].UpperBoundary + boundOff, DataBase.InternalModels.Models[Consts.ModelKeyGrappleGunPoint].LowerBoundary - boundOff);
            }
            else
            {
                //DataBase.ShaderBoundingBox.Use();
                BoundingBox.draw_solid(boundNoneExtras, -boundNoneExtras);
            }

        }


        #endregion

    }
}
