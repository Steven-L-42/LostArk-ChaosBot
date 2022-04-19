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
            ((System.ComponentModel.ISupportInitialize) (this.trackBarTreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxMask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarThreadSleep)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(11, 377);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(314, 27);
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
            this.btnGetMinimap.Location = new System.Drawing.Point(14, 281);
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
            this.buttonSelectPicture.Location = new System.Drawing.Point(63, 314);
            this.buttonSelectPicture.Name = "buttonSelectPicture";
            this.buttonSelectPicture.Size = new System.Drawing.Size(262, 27);
            this.buttonSelectPicture.TabIndex = 12;
            this.buttonSelectPicture.Text = "Select Picture";
            this.buttonSelectPicture.UseVisualStyleBackColor = true;
            this.buttonSelectPicture.Click += new System.EventHandler(this.buttonSelectPicture_Click);
            // 
            // buttonSelectMask
            // 
            this.buttonSelectMask.Location = new System.Drawing.Point(63, 347);
            this.buttonSelectMask.Name = "buttonSelectMask";
            this.buttonSelectMask.Size = new System.Drawing.Size(262, 27);
            this.buttonSelectMask.TabIndex = 13;
            this.buttonSelectMask.Text = "Select Mask";
            this.buttonSelectMask.UseVisualStyleBackColor = true;
            this.buttonSelectMask.Click += new System.EventHandler(this.buttonSelectMask_Click);
            // 
            // labelTreshold
            // 
            this.labelTreshold.AutoSize = true;
            this.labelTreshold.Location = new System.Drawing.Point(10, 149);
            this.labelTreshold.Name = "labelTreshold";
            this.labelTreshold.Size = new System.Drawing.Size(72, 13);
            this.labelTreshold.TabIndex = 17;
            this.labelTreshold.Text = "Treshold (0.7)";
            // 
            // trackBarTreshold
            // 
            this.trackBarTreshold.Location = new System.Drawing.Point(116, 143);
            this.trackBarTreshold.Maximum = 100;
            this.trackBarTreshold.Name = "trackBarTreshold";
            this.trackBarTreshold.Size = new System.Drawing.Size(211, 45);
            this.trackBarTreshold.TabIndex = 18;
            this.trackBarTreshold.Value = 70;
            this.trackBarTreshold.ValueChanged += new System.EventHandler(this.trackBarTreshold_Changed);
            // 
            // buttonSelectArea
            // 
            this.buttonSelectArea.Location = new System.Drawing.Point(178, 281);
            this.buttonSelectArea.Name = "buttonSelectArea";
            this.buttonSelectArea.Size = new System.Drawing.Size(147, 27);
            this.buttonSelectArea.TabIndex = 19;
            this.buttonSelectArea.Text = "Select Area";
            this.buttonSelectArea.UseVisualStyleBackColor = true;
            this.buttonSelectArea.Click += new System.EventHandler(this.buttonSelectArea_Click);
            // 
            // pictureBoxMask
            // 
            this.pictureBoxMask.Location = new System.Drawing.Point(14, 347);
            this.pictureBoxMask.Name = "pictureBoxMask";
            this.pictureBoxMask.Size = new System.Drawing.Size(43, 27);
            this.pictureBoxMask.TabIndex = 15;
            this.pictureBoxMask.TabStop = false;
            // 
            // pictureBoxPicture
            // 
            this.pictureBoxPicture.Location = new System.Drawing.Point(14, 314);
            this.pictureBoxPicture.Name = "pictureBoxPicture";
            this.pictureBoxPicture.Size = new System.Drawing.Size(43, 27);
            this.pictureBoxPicture.TabIndex = 14;
            this.pictureBoxPicture.TabStop = false;
            // 
            // labelRefresh
            // 
            this.labelRefresh.AutoSize = true;
            this.labelRefresh.Location = new System.Drawing.Point(10, 175);
            this.labelRefresh.Name = "labelRefresh";
            this.labelRefresh.Size = new System.Drawing.Size(84, 13);
            this.labelRefresh.TabIndex = 20;
            this.labelRefresh.Text = "Refresh (100ms)";
            // 
            // trackBarThreadSleep
            // 
            this.trackBarThreadSleep.Location = new System.Drawing.Point(116, 175);
            this.trackBarThreadSleep.Maximum = 1000;
            this.trackBarThreadSleep.Name = "trackBarThreadSleep";
            this.trackBarThreadSleep.Size = new System.Drawing.Size(211, 45);
            this.trackBarThreadSleep.TabIndex = 21;
            this.trackBarThreadSleep.Value = 100;
            this.trackBarThreadSleep.ValueChanged += new System.EventHandler(this.trackBarThreadSleep_ValueChanged);
            // 
            // checkBoxShowAll
            // 
            this.checkBoxShowAll.AutoSize = true;
            this.checkBoxShowAll.Checked = true;
            this.checkBoxShowAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowAll.Location = new System.Drawing.Point(14, 234);
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
            this.radioButtonGetBest.Location = new System.Drawing.Point(14, 258);
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
            this.radioButtonGetClosest.Location = new System.Drawing.Point(86, 258);
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
            this.radioButtonGetClosestBest.Location = new System.Drawing.Point(171, 258);
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
            this.buttonGenerateCode.Location = new System.Drawing.Point(11, 410);
            this.buttonGenerateCode.Name = "buttonGenerateCode";
            this.buttonGenerateCode.Size = new System.Drawing.Size(314, 27);
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
            this.label1.Location = new System.Drawing.Point(11, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Comparison Method";
            // 
            // comboBoxMethod
            // 
            this.comboBoxMethod.FormattingEnabled = true;
            this.comboBoxMethod.Items.AddRange(new object[] {"TemplateMatchingType.SqdiffNormed", "TemplateMatchingType.Sqdiff", "TemplateMatchingType.Ccoeff", "TemplateMatchingType.CcoeffNormed", "TemplateMatchingType.Ccorr", "TemplateMatchingType.CcorrNormed", "            "});
            this.comboBoxMethod.Location = new System.Drawing.Point(131, 116);
            this.comboBoxMethod.Name = "comboBoxMethod";
            this.comboBoxMethod.Size = new System.Drawing.Size(194, 21);
            this.comboBoxMethod.TabIndex = 31;
            this.comboBoxMethod.SelectedIndexChanged += new System.EventHandler(this.comboBoxMethod_SelectedIndexChanged);
            // 
            // Debugging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 449);
            this.Controls.Add(this.comboBoxMethod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRecalcToBotresolution);
            this.Controls.Add(this.buttonRecalc);
            this.Controls.Add(this.buttonGenerateCode);
            this.Controls.Add(this.radioButtonGetClosestBest);
            this.Controls.Add(this.radioButtonGetClosest);
            this.Controls.Add(this.radioButtonGetBest);
            this.Controls.Add(this.checkBoxShowAll);
            this.Controls.Add(this.trackBarThreadSleep);
            this.Controls.Add(this.labelRefresh);
            this.Controls.Add(this.buttonSelectArea);
            this.Controls.Add(this.btnGetMinimap);
            this.Controls.Add(this.trackBarTreshold);
            this.Controls.Add(this.labelTreshold);
            this.Controls.Add(this.pictureBoxMask);
            this.Controls.Add(this.pictureBoxPicture);
            this.Controls.Add(this.buttonSelectMask);
            this.Controls.Add(this.buttonSelectPicture);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.textBoxHeight);
            this.Controls.Add(this.textBoxWidth);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.button2);
            this.Name = "Debugging";
            this.Text = "Debugging";
            this.Load += new System.EventHandler(this.Debugging_Load);
            ((System.ComponentModel.ISupportInitialize) (this.trackBarTreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxMask)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.trackBarThreadSleep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

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
    }
}