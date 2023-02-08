using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Re4QuadExtremeEditor.src.Class.Enums;

namespace Re4QuadExtremeEditor.src.Forms
{
    public partial class OptionsForm : Form
    {
        public event EventHandler onLoadButtonClick;
        public event Class.CustomDelegates.ActivateMethod OnOKButtonClick;

        private int FrationalAmount = 9;

        private bool EnableRadioButtons = false;

        public OptionsForm()
        {
            InitializeComponent();
            KeyPreview = true;
            Size = MinimumSize;

            comboBoxLanguage.Items.Add(Lang.GetText(eLang.OptionsUseInternalLanguage));

            comboBoxLanguage.Items.AddRange(Globals.Langs.ToArray());
            int langIndex = comboBoxLanguage.Items.IndexOf(new JSON.LangObjForList(Globals.BackupConfigs.LangID, "", ""));
            if (langIndex == -1 || Globals.BackupConfigs.LoadLangTranslation == false)
            {
                langIndex = 0;
            }
            comboBoxLanguage.SelectedIndex = langIndex;

            comboBoxItemRotationOrder.Items.AddRange(Utils.ItemRotationOrderForListBox());

            textBoxXfileDiretory.Text = Globals.xfileDiretory;
            textBoxXscrDiretory.Text = Globals.xscrDiretory;
            panelSkyColor.BackColor = Globals.SkyColor;

            ConfigFrationalSymbol frationalSymbol = Globals.FrationalSymbol;
            switch (frationalSymbol)
            {
                case ConfigFrationalSymbol.AcceptsCommaAndPeriod_OutputComma:
                    radioButtonAcceptsCommaAndPeriod.Checked = true;
                    radioButtonOutputComma.Checked = true;
                    break;
                case ConfigFrationalSymbol.OnlyAcceptComma:
                    radioButtonOnlyAcceptComma.Checked = true;
                    radioButtonOutputComma.Checked = true;
                    groupBoxOutputFractionalSymbol.Enabled = false;
                    break;
                case ConfigFrationalSymbol.OnlyAcceptPeriod:
                    radioButtonOnlyAcceptPeriod.Checked = true;
                    radioButtonOutputPeriod.Checked = true;
                    groupBoxOutputFractionalSymbol.Enabled = false;
                    break;
             case ConfigFrationalSymbol.AcceptsCommaAndPeriod_OutputPeriod:
                default:
                    radioButtonAcceptsCommaAndPeriod.Checked = true;
                    radioButtonOutputPeriod.Checked = true;
                    break;
            }

            FrationalAmount = Globals.FrationalAmount;
            labelFrationalAmount.Text = FrationalAmount.ToString();

            checkBoxDisableItemRotations.Checked = Globals.ItemDisableRotationAll;
            checkBoxIgnoreRotationForZeroXYZ.Checked = Globals.ItemDisableRotationIfXorYorZequalZero;
            checkBoxIgnoreRotationForZisNotGreaterThanZero.Checked = Globals.ItemDisableRotationIfZisNotGreaterThanZero;
            numericUpDownDivider.Value = (decimal)Globals.ItemRotationCalculationDivider;
            numericUpDownMultiplier.Value = (decimal)Globals.ItemRotationCalculationMultiplier;
            comboBoxItemRotationOrder.SelectedIndex = (int)Globals.ItemRotationOrder;

            EnableRadioButtons = true;

            if (Lang.LoadedTranslation)
            {
                StartUpdateTranslation();
            }
        }

