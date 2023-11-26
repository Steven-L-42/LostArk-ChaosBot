using Cyotek.Windows.Forms;

namespace PixelAimbot
{
    partial class Debugging
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Debugging));
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.labelX = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGetMinimap = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonSelectPicture = new System.Windows.Forms.Button();
            this.buttonSelectMask = new System.Windows.Forms.Button();
            this.labelTreshold = new System.Windows.Forms.Label();
            this.trackBarTreshold = new System.Windows.Forms.TrackBar();
            this.buttonSelectArea = new System.Windows.Forms.Button();
            this.pictureBoxMask = new System.Windows.Forms.PictureBox();
            this.pictureBoxPicture = new System.Windows.Forms.PictureBox();
            this.labelRefresh = new System.Windows.Forms.Label();
            this.trackBarThreadSleep = new System.Windows.Forms.TrackBar();
            this.checkBoxShowAll = new System.Windows.Forms.CheckBox();
            this.radioButtonGetBest = new System.Windows.Forms.RadioButton();
            this.radioButtonGetClosest = new System.Windows.Forms.RadioButton();
            this.radioButtonGetClosestBest = new System.Windows.Forms.RadioButton();
            this.buttonGenerateCode = new System.Windows.Forms.Button();
            this.buttonRecalc = new System.Windows.Forms.Button();
            this.buttonRecalcToBotresolution = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxMethod = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxTextSearch = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.labelVariantShade = new System.Windows.Forms.Label();
            this.trackBarVariant = new System.Windows.Forms.TrackBar();
            this.screenColorPicker1 = new Cyotek.Windows.Forms.ScreenColorPicker();
            this.labelColorARGB = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.trackBarCannySecond = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.trackBarCannyFirst = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.TresholdBinarie = new System.Windows.Forms.Label();
            this.trackBarErode = new System.Windows.Forms.TrackBar();
            this.trackBarDilade = new System.Windows.Forms.TrackBar();
            this.trackBarTreshBinary = new System.Windows.Forms.TrackBar();
            this.imageBoxMinimap = new Emgu.CV.UI.ImageBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbYCoord = new System.Windows.Forms.Label();
            this.lbXCoord = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.label11 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarTreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxMask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarThreadSleep)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarVariant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarCannySecond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarCannyFirst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarErode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarDilade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarTreshBinary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageBoxMinimap)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.imageBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBar3)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(18, 556);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(317, 27);
            this.button2.TabIndex = 2;
            this.button2.Text = "Live";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(225, 13);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(100, 20);
            this.textBoxX.TabIndex = 3;
            this.textBoxX.TextChanged += new System.EventHandler(this.textBoxX_TextChanged);
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(225, 39);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(100, 20);
            this.textBoxY.TabIndex = 4;
            this.textBoxY.TextChanged += new System.EventHandler(this.textBoxY_TextChanged);
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(225, 65);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(100, 20);
            this.textBoxWidth.TabIndex = 5;
            this.textBoxWidth.TextChanged += new System.EventHandler(this.textBoxWidth_TextChanged);
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(225, 91);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(100, 20);
            this.textBoxHeight.TabIndex = 6;
            this.textBoxHeight.TextChanged += new System.EventHandler(this.textBoxHeight_TextChanged);
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(11, 16);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(79, 13);
            this.labelX.TabIndex = 7;
            this.labelX.Text = "X Start Position";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Y Start Position";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Height";
            // 
            // btnGetMinimap
            // 
            this.btnGetMinimap.Location = new System.Drawing.Point(11, 174);
            this.btnGetMinimap.Name = "btnGetMinimap";
            this.btnGetMinimap.Size = new System.Drawing.Size(148, 27);
            this.btnGetMinimap.TabIndex = 11;
            this.btnGetMinimap.Text = "Get Minimap";
            this.btnGetMinimap.UseVisualStyleBackColor = true;
            this.btnGetMinimap.Click += new System.EventHandler(this.btnGetMinimap_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "png";
            // 
            // buttonSelectPicture
            // 
            this.buttonSelectPicture.Location = new System.Drawing.Point(56, 146);
            this.buttonSelectPicture.Name = "buttonSelectPicture";
            this.buttonSelectPicture.Size = new System.Drawing.Size(264, 27);
            this.buttonSelectPicture.TabIndex = 12;
            this.buttonSelectPicture.Text = "Select Picture";
            this.buttonSelectPicture.UseVisualStyleBackColor = true;
            this.buttonSelectPicture.Click += new System.EventHandler(this.buttonSelectPicture_Click);
            // 
            // buttonSelectMask
            // 
            this.buttonSelectMask.Location = new System.Drawing.Point(56, 179);
            this.buttonSelectMask.Name = "buttonSelectMask";
            this.buttonSelectMask.Size = new System.Drawing.Size(264, 27);
            this.buttonSelectMask.TabIndex = 13;
            this.buttonSelectMask.Text = "Select Mask";
            this.buttonSelectMask.UseVisualStyleBackColor = true;
            this.buttonSelectMask.Click += new System.EventHandler(this.buttonSelectMask_Click);
            // 
            // labelTreshold
            // 
            this.labelTreshold.AutoSize = true;
            this.labelTreshold.Location = new System.Drawing.Point(5, 38);
            this.labelTreshold.Name = "labelTreshold";
            this.labelTreshold.Size = new System.Drawing.Size(72, 13);
            this.labelTreshold.TabIndex = 17;
            this.labelTreshold.Text = "Treshold (0.7)";
            // 
            // trackBarTreshold
            // 
            this.trackBarTreshold.Location = new System.Drawing.Point(111, 32);
            this.trackBarTreshold.Maximum = 100;
            this.trackBarTreshold.Name = "trackBarTreshold";
            this.trackBarTreshold.Size = new System.Drawing.Size(211, 45);
            this.trackBarTreshold.TabIndex = 18;
            this.trackBarTreshold.Value = 70;
            this.trackBarTreshold.ValueChanged += new System.EventHandler(this.trackBarTreshold_Changed);
            // 
            // buttonSelectArea
            // 
            this.buttonSelectArea.Location = new System.Drawing.Point(169, 174);
            this.buttonSelectArea.Name = "buttonSelectArea";
            this.buttonSelectArea.Size = new System.Drawing.Size(156, 27);
            this.buttonSelectArea.TabIndex = 19;
            this.buttonSelectArea.Text = "Select Area";
            this.buttonSelectArea.UseVisualStyleBackColor = true;
            this.buttonSelectArea.Click += new System.EventHandler(this.buttonSelectArea_Click);
            // 
            // pictureBoxMask
            // 
            this.pictureBoxMask.Location = new System.Drawing.Point(7, 179);
            this.pictureBoxMask.Name = "pictureBoxMask";
            this.pictureBoxMask.Size = new System.Drawing.Size(43, 27);
            this.pictureBoxMask.TabIndex = 15;
            this.pictureBoxMask.TabStop = false;
            // 
            // pictureBoxPicture
            // 
            this.pictureBoxPicture.Location = new System.Drawing.Point(7, 146);
            this.pictureBoxPicture.Name = "pictureBoxPicture";
            this.pictureBoxPicture.Size = new System.Drawing.Size(43, 27);
            this.pictureBoxPicture.TabIndex = 14;
            this.pictureBoxPicture.TabStop = false;
            // 
            // labelRefresh
            // 
            this.labelRefresh.AutoSize = true;
            this.labelRefresh.Location = new System.Drawing.Point(11, 128);
            this.labelRefresh.Name = "labelRefresh";
            this.labelRefresh.Size = new System.Drawing.Size(84, 13);
            this.labelRefresh.TabIndex = 20;
            this.labelRefresh.Text = "Refresh (100ms)";
            // 
            // trackBarThreadSleep
            // 
            this.trackBarThreadSleep.Location = new System.Drawing.Point(120, 123);
            this.trackBarThreadSleep.Maximum = 1000;
            this.trackBarThreadSleep.Name = "trackBarThreadSleep";
            this.trackBarThreadSleep.Size = new System.Drawing.Size(205, 45);
            this.trackBarThreadSleep.TabIndex = 21;
            this.trackBarThreadSleep.Value = 100;
            this.trackBarThreadSleep.ValueChanged += new System.EventHandler(this.trackBarThreadSleep_ValueChanged);
            // 
            // checkBoxShowAll
            // 
            this.checkBoxShowAll.AutoSize = true;
            this.checkBoxShowAll.Checked = true;
            this.checkBoxShowAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowAll.Location = new System.Drawing.Point(6, 100);
            this.checkBoxShowAll.Name = "checkBoxShowAll";
            this.checkBoxShowAll.Size = new System.Drawing.Size(161, 17);
            this.checkBoxShowAll.TabIndex = 22;
            this.checkBoxShowAll.Text = "Show only Closest Detection";
            this.checkBoxShowAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxShowAll.UseVisualStyleBackColor = true;
            // 
            // radioButtonGetBest
            // 
            this.radioButtonGetBest.AutoSize = true;
            this.radioButtonGetBest.Checked = true;
            this.radioButtonGetBest.Location = new System.Drawing.Point(6, 123);
            this.radioButtonGetBest.Name = "radioButtonGetBest";
            this.radioButtonGetBest.Size = new System.Drawing.Size(66, 17);
            this.radioButtonGetBest.TabIndex = 24;
            this.radioButtonGetBest.TabStop = true;
            this.radioButtonGetBest.Text = "Get Best";
            this.radioButtonGetBest.UseVisualStyleBackColor = true;
            // 
            // radioButtonGetClosest
            // 
            this.radioButtonGetClosest.AutoSize = true;
            this.radioButtonGetClosest.Location = new System.Drawing.Point(78, 123);
            this.radioButtonGetClosest.Name = "radioButtonGetClosest";
            this.radioButtonGetClosest.Size = new System.Drawing.Size(79, 17);
            this.radioButtonGetClosest.TabIndex = 25;
            this.radioButtonGetClosest.TabStop = true;
            this.radioButtonGetClosest.Text = "Get Closest";
            this.radioButtonGetClosest.UseVisualStyleBackColor = true;
            // 
            // radioButtonGetClosestBest
            // 
            this.radioButtonGetClosestBest.AutoSize = true;
            this.radioButtonGetClosestBest.Location = new System.Drawing.Point(163, 123);
            this.radioButtonGetClosestBest.Name = "radioButtonGetClosestBest";
            this.radioButtonGetClosestBest.Size = new System.Drawing.Size(103, 17);
            this.radioButtonGetClosestBest.TabIndex = 26;
            this.radioButtonGetClosestBest.TabStop = true;
            this.radioButtonGetClosestBest.Text = "Get Closest Best";
            this.radioButtonGetClosestBest.UseVisualStyleBackColor = true;
            // 
            // buttonGenerateCode
            // 
            this.buttonGenerateCode.Enabled = false;
            this.buttonGenerateCode.Location = new System.Drawing.Point(18, 589);
            this.buttonGenerateCode.Name = "buttonGenerateCode";
            this.buttonGenerateCode.Size = new System.Drawing.Size(317, 27);
            this.buttonGenerateCode.TabIndex = 27;
            this.buttonGenerateCode.Text = "Generate Code to Clipboard";
            this.buttonGenerateCode.UseVisualStyleBackColor = true;
            this.buttonGenerateCode.Click += new System.EventHandler(this.buttonGenerateCode_Click);
            // 
            // buttonRecalc
            // 
            this.buttonRecalc.Location = new System.Drawing.Point(131, 13);
            this.buttonRecalc.Name = "buttonRecalc";
            this.buttonRecalc.Size = new System.Drawing.Size(88, 46);
            this.buttonRecalc.TabIndex = 28;
            this.buttonRecalc.Text = "Recalc to my Resolution";
            this.buttonRecalc.UseVisualStyleBackColor = true;
            this.buttonRecalc.Click += new System.EventHandler(this.buttonRecalc_Click);
            // 
            // buttonRecalcToBotresolution
            // 
            this.buttonRecalcToBotresolution.Location = new System.Drawing.Point(131, 65);
            this.buttonRecalcToBotresolution.Name = "buttonRecalcToBotresolution";
            this.buttonRecalcToBotresolution.Size = new System.Drawing.Size(88, 46);
            this.buttonRecalcToBotresolution.TabIndex = 29;
            this.buttonRecalcToBotresolution.Text = "Recalc to Bot Resolution";
            this.buttonRecalcToBotresolution.UseVisualStyleBackColor = true;
            this.buttonRecalcToBotresolution.Click += new System.EventHandler(this.buttonRecalcToBotresolution_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Comparison Method";
            // 
            // comboBoxMethod
            // 
            this.comboBoxMethod.FormattingEnabled = true;
            this.comboBoxMethod.Items.AddRange(new object[] {"TemplateMatchingType.SqdiffNormed", "TemplateMatchingType.Sqdiff", "TemplateMatchingType.Ccoeff", "TemplateMatchingType.CcoeffNormed", "TemplateMatchingType.Ccorr", "TemplateMatchingType.CcorrNormed", "            "});
            this.comboBoxMethod.Location = new System.Drawing.Point(126, 5);
            this.comboBoxMethod.Name = "comboBoxMethod";
            this.comboBoxMethod.Size = new System.Drawing.Size(194, 21);
            this.comboBoxMethod.TabIndex = 31;
            this.comboBoxMethod.SelectedIndexChanged += new System.EventHandler(this.comboBoxMethod_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(303, 253);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 27);
            this.button1.TabIndex = 35;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(11, 221);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(558, 329);
            this.tabControl1.TabIndex = 36;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.trackBarTreshold);
            this.tabPage1.Controls.Add(this.labelTreshold);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.comboBoxMethod);
            this.tabPage1.Controls.Add(this.checkBoxShowAll);
            this.tabPage1.Controls.Add(this.radioButtonGetBest);
            this.tabPage1.Controls.Add(this.radioButtonGetClosest);
            this.tabPage1.Controls.Add(this.radioButtonGetClosestBest);
            this.tabPage1.Controls.Add(this.buttonSelectPicture);
            this.tabPage1.Controls.Add(this.pictureBoxMask);
            this.tabPage1.Controls.Add(this.buttonSelectMask);
            this.tabPage1.Controls.Add(this.pictureBoxPicture);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(550, 303);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "OpenCV";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBoxTextSearch);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(550, 303);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Text Search";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxTextSearch
            // 
            this.textBoxTextSearch.Location = new System.Drawing.Point(6, 14);
            this.textBoxTextSearch.Multiline = true;
            this.textBoxTextSearch.Name = "textBoxTextSearch";
            this.textBoxTextSearch.Size = new System.Drawing.Size(314, 212);
            this.textBoxTextSearch.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.labelVariantShade);
            this.tabPage3.Controls.Add(this.trackBarVariant);
            this.tabPage3.Controls.Add(this.screenColorPicker1);
            this.tabPage3.Controls.Add(this.labelColorARGB);
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(550, 303);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Pixelsearch";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // labelVariantShade
            // 
            this.labelVariantShade.AutoSize = true;
            this.labelVariantShade.Location = new System.Drawing.Point(6, 60);
            this.labelVariantShade.Name = "labelVariantShade";
            this.labelVariantShade.Size = new System.Drawing.Size(89, 13);
            this.labelVariantShade.TabIndex = 37;
            this.labelVariantShade.Text = "Variant Shade (1)";
            // 
            // trackBarVariant
            // 
            this.trackBarVariant.Location = new System.Drawing.Point(6, 87);
            this.trackBarVariant.Maximum = 25;
            this.trackBarVariant.Minimum = 1;
            this.trackBarVariant.Name = "trackBarVariant";
            this.trackBarVariant.Size = new System.Drawing.Size(314, 45);
            this.trackBarVariant.TabIndex = 3;
            this.trackBarVariant.Value = 1;
            this.trackBarVariant.ValueChanged += new System.EventHandler(this.trackBarVariant_ValueChanged);
            // 
            // screenColorPicker1
            // 
            this.screenColorPicker1.Color = System.Drawing.Color.Empty;
            this.screenColorPicker1.Location = new System.Drawing.Point(6, 6);
            this.screenColorPicker1.Name = "screenColorPicker1";
            this.screenColorPicker1.Size = new System.Drawing.Size(116, 37);
            this.screenColorPicker1.Text = "Click to Select Color";
            this.screenColorPicker1.ColorChanged += new System.EventHandler(this.screenColorPicker1_ColorChanged);
            // 
            // labelColorARGB
            // 
            this.labelColorARGB.Location = new System.Drawing.Point(162, 14);
            this.labelColorARGB.Name = "labelColorARGB";
            this.labelColorARGB.Size = new System.Drawing.Size(100, 23);
            this.labelColorARGB.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(268, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(52, 37);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Controls.Add(this.trackBarCannySecond);
            this.tabPage4.Controls.Add(this.label9);
            this.tabPage4.Controls.Add(this.trackBarCannyFirst);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.TresholdBinarie);
            this.tabPage4.Controls.Add(this.trackBarErode);
            this.tabPage4.Controls.Add(this.trackBarDilade);
            this.tabPage4.Controls.Add(this.trackBarTreshBinary);
            this.tabPage4.Controls.Add(this.imageBoxMinimap);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(550, 303);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Pathfinder";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(214, 235);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 23);
            this.label10.TabIndex = 31;
            this.label10.Text = "Canny Second";
            // 
            // trackBarCannySecond
            // 
            this.trackBarCannySecond.Location = new System.Drawing.Point(214, 252);
            this.trackBarCannySecond.Maximum = 255;
            this.trackBarCannySecond.Name = "trackBarCannySecond";
            this.trackBarCannySecond.Size = new System.Drawing.Size(205, 45);
            this.trackBarCannySecond.TabIndex = 30;
            this.trackBarCannySecond.ValueChanged += new System.EventHandler(this.trackBarCannySecond_ValueChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 235);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 23);
            this.label9.TabIndex = 29;
            this.label9.Text = "Canny First";
            // 
            // trackBarCannyFirst
            // 
            this.trackBarCannyFirst.Location = new System.Drawing.Point(6, 252);
            this.trackBarCannyFirst.Maximum = 255;
            this.trackBarCannyFirst.Name = "trackBarCannyFirst";
            this.trackBarCannyFirst.Size = new System.Drawing.Size(205, 45);
            this.trackBarCannyFirst.TabIndex = 28;
            this.trackBarCannyFirst.ValueChanged += new System.EventHandler(this.trackBarCannyFirst_ValueChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(335, 161);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 23);
            this.label8.TabIndex = 27;
            this.label8.Text = "Erode";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(335, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 26;
            this.label7.Text = "Dilade";
            // 
            // TresholdBinarie
            // 
            this.TresholdBinarie.Location = new System.Drawing.Point(335, 10);
            this.TresholdBinarie.Name = "TresholdBinarie";
            this.TresholdBinarie.Size = new System.Drawing.Size(100, 23);
            this.TresholdBinarie.TabIndex = 25;
            this.TresholdBinarie.Text = "Treshold Binarie";
            // 
            // trackBarErode
            // 
            this.trackBarErode.Location = new System.Drawing.Point(335, 187);
            this.trackBarErode.Maximum = 15;
            this.trackBarErode.Name = "trackBarErode";
            this.trackBarErode.Size = new System.Drawing.Size(205, 45);
            this.trackBarErode.TabIndex = 24;
            this.trackBarErode.Value = 1;
            this.trackBarErode.ValueChanged += new System.EventHandler(this.trackBarErode_ValueChanged);
            // 
            // trackBarDilade
            // 
            this.trackBarDilade.Location = new System.Drawing.Point(335, 107);
            this.trackBarDilade.Maximum = 15;
            this.trackBarDilade.Name = "trackBarDilade";
            this.trackBarDilade.Size = new System.Drawing.Size(205, 45);
            this.trackBarDilade.TabIndex = 23;
            this.trackBarDilade.Value = 1;
            this.trackBarDilade.ValueChanged += new System.EventHandler(this.trackBarDilade_ValueChanged);
            // 
            // trackBarTreshBinary
            // 
            this.trackBarTreshBinary.Location = new System.Drawing.Point(335, 31);
            this.trackBarTreshBinary.Maximum = 255;
            this.trackBarTreshBinary.Minimum = 1;
            this.trackBarTreshBinary.Name = "trackBarTreshBinary";
            this.trackBarTreshBinary.Size = new System.Drawing.Size(205, 45);
            this.trackBarTreshBinary.TabIndex = 22;
            this.trackBarTreshBinary.Value = 1;
            this.trackBarTreshBinary.ValueChanged += new System.EventHandler(this.trackBarTreshBinary_ValueChanged);
            // 
            // imageBoxMinimap
            // 
            this.imageBoxMinimap.Location = new System.Drawing.Point(0, 0);
            this.imageBoxMinimap.Name = "imageBoxMinimap";
            this.imageBoxMinimap.Size = new System.Drawing.Size(329, 232);
            this.imageBoxMinimap.TabIndex = 2;
            this.imageBoxMinimap.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "X:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(67, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "Y:";
            // 
            // lbYCoord
            // 
            this.lbYCoord.AutoSize = true;
            this.lbYCoord.Location = new System.Drawing.Point(80, 155);
            this.lbYCoord.Name = "lbYCoord";
            this.lbYCoord.Size = new System.Drawing.Size(31, 13);
            this.lbYCoord.TabIndex = 39;
            this.lbYCoord.Text = "0000";
            this.lbYCoord.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbYCoord_MouseDoubleClick);
            this.lbYCoord.MouseLeave += new System.EventHandler(this.lbYCoord_MouseLeave);
            this.lbYCoord.MouseHover += new System.EventHandler(this.lbYCoord_MouseHover);
            // 
            // lbXCoord
            // 
            this.lbXCoord.AutoSize = true;
            this.lbXCoord.Location = new System.Drawing.Point(32, 155);
            this.lbXCoord.Name = "lbXCoord";
            this.lbXCoord.Size = new System.Drawing.Size(31, 13);
            this.lbXCoord.TabIndex = 40;
            this.lbXCoord.Text = "0000";
            this.lbXCoord.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbXCoord_MouseDoubleClick);
            this.lbXCoord.MouseLeave += new System.EventHandler(this.lbXCoord_MouseLeave);
            this.lbXCoord.MouseHover += new System.EventHandler(this.lbXCoord_MouseHover);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.checkBox1.Location = new System.Drawing.Point(117, 154);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(124, 17);
            this.checkBox1.TabIndex = 42;
            this.checkBox1.Text = "SAVE/GET POS (F8)";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.checkBox1_KeyDown);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label12);
            this.tabPage5.Controls.Add(this.label13);
            this.tabPage5.Controls.Add(this.trackBar2);
            this.tabPage5.Controls.Add(this.trackBar3);
            this.tabPage5.Controls.Add(this.label11);
            this.tabPage5.Controls.Add(this.trackBar1);
            this.tabPage5.Controls.Add(this.imageBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(550, 303);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Lopang";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(3, 3);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(329, 232);
            this.imageBox1.TabIndex = 3;
            this.imageBox1.TabStop = false;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(338, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 23);
            this.label11.TabIndex = 27;
            this.label11.Text = "Treshold Binarie";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(338, 31);
            this.trackBar1.Maximum = 255;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(205, 45);
            this.trackBar1.TabIndex = 26;
            this.trackBar1.Value = 1;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(338, 159);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 23);
            this.label12.TabIndex = 31;
            this.label12.Text = "Erode";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(338, 79);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 23);
            this.label13.TabIndex = 30;
            this.label13.Text = "Dilade";
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(338, 185);
            this.trackBar2.Maximum = 15;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(205, 45);
            this.trackBar2.TabIndex = 29;
            this.trackBar2.Value = 1;
            // 
            // trackBar3
            // 
            this.trackBar3.Location = new System.Drawing.Point(338, 105);
            this.trackBar3.Maximum = 15;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(205, 45);
            this.trackBar3.TabIndex = 28;
            this.trackBar3.Value = 1;
            // 
            // Debugging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 628);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.lbXCoord);
            this.Controls.Add(this.lbYCoord);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonRecalcToBotresolution);
            this.Controls.Add(this.buttonRecalc);
            this.Controls.Add(this.buttonGenerateCode);
            this.Controls.Add(this.trackBarThreadSleep);
            this.Controls.Add(this.labelRefresh);
            this.Controls.Add(this.buttonSelectArea);
            this.Controls.Add(this.btnGetMinimap);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.textBoxHeight);
            this.Controls.Add(this.textBoxWidth);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.Name = "Debugging";
            this.Text = "Debugging";
            this.Shown += new System.EventHandler(this.Debugging_Shown);
            ((System.ComponentModel.ISupportInitialize) (this.trackBarTreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxMask)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarThreadSleep)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarVariant)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarCannySecond)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarCannyFirst)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarErode)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarDilade)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarTreshBinary)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.imageBoxMinimap)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.imageBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBar3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TrackBar trackBar3;
        private Emgu.CV.UI.ImageBox imageBox1;

        private System.Windows.Forms.TabPage tabPage5;

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar trackBarCannyFirst;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TrackBar trackBarCannySecond;

        private System.Windows.Forms.TrackBar trackBarTreshBinary;
        private System.Windows.Forms.TrackBar trackBarDilade;
        private System.Windows.Forms.Label TresholdBinarie;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;

        private System.Windows.Forms.TrackBar trackBarErode;

        private Emgu.CV.UI.ImageBox imageBoxMinimap;

        private System.Windows.Forms.TabPage tabPage4;

        private System.Windows.Forms.TrackBar trackBarVariant;
        private System.Windows.Forms.Label labelVariantShade;

        private System.Windows.Forms.Label labelColorARGB;

        private System.Windows.Forms.PictureBox pictureBox1;

        private System.Windows.Forms.TextBox textBoxTextSearch;

        private System.Windows.Forms.TabPage tabPage3;

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        
        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxMethod;

        private System.Windows.Forms.Button buttonRecalcToBotresolution;

        private System.Windows.Forms.Button buttonRecalc;

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGetMinimap;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonSelectPicture;
        private System.Windows.Forms.Button buttonSelectMask;
        private System.Windows.Forms.PictureBox pictureBoxPicture;
        private System.Windows.Forms.PictureBox pictureBoxMask;
        private System.Windows.Forms.Label labelTreshold;
        private System.Windows.Forms.TrackBar trackBarTreshold;
        private System.Windows.Forms.Button buttonSelectArea;
        public System.Windows.Forms.TextBox textBoxX;
        public System.Windows.Forms.TextBox textBoxY;
        public System.Windows.Forms.TextBox textBoxWidth;
        public System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.Label labelRefresh;
        private System.Windows.Forms.TrackBar trackBarThreadSleep;
        private System.Windows.Forms.CheckBox checkBoxShowAll;
        private System.Windows.Forms.RadioButton radioButtonGetBest;
        private System.Windows.Forms.RadioButton radioButtonGetClosest;
        private System.Windows.Forms.RadioButton radioButtonGetClosestBest;
        private System.Windows.Forms.Button buttonGenerateCode;
        private ScreenColorPicker screenColorPicker1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbYCoord;
        private System.Windows.Forms.Label lbXCoord;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}