
namespace Re4QuadExtremeEditor.src.Forms
{
    partial class OptionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.tabControlConfigs = new System.Windows.Forms.TabControl();
            this.tabPageDiretory = new System.Windows.Forms.TabPage();
            this.groupBoxDiretory = new System.Windows.Forms.GroupBox();
            this.buttonXscrDiretory = new System.Windows.Forms.Button();
            this.textBoxXscrDiretory = new System.Windows.Forms.TextBox();
            this.buttonXfileDiretory = new System.Windows.Forms.Button();
            this.textBoxXfileDiretory = new System.Windows.Forms.TextBox();
            this.labelxscr = new System.Windows.Forms.Label();
            this.labelxfile = new System.Windows.Forms.Label();
            this.tabPageOthers = new System.Windows.Forms.TabPage();
            this.groupBoxLanguage = new System.Windows.Forms.GroupBox();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.labelLanguageWarning = new System.Windows.Forms.Label();
            this.groupBoxItemRotations = new System.Windows.Forms.GroupBox();
            this.checkBoxIgnoreRotationForZisNotGreaterThanZero = new System.Windows.Forms.CheckBox();
            this.numericUpDownDivider = new System.Windows.Forms.NumericUpDown();
            this.labelDivider = new System.Windows.Forms.Label();
            this.numericUpDownMultiplier = new System.Windows.Forms.NumericUpDown();
            this.labelMultiplier = new System.Windows.Forms.Label();
            this.labelItemExtraCalculation = new System.Windows.Forms.Label();
            this.comboBoxItemRotationOrder = new System.Windows.Forms.ComboBox();
            this.labelitemRotationOrderText = new System.Windows.Forms.Label();
            this.checkBoxIgnoreRotationForZeroXYZ = new System.Windows.Forms.CheckBox();
            this.checkBoxDisableItemRotations = new System.Windows.Forms.CheckBox();
            this.groupBoxFloatStyle = new System.Windows.Forms.GroupBox();
            this.groupBoxInputFractionalSymbol = new System.Windows.Forms.GroupBox();
            this.radioButtonOnlyAcceptPeriod = new System.Windows.Forms.RadioButton();
            this.radioButtonOnlyAcceptComma = new System.Windows.Forms.RadioButton();
            this.radioButtonAcceptsCommaAndPeriod = new System.Windows.Forms.RadioButton();
            this.groupBoxOutputFractionalSymbol = new System.Windows.Forms.GroupBox();
            this.radioButtonOutputPeriod = new System.Windows.Forms.RadioButton();
            this.radioButtonOutputComma = new System.Windows.Forms.RadioButton();
            this.groupBoxFractionalPart = new System.Windows.Forms.GroupBox();
            this.labelFrationalAmount = new System.Windows.Forms.Label();
            this.buttonFrationalPlus = new System.Windows.Forms.Button();
            this.buttonFrationalMinus = new System.Windows.Forms.Button();
            this.groupBoxColors = new System.Windows.Forms.GroupBox();
            this.labelSkyColor = new System.Windows.Forms.Label();
            this.panelSkyColor = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxForceReloadModels = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialogDiretory = new System.Windows.Forms.FolderBrowserDialog();
            this.colorDialogColors = new System.Windows.Forms.ColorDialog();
            this.tabControlConfigs.SuspendLayout();
            this.tabPageDiretory.SuspendLayout();
            this.groupBoxDiretory.SuspendLayout();
            this.tabPageOthers.SuspendLayout();
            this.groupBoxLanguage.SuspendLayout();
            this.groupBoxItemRotations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDivider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMultiplier)).BeginInit();
            this.groupBoxFloatStyle.SuspendLayout();
            this.groupBoxInputFractionalSymbol.SuspendLayout();
            this.groupBoxOutputFractionalSymbol.SuspendLayout();
            this.groupBoxFractionalPart.SuspendLayout();
            this.groupBoxColors.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlConfigs
            // 
            this.tabControlConfigs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlConfigs.Controls.Add(this.tabPageDiretory);
            this.tabControlConfigs.Controls.Add(this.tabPageOthers);
            this.tabControlConfigs.Location = new System.Drawing.Point(2, 4);
            this.tabControlConfigs.Name = "tabControlConfigs";
            this.tabControlConfigs.SelectedIndex = 0;
            this.tabControlConfigs.Size = new System.Drawing.Size(789, 1008);
            this.tabControlConfigs.TabIndex = 0;
            // 
            // tabPageDiretory
            // 
            this.tabPageDiretory.AutoScroll = true;
            this.tabPageDiretory.Controls.Add(this.groupBoxDiretory);
            this.tabPageDiretory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageDiretory.Location = new System.Drawing.Point(4, 22);
            this.tabPageDiretory.Name = "tabPageDiretory";
            this.tabPageDiretory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiretory.Size = new System.Drawing.Size(781, 982);
            this.tabPageDiretory.TabIndex = 0;
            this.tabPageDiretory.Text = "Diretory";
            // 
            // groupBoxDiretory
            // 
            this.groupBoxDiretory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDiretory.Controls.Add(this.buttonXscrDiretory);
            this.groupBoxDiretory.Controls.Add(this.textBoxXscrDiretory);
            this.groupBoxDiretory.Controls.Add(this.buttonXfileDiretory);
            this.groupBoxDiretory.Controls.Add(this.textBoxXfileDiretory);
            this.groupBoxDiretory.Controls.Add(this.labelxscr);
            this.groupBoxDiretory.Controls.Add(this.labelxfile);
            this.groupBoxDiretory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDiretory.Location = new System.Drawing.Point(8, 8);
            this.groupBoxDiretory.Name = "groupBoxDiretory";
            this.groupBoxDiretory.Size = new System.Drawing.Size(767, 120);
            this.groupBoxDiretory.TabIndex = 0;
            this.groupBoxDiretory.TabStop = false;
            this.groupBoxDiretory.Text = "Diretorys";
            // 
            // buttonXscrDiretory
            // 
            this.buttonXscrDiretory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXscrDiretory.Location = new System.Drawing.Point(732, 78);
            this.buttonXscrDiretory.Name = "buttonXscrDiretory";
            this.buttonXscrDiretory.Size = new System.Drawing.Size(28, 23);
            this.buttonXscrDiretory.TabIndex = 5;
            this.buttonXscrDiretory.Text = "...";
            this.buttonXscrDiretory.UseVisualStyleBackColor = true;
            this.buttonXscrDiretory.Click += new System.EventHandler(this.buttonXscrDiretory_Click);
            // 
            // textBoxXscrDiretory
            // 
            this.textBoxXscrDiretory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxXscrDiretory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxXscrDiretory.Location = new System.Drawing.Point(15, 80);
            this.textBoxXscrDiretory.MaxLength = 3000;
            this.textBoxXscrDiretory.Name = "textBoxXscrDiretory";
            this.textBoxXscrDiretory.Size = new System.Drawing.Size(711, 20);
            this.textBoxXscrDiretory.TabIndex = 4;
            // 
            // buttonXfileDiretory
            // 
            this.buttonXfileDiretory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonXfileDiretory.Location = new System.Drawing.Point(732, 34);
            this.buttonXfileDiretory.Name = "buttonXfileDiretory";
            this.buttonXfileDiretory.Size = new System.Drawing.Size(28, 23);
            this.buttonXfileDiretory.TabIndex = 2;
            this.buttonXfileDiretory.Text = "...";
            this.buttonXfileDiretory.UseVisualStyleBackColor = true;
            this.buttonXfileDiretory.Click += new System.EventHandler(this.buttonXfileDiretory_Click);
            // 
            // textBoxXfileDiretory
            // 
            this.textBoxXfileDiretory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxXfileDiretory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxXfileDiretory.Location = new System.Drawing.Point(15, 36);
            this.textBoxXfileDiretory.MaxLength = 3000;
            this.textBoxXfileDiretory.Name = "textBoxXfileDiretory";
            this.textBoxXfileDiretory.Size = new System.Drawing.Size(711, 20);
            this.textBoxXfileDiretory.TabIndex = 1;
            // 
            // labelxscr
            // 
            this.labelxscr.AutoSize = true;
            this.labelxscr.Location = new System.Drawing.Point(15, 61);
            this.labelxscr.Name = "labelxscr";
            this.labelxscr.Size = new System.Drawing.Size(92, 13);
            this.labelxscr.TabIndex = 3;
            this.labelxscr.Text = "XSCR Diretory:";
            // 
            // labelxfile
            // 
            this.labelxfile.AutoSize = true;
            this.labelxfile.Location = new System.Drawing.Point(12, 19);
            this.labelxfile.Name = "labelxfile";
            this.labelxfile.Size = new System.Drawing.Size(93, 13);
            this.labelxfile.TabIndex = 0;
            this.labelxfile.Text = "XFILE Diretory:";
            // 
            // tabPageOthers
            // 
            this.tabPageOthers.AutoScroll = true;
            this.tabPageOthers.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageOthers.Controls.Add(this.groupBoxLanguage);
            this.tabPageOthers.Controls.Add(this.groupBoxItemRotations);
            this.tabPageOthers.Controls.Add(this.groupBoxFloatStyle);
            this.tabPageOthers.Controls.Add(this.groupBoxColors);
            this.tabPageOthers.Location = new System.Drawing.Point(4, 22);
            this.tabPageOthers.Name = "tabPageOthers";
            this.tabPageOthers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOthers.Size = new System.Drawing.Size(781, 982);
            this.tabPageOthers.TabIndex = 1;
            this.tabPageOthers.Text = "Other";
            // 
            // groupBoxLanguage
            // 
            this.groupBoxLanguage.Controls.Add(this.comboBoxLanguage);
            this.groupBoxLanguage.Controls.Add(this.labelLanguageWarning);
            this.groupBoxLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxLanguage.Location = new System.Drawing.Point(10, 424);
            this.groupBoxLanguage.Name = "groupBoxLanguage";
            this.groupBoxLanguage.Size = new System.Drawing.Size(662, 83);
            this.groupBoxLanguage.TabIndex = 3;
            this.groupBoxLanguage.TabStop = false;
            this.groupBoxLanguage.Text = "Language";
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Location = new System.Drawing.Point(9, 44);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(641, 21);
            this.comboBoxLanguage.TabIndex = 1;
            // 
            // labelLanguageWarning
            // 
            this.labelLanguageWarning.AutoSize = true;
            this.labelLanguageWarning.Location = new System.Drawing.Point(14, 26);
            this.labelLanguageWarning.Name = "labelLanguageWarning";
            this.labelLanguageWarning.Size = new System.Drawing.Size(327, 13);
            this.labelLanguageWarning.TabIndex = 0;
            this.labelLanguageWarning.Text = "Language changes only take effect after program restart";
            // 
            // groupBoxItemRotations
            // 
            this.groupBoxItemRotations.Controls.Add(this.checkBoxIgnoreRotationForZisNotGreaterThanZero);
            this.groupBoxItemRotations.Controls.Add(this.numericUpDownDivider);
            this.groupBoxItemRotations.Controls.Add(this.labelDivider);
            this.groupBoxItemRotations.Controls.Add(this.numericUpDownMultiplier);
            this.groupBoxItemRotations.Controls.Add(this.labelMultiplier);
            this.groupBoxItemRotations.Controls.Add(this.labelItemExtraCalculation);
            this.groupBoxItemRotations.Controls.Add(this.comboBoxItemRotationOrder);
            this.groupBoxItemRotations.Controls.Add(this.labelitemRotationOrderText);
            this.groupBoxItemRotations.Controls.Add(this.checkBoxIgnoreRotationForZeroXYZ);
            this.groupBoxItemRotations.Controls.Add(this.checkBoxDisableItemRotations);
            this.groupBoxItemRotations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxItemRotations.Location = new System.Drawing.Point(10, 210);
            this.groupBoxItemRotations.Name = "groupBoxItemRotations";
            this.groupBoxItemRotations.Size = new System.Drawing.Size(662, 208);
            this.groupBoxItemRotations.TabIndex = 2;
            this.groupBoxItemRotations.TabStop = false;
            this.groupBoxItemRotations.Text = "Item Rotations";
            // 
            // checkBoxIgnoreRotationForZisNotGreaterThanZero
            // 
            this.checkBoxIgnoreRotationForZisNotGreaterThanZero.AutoSize = true;
            this.checkBoxIgnoreRotationForZisNotGreaterThanZero.Location = new System.Drawing.Point(9, 61);
            this.checkBoxIgnoreRotationForZisNotGreaterThanZero.Name = "checkBoxIgnoreRotationForZisNotGreaterThanZero";
            this.checkBoxIgnoreRotationForZisNotGreaterThanZero.Size = new System.Drawing.Size(296, 17);
            this.checkBoxIgnoreRotationForZisNotGreaterThanZero.TabIndex = 9;
            this.checkBoxIgnoreRotationForZisNotGreaterThanZero.Text = "Ignore rotation if Z field is not greater than zero";
            this.checkBoxIgnoreRotationForZisNotGreaterThanZero.UseVisualStyleBackColor = true;
            // 
            // numericUpDownDivider
            // 
            this.numericUpDownDivider.DecimalPlaces = 9;
            this.numericUpDownDivider.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownDivider.Location = new System.Drawing.Point(185, 173);
            this.numericUpDownDivider.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDownDivider.Minimum = new decimal(new int[] {
            1000000000,
            0,
            0,
            -2147483648});
            this.numericUpDownDivider.Name = "numericUpDownDivider";
            this.numericUpDownDivider.Size = new System.Drawing.Size(158, 20);
            this.numericUpDownDivider.TabIndex = 8;
            this.numericUpDownDivider.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelDivider
            // 
            this.labelDivider.AutoSize = true;
            this.labelDivider.Location = new System.Drawing.Point(183, 154);
            this.labelDivider.Name = "labelDivider";
            this.labelDivider.Size = new System.Drawing.Size(51, 13);
            this.labelDivider.TabIndex = 7;
            this.labelDivider.Text = "Divider:";
            // 
            // numericUpDownMultiplier
            // 
            this.numericUpDownMultiplier.DecimalPlaces = 9;
            this.numericUpDownMultiplier.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownMultiplier.Location = new System.Drawing.Point(19, 173);
            this.numericUpDownMultiplier.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numericUpDownMultiplier.Minimum = new decimal(new int[] {
            1000000000,
            0,
            0,
            -2147483648});
            this.numericUpDownMultiplier.Name = "numericUpDownMultiplier";
            this.numericUpDownMultiplier.Size = new System.Drawing.Size(158, 20);
            this.numericUpDownMultiplier.TabIndex = 6;
            this.numericUpDownMultiplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelMultiplier
            // 
            this.labelMultiplier.AutoSize = true;
            this.labelMultiplier.Location = new System.Drawing.Point(17, 154);
            this.labelMultiplier.Name = "labelMultiplier";
            this.labelMultiplier.Size = new System.Drawing.Size(62, 13);
            this.labelMultiplier.TabIndex = 5;
            this.labelMultiplier.Text = "Multiplier:";
            // 
            // labelItemExtraCalculation
            // 
            this.labelItemExtraCalculation.AutoSize = true;
            this.labelItemExtraCalculation.Location = new System.Drawing.Point(18, 135);
            this.labelItemExtraCalculation.Name = "labelItemExtraCalculation";
            this.labelItemExtraCalculation.Size = new System.Drawing.Size(321, 13);
            this.labelItemExtraCalculation.TabIndex = 4;
            this.labelItemExtraCalculation.Text = "Extra Calculation: Radian = (Input * Multiplier) / Divider";
            // 
            // comboBoxItemRotationOrder
            // 
            this.comboBoxItemRotationOrder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxItemRotationOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxItemRotationOrder.FormattingEnabled = true;
            this.comboBoxItemRotationOrder.Location = new System.Drawing.Point(9, 105);
            this.comboBoxItemRotationOrder.Name = "comboBoxItemRotationOrder";
            this.comboBoxItemRotationOrder.Size = new System.Drawing.Size(641, 21);
            this.comboBoxItemRotationOrder.TabIndex = 3;
            // 
            // labelitemRotationOrderText
            // 
            this.labelitemRotationOrderText.AutoSize = true;
            this.labelitemRotationOrderText.Location = new System.Drawing.Point(7, 84);
            this.labelitemRotationOrderText.Name = "labelitemRotationOrderText";
            this.labelitemRotationOrderText.Size = new System.Drawing.Size(94, 13);
            this.labelitemRotationOrderText.TabIndex = 2;
            this.labelitemRotationOrderText.Text = "Rotation Order:";
            // 
            // checkBoxIgnoreRotationForZeroXYZ
            // 
            this.checkBoxIgnoreRotationForZeroXYZ.AutoSize = true;
            this.checkBoxIgnoreRotationForZeroXYZ.Location = new System.Drawing.Point(9, 41);
            this.checkBoxIgnoreRotationForZeroXYZ.Name = "checkBoxIgnoreRotationForZeroXYZ";
            this.checkBoxIgnoreRotationForZeroXYZ.Size = new System.Drawing.Size(256, 17);
            this.checkBoxIgnoreRotationForZeroXYZ.TabIndex = 1;
            this.checkBoxIgnoreRotationForZeroXYZ.Text = "Ignore rotation if any of XYZ field is zero";
            this.checkBoxIgnoreRotationForZeroXYZ.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisableItemRotations
            // 
            this.checkBoxDisableItemRotations.AutoSize = true;
            this.checkBoxDisableItemRotations.Location = new System.Drawing.Point(9, 21);
            this.checkBoxDisableItemRotations.Name = "checkBoxDisableItemRotations";
            this.checkBoxDisableItemRotations.Size = new System.Drawing.Size(154, 17);
            this.checkBoxDisableItemRotations.TabIndex = 0;
            this.checkBoxDisableItemRotations.Text = "Disable Item Rotations";
            this.checkBoxDisableItemRotations.UseVisualStyleBackColor = true;
            // 
            // groupBoxFloatStyle
            // 
            this.groupBoxFloatStyle.Controls.Add(this.groupBoxInputFractionalSymbol);
            this.groupBoxFloatStyle.Controls.Add(this.groupBoxOutputFractionalSymbol);
            this.groupBoxFloatStyle.Controls.Add(this.groupBoxFractionalPart);
            this.groupBoxFloatStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxFloatStyle.Location = new System.Drawing.Point(6, 80);
            this.groupBoxFloatStyle.Name = "groupBoxFloatStyle";
            this.groupBoxFloatStyle.Size = new System.Drawing.Size(666, 124);
            this.groupBoxFloatStyle.TabIndex = 1;
            this.groupBoxFloatStyle.TabStop = false;
            this.groupBoxFloatStyle.Text = "Float Style";
            // 
            // groupBoxInputFractionalSymbol
            // 
            this.groupBoxInputFractionalSymbol.Controls.Add(this.radioButtonOnlyAcceptPeriod);
            this.groupBoxInputFractionalSymbol.Controls.Add(this.radioButtonOnlyAcceptComma);
            this.groupBoxInputFractionalSymbol.Controls.Add(this.radioButtonAcceptsCommaAndPeriod);
            this.groupBoxInputFractionalSymbol.Location = new System.Drawing.Point(13, 19);
            this.groupBoxInputFractionalSymbol.Name = "groupBoxInputFractionalSymbol";
            this.groupBoxInputFractionalSymbol.Size = new System.Drawing.Size(241, 94);
            this.groupBoxInputFractionalSymbol.TabIndex = 0;
            this.groupBoxInputFractionalSymbol.TabStop = false;
            this.groupBoxInputFractionalSymbol.Text = "Input fractional symbol";
            // 
            // radioButtonOnlyAcceptPeriod
            // 
            this.radioButtonOnlyAcceptPeriod.AutoSize = true;
            this.radioButtonOnlyAcceptPeriod.Location = new System.Drawing.Point(13, 66);
            this.radioButtonOnlyAcceptPeriod.Name = "radioButtonOnlyAcceptPeriod";
            this.radioButtonOnlyAcceptPeriod.Size = new System.Drawing.Size(130, 17);
            this.radioButtonOnlyAcceptPeriod.TabIndex = 2;
            this.radioButtonOnlyAcceptPeriod.TabStop = true;
            this.radioButtonOnlyAcceptPeriod.Text = "only accept period";
            this.radioButtonOnlyAcceptPeriod.UseVisualStyleBackColor = true;
            this.radioButtonOnlyAcceptPeriod.CheckedChanged += new System.EventHandler(this.radioButtonOnlyAcceptPeriod_CheckedChanged);
            // 
            // radioButtonOnlyAcceptComma
            // 
            this.radioButtonOnlyAcceptComma.AutoSize = true;
            this.radioButtonOnlyAcceptComma.Location = new System.Drawing.Point(13, 43);
            this.radioButtonOnlyAcceptComma.Name = "radioButtonOnlyAcceptComma";
            this.radioButtonOnlyAcceptComma.Size = new System.Drawing.Size(136, 17);
            this.radioButtonOnlyAcceptComma.TabIndex = 1;
            this.radioButtonOnlyAcceptComma.TabStop = true;
            this.radioButtonOnlyAcceptComma.Text = "Only accept comma";
            this.radioButtonOnlyAcceptComma.UseVisualStyleBackColor = true;
            this.radioButtonOnlyAcceptComma.CheckedChanged += new System.EventHandler(this.radioButtonOnlyAcceptComma_CheckedChanged);
            // 
            // radioButtonAcceptsCommaAndPeriod
            // 
            this.radioButtonAcceptsCommaAndPeriod.AutoSize = true;
            this.radioButtonAcceptsCommaAndPeriod.Location = new System.Drawing.Point(13, 20);
            this.radioButtonAcceptsCommaAndPeriod.Name = "radioButtonAcceptsCommaAndPeriod";
            this.radioButtonAcceptsCommaAndPeriod.Size = new System.Drawing.Size(178, 17);
            this.radioButtonAcceptsCommaAndPeriod.TabIndex = 0;
            this.radioButtonAcceptsCommaAndPeriod.TabStop = true;
            this.radioButtonAcceptsCommaAndPeriod.Text = "Accepts comma and period";
            this.radioButtonAcceptsCommaAndPeriod.UseVisualStyleBackColor = true;
            this.radioButtonAcceptsCommaAndPeriod.CheckedChanged += new System.EventHandler(this.radioButtonAcceptsCommaAndPeriod_CheckedChanged);
            // 
            // groupBoxOutputFractionalSymbol
            // 
            this.groupBoxOutputFractionalSymbol.Controls.Add(this.radioButtonOutputPeriod);
            this.groupBoxOutputFractionalSymbol.Controls.Add(this.radioButtonOutputComma);
            this.groupBoxOutputFractionalSymbol.Location = new System.Drawing.Point(260, 19);
            this.groupBoxOutputFractionalSymbol.Name = "groupBoxOutputFractionalSymbol";
            this.groupBoxOutputFractionalSymbol.Size = new System.Drawing.Size(223, 94);
            this.groupBoxOutputFractionalSymbol.TabIndex = 1;
            this.groupBoxOutputFractionalSymbol.TabStop = false;
            this.groupBoxOutputFractionalSymbol.Text = "Output fractional symbol";
            // 
            // radioButtonOutputPeriod
            // 
            this.radioButtonOutputPeriod.AutoSize = true;
            this.radioButtonOutputPeriod.Location = new System.Drawing.Point(14, 43);
            this.radioButtonOutputPeriod.Name = "radioButtonOutputPeriod";
            this.radioButtonOutputPeriod.Size = new System.Drawing.Size(103, 17);
            this.radioButtonOutputPeriod.TabIndex = 1;
            this.radioButtonOutputPeriod.TabStop = true;
            this.radioButtonOutputPeriod.Text = "Output Period";
            this.radioButtonOutputPeriod.UseVisualStyleBackColor = true;
            // 
            // radioButtonOutputComma
            // 
            this.radioButtonOutputComma.AutoSize = true;
            this.radioButtonOutputComma.Location = new System.Drawing.Point(14, 19);
            this.radioButtonOutputComma.Name = "radioButtonOutputComma";
            this.radioButtonOutputComma.Size = new System.Drawing.Size(107, 17);
            this.radioButtonOutputComma.TabIndex = 0;
            this.radioButtonOutputComma.TabStop = true;
            this.radioButtonOutputComma.Text = "Output Comma";
            this.radioButtonOutputComma.UseVisualStyleBackColor = true;
            // 
            // groupBoxFractionalPart
            // 
            this.groupBoxFractionalPart.Controls.Add(this.labelFrationalAmount);
            this.groupBoxFractionalPart.Controls.Add(this.buttonFrationalPlus);
            this.groupBoxFractionalPart.Controls.Add(this.buttonFrationalMinus);
            this.groupBoxFractionalPart.Location = new System.Drawing.Point(488, 19);
            this.groupBoxFractionalPart.Name = "groupBoxFractionalPart";
            this.groupBoxFractionalPart.Size = new System.Drawing.Size(171, 94);
            this.groupBoxFractionalPart.TabIndex = 2;
            this.groupBoxFractionalPart.TabStop = false;
            this.groupBoxFractionalPart.Text = "Fractional part amount";
            // 
            // labelFrationalAmount
            // 
            this.labelFrationalAmount.AutoSize = true;
            this.labelFrationalAmount.Location = new System.Drawing.Point(78, 46);
            this.labelFrationalAmount.Name = "labelFrationalAmount";
            this.labelFrationalAmount.Size = new System.Drawing.Size(14, 13);
            this.labelFrationalAmount.TabIndex = 1;
            this.labelFrationalAmount.Text = "9";
            // 
            // buttonFrationalPlus
            // 
            this.buttonFrationalPlus.Location = new System.Drawing.Point(103, 41);
            this.buttonFrationalPlus.Name = "buttonFrationalPlus";
            this.buttonFrationalPlus.Size = new System.Drawing.Size(32, 23);
            this.buttonFrationalPlus.TabIndex = 2;
            this.buttonFrationalPlus.Text = ">>";
            this.buttonFrationalPlus.UseVisualStyleBackColor = true;
            this.buttonFrationalPlus.Click += new System.EventHandler(this.buttonFrationalPlus_Click);
            // 
            // buttonFrationalMinus
            // 
            this.buttonFrationalMinus.Location = new System.Drawing.Point(35, 41);
            this.buttonFrationalMinus.Name = "buttonFrationalMinus";
            this.buttonFrationalMinus.Size = new System.Drawing.Size(32, 23);
            this.buttonFrationalMinus.TabIndex = 0;
            this.buttonFrationalMinus.Text = "<<";
            this.buttonFrationalMinus.UseVisualStyleBackColor = true;
            this.buttonFrationalMinus.Click += new System.EventHandler(this.buttonFrationalMinus_Click);
            // 
            // groupBoxColors
            // 
            this.groupBoxColors.Controls.Add(this.labelSkyColor);
            this.groupBoxColors.Controls.Add(this.panelSkyColor);
            this.groupBoxColors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxColors.Location = new System.Drawing.Point(6, 6);
            this.groupBoxColors.Name = "groupBoxColors";
            this.groupBoxColors.Size = new System.Drawing.Size(666, 68);
            this.groupBoxColors.TabIndex = 0;
            this.groupBoxColors.TabStop = false;
            this.groupBoxColors.Text = "Colors";
            // 
            // labelSkyColor
            // 
            this.labelSkyColor.AutoSize = true;
            this.labelSkyColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSkyColor.Location = new System.Drawing.Point(49, 32);
            this.labelSkyColor.Name = "labelSkyColor";
            this.labelSkyColor.Size = new System.Drawing.Size(61, 13);
            this.labelSkyColor.TabIndex = 1;
            this.labelSkyColor.Text = "Sky Color";
            // 
            // panelSkyColor
            // 
            this.panelSkyColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSkyColor.Location = new System.Drawing.Point(13, 23);
            this.panelSkyColor.Name = "panelSkyColor";
            this.panelSkyColor.Size = new System.Drawing.Size(30, 30);
            this.panelSkyColor.TabIndex = 0;
            this.panelSkyColor.Click += new System.EventHandler(this.panelSkyColor_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(695, 1021);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "CANCEL";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(601, 1021);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(90, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxForceReloadModels
            // 
            this.checkBoxForceReloadModels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxForceReloadModels.AutoSize = true;
            this.checkBoxForceReloadModels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxForceReloadModels.Location = new System.Drawing.Point(12, 1024);
            this.checkBoxForceReloadModels.Name = "checkBoxForceReloadModels";
            this.checkBoxForceReloadModels.Size = new System.Drawing.Size(232, 17);
            this.checkBoxForceReloadModels.TabIndex = 1;
            this.checkBoxForceReloadModels.Text = "Force Reload Models And Json Files";
            this.checkBoxForceReloadModels.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialogDiretory
            // 
            this.folderBrowserDialogDiretory.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialogDiretory.ShowNewFolderButton = false;
            // 
            // colorDialogColors
            // 
            this.colorDialogColors.FullOpen = true;
            this.colorDialogColors.SolidColorOnly = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 1053);
            this.Controls.Add(this.checkBoxForceReloadModels);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.tabControlConfigs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OptionsForm_KeyDown);
            this.tabControlConfigs.ResumeLayout(false);
            this.tabPageDiretory.ResumeLayout(false);
            this.groupBoxDiretory.ResumeLayout(false);
            this.groupBoxDiretory.PerformLayout();
            this.tabPageOthers.ResumeLayout(false);
            this.groupBoxLanguage.ResumeLayout(false);
            this.groupBoxLanguage.PerformLayout();
            this.groupBoxItemRotations.ResumeLayout(false);
            this.groupBoxItemRotations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDivider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMultiplier)).EndInit();
            this.groupBoxFloatStyle.ResumeLayout(false);
            this.groupBoxInputFractionalSymbol.ResumeLayout(false);
            this.groupBoxInputFractionalSymbol.PerformLayout();
            this.groupBoxOutputFractionalSymbol.ResumeLayout(false);
            this.groupBoxOutputFractionalSymbol.PerformLayout();
            this.groupBoxFractionalPart.ResumeLayout(false);
            this.groupBoxFractionalPart.PerformLayout();
            this.groupBoxColors.ResumeLayout(false);
            this.groupBoxColors.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlConfigs;
        private System.Windows.Forms.TabPage tabPageDiretory;
        private System.Windows.Forms.TabPage tabPageOthers;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxForceReloadModels;
        private System.Windows.Forms.GroupBox groupBoxDiretory;
        private System.Windows.Forms.Button buttonXscrDiretory;
        private System.Windows.Forms.TextBox textBoxXscrDiretory;
        private System.Windows.Forms.Button buttonXfileDiretory;
        private System.Windows.Forms.TextBox textBoxXfileDiretory;
        private System.Windows.Forms.Label labelxscr;
        private System.Windows.Forms.Label labelxfile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogDiretory;
        private System.Windows.Forms.Label labelSkyColor;
        private System.Windows.Forms.Panel panelSkyColor;
        private System.Windows.Forms.ColorDialog colorDialogColors;
        private System.Windows.Forms.GroupBox groupBoxLanguage;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
        private System.Windows.Forms.Label labelLanguageWarning;
        private System.Windows.Forms.GroupBox groupBoxItemRotations;
        private System.Windows.Forms.NumericUpDown numericUpDownDivider;
        private System.Windows.Forms.Label labelDivider;
        private System.Windows.Forms.NumericUpDown numericUpDownMultiplier;
        private System.Windows.Forms.Label labelMultiplier;
        private System.Windows.Forms.Label labelItemExtraCalculation;
        private System.Windows.Forms.ComboBox comboBoxItemRotationOrder;
        private System.Windows.Forms.Label labelitemRotationOrderText;
        private System.Windows.Forms.CheckBox checkBoxIgnoreRotationForZeroXYZ;
        private System.Windows.Forms.CheckBox checkBoxDisableItemRotations;
        private System.Windows.Forms.GroupBox groupBoxFloatStyle;
        private System.Windows.Forms.GroupBox groupBoxInputFractionalSymbol;
        private System.Windows.Forms.RadioButton radioButtonOnlyAcceptPeriod;
        private System.Windows.Forms.RadioButton radioButtonOnlyAcceptComma;
        private System.Windows.Forms.RadioButton radioButtonAcceptsCommaAndPeriod;
        private System.Windows.Forms.GroupBox groupBoxOutputFractionalSymbol;
        private System.Windows.Forms.RadioButton radioButtonOutputPeriod;
        private System.Windows.Forms.RadioButton radioButtonOutputComma;
        private System.Windows.Forms.GroupBox groupBoxFractionalPart;
        private System.Windows.Forms.GroupBox groupBoxColors;
        private System.Windows.Forms.Label labelFrationalAmount;
        private System.Windows.Forms.Button buttonFrationalPlus;
        private System.Windows.Forms.Button buttonFrationalMinus;
        private System.Windows.Forms.CheckBox checkBoxIgnoreRotationForZisNotGreaterThanZero;
    }
}