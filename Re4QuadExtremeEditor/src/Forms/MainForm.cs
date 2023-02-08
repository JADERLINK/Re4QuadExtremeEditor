﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Re4QuadExtremeEditor.src;
using Re4QuadExtremeEditor.src.Class;
using Re4QuadExtremeEditor.src.Class.TreeNodeObj;
using Re4QuadExtremeEditor.src.Forms;
using Re4QuadExtremeEditor.src.Class.Enums;
using Re4QuadExtremeEditor.src.Class.MyProperty;
using Re4QuadExtremeEditor.src.Class.MyProperty.CustomAttribute;
using Re4QuadExtremeEditor.src.Class.ObjMethods;
using Re4QuadExtremeEditor.src.Class.Files;
using Re4QuadExtremeEditor.src.Controls;
using System.IO;

namespace Re4QuadExtremeEditor
{
    public partial class MainForm : Form
    {
        GLControl glControl;
        readonly Timer myTimer = new Timer();

        CameraMoveControl cameraMove;
        ObjectMoveControl objectMove;

        #region Camera // variaveis para a camera
        Camera camera = new Camera();
        Matrix4 camMtx = Matrix4.Identity;
        Matrix4 ProjMatrix;
        // movimentação da camera
        bool isShiftDown = false, isControlDown = false, isSpaceDown = false;
        bool isMouseDown = false, isMouseMove = false;
        bool isWDown = false, isSDown = false, isADown = false, isDDown = false;
        #endregion

        // Property que fica no PropertyGrid quando não tem nada selecionado;
        readonly NoneProperty none = new NoneProperty();

        // define se esta com o PropertyGrid selecionado;
        bool InPropertyGrid = false;


        UpdateMethods updateMethods;


        public MainForm()
        {
            InitializeComponent();
            propertyGridObjs.SelectedItemWithFocusBackColor = Color.FromArgb(0x70, 0xBB, 0xDB);
            propertyGridObjs.SelectedItemWithFocusForeColor = Color.Black;
            treeViewObjs.SelectedNodeBackColor = Color.FromArgb(0x70, 0xBB, 0xDB);

            propertyGridObjs.SelectedObject = none;
            DataBase.SelectedNodes = treeViewObjs.SelectedNodes; // vinculo de referencia entra as listas

            glControl = new OpenTK.GLControl();
            glControl.Dock = DockStyle.Fill;
            glControl.Name = "glControl";
            glControl.TabIndex = 999;
            glControl.TabStop = false;
            glControl.Paint += GlControl_Paint;
            glControl.Load += GlControl_Load;
            glControl.KeyDown += GlControl_KeyDown;
            glControl.KeyUp += GlControl_KeyUp;
            glControl.Leave += GlControl_Leave;
            glControl.MouseWheel += GlControl_MouseWheel;
            glControl.MouseMove += GlControl_MouseMove;
            glControl.MouseDown += GlControl_MouseDown;
            glControl.MouseUp += GlControl_MouseUp;
            glControl.MouseLeave += GlControl_MouseLeave;
            glControl.Resize += GlControl_Resize;
            panelGL.Controls.Add(glControl);

            cameraMove = new CameraMoveControl(ref camera, updateGL, UpdateCameraMatrix);
            cameraMove.Location = new Point(panelControls.Width - cameraMove.Width, 0);
            cameraMove.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            cameraMove.Name = "cameraMove";
            cameraMove.TabIndex = 998;
            cameraMove.TabStop = false;
            panelControls.Controls.Add(cameraMove);

            objectMove = new ObjectMoveControl(ref camera, updateGL, UpdateCameraMatrix, UpdatePropertyGrid);
            objectMove.Location = new Point(0, 0);
            objectMove.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left;
            objectMove.Name = "objectMove";
            objectMove.TabIndex = 997;
            objectMove.TabStop = false;
            panelControls.Controls.Add(objectMove);

            KeyPreview = true;

            myTimer.Tick += updateWASDControls;
            myTimer.Interval = 10;
            myTimer.Enabled = false;

            camMtx = camera.GetViewMatrix();
            ProjMatrix = ReturnNewProjMatrix();

            // toda os metodos listados abaixos, tem que seguir a sequencia abaixo, se não dara erro.

            Lang.StartAttributeTexts();
            Lang.StartTexts();

            Utils.StartLoadLangList();
            Utils.StartLoadConfigs();
            Utils.StartLoadRoomInfoList();
            Utils.StartLoadObjsInfoLists();
            Utils.StartLoadPromptMessageList();
            Utils.StartLoadLangFile();
            Utils.StartSetTextTranslationLists();
            Utils.StartEnemyExtraSegmentList();
            Utils.StartSetListBoxsProperty();
            Utils.StartSetListBoxsPropertybjsInfoLists();
            if (Lang.LoadedTranslation) 
            { 
                StartUpdateTranslation();
                cameraMove.StartUpdateTranslation();
                objectMove.StartUpdateTranslation();
            }

            Utils.StartCreateNodes();
            Utils.StartExtraGroup();
            treeViewObjs.Nodes.Add(DataBase.NodeESL);
            treeViewObjs.Nodes.Add(DataBase.NodeETS);
            treeViewObjs.Nodes.Add(DataBase.NodeITA);
            treeViewObjs.Nodes.Add(DataBase.NodeAEV);
            treeViewObjs.Nodes.Add(DataBase.NodeEXTRAS);

            updateMethods = new UpdateMethods();
            updateMethods.UpdateGL = updateGL;
            updateMethods.UpdatePropertyGrid = UpdatePropertyGrid;
            updateMethods.UpdateTreeViewObjs = UpdateTreeViewObjs;
            updateMethods.UpdateMoveObjSelection = objectMove.UpdateSelection;

            //apenas para testes
            //src.JSON.LangFile.writeLangFile("SourceLang.json");
            //int finish = 0;
        }

     
        #region GlControl Events

        private Matrix4 ReturnNewProjMatrix() 
        {
            return Matrix4.CreatePerspectiveFieldOfView(Globals.FOV * ((float)Math.PI / 180.0f), (float)glControl.Width / (float)glControl.Height, 0.01f, 1000000f); //10000f
        }

        private void GlControl_Resize(object sender, EventArgs e)
        {            
            glControl.Context.Update(glControl.WindowInfo);
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            ProjMatrix = ReturnNewProjMatrix();
            glControl.Invalidate(); 
        }

        private void splitContainer1_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            glControl.Invalidate();
        }

        private void GlControl_MouseLeave(object sender, EventArgs e)
        {
            camera.resetMouseStuff();
            isMouseDown = false;
            isMouseMove = false;
        }

