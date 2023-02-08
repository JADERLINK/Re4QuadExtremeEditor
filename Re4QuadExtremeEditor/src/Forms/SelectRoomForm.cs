using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Re4QuadExtremeEditor.src.JSON;
using Re4QuadExtremeEditor.src.Class;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src.Forms
{
    public partial class SelectRoomForm : Form
    {
        /// <summary>
        /// evendo que acontece depois de clicar em load;
        /// </summary>
        public event EventHandler onLoadButtonClick;

        public SelectRoomForm()
        {
            InitializeComponent();

            KeyPreview = true;
            comboBoxRoomList.Items.Add(Lang.GetText(eLang.NoneRoom));
            comboBoxRoomList.SelectedIndex = 0;
            comboBoxRoomList.Items.AddRange(DataBase.RoomList.ToArray());
            if (DataBase.SelectedRoom != null)
            {
                if (comboBoxRoomList.Items.Contains(DataBase.SelectedRoom.GetRoomInfo))
                {
                    comboBoxRoomList.SelectedIndex = comboBoxRoomList.Items.IndexOf(DataBase.SelectedRoom.GetRoomInfo);
                }
            }

            if (Lang.LoadedTranslation)
            {
                StartUpdateTranslation();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            buttonLoad.Enabled = false;
            buttonCancel.Enabled = false;

            // remove a antiga
            if (DataBase.SelectedRoom != null)
            {
                DataBase.SelectedRoom.ClearGL();
                DataBase.SelectedRoom = null;
                GC.Collect();
            }
            // cria uma nova
            if (comboBoxRoomList.SelectedItem is RoomInfo)
            {
                DataBase.SelectedRoom = new Room((RoomInfo)comboBoxRoomList.SelectedItem);
                GC.Collect();
            }
            onLoadButtonClick?.Invoke(comboBoxRoomList.SelectedItem, new EventArgs());
            Close();
        }

        private void SelectRoomForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void StartUpdateTranslation() 
        {
            this.Text = Lang.GetText(eLang.SelectRoomForm);
            labelInfo.Text = Lang.GetText(eLang.labelInfo);
            buttonLoad.Text = Lang.GetText(eLang.buttonLoad);
            buttonCancel.Text = Lang.GetText(eLang.buttonCancel2);
        }

    }
}