        private void OptionsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBoxXfileDiretory.Text) && Directory.Exists(textBoxXscrDiretory.Text))
            {
                if (textBoxXscrDiretory.Text[textBoxXscrDiretory.Text.Length - 1] != '\\')
                {
                    textBoxXscrDiretory.Text += "\\";
                }
                if (textBoxXfileDiretory.Text[textBoxXfileDiretory.Text.Length - 1] != '\\')
                {
                    textBoxXfileDiretory.Text += "\\";
                }


                bool ForceReload = checkBoxForceReloadModels.Checked;
                bool DiretoryChanged = false;

                if (textBoxXfileDiretory.Text != Globals.BackupConfigs.xfileDiretory || textBoxXscrDiretory.Text != Globals.BackupConfigs.xscrDiretory)
                {
                    DiretoryChanged = true;
                }

                Globals.SkyColor = panelSkyColor.BackColor;
                Globals.xfileDiretory = textBoxXfileDiretory.Text;
                Globals.xscrDiretory = textBoxXscrDiretory.Text;

                if (radioButtonAcceptsCommaAndPeriod.Checked && radioButtonOutputComma.Checked)
                {
                    Globals.FrationalSymbol = ConfigFrationalSymbol.AcceptsCommaAndPeriod_OutputComma;
                }
                else if (radioButtonAcceptsCommaAndPeriod.Checked && radioButtonOutputPeriod.Checked)
                {
                    Globals.FrationalSymbol = ConfigFrationalSymbol.AcceptsCommaAndPeriod_OutputPeriod;
                }
                else if (radioButtonOnlyAcceptComma.Checked)
                {
                    Globals.FrationalSymbol = ConfigFrationalSymbol.OnlyAcceptComma;
                }
                else if (radioButtonOnlyAcceptPeriod.Checked)
                {
                    Globals.FrationalSymbol = ConfigFrationalSymbol.OnlyAcceptPeriod;
                }

                Globals.FrationalAmount = FrationalAmount;

                Globals.ItemDisableRotationAll = checkBoxDisableItemRotations.Checked;
                Globals.ItemDisableRotationIfXorYorZequalZero = checkBoxIgnoreRotationForZeroXYZ.Checked;
                Globals.ItemDisableRotationIfZisNotGreaterThanZero = checkBoxIgnoreRotationForZisNotGreaterThanZero.Checked;
                Globals.ItemRotationCalculationDivider = (float)numericUpDownDivider.Value;
                Globals.ItemRotationCalculationMultiplier = (float)numericUpDownMultiplier.Value;
                Globals.ItemRotationOrder = (ObjRotationOrder)comboBoxItemRotationOrder.SelectedIndex;


                Globals.BackupConfigs.SkyColor = Globals.SkyColor;
                Globals.BackupConfigs.xfileDiretory = Globals.xfileDiretory;
                Globals.BackupConfigs.xscrDiretory = Globals.xscrDiretory;
                Globals.BackupConfigs.FrationalAmount = Globals.FrationalAmount;
                Globals.BackupConfigs.FrationalSymbol = Globals.FrationalSymbol;
                Globals.BackupConfigs.ItemDisableRotationAll = Globals.ItemDisableRotationAll;
                Globals.BackupConfigs.ItemDisableRotationIfXorYorZequalZero = Globals.ItemDisableRotationIfXorYorZequalZero;
                Globals.BackupConfigs.ItemDisableRotationIfZisNotGreaterThanZero = Globals.ItemDisableRotationIfZisNotGreaterThanZero;
                Globals.BackupConfigs.ItemRotationCalculationDivider = Globals.ItemRotationCalculationDivider;
                Globals.BackupConfigs.ItemRotationCalculationMultiplier = Globals.ItemRotationCalculationMultiplier;
                Globals.BackupConfigs.ItemRotationOrder = Globals.ItemRotationOrder;

                if (comboBoxLanguage.SelectedIndex <= 0)
                {
                    Globals.BackupConfigs.LoadLangTranslation = false;
                    Globals.BackupConfigs.LangID = "";
                }
                else 
                {
                    var langSelected = (JSON.LangObjForList)comboBoxLanguage.SelectedItem;
                    Globals.BackupConfigs.LoadLangTranslation = true;
                    Globals.BackupConfigs.LangID = langSelected.LangID;
                }

                JSON.ConfigsFile.writeConfigsFile(Consts.ConfigsFileDiretory, Globals.BackupConfigs);


                // aviso de demora
                if (ForceReload || DiretoryChanged)
                {
                    MessageBox.Show(Lang.GetText(eLang.OptionsFormWarningLoadModelsMessageBoxDialog), Lang.GetText(eLang.OptionsFormWarningLoadModelsMessageBoxTitle));
                }

                tabControlConfigs.Enabled = false;
                checkBoxForceReloadModels.Enabled = false;
                buttonCancel.Enabled = false;
                buttonOK.Enabled = false;

                if (ForceReload)
                {
                    Utils.ReloadJsonFiles();
                }
                if (ForceReload || DiretoryChanged)
                {
                    Utils.ReloadModels();
                }

                onLoadButtonClick?.Invoke("", new EventArgs());
                OnOKButtonClick?.Invoke();
                Close();
            }
            else
            {
                MessageBox.Show(Lang.GetText(eLang.OptionsFormDiretoryMessageBoxDialog), Lang.GetText(eLang.OptionsFormDiretoryMessageBoxTitle));
            }           
        }

        private void buttonXfileDiretory_Click(object sender, EventArgs e)
        {
            folderBrowserDialogDiretory.Description = Lang.GetText(eLang.OptionsFormSelectDiretoryXFILE);
            folderBrowserDialogDiretory.SelectedPath = "";
            if (Directory.Exists(textBoxXfileDiretory.Text))
            {
                folderBrowserDialogDiretory.SelectedPath = textBoxXfileDiretory.Text;
            }
            if (folderBrowserDialogDiretory.ShowDialog() == DialogResult.OK)
            {
                textBoxXfileDiretory.Text = folderBrowserDialogDiretory.SelectedPath;
                if (textBoxXfileDiretory.Text[textBoxXfileDiretory.Text.Length -1] != '\\')
                {
                    textBoxXfileDiretory.Text += "\\";
                }
            }
        }

        private void buttonXscrDiretory_Click(object sender, EventArgs e)
        {
            folderBrowserDialogDiretory.Description = Lang.GetText(eLang.OptionsFormSelectDiretoryXSCR);
            folderBrowserDialogDiretory.SelectedPath = "";
            if (Directory.Exists(textBoxXscrDiretory.Text))
            {
                folderBrowserDialogDiretory.SelectedPath = textBoxXscrDiretory.Text;
            }
            if (folderBrowserDialogDiretory.ShowDialog() == DialogResult.OK)
            {
                textBoxXscrDiretory.Text = folderBrowserDialogDiretory.SelectedPath;
                if (textBoxXscrDiretory.Text[textBoxXscrDiretory.Text.Length - 1] != '\\')
                {
                    textBoxXscrDiretory.Text += "\\";
                }
            }
        }

        private void panelSkyColor_Click(object sender, EventArgs e)
        {
            colorDialogColors.Color = panelSkyColor.BackColor;
            if (colorDialogColors.ShowDialog() == DialogResult.OK)
            {
                panelSkyColor.BackColor = colorDialogColors.Color;
            }
        }

        private void radioButtonAcceptsCommaAndPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableRadioButtons && radioButtonAcceptsCommaAndPeriod.Checked)
            {
                groupBoxOutputFractionalSymbol.Enabled = true;
            }
        }

        private void radioButtonOnlyAcceptComma_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableRadioButtons && radioButtonOnlyAcceptComma.Checked)
            {
                groupBoxOutputFractionalSymbol.Enabled = false;
                radioButtonOutputPeriod.Checked = false;
                radioButtonOutputComma.Checked = true;
            }
        }

        private void radioButtonOnlyAcceptPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableRadioButtons && radioButtonOnlyAcceptPeriod.Checked)
            {
                groupBoxOutputFractionalSymbol.Enabled = false;
                radioButtonOutputComma.Checked = false;
                radioButtonOutputPeriod.Checked = true;
            }
        }

        private void buttonFrationalMinus_Click(object sender, EventArgs e)
        {
            if (FrationalAmount > 4)
            {
                FrationalAmount -= 1;
                labelFrationalAmount.Text = FrationalAmount.ToString();
            }
        }

        private void buttonFrationalPlus_Click(object sender, EventArgs e)
        {
            if (FrationalAmount < 9)
            {
                FrationalAmount += 1;
                labelFrationalAmount.Text = FrationalAmount.ToString();
            }
        }

        private void StartUpdateTranslation() 
        {
            this.Text = Lang.GetText(eLang.OptionsForm);
            buttonCancel.Text = Lang.GetText(eLang.Options_buttonCancel);
            buttonOK.Text = Lang.GetText(eLang.Options_buttonOK);
            checkBoxDisableItemRotations.Text = Lang.GetText(eLang.checkBoxDisableItemRotations);
            checkBoxForceReloadModels.Text = Lang.GetText(eLang.checkBoxForceReloadModels);
            checkBoxIgnoreRotationForZeroXYZ.Text = Lang.GetText(eLang.checkBoxIgnoreRotationForZeroXYZ);
            checkBoxIgnoreRotationForZisNotGreaterThanZero.Text = Lang.GetText(eLang.checkBoxIgnoreRotationForZisNotGreaterThanZero);
            groupBoxColors.Text = Lang.GetText(eLang.groupBoxColors);
            groupBoxDiretory.Text = Lang.GetText(eLang.groupBoxDiretory);
            groupBoxFloatStyle.Text = Lang.GetText(eLang.groupBoxFloatStyle);
            groupBoxFractionalPart.Text = Lang.GetText(eLang.groupBoxFractionalPart);
            groupBoxInputFractionalSymbol.Text = Lang.GetText(eLang.groupBoxInputFractionalSymbol);
            groupBoxItemRotations.Text = Lang.GetText(eLang.groupBoxItemRotations);
            groupBoxLanguage.Text = Lang.GetText(eLang.groupBoxLanguage);
            groupBoxOutputFractionalSymbol.Text = Lang.GetText(eLang.groupBoxOutputFractionalSymbol);
            labelDivider.Text = Lang.GetText(eLang.labelDivider);
            labelItemExtraCalculation.Text = Lang.GetText(eLang.labelItemExtraCalculation);
            labelitemRotationOrderText.Text = Lang.GetText(eLang.labelitemRotationOrderText);
            labelLanguageWarning.Text = Lang.GetText(eLang.labelLanguageWarning);
            labelMultiplier.Text = Lang.GetText(eLang.labelMultiplier);
            labelSkyColor.Text = Lang.GetText(eLang.labelSkyColor);
            labelxfile.Text = Lang.GetText(eLang.labelxfile);
            labelxscr.Text = Lang.GetText(eLang.labelxscr);
            radioButtonAcceptsCommaAndPeriod.Text = Lang.GetText(eLang.radioButtonAcceptsCommaAndPeriod);
            radioButtonOnlyAcceptComma.Text = Lang.GetText(eLang.radioButtonOnlyAcceptComma);
            radioButtonOnlyAcceptPeriod.Text = Lang.GetText(eLang.radioButtonOnlyAcceptPeriod);
            radioButtonOutputComma.Text = Lang.GetText(eLang.radioButtonOutputComma);
            radioButtonOutputPeriod.Text = Lang.GetText(eLang.radioButtonOutputPeriod);
            tabPageDiretory.Text = Lang.GetText(eLang.tabPageDiretory);
            tabPageOthers.Text = Lang.GetText(eLang.tabPageOthers);
        }

    }
}