        private void GlControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                camera.resetMouseStuff();
                isMouseDown = false;
                isMouseMove = false;
                camera.SaveCameraPosition();
                if (!isWDown && !isSDown && !isADown && !isDDown && !isMouseMove && !isShiftDown && !isSpaceDown)
                {
                    myTimer.Enabled = false;
                }
            }    
        }

        private void GlControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                camera.resetMouseStuff();
                isMouseDown = true;
                isMouseMove = true;
                camera.SaveCameraPosition();
                myTimer.Enabled = true;
            }       
            if (e.Button == MouseButtons.Right)
            {
                selectObject(e.X, e.Y);
                glControl.Invalidate();
            }
        }

        /// <summary>
        /// metodo destinado para a seleção dos objetos no ambiente GL
        /// </summary>
        private void selectObject(int mx, int my)
        {
            int h = glControl.Height;
            TheRender.RenderToSelect(ref camMtx, ref ProjMatrix); // renderiza o ambiente GL no modo seleção.
            byte[] pixel = new byte[4];
            GL.ReadPixels(mx, h - my, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, pixel);

            //Console.WriteLine("pixel[0]: " + pixel[0]); // lineID
            //Console.WriteLine("pixel[1]: " + pixel[1]); // lineID
            //Console.WriteLine("pixel[2]: " + pixel[2]); // id da lista
            //Console.WriteLine("pixel[3]: " + pixel[3]);

            // listas
            // aviso: proibido usar os valores 0 e 255, pois fazem parte das cores preta (renderização do cenario) e da cor branca (fundo);
            // 1 = enemy
            // 2 = etcmodel
            // 3 = ITA
            // 4 = AEV
            // 5 = Extras
            if (pixel[2] > 0 && pixel[2] < 6) // caso adicionar novas lista aumentar o valor de 6;
            {
                ushort LineID = BitConverter.ToUInt16(pixel, 0);

                TreeNode selected = null;
                switch (pixel[2])
                {
                    case 1:
                        int index1 = DataBase.NodeESL.Nodes.IndexOfKey(LineID.ToString());
                        if (index1 > -1)
                        {
                            selected = DataBase.NodeESL.Nodes[index1];
                        }
                        break;
                    case 2:
                        int index2 = DataBase.NodeETS.Nodes.IndexOfKey(LineID.ToString());
                        if (index2 > -1)
                        {
                            selected = DataBase.NodeETS.Nodes[index2];
                        }
                        break;
                    case 3:
                        int index3 = DataBase.NodeITA.Nodes.IndexOfKey(LineID.ToString());
                        if (index3 > -1)
                        {
                            selected = DataBase.NodeITA.Nodes[index3];
                        }
                        break;
                    case 4:
                        int index4 = DataBase.NodeAEV.Nodes.IndexOfKey(LineID.ToString());
                        if (index4 > -1)
                        {
                            selected = DataBase.NodeAEV.Nodes[index4];
                        }
                        break;
                    case 5:
                        int index5 = DataBase.NodeEXTRAS.Nodes.IndexOfKey(LineID.ToString());
                        if (index5 > -1)
                        {
                            selected = DataBase.NodeEXTRAS.Nodes[index5];
                        }
                        break;
                }

                if (selected != null)
                {
                    if (isControlDown) // add ou remove da seleção
                    {
                        treeViewObjs.ToSelectMultiNode(selected);
                    }
                    else // seleciona so esse
                    {
                        treeViewObjs.ToSelectSingleNode(selected);
                    }

                }
            }
        }

        private void GlControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown && e.Button == MouseButtons.Left)
            {
                camera.updateCameraOffsetMatrixWithMouse(isControlDown, e.X, e.Y);
                camMtx = camera.GetViewMatrix();
            }
        }

        private void GlControl_MouseWheel(object sender, MouseEventArgs e)
        {
            camera.resetMouseStuff();
            camera.updateCameraMatrixWithScrollWheel((int)(e.Delta * 0.5f));
            camMtx = camera.GetViewMatrix();
            camera.SaveCameraPosition();
            glControl.Invalidate();
        }

        private void GlControl_Leave(object sender, EventArgs e)
        {
            isWDown = false;
            isSDown = false;
            isADown = false;
            isDDown = false;
            isSpaceDown = false;
            isShiftDown = false;
            isControlDown = false;
            isMouseDown = false;
            isMouseMove = false;
            myTimer.Enabled = false;
        }

        private void GlControl_KeyUp(object sender, KeyEventArgs e)
        {
            isShiftDown = e.Shift;
            isControlDown = e.Control;
            switch (e.KeyCode)
            {
                case Keys.W: isWDown = false; break;
                case Keys.S: isSDown = false; break;
                case Keys.A: isADown = false; break;
                case Keys.D: isDDown = false; break;
                case Keys.Space: isSpaceDown = false; break;
            }
            if (!isWDown && !isSDown && !isADown && !isDDown && !isMouseMove && !isShiftDown && !isSpaceDown)
            {
                myTimer.Enabled = false;
            }
            if (isControlDown)
            {
                camera.SaveCameraPosition();
                camera.resetMouseStuff();
            }
        }

        private void GlControl_KeyDown(object sender, KeyEventArgs e)
        {
            isShiftDown = e.Shift;
            isControlDown = e.Control;
            switch (e.KeyCode)
            {
                case Keys.W:
                    isWDown = true;
                    myTimer.Enabled = true;
                    break;
                case Keys.S:
                    isSDown = true;
                    myTimer.Enabled = true;
                    break;
                case Keys.A:
                    isADown = true;
                    myTimer.Enabled = true;
                    break;
                case Keys.D:
                    isDDown = true;
                    myTimer.Enabled = true;
                    break;
                case Keys.Space:
                    isSpaceDown = true;
                    myTimer.Enabled = true;
                    break;
            }
            if (isShiftDown)
            {
                myTimer.Enabled = true;
            }
            if (isControlDown)
            {
                camera.SaveCameraPosition();
                camera.resetMouseStuff();
            }

        }

        /// <summary>
        /// Atualiza a movimentação de wasd, e cria os "frames" da renderização.
        /// </summary>
        private void updateWASDControls(object sender, EventArgs e)
        {
            if (!isControlDown && camera.CamMode == Camera.CameraMode.FLY)
            {
                if (isWDown)
                {
                    camera.updateCameraToFront();
                }
                if (isSDown)
                {
                    camera.updateCameraToBack();
                }
                if (isDDown)
                {
                    camera.updateCameraToRight();
                }
                if (isADown)
                {
                    camera.updateCameraToLeft();
                }

                if (isShiftDown)
                {
                    camera.updateCameraToDown();
                }

                if (isSpaceDown)
                {
                    camera.updateCameraToUp();
                }

                if (isWDown || isSDown || isDDown || isADown || isShiftDown || isSpaceDown || isMouseMove)
                {
                    camMtx = camera.GetViewMatrix();
                    glControl.Invalidate();
                }

            }
            else 
            {
                glControl.Invalidate();
            }
        }


        private void GlControl_Load(object sender, EventArgs e)
        {
            Globals.OpenGLVersion = GL.GetString(StringName.Version);

            Utils.Defines_The_OpenGL_Used();

            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            GL.ClearColor(Globals.SkyColor);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            DataBase.NoTextureIdGL = Texture.GetTextureIdGL(Properties.Resources.NoTexture);
            DataBase.TransparentTextureIdGL = Texture.GetTextureIdGL(Properties.Resources.Transparent);
            DataBase.SolidTextureIdGL = Texture.GetTextureIdGL(Properties.Resources.SolidTexture);

            if (Globals.UseOldGL)
            {
                Utils.StartLoadNoShader_OldGL();
            }
            else 
            {
                Utils.StartLoadShader();
            }     

            Utils.StartLoadObjsModels();

            glControl.SwapBuffers();
        }


        private void GlControl_Paint(object sender, PaintEventArgs e)
        {
            //TheRender.RenderToSelect(ref camMtx, ref ProjMatrix); // este é da seleção
            TheRender.Render(ref camMtx, ref ProjMatrix, camera.SelectedObjPosY()); // rederiza todos os objetos do GL;
            glControl.SwapBuffers();
        }

        #endregion


        #region botões do menu edit

        private void toolStripMenuItemAddNewObj_Click(object sender, EventArgs e)
        {
            AddNewObjForm form = new AddNewObjForm();
            form.OnButtonOk_Click += OnButtonOk_Click;
            form.ShowDialog();
        }

        private void OnButtonOk_Click()
        {
            glControl.Invalidate();
        }

        private void toolStripMenuItemDeleteSelectedObj_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Lang.GetText(eLang.DeleteObjDialog), Lang.GetText(eLang.DeleteObjWarning), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (Object3D item in treeViewObjs.SelectedNodes)
                {
                    if (item.Group == GroupType.ETS)
                    {                    
                        ((EtcModelNodeGroup)item.Parent).ChangeAmountMethods.RemoveLineID(item.ObjLineRef);
                        item.Remove();
                    }
                    else if (item.Group == GroupType.ITA || item.Group == GroupType.AEV)
                    {
                        DataBase.Extras.RemoveObj(item.ObjLineRef, Utils.GroupTypeToSpecialFileFormat(item.Group));
                        ((SpecialNodeGroup)item.Parent).ChangeAmountMethods.RemoveLineID(item.ObjLineRef);
                        item.Remove();
                    }
                }
                treeViewObjs.SelectedNodes = null;
                glControl.Invalidate();
            }
        }

        private void toolStripMenuItemMoveUp_Click(object sender, EventArgs e)
        {
            var ordernedSelectedNodes = treeViewObjs.SelectedNodes.OrderBy(n => n.Index);
            foreach (Object3D item in ordernedSelectedNodes)
            {
                if (item.Group == GroupType.ETS || item.Group == GroupType.ITA || item.Group == GroupType.AEV)
                {
                    int index = item.Index;     
                    if (index > 0)
                    {
                        var Parent = item.Parent;
                        item.Remove();
                        Parent.Nodes.Insert(index -1, item);
                    }
                }
            }
        }

        private void toolStripMenuItemMoveDown_Click(object sender, EventArgs e)
        {
            var invSelectedNodes = treeViewObjs.SelectedNodes.OrderByDescending(n => n.Index);
            foreach (Object3D item in invSelectedNodes)
            {
                if (item.Group == GroupType.ETS || item.Group == GroupType.ITA || item.Group == GroupType.AEV)
                {
                    int index = item.Index;
                    var Parent = item.Parent;
                    if (index < Parent.GetNodeCount(false) -1)
                    {
                        item.Remove();
                        Parent.Nodes.Insert(index +1, item);
                    }                 
                }
            }
        }


        private void toolStripMenuItemSearch_Click(object sender, EventArgs e)
        {
            var selectedObj = propertyGridObjs.SelectedObject;
            if (selectedObj is EnemyProperty enemy)
            {
                SearchForm search = new SearchForm(ListBoxProperty.EnemiesList.Values.ToArray(), new UshortObjForListBox(enemy.ReturnUshortFirstSearchSelect(), ""));
                search.Search += enemy.Searched;
                search.ShowDialog();
            }
            else if (selectedObj is EtcModelProperty etcModel)
            {
                SearchForm search = new SearchForm(ListBoxProperty.EtcmodelsList.Values.ToArray(), new UshortObjForListBox(etcModel.ReturnUshortFirstSearchSelect(), ""));
                search.Search += etcModel.Searched;
                search.ShowDialog();
            }
            else if (selectedObj is SpecialProperty special)
            {
                var specialType = special.GetSpecialType();
                if (specialType == SpecialType.T03_Items || specialType == SpecialType.T11_ItemDependentEvents)
                {
                    SearchForm search = new SearchForm(ListBoxProperty.ItemsList.Values.ToArray(), new UshortObjForListBox(special.ReturnUshortFirstSearchSelect(), ""));
                    search.Search += special.Searched;
                    search.ShowDialog();
                }
            }
        }


        #endregion


        #region Botoes do menu

        private void SelectRoom_onLoadButtonClick(object sender, EventArgs e)
        {
            if (sender is string == false)
            {
                string text = Lang.GetText(eLang.SelectedRoom) + ": " + sender.ToString();
                if (text.Length > 100)
                {
                    text = text.Substring(0,100);
                    text += "...";
                }
                toolStripMenuItemSelectRoom.Text = text;
            }
            else
            {
                toolStripMenuItemSelectRoom.Text = Lang.GetText(eLang.SelectRoom);
            }

            if (Globals.AutoDefinedRoom)
            {
                if (DataBase.SelectedRoom != null)
                {
                    toolStripTextBoxDefinedRoom.Text = DataBase.SelectedRoom.GetRoomInfo.RoomId.ToString("X4");
                }
                else
                {
                    toolStripTextBoxDefinedRoom.Text = "0000";
                }
            }
        }

        private void toolStripMenuItemSelectRoom_Click(object sender, EventArgs e)
        {
            SelectRoomForm selectRoom = new SelectRoomForm();
            selectRoom.onLoadButtonClick += SelectRoom_onLoadButtonClick;
            selectRoom.ShowDialog();
            glControl.Invalidate();
        }

        private void toolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItemCredits_Click(object sender, EventArgs e)
        {
            CreditsForm form = new CreditsForm();
            form.ShowDialog();
        }

        private void toolStripMenuItemOptions_Click(object sender, EventArgs e)
        {
            OptionsForm form = new OptionsForm();
            form.onLoadButtonClick += SelectRoom_onLoadButtonClick;
            form.OnOKButtonClick += UpdateTreeViewObjs;
            form.OnOKButtonClick += UpdatePropertyGrid;
            form.ShowDialog();
            glControl.Invalidate();
        }


        #endregion


        #region botoes do menu view

        private void toolStripMenuItemHideRoomModel_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideRoomModel.Checked = !toolStripMenuItemHideRoomModel.Checked;
            Globals.RenderRoom = !toolStripMenuItemHideRoomModel.Checked;
            glControl.Invalidate();
        }

        private void toolStripMenuItemHideEnemyESL_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideEnemyESL.Checked = !toolStripMenuItemHideEnemyESL.Checked;
            Globals.RenderEnemyESL = !toolStripMenuItemHideEnemyESL.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemHideEtcmodelETS_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideEtcmodelETS.Checked = !toolStripMenuItemHideEtcmodelETS.Checked;
            Globals.RenderEtcmodelETS = !toolStripMenuItemHideEtcmodelETS.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemHideItemsITA_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideItemsITA.Checked = !toolStripMenuItemHideItemsITA.Checked;
            Globals.RenderItemsITA = !toolStripMenuItemHideItemsITA.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemHideEventsAEV_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideEventsAEV.Checked = !toolStripMenuItemHideEventsAEV.Checked;
            Globals.RenderEventsAEV = !toolStripMenuItemHideEventsAEV.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }


        private void toolStripMenuItemHideDesabledEnemy_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideDesabledEnemy.Checked = !toolStripMenuItemHideDesabledEnemy.Checked;
            Globals.RenderDisabledEnemy = !toolStripMenuItemHideDesabledEnemy.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripTextBoxDefinedRoom_TextChanged(object sender, EventArgs e)
        {
            Globals.RenderEnemyFromDefinedRoom = ushort.Parse(toolStripTextBoxDefinedRoom.Text, System.Globalization.NumberStyles.HexNumber);
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripTextBoxDefinedRoom_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) 
                || e.KeyChar == 'A'
                || e.KeyChar == 'B'
                || e.KeyChar == 'C'
                || e.KeyChar == 'D'
                || e.KeyChar == 'E'
                || e.KeyChar == 'F'
                || e.KeyChar == 'a'
                || e.KeyChar == 'b'
                || e.KeyChar == 'c'
                || e.KeyChar == 'd'
                || e.KeyChar == 'e'
                || e.KeyChar == 'f'
                )
            {
                if (toolStripTextBoxDefinedRoom.SelectionStart < toolStripTextBoxDefinedRoom.TextLength)
                {
                    int CacheSelectionStart = toolStripTextBoxDefinedRoom.SelectionStart;
                    StringBuilder sb = new StringBuilder(toolStripTextBoxDefinedRoom.Text);
                    sb[toolStripTextBoxDefinedRoom.SelectionStart] = e.KeyChar;
                    toolStripTextBoxDefinedRoom.Text = sb.ToString();
                    toolStripTextBoxDefinedRoom.SelectionStart = CacheSelectionStart + 1;
                }
            }
            e.Handled = true;
        }


        private void toolStripMenuItemShowOnlyDefinedRoom_Click(object sender, EventArgs e)
        {
            toolStripMenuItemShowOnlyDefinedRoom.Checked = !toolStripMenuItemShowOnlyDefinedRoom.Checked;
            Globals.RenderDontShowOnlyDefinedRoom = !toolStripMenuItemShowOnlyDefinedRoom.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemAutoDefineRoom_Click(object sender, EventArgs e)
        {
            toolStripMenuItemAutoDefineRoom.Checked = !toolStripMenuItemAutoDefineRoom.Checked;
            Globals.AutoDefinedRoom = toolStripMenuItemAutoDefineRoom.Checked;
        }

        private void toolStripMenuItemHideItemTriggerZone_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideItemTriggerZone.Checked = !toolStripMenuItemHideItemTriggerZone.Checked;
            Globals.RenderItemTriggerZone = !toolStripMenuItemHideItemTriggerZone.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemHideItemTriggerRadius_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideItemTriggerRadius.Checked = !toolStripMenuItemHideItemTriggerRadius.Checked;
            Globals.RenderItemTriggerRadius = !toolStripMenuItemHideItemTriggerRadius.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }


        private void toolStripMenuItemItemPositionAtAssociatedObjectLocation_Click(object sender, EventArgs e)
        {
            toolStripMenuItemItemPositionAtAssociatedObjectLocation.Checked = !toolStripMenuItemItemPositionAtAssociatedObjectLocation.Checked;
            Globals.RenderItemPositionAtAssociatedObjectLocation = toolStripMenuItemItemPositionAtAssociatedObjectLocation.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemHideExtraObjs_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideExtraObjs.Checked = !toolStripMenuItemHideExtraObjs.Checked;
            Globals.RenderExtraObjs = !toolStripMenuItemHideExtraObjs.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemHideSpecialTriggerZone_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideSpecialTriggerZone.Checked = !toolStripMenuItemHideSpecialTriggerZone.Checked;
            Globals.RenderSpecialTriggerZone = !toolStripMenuItemHideSpecialTriggerZone.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemUseMoreSpecialColors_Click(object sender, EventArgs e)
        {
            toolStripMenuItemUseMoreSpecialColors.Checked = !toolStripMenuItemUseMoreSpecialColors.Checked;
            Globals.UseMoreSpecialColors = toolStripMenuItemUseMoreSpecialColors.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemEtcModelUseScale_Click(object sender, EventArgs e)
        {
            toolStripMenuItemEtcModelUseScale.Checked = !toolStripMenuItemEtcModelUseScale.Checked;
            Globals.RenderEtcmodelUsingScale = toolStripMenuItemEtcModelUseScale.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemHideExtraExceptWarpDoor_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideExtraExceptWarpDoor.Checked = !toolStripMenuItemHideExtraExceptWarpDoor.Checked;
            Globals.HideExtraExceptWarpDoor = toolStripMenuItemHideExtraExceptWarpDoor.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemHideOnlyWarpDoor_Click(object sender, EventArgs e)
        {
            toolStripMenuItemHideOnlyWarpDoor.Checked = !toolStripMenuItemHideOnlyWarpDoor.Checked;
            Globals.RenderExtraWarpDoor = !toolStripMenuItemHideOnlyWarpDoor.Checked;
            treeViewObjs.Refresh();
            glControl.Invalidate();
        }

        private void toolStripMenuItemNodeDisplayNameInHex_Click(object sender, EventArgs e)
        {
            toolStripMenuItemNodeDisplayNameInHex.Checked = !toolStripMenuItemNodeDisplayNameInHex.Checked;
            Globals.TreeNodeRenderHexValues = toolStripMenuItemNodeDisplayNameInHex.Checked;
            if (Globals.TreeNodeRenderHexValues)
            {
                treeViewObjs.Font = Globals.TreeNodeFontHex;
            }
            else 
            {
                treeViewObjs.Font = Globals.TreeNodeFontText;
            }
            treeViewObjs.Refresh();
        }

        private void toolStripMenuItemRefresh_Click(object sender, EventArgs e)
        {
            glControl.Invalidate();
            treeViewObjs.Refresh();
            propertyGridObjs.Refresh();
            glControl.Update(); // Needed after calling propertyGridObjs.Refresh();
        }

        private void toolStripMenuItemResetCamera_Click(object sender, EventArgs e)
        {
            cameraMove.ResetCamera();
        }

        #endregion


        #region propertyGridObjs and TreeViewObjs

        private void updateGL() 
        {
            glControl.Invalidate();
        }

        private void UpdateCameraMatrix() 
        {
            camMtx = camera.GetViewMatrix();
        }

        public void UpdatePropertyGrid() 
        {
            propertyGridObjs.Refresh();
            glControl.Update(); // Needed after calling propertyGridObjs.Refresh();
        }

        public void UpdateTreeViewObjs()
        {
            treeViewObjs.Refresh();
        }


        private void propertyGridObjs_Enter(object sender, EventArgs e)
        {
            InPropertyGrid = true;
        }

        private void propertyGridObjs_Leave(object sender, EventArgs e)
        {
            InPropertyGrid = false;
        }

        private void propertyGridObjs_PropertySortChanged(object sender, EventArgs e)
        {
            if (propertyGridObjs.PropertySort == PropertySort.CategorizedAlphabetical)
               {propertyGridObjs.PropertySort = PropertySort.Categorized;}
        }


        private void propertyGridObjs_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            propertyGridObjs.Refresh();
            treeViewObjs.Refresh();
        }

        private void propertyGridObjs_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            //
            if (camera.isOrbitCamera())
            {
                camera.UpdateCameraOrbitOnChangeValue();
                camMtx = camera.GetViewMatrix();
            }
        }

        private void TreeViewUpdateSelecteds()
        {
            treeViewObjs.SelectedNodes = null;
            propertyGridObjs.SelectedObject = none;
        }

        private void treeViewObjs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool OldLastNodeIsNull = !(DataBase.LastSelectNode is Object3D);
            //Console.WriteLine(e.Node);
            //Console.WriteLine(treeViewObjs.SelectedNodes.Count);
            if (e.Node == null || e.Node.Parent == null || treeViewObjs.SelectedNodes.Count == 0)
            {
                propertyGridObjs.SelectedObject = none;
                DataBase.LastSelectNode = null;
            }
            else if (treeViewObjs.SelectedNodes.Count == 1 && e.Node is Object3D node)
            {
                DataBase.LastSelectNode = node;

                if (node.Group == GroupType.ESL)
                {
                    EnemyProperty p = new EnemyProperty(node.ObjLineRef, updateMethods, ((EnemyNodeGroup)node.Parent).PropertyMethods);
                    propertyGridObjs.SelectedObject = p;
                }
                else if (node.Group == GroupType.ETS)
                {
                    EtcModelProperty p = new EtcModelProperty(node.ObjLineRef, updateMethods, ((EtcModelNodeGroup)node.Parent).PropertyMethods);
                    propertyGridObjs.SelectedObject = p;
                }
                else if (node.Group == GroupType.ITA)
                {
                    SpecialProperty p = new SpecialProperty(node.ObjLineRef, updateMethods, ((SpecialNodeGroup)node.Parent).PropertyMethods);
                    propertyGridObjs.SelectedObject = p;
                }
                else if (node.Group == GroupType.AEV)
                {
                    SpecialProperty p = new SpecialProperty(node.ObjLineRef, updateMethods, ((SpecialNodeGroup)node.Parent).PropertyMethods);
                    propertyGridObjs.SelectedObject = p;
                }
                else if (node.Group == GroupType.EXTRAS)
                {
                    var r = DataBase.Extras.AssociationList[node.ObjLineRef];
                    if (r.FileFormat == SpecialFileFormat.AEV)
                    {
                        SpecialProperty p = new SpecialProperty(r.LineID, updateMethods, DataBase.NodeAEV.PropertyMethods, true);
                        propertyGridObjs.SelectedObject = p;
                    }
                    else if (r.FileFormat == SpecialFileFormat.ITA)
                    {
                        SpecialProperty p = new SpecialProperty(r.LineID, updateMethods, DataBase.NodeITA.PropertyMethods, true);
                        propertyGridObjs.SelectedObject = p;
                    }
                    else
                    {
                        propertyGridObjs.SelectedObject = none;
                    }

                }
            }
            else if (treeViewObjs.SelectedNodes.Count > 1)
            {
                DataBase.LastSelectNode = treeViewObjs.SelectedNodes[treeViewObjs.SelectedNodes.Count - 1];

                MultiSelectProperty p = new MultiSelectProperty(treeViewObjs.SelectedNodes, updateMethods);
                propertyGridObjs.SelectedObject = p;
                //propertyGridObjs.ExpandAllGridItems(); // lag
            }
            else 
            {
                propertyGridObjs.SelectedObject = none;
                DataBase.LastSelectNode = null;
            }
            if (camera.isOrbitCamera())
            {
                if (OldLastNodeIsNull)
                {
                    camera.resetOrbitToSelectedObject();
                }
                camera.UpdateCameraOrbitOnChangeObj();
                camMtx = camera.GetViewMatrix();
            }
            objectMove.UpdateSelection();
            glControl.Invalidate();
        }  

        #endregion


        #region Gerenciamento de arquivos //new

        private void toolStripMenuItemNewESL_Click(object sender, EventArgs e)
        {
            FileManager.NewFileESL();
            Globals.FilePathESL = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }

        private void toolStripMenuItemNewETS_Classic_Click(object sender, EventArgs e)
        {
            FileManager.NewFileETS(Re4Version.Classic);
            Globals.FilePathETS = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }

        private void toolStripMenuItemNewETS_UHD_Click(object sender, EventArgs e)
        {
            FileManager.NewFileETS(Re4Version.UHD);
            Globals.FilePathETS = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }

        private void toolStripMenuItemNewITA_Classic_Click(object sender, EventArgs e)
        {
            FileManager.NewFileITA(Re4Version.Classic);
            Globals.FilePathITA = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }

        private void toolStripMenuItemNewITA_UHD_Click(object sender, EventArgs e)
        {
            FileManager.NewFileITA(Re4Version.UHD);
            Globals.FilePathITA = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }

        private void toolStripMenuItemNewAEV_Classic_Click(object sender, EventArgs e)
        {
            FileManager.NewFileAEV(Re4Version.Classic);
            Globals.FilePathAEV = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }

        private void toolStripMenuItemNewAEV_UHD_Click(object sender, EventArgs e)
        {
            FileManager.NewFileAEV(Re4Version.UHD);
            Globals.FilePathAEV = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }

        #endregion

        #region Gerenciamento de arquivos //open

        private bool OpenIsUHD = false;
        private void toolStripMenuItemOpenESL_Click(object sender, EventArgs e)
        {
            openFileDialogESL.ShowDialog();
        }
        private void toolStripMenuItemOpenETS_Classic_Click(object sender, EventArgs e)
        {
            OpenIsUHD = false;
            openFileDialogETS.ShowDialog();
        }
        private void toolStripMenuItemOpenETS_UHD_Click(object sender, EventArgs e)
        {
            OpenIsUHD = true;
            openFileDialogETS.ShowDialog();
        }
        private void toolStripMenuItemOpenITA_Classic_Click(object sender, EventArgs e)
        {
            OpenIsUHD = false;
            openFileDialogITA.ShowDialog();
        }

        private void toolStripMenuItemOpenITA_UHD_Click(object sender, EventArgs e)
        {
            OpenIsUHD = true;
            openFileDialogITA.ShowDialog();
        }

        private void toolStripMenuItemOpenAEV_Classic_Click(object sender, EventArgs e)
        {
            OpenIsUHD = false;
            openFileDialogAEV.ShowDialog();
        }

        private void toolStripMenuItemOpenAEV_UHD_Click(object sender, EventArgs e)
        {
            OpenIsUHD = true;
            openFileDialogAEV.ShowDialog();
        }

        private void openFileDialogESL_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo fileInfo = null;
            try
            {
                fileInfo = new FileInfo(openFileDialogESL.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }
            if (fileInfo != null)
            {
                if (fileInfo.Length > 0x1000000)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile16MB), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else if (fileInfo.Length == 0)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile0MB), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else
                {
                    FileStream file = null;
                    try
                    {
                        file = fileInfo.OpenRead();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                        e.Cancel = true;
                        return;
                    }
                    if (file != null && fileInfo != null)
                    {
                        FileManager.LoadFileESL(file, fileInfo);
                        file.Close();
                        Globals.FilePathESL = openFileDialogESL.FileName;
                        openFileDialogESL.FileName = null;
                        TreeViewUpdateSelecteds();
                        glControl.Invalidate();
                    }
 
                }
            }

        }

        private void openFileDialogETS_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo fileInfo = null;
            try
            {
                fileInfo = new FileInfo(openFileDialogETS.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }
            if (fileInfo != null)
            {
                if (fileInfo.Length > 0x1000000)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile16MB), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else if (fileInfo.Length == 0)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile0MB), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else if (fileInfo.Length < 16)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile16Bytes), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else
                {
                    FileStream file = null;
                    try
                    {
                        file = fileInfo.OpenRead();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                        e.Cancel = true;
                        return;
                    }
                    if (file != null && fileInfo != null)
                    {
                        if (OpenIsUHD)
                        {
                            FileManager.LoadFileETS_UHD(file, fileInfo);
                        }
                        else
                        {
                            FileManager.LoadFileETS_Classic(file, fileInfo);
                        }
                        file.Close();
                        Globals.FilePathETS = openFileDialogETS.FileName;
                        openFileDialogETS.FileName = null;
                        TreeViewUpdateSelecteds();
                        glControl.Invalidate();
                    }
                }
            }
        }
        private void openFileDialogITA_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo fileInfo = null;
            try
            {
                fileInfo = new FileInfo(openFileDialogITA.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }
            if (fileInfo != null)
            {
                if (fileInfo.Length > 0x1000000)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile16MB), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else if (fileInfo.Length == 0)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile0MB), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else if (fileInfo.Length < 16)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile16Bytes), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else
                {
                    FileStream file = null;
                    try
                    {
                        file = fileInfo.OpenRead();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                        e.Cancel = true;
                        return;
                    }
                    if (file != null && fileInfo != null)
                    {
                        if (OpenIsUHD)
                        {
                            FileManager.LoadFileITA_UHD(file, fileInfo);
                        }
                        else
                        {
                            FileManager.LoadFileITA_Classic(file, fileInfo);
                        }
                        file.Close();
                        Globals.FilePathITA = openFileDialogITA.FileName;
                        openFileDialogITA.FileName = null;
                        TreeViewUpdateSelecteds();
                        glControl.Invalidate();
                    }
                }
            }
        }
        private void openFileDialogAEV_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo fileInfo = null;
            try
            {
                fileInfo = new FileInfo(openFileDialogAEV.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }
            if (fileInfo != null)
            {
                if (fileInfo.Length > 0x1000000)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile16MB), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else if (fileInfo.Length == 0)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile0MB), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else if (fileInfo.Length < 16)
                {
                    MessageBox.Show(Lang.GetText(eLang.MessageBoxFile16Bytes), Lang.GetText(eLang.MessageBoxWarningTitle), MessageBoxButtons.OK);
                    e.Cancel = true;
                    return;
                }
                else
                {
                    FileStream file = null;
                    try
                    {
                        file = fileInfo.OpenRead();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                        e.Cancel = true;
                        return;
                    }
                    if (file != null && fileInfo != null)
                    {
                        if (OpenIsUHD)
                        {
                            FileManager.LoadFileAEV_UHD(file, fileInfo);
                        }
                        else
                        {
                            FileManager.LoadFileAEV_Classic(file, fileInfo);
                        }
                        file.Close();
                        Globals.FilePathAEV = openFileDialogAEV.FileName;
                        openFileDialogAEV.FileName = null;
                        TreeViewUpdateSelecteds();
                        glControl.Invalidate();
                    }
                }
            }
        }
        #endregion

        #region Gerenciamento de arquivos //Clear

        private void toolStripMenuItemClear_DropDownOpening(object sender, EventArgs e)
        {
            toolStripMenuItemClearESL.Enabled = DataBase.FileESL != null;
            toolStripMenuItemClearETS.Enabled = DataBase.FileETS != null;
            toolStripMenuItemClearITA.Enabled = DataBase.FileITA != null;
            toolStripMenuItemClearAEV.Enabled = DataBase.FileAEV != null;
        }

        private void toolStripMenuItemClearESL_Click(object sender, EventArgs e)
        {
            FileManager.ClearESL();
            Globals.FilePathESL = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }

        private void toolStripMenuItemClearETS_Click(object sender, EventArgs e)
        {
            FileManager.ClearETS();
            Globals.FilePathETS = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }

        private void toolStripMenuItemClearITA_Click(object sender, EventArgs e)
        {
            FileManager.ClearITA();
            Globals.FilePathITA = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }

        private void toolStripMenuItemClearAEV_Click(object sender, EventArgs e)
        {
            FileManager.ClearAEV();
            Globals.FilePathAEV = null;
            TreeViewUpdateSelecteds();
            glControl.Invalidate();
        }



        #endregion

        #region Gerenciamento de arquivos //Save As..

        private void toolStripMenuItemSaveAs_DropDownOpening(object sender, EventArgs e)
        {
            toolStripMenuItemSaveAsESL.Enabled = DataBase.FileESL != null;
            toolStripMenuItemSaveAsETS.Enabled = DataBase.FileETS != null;
            toolStripMenuItemSaveAsITA.Enabled = DataBase.FileITA != null;
            toolStripMenuItemSaveAsAEV.Enabled = DataBase.FileAEV != null;

            if (DataBase.FileETS != null && DataBase.FileETS.GetRe4Version == Re4Version.Classic)
            {
                toolStripMenuItemSaveAsETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsETS_Classic);
            }
            else if (DataBase.FileETS != null && DataBase.FileETS.GetRe4Version == Re4Version.UHD)
            {
                toolStripMenuItemSaveAsETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsETS_UHD);
            }
            else 
            {
                toolStripMenuItemSaveAsETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsETS);
            }

            if (DataBase.FileITA != null && DataBase.FileITA.GetRe4Version == Re4Version.Classic)
            {
                toolStripMenuItemSaveAsITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsITA_Classic);
            }
            else if (DataBase.FileITA != null && DataBase.FileITA.GetRe4Version == Re4Version.UHD)
            {
                toolStripMenuItemSaveAsITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsITA_UHD);
            }
            else
            {
                toolStripMenuItemSaveAsITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsITA);
            }

            if (DataBase.FileAEV != null && DataBase.FileAEV.GetRe4Version == Re4Version.Classic)
            {
                toolStripMenuItemSaveAsAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsAEV_Classic);
            }
            else if (DataBase.FileAEV != null && DataBase.FileAEV.GetRe4Version == Re4Version.UHD)
            {
                toolStripMenuItemSaveAsAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsAEV_UHD);
            }
            else
            {
                toolStripMenuItemSaveAsAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsAEV);
            }
        }

        private void toolStripMenuItemSaveAsESL_Click(object sender, EventArgs e)
        {
            saveFileDialogESL.FileName = Globals.FilePathESL;
            saveFileDialogESL.ShowDialog();
        }

        private void toolStripMenuItemSaveAsETS_Click(object sender, EventArgs e)
        {
            saveFileDialogETS.FileName = Globals.FilePathETS;
            saveFileDialogETS.ShowDialog();
        }

        private void toolStripMenuItemSaveAsITA_Click(object sender, EventArgs e)
        {
            saveFileDialogITA.FileName = Globals.FilePathITA;
            saveFileDialogITA.ShowDialog();
        }

        private void toolStripMenuItemSaveAsAEV_Click(object sender, EventArgs e)
        {
            saveFileDialogAEV.FileName = Globals.FilePathAEV;
            saveFileDialogAEV.ShowDialog();
        }

        private void saveFileDialogESL_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(saveFileDialogESL.FileName);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveFileESL(stream);
                stream.Close();
                Globals.FilePathESL = saveFileDialogESL.FileName;
                saveFileDialogESL.FileName = null;
            }
            
        }

        private void saveFileDialogETS_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(saveFileDialogETS.FileName);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveFileETS(stream);
                stream.Close();
                Globals.FilePathETS = saveFileDialogETS.FileName;
                saveFileDialogETS.FileName = null;
            }
        }

        private void saveFileDialogITA_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(saveFileDialogITA.FileName);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveFileITA(stream);
                stream.Close();
                Globals.FilePathITA = saveFileDialogITA.FileName;
                saveFileDialogITA.FileName = null;
            }
        }

        private void saveFileDialogAEV_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(saveFileDialogAEV.FileName);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveFileAEV(stream);
                stream.Close();
                Globals.FilePathAEV = saveFileDialogAEV.FileName;
                saveFileDialogAEV.FileName = null;
            }
        }

        #endregion

        #region Gerenciamento de arquivos //Save

        private void toolStripMenuItemSave_DropDownOpening(object sender, EventArgs e)
        {
            toolStripMenuItemSaveESL.Enabled = DataBase.FileESL != null;
            toolStripMenuItemSaveETS.Enabled = DataBase.FileETS != null;
            toolStripMenuItemSaveITA.Enabled = DataBase.FileITA != null;
            toolStripMenuItemSaveAEV.Enabled = DataBase.FileAEV != null;

            if (DataBase.FileETS != null && DataBase.FileETS.GetRe4Version == Re4Version.Classic)
            {
                toolStripMenuItemSaveETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveETS_Classic);
            }
            else if (DataBase.FileETS != null && DataBase.FileETS.GetRe4Version == Re4Version.UHD)
            {
                toolStripMenuItemSaveETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveETS_UHD);
            }
            else
            {
                toolStripMenuItemSaveETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveETS);
            }

            if (DataBase.FileITA != null && DataBase.FileITA.GetRe4Version == Re4Version.Classic)
            {
                toolStripMenuItemSaveITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveITA_Classic);
            }
            else if (DataBase.FileITA != null && DataBase.FileITA.GetRe4Version == Re4Version.UHD)
            {
                toolStripMenuItemSaveITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveITA_UHD);
            }
            else
            {
                toolStripMenuItemSaveITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveITA);
            }

            if (DataBase.FileAEV != null && DataBase.FileAEV.GetRe4Version == Re4Version.Classic)
            {
                toolStripMenuItemSaveAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveAEV_Classic);
            }
            else if (DataBase.FileAEV != null && DataBase.FileAEV.GetRe4Version == Re4Version.UHD)
            {
                toolStripMenuItemSaveAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveAEV_UHD);
            }
            else
            {
                toolStripMenuItemSaveAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveAEV);
            }

        }

        private void toolStripMenuItemSaveESL_Click(object sender, EventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(Globals.FilePathESL);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                saveFileDialogESL.FileName = Globals.FilePathESL;
                saveFileDialogESL.ShowDialog();
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveFileESL(stream);
                stream.Close();
            }
        }

        private void toolStripMenuItemSaveETS_Click(object sender, EventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(Globals.FilePathETS);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                saveFileDialogETS.FileName = Globals.FilePathETS;
                saveFileDialogETS.ShowDialog();
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveFileETS(stream);
                stream.Close();
            }
        }

        private void toolStripMenuItemSaveITA_Click(object sender, EventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(Globals.FilePathITA);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                saveFileDialogITA.FileName = Globals.FilePathITA;
                saveFileDialogITA.ShowDialog();
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveFileITA(stream);
                stream.Close();
            }
        }

        private void toolStripMenuItemSaveAEV_Click(object sender, EventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(Globals.FilePathAEV);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                saveFileDialogAEV.FileName = Globals.FilePathAEV;
                saveFileDialogAEV.ShowDialog();
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveFileAEV(stream);
                stream.Close();
            }
        }

        private void toolStripMenuItemSaveDirectories_DropDownOpening(object sender, EventArgs e)
        {
            toolStripMenuItemDiretory_ESL.Text = Lang.GetText(eLang.DiretoryESL) + " " + Globals.FilePathESL;
            toolStripMenuItemDiretory_ETS.Text = Lang.GetText(eLang.DiretoryETS) + " " + Globals.FilePathETS;
            toolStripMenuItemDiretory_ITA.Text = Lang.GetText(eLang.DiretoryITA) + " " + Globals.FilePathITA;
            toolStripMenuItemDiretory_AEV.Text = Lang.GetText(eLang.DiretoryAEV) + " " + Globals.FilePathAEV;
        }

        #endregion

        #region Gerenciamento de arquivos //Save Convert

        private void toolStripMenuItemSaveConverter_DropDownOpening(object sender, EventArgs e)
        {
            toolStripMenuItemSaveConverterETS.Enabled = DataBase.FileETS != null;
            toolStripMenuItemSaveConverterITA.Enabled = DataBase.FileITA != null;
            toolStripMenuItemSaveConverterAEV.Enabled = DataBase.FileAEV != null;

            if (DataBase.FileETS != null && DataBase.FileETS.GetRe4Version == Re4Version.Classic)
            {
                toolStripMenuItemSaveConverterETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterETS_UHD);
            }
            else if (DataBase.FileETS != null && DataBase.FileETS.GetRe4Version == Re4Version.UHD)
            {
                toolStripMenuItemSaveConverterETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterETS_Classic);
            }
            else
            {
                toolStripMenuItemSaveConverterETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterETS);
            }

            if (DataBase.FileITA != null && DataBase.FileITA.GetRe4Version == Re4Version.Classic)
            {
                toolStripMenuItemSaveConverterITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterITA_UHD);
            }
            else if (DataBase.FileITA != null && DataBase.FileITA.GetRe4Version == Re4Version.UHD)
            {
                toolStripMenuItemSaveConverterITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterITA_Classic);
            }
            else
            {
                toolStripMenuItemSaveConverterITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterITA);
            }

            if (DataBase.FileAEV != null && DataBase.FileAEV.GetRe4Version == Re4Version.Classic)
            {
                toolStripMenuItemSaveConverterAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterAEV_UHD);
            }
            else if (DataBase.FileAEV != null && DataBase.FileAEV.GetRe4Version == Re4Version.UHD)
            {
                toolStripMenuItemSaveConverterAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterAEV_Classic);
            }
            else
            {
                toolStripMenuItemSaveConverterAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterAEV);
            }
        }

        private void toolStripMenuItemSaveConverterETS_Click(object sender, EventArgs e)
        {
            saveFileDialogConvertETS.ShowDialog();
        }

        private void toolStripMenuItemSaveConverterITA_Click(object sender, EventArgs e)
        {
            saveFileDialogConvertITA.ShowDialog();
        }

        private void toolStripMenuItemSaveConverterAEV_Click(object sender, EventArgs e)
        {
            saveFileDialogConvertAEV.ShowDialog();
        }

        private void saveFileDialogConvertETS_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(saveFileDialogConvertETS.FileName);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveConvertFileETS(stream);
                stream.Close();
            }
        }

        private void saveFileDialogConvertITA_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(saveFileDialogConvertITA.FileName);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveConvertFileITA(stream);
                stream.Close();
            }
        }

        private void saveFileDialogConvertAEV_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo file = null;
            FileStream stream = null;
            try
            {
                file = new FileInfo(saveFileDialogConvertAEV.FileName);
                stream = file.Create();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Lang.GetText(eLang.MessageBoxErrorTitle), MessageBoxButtons.OK);
                e.Cancel = true;
                return;
            }

            if (file != null && stream != null)
            {
                FileManager.SaveConvertFileAEV(stream);
                stream.Close();
            }
        }


        #endregion




        #region MainForm events/ metodos


        private void StartUpdateTranslation()
        {
            // menu principal
            toolStripMenuItemFile.Text = Lang.GetText(eLang.toolStripMenuItemFile);
            toolStripMenuItemEdit.Text = Lang.GetText(eLang.toolStripMenuItemEdit);
            toolStripMenuItemView.Text = Lang.GetText(eLang.toolStripMenuItemView);
            toolStripMenuItemMisc.Text = Lang.GetText(eLang.toolStripMenuItemMisc);
            toolStripMenuItemSelectRoom.Text = Lang.GetText(eLang.SelectRoom);
            //submenu File
            toolStripMenuItemNewFile.Text = Lang.GetText(eLang.toolStripMenuItemNewFile);
            toolStripMenuItemOpen.Text = Lang.GetText(eLang.toolStripMenuItemOpen);
            toolStripMenuItemSave.Text = Lang.GetText(eLang.toolStripMenuItemSave);
            toolStripMenuItemSaveAs.Text = Lang.GetText(eLang.toolStripMenuItemSaveAs);
            toolStripMenuItemSaveConverter.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverter);
            toolStripMenuItemClear.Text = Lang.GetText(eLang.toolStripMenuItemClear);
            toolStripMenuItemClose.Text = Lang.GetText(eLang.toolStripMenuItemClose);
            // subsubmenu New
            toolStripMenuItemNewESL.Text = Lang.GetText(eLang.toolStripMenuItemNewESL);
            toolStripMenuItemNewETS_Classic.Text = Lang.GetText(eLang.toolStripMenuItemNewETS_Classic);
            toolStripMenuItemNewITA_Classic.Text = Lang.GetText(eLang.toolStripMenuItemNewITA_Classic);
            toolStripMenuItemNewAEV_Classic.Text = Lang.GetText(eLang.toolStripMenuItemNewAEV_Classic);
            toolStripMenuItemNewETS_UHD.Text = Lang.GetText(eLang.toolStripMenuItemNewETS_UHD);
            toolStripMenuItemNewITA_UHD.Text = Lang.GetText(eLang.toolStripMenuItemNewITA_UHD);
            toolStripMenuItemNewAEV_UHD.Text = Lang.GetText(eLang.toolStripMenuItemNewAEV_UHD);
            // subsubmenu Open
            toolStripMenuItemOpenESL.Text = Lang.GetText(eLang.toolStripMenuItemOpenESL);
            toolStripMenuItemOpenETS_Classic.Text = Lang.GetText(eLang.toolStripMenuItemOpenETS_Classic);
            toolStripMenuItemOpenITA_Classic.Text = Lang.GetText(eLang.toolStripMenuItemOpenITA_Classic);
            toolStripMenuItemOpenAEV_Classic.Text = Lang.GetText(eLang.toolStripMenuItemOpenAEV_Classic);
            toolStripMenuItemOpenETS_UHD.Text = Lang.GetText(eLang.toolStripMenuItemOpenETS_UHD);
            toolStripMenuItemOpenITA_UHD.Text = Lang.GetText(eLang.toolStripMenuItemOpenITA_UHD);
            toolStripMenuItemOpenAEV_UHD.Text = Lang.GetText(eLang.toolStripMenuItemOpenAEV_UHD);
            // subsubmenu Save
            toolStripMenuItemSaveESL.Text = Lang.GetText(eLang.toolStripMenuItemSaveESL);
            toolStripMenuItemSaveETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveETS);
            toolStripMenuItemSaveITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveITA);
            toolStripMenuItemSaveAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveAEV);
            toolStripMenuItemSaveDirectories.Text = Lang.GetText(eLang.toolStripMenuItemSaveDirectories);
            // subsubmenu Save As...
            toolStripMenuItemSaveAsESL.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsESL);
            toolStripMenuItemSaveAsETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsETS);
            toolStripMenuItemSaveAsITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsITA);
            toolStripMenuItemSaveAsAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveAsAEV);
            // subsubmenu Save As (Convert)
            toolStripMenuItemSaveConverterETS.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterETS);
            toolStripMenuItemSaveConverterITA.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterITA);
            toolStripMenuItemSaveConverterAEV.Text = Lang.GetText(eLang.toolStripMenuItemSaveConverterAEV);
            // subsubmenu Clear
            toolStripMenuItemClearESL.Text = Lang.GetText(eLang.toolStripMenuItemClearESL);
            toolStripMenuItemClearETS.Text = Lang.GetText(eLang.toolStripMenuItemClearETS);
            toolStripMenuItemClearITA.Text = Lang.GetText(eLang.toolStripMenuItemClearITA);
            toolStripMenuItemClearAEV.Text = Lang.GetText(eLang.toolStripMenuItemClearAEV);

            // sub menu edit
            toolStripMenuItemAddNewObj.Text = Lang.GetText(eLang.toolStripMenuItemAddNewObj);
            toolStripMenuItemDeleteSelectedObj.Text = Lang.GetText(eLang.toolStripMenuItemDeleteSelectedObj);
            toolStripMenuItemMoveUp.Text = Lang.GetText(eLang.toolStripMenuItemMoveUp);
            toolStripMenuItemMoveDown.Text = Lang.GetText(eLang.toolStripMenuItemMoveDown);
            toolStripMenuItemSearch.Text = Lang.GetText(eLang.toolStripMenuItemSearch);

            // sub menu Misc
            toolStripMenuItemOptions.Text = Lang.GetText(eLang.toolStripMenuItemOptions);
            toolStripMenuItemCredits.Text = Lang.GetText(eLang.toolStripMenuItemCredits);

            // sub menu View
            toolStripMenuItemHideRoomModel.Text = Lang.GetText(eLang.toolStripMenuItemHideRoomModel);
            toolStripMenuItemHideEnemyESL.Text = Lang.GetText(eLang.toolStripMenuItemHideEnemyESL);
            toolStripMenuItemHideEtcmodelETS.Text = Lang.GetText(eLang.toolStripMenuItemHideEtcmodelETS);
            toolStripMenuItemHideItemsITA.Text = Lang.GetText(eLang.toolStripMenuItemHideItemsITA);
            toolStripMenuItemHideEventsAEV.Text = Lang.GetText(eLang.toolStripMenuItemHideEventsAEV);
            toolStripMenuItemSubMenuEnemy.Text = Lang.GetText(eLang.toolStripMenuItemSubMenuEnemy);
            toolStripMenuItemSubMenuItem.Text = Lang.GetText(eLang.toolStripMenuItemSubMenuItem);
            toolStripMenuItemSubMenuSpecial.Text = Lang.GetText(eLang.toolStripMenuItemSubMenuSpecial);
            toolStripMenuItemSubMenuEtcModel.Text = Lang.GetText(eLang.toolStripMenuItemSubMenuEtcModel);
            toolStripMenuItemNodeDisplayNameInHex.Text = Lang.GetText(eLang.toolStripMenuItemNodeDisplayNameInHex);
            toolStripMenuItemResetCamera.Text = Lang.GetText(eLang.toolStripMenuItemResetCamera);
            toolStripMenuItemRefresh.Text = Lang.GetText(eLang.toolStripMenuItemRefresh);

            // sub menus de view
            toolStripMenuItemHideDesabledEnemy.Text = Lang.GetText(eLang.toolStripMenuItemHideDesabledEnemy);
            toolStripMenuItemShowOnlyDefinedRoom.Text = Lang.GetText(eLang.toolStripMenuItemShowOnlyDefinedRoom);
            toolStripMenuItemAutoDefineRoom.Text = Lang.GetText(eLang.toolStripMenuItemAutoDefineRoom);
            toolStripMenuItemItemPositionAtAssociatedObjectLocation.Text = Lang.GetText(eLang.toolStripMenuItemItemPositionAtAssociatedObjectLocation);
            toolStripMenuItemHideItemTriggerZone.Text = Lang.GetText(eLang.toolStripMenuItemHideItemTriggerZone);
            toolStripMenuItemHideItemTriggerRadius.Text = Lang.GetText(eLang.toolStripMenuItemHideItemTriggerRadius);
            toolStripMenuItemHideSpecialTriggerZone.Text = Lang.GetText(eLang.toolStripMenuItemHideSpecialTriggerZone);
            toolStripMenuItemHideExtraObjs.Text = Lang.GetText(eLang.toolStripMenuItemHideExtraObjs);
            toolStripMenuItemHideOnlyWarpDoor.Text = Lang.GetText(eLang.toolStripMenuItemHideOnlyWarpDoor);
            toolStripMenuItemHideExtraExceptWarpDoor.Text = Lang.GetText(eLang.toolStripMenuItemHideExtraExceptWarpDoor);
            toolStripMenuItemUseMoreSpecialColors.Text = Lang.GetText(eLang.toolStripMenuItemUseMoreSpecialColors);
            toolStripMenuItemEtcModelUseScale.Text = Lang.GetText(eLang.toolStripMenuItemEtcModelUseScale);

            //save and open windows
            openFileDialogAEV.Title = Lang.GetText(eLang.openFileDialogAEV);
            openFileDialogESL.Title = Lang.GetText(eLang.openFileDialogESL);
            openFileDialogETS.Title = Lang.GetText(eLang.openFileDialogETS);
            openFileDialogITA.Title = Lang.GetText(eLang.openFileDialogITA);
            saveFileDialogAEV.Title = Lang.GetText(eLang.saveFileDialogAEV);
            saveFileDialogConvertAEV.Title = Lang.GetText(eLang.saveFileDialogConvertAEV);
            saveFileDialogConvertETS.Title = Lang.GetText(eLang.saveFileDialogConvertETS);
            saveFileDialogConvertITA.Title = Lang.GetText(eLang.saveFileDialogConvertITA);
            saveFileDialogESL.Title = Lang.GetText(eLang.saveFileDialogESL);
            saveFileDialogETS.Title = Lang.GetText(eLang.saveFileDialogETS);
            saveFileDialogITA.Title = Lang.GetText(eLang.saveFileDialogITA);

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (MessageBox.Show(Lang.GetText(eLang.MessageBoxFormClosingDialog), Lang.GetText(eLang.MessageBoxFormClosingTitle), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
        }


        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // entrada de teclas para açoes especiais
            cameraMove.isControlDown = e.Control;

            #region usado em propery
            // proibe a estrada de caracteres que não vão nos campos de numeros
            if (InPropertyGrid && propertyGridObjs.SelectedGridItem != null && propertyGridObjs.SelectedGridItem.PropertyDescriptor != null)
            {

                if (propertyGridObjs.SelectedGridItem.PropertyDescriptor.Attributes.Contains(new DecNumberAttribute()))
                {

                    e.SuppressKeyPress = true;
                    if (KeysCheck.KeyIsNum(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (e.Control)
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (e.Alt || e.Shift || e.KeyCode == Keys.Alt)
                    {
                        e.SuppressKeyPress = true;
                    }
                    if (KeysCheck.KeyIsEssential(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }

                }

                if (propertyGridObjs.SelectedGridItem.PropertyDescriptor.Attributes.Contains(new DecNegativeNumberAttribute()))
                {

                    e.SuppressKeyPress = true;
                    if (KeysCheck.KeyIsNum(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (KeysCheck.KeyIsMinus(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (e.Control)
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (e.Alt || e.Shift || e.KeyCode == Keys.Alt)
                    {
                        e.SuppressKeyPress = true;
                    }
                    if (KeysCheck.KeyIsEssential(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }

                }

                if (propertyGridObjs.SelectedGridItem.PropertyDescriptor.Attributes.Contains(new HexNumberAttribute()))
                {

                    e.SuppressKeyPress = true;
                    if (KeysCheck.KeyIsNum(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (e.Shift)
                    {
                        e.SuppressKeyPress = true;
                    }
                    if (KeysCheck.KeyIsHex(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (e.Control)
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (e.Alt || e.KeyCode == Keys.Alt)
                    {
                        e.SuppressKeyPress = true;
                    }
                    if (KeysCheck.KeyIsEssential(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }

                }

                if (propertyGridObjs.SelectedGridItem.PropertyDescriptor.Attributes.Contains(new FloatNumberAttribute()))
                {

                    e.SuppressKeyPress = true;
                    if (KeysCheck.KeyIsNum(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (KeysCheck.KeyIsMinus(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (KeysCheck.KeyIsCommaDot(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (KeysCheck.KeyIsOnlyDot(e.KeyValue))
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (e.Control)
                    {
                        e.SuppressKeyPress = false;
                    }
                    if (e.Alt || e.Shift || e.KeyCode == Keys.Alt)
                    {
                        e.SuppressKeyPress = true;
                    }
                    if (KeysCheck.KeyIsEssential(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }
                }

                if (propertyGridObjs.SelectedGridItem.PropertyDescriptor.Attributes.Contains(new NoKeyAttribute()))
                {
                    e.SuppressKeyPress = true;
                    if (KeysCheck.KeyIsEssentialNoKey(e.KeyCode))
                    {
                        e.SuppressKeyPress = false;
                    }
                }
            }

            #endregion
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            cameraMove.isControlDown = e.Control;
        }

        #endregion

    }
}
