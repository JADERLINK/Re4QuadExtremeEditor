using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//https://stackoverflow.com/questions/4580263/how-to-open-in-default-browser-in-c-sharp


namespace Re4QuadExtremeEditor.src.Forms
{
    public partial class CreditsForm : Form
    {
        public CreditsForm()
        {
            InitializeComponent();
            KeyPreview = true;
        }

        private void CreditsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void To(string url)
        {
            try { System.Diagnostics.Process.Start("explorer.exe", url); } catch (Exception) { }
        }

        private void linkLabelProjectGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://github.com/JADERLINK/Re4QuadExtremeEditor");
        }

        private void linkLabelJaderLinkBlog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://jaderlink.blogspot.com/");
        }

        private void linkLabelJaderLinkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://github.com/JADERLINK");
        }

        private void linkLabelLordVincBlog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://lordvinc.blogspot.com/");
        }

        private void linkLabelLordVincGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://github.com/Lordvinc");
        }

        private void linkLabelGuad64Project_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://github.com/DavidSM64/Quad64");
        }

        private void linkLabelLicenceQuad64_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://github.com/DavidSM64/Quad64/blob/master/LICENSE");
        }

        private void linkLabelTgaGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://github.com/ALEXGREENALEX/TGASharpLib");
        }

        private void linkLabelTgaGitLab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://gitlab.com/Alex_Green/TGASharpLib");
        }

        private void linkLabelLicenseTGA_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://github.com/ALEXGREENALEX/TGASharpLib/blob/master/LICENSE");
        }

        private void linkLabelSiteJsonNET_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://www.newtonsoft.com/json");
        }

        private void linkLabelLicenseJsonNET_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md");
        }

        private void linkLabelSiteOpenTK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://opentk.net/");
        }

        private void linkLabelNugetOpenTK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://www.nuget.org/packages/OpenTK/");
        }

        private void linkLabelNugetGLControl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://www.nuget.org/packages/OpenTK.GLControl/");
        }

        private void linkLabelLicenseOpenTK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://github.com/opentk/opentk/blob/master/LICENSE.md");
        }

        private void linkLabelLicenseCodeProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://www.codeproject.com/info/cpol10.aspx");
        }

        private void linkLabelGridComboBox_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://www.codeproject.com/Articles/23242/Property-Grid-Dynamic-List-ComboBox-Validation-and");
        }

        private void linkLabelMultiselectTreeView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://www.codeproject.com/Articles/20581/Multiselect-Treeview-Implementation");
        }

        private void linkLabelDynamicProperties_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://www.codeproject.com/Articles/189521/Dynamic-Properties-for-PropertyGrid");
        }

        private void linkLabelCustomizedDisplay_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://www.codeproject.com/Articles/4448/Customized-Display-of-Collection-Data-in-a-Propert");
        }

        private void linkLabelYoutubeJaderLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://www.youtube.com/@JADERLINK");
        }

        private void linkLabelYoutubeLordvinc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://www.youtube.com/@Lordvinc");
        }

        private void linkLabelSonOfPercia_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://residentevilmodding.boards.net/thread/9780/2018-re4uhd-toolset-persia-released");
        }

        private void linkLabelMrCurious_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            To("https://drive.google.com/drive/folders/1BKribmvU37thKtI44uZivUsoYqFnyW09");
        }

        private void buttonCLOSE_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
