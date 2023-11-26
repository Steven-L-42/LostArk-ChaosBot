namespace PixelAimbot
{
    partial class FrmLogin
    {
    
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.lbClose = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chBoxShowPassword = new ReaLTaiizor.Controls.RibbonCheckBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.chBoxRemember = new ReaLTaiizor.Controls.RibbonCheckBox();
            this.lbVersionOld = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new ReaLTaiizor.Controls.RibbonRadioButton();
            this.radioButton2 = new ReaLTaiizor.Controls.RibbonRadioButton();
            this.radioButton3 = new ReaLTaiizor.Controls.RibbonRadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new PixelAimbot.Classes.Misc.CustomProgressBar();
            this.nightButtonChangeLogs = new ReaLTaiizor.Controls.NightButton();
            this.nightButtonPurchase = new ReaLTaiizor.Controls.NightButton();
            this.nightButtonRegister = new ReaLTaiizor.Controls.NightButton();
            this.lbPreActive = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lbClose
            // 
            this.lbClose.AutoSize = true;
            this.lbClose.BackColor = System.Drawing.Color.Transparent;
            this.lbClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbClose.Location = new System.Drawing.Point(681, 12);
            this.lbClose.Name = "lbClose";
            this.lbClose.Size = new System.Drawing.Size(47, 17);
            this.lbClose.TabIndex = 0;
            this.lbClose.Text = "CLOSE";
            this.lbClose.Click += new System.EventHandler(this.lbClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Orange;
            this.label2.Location = new System.Drawing.Point(169, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Username:";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            // 
            // tbUser
            // 
            this.tbUser.AcceptsTab = true;
            this.tbUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.tbUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbUser.Font = new System.Drawing.Font("MS UI Gothic", 15.75F);
            this.tbUser.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tbUser.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbUser.Location = new System.Drawing.Point(171, 199);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(192, 21);
            this.tbUser.TabIndex = 1;
            this.tbUser.WordWrap = false;
            // 
            // tbPass
            // 
            this.tbPass.AcceptsTab = true;
            this.tbPass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.tbPass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPass.Font = new System.Drawing.Font("MS UI Gothic", 15.75F);
            this.tbPass.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tbPass.Location = new System.Drawing.Point(378, 199);
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new System.Drawing.Size(192, 21);
            this.tbPass.TabIndex = 2;
            this.tbPass.WordWrap = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Orange;
            this.label3.Location = new System.Drawing.Point(376, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            // 
            // chBoxShowPassword
            // 
            this.chBoxShowPassword.BackColor = System.Drawing.Color.Transparent;
            this.chBoxShowPassword.BaseColor = System.Drawing.Color.Transparent;
            this.chBoxShowPassword.CheckBackColorA = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.chBoxShowPassword.CheckBackColorB = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.chBoxShowPassword.CheckBorderColorA = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.chBoxShowPassword.CheckBorderColorB = System.Drawing.Color.Orange;
            this.chBoxShowPassword.Checked = false;
            this.chBoxShowPassword.CheckedColor = System.Drawing.Color.Orange;
            this.chBoxShowPassword.CompositingQualityType = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            this.chBoxShowPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chBoxShowPassword.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
            this.chBoxShowPassword.ForeColor = System.Drawing.Color.Orange;
            this.chBoxShowPassword.Location = new System.Drawing.Point(267, 229);
            this.chBoxShowPassword.Name = "chBoxShowPassword";
            this.chBoxShowPassword.Size = new System.Drawing.Size(122, 14);
            this.chBoxShowPassword.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.chBoxShowPassword.TabIndex = 6;
            this.chBoxShowPassword.Text = "Show Password";
            this.chBoxShowPassword.TextRenderingType = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.chBoxShowPassword.CheckedChanged += new ReaLTaiizor.Controls.RibbonCheckBox.CheckedChangedEventHandler(this.chBoxShowPassword_CheckedChanged);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Location = new System.Drawing.Point(171, 256);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(399, 34);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "LOGIN";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(171, 296);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(399, 25);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnClear.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Orange;
            this.label6.Location = new System.Drawing.Point(279, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(185, 25);
            this.label6.TabIndex = 11;
            this.label6.Text = "ChaosDungeon Bot";
            this.label6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Peru;
            this.label15.Location = new System.Drawing.Point(658, 421);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 17);
            this.label15.TabIndex = 12;
            this.label15.Text = "4.0.0 (free)";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label15.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            // 
            // chBoxRemember
            // 
            this.chBoxRemember.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chBoxRemember.BackColor = System.Drawing.Color.Transparent;
            this.chBoxRemember.BaseColor = System.Drawing.Color.Transparent;
            this.chBoxRemember.CheckBackColorA = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.chBoxRemember.CheckBackColorB = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.chBoxRemember.CheckBorderColorA = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.chBoxRemember.CheckBorderColorB = System.Drawing.Color.Orange;
            this.chBoxRemember.Checked = false;
            this.chBoxRemember.CheckedColor = System.Drawing.Color.Orange;
            this.chBoxRemember.CompositingQualityType = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            this.chBoxRemember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chBoxRemember.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
            this.chBoxRemember.ForeColor = System.Drawing.Color.Orange;
            this.chBoxRemember.Location = new System.Drawing.Point(171, 229);
            this.chBoxRemember.Name = "chBoxRemember";
            this.chBoxRemember.Size = new System.Drawing.Size(93, 14);
            this.chBoxRemember.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.chBoxRemember.TabIndex = 13;
            this.chBoxRemember.Text = "Remember";
            this.chBoxRemember.TextRenderingType = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // lbVersionOld
            // 
            this.lbVersionOld.AutoSize = true;
            this.lbVersionOld.BackColor = System.Drawing.Color.Transparent;
            this.lbVersionOld.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVersionOld.Location = new System.Drawing.Point(284, 351);
            this.lbVersionOld.Name = "lbVersionOld";
            this.lbVersionOld.Size = new System.Drawing.Size(90, 17);
            this.lbVersionOld.TabIndex = 21;
            this.lbVersionOld.Text = "Version: 3.5.7r";
            this.lbVersionOld.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(171, 351);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 17);
            this.label1.TabIndex = 24;
            this.label1.Text = "Version: BETA";
            this.label1.Visible = false;
            // 
            // radioButton1
            // 
            this.radioButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton1.BackColor = System.Drawing.Color.Transparent;
            this.radioButton1.Checked = false;
            this.radioButton1.CheckedBorderColorA = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.radioButton1.CheckedBorderColorB = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.radioButton1.CheckedColor = System.Drawing.Color.Orange;
            this.radioButton1.CircleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.radioButton1.CircleEdgeColor = System.Drawing.Color.Orange;
            this.radioButton1.CompositingQualityType = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            this.radioButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButton1.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
            this.radioButton1.ForeColor = System.Drawing.Color.Orange;
            this.radioButton1.Location = new System.Drawing.Point(171, 330);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(113, 16);
            this.radioButton1.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.radioButton1.TabIndex = 25;
            this.radioButton1.Text = "DEV VERSION";
            this.radioButton1.TextRenderingType = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.radioButton1.Visible = false;
            this.radioButton1.CheckedChanged += new ReaLTaiizor.Controls.RibbonRadioButton.CheckedChangedEventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton2.BackColor = System.Drawing.Color.Transparent;
            this.radioButton2.Checked = false;
            this.radioButton2.CheckedBorderColorA = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.radioButton2.CheckedBorderColorB = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.radioButton2.CheckedColor = System.Drawing.Color.Orange;
            this.radioButton2.CircleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.radioButton2.CircleEdgeColor = System.Drawing.Color.Orange;
            this.radioButton2.CompositingQualityType = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            this.radioButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButton2.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
            this.radioButton2.ForeColor = System.Drawing.Color.Orange;
            this.radioButton2.Location = new System.Drawing.Point(284, 330);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(113, 16);
            this.radioButton2.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.radioButton2.TabIndex = 26;
            this.radioButton2.Text = "OLD VERSION";
            this.radioButton2.TextRenderingType = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.radioButton2.Visible = false;
            this.radioButton2.CheckedChanged += new ReaLTaiizor.Controls.RibbonRadioButton.CheckedChangedEventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton3.BackColor = System.Drawing.Color.Transparent;
            this.radioButton3.Checked = false;
            this.radioButton3.CheckedBorderColorA = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.radioButton3.CheckedBorderColorB = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.radioButton3.CheckedColor = System.Drawing.Color.Orange;
            this.radioButton3.CircleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.radioButton3.CircleEdgeColor = System.Drawing.Color.Orange;
            this.radioButton3.CompositingQualityType = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            this.radioButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButton3.Font = new System.Drawing.Font("Nirmala UI", 9.75F);
            this.radioButton3.ForeColor = System.Drawing.Color.Orange;
            this.radioButton3.Location = new System.Drawing.Point(397, 330);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(122, 16);
            this.radioButton3.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.radioButton3.TabIndex = 27;
            this.radioButton3.Text = "STABLE VERSION";
            this.radioButton3.TextRenderingType = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.radioButton3.Visible = false;
            this.radioButton3.CheckedChanged += new ReaLTaiizor.Controls.RibbonRadioButton.CheckedChangedEventHandler(this.radioButton3_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(397, 351);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 17);
            this.label5.TabIndex = 28;
            this.label5.Text = "Version: 3.5.9r";
            this.label5.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.Orange;
            this.progressBar1.CustomText = "Updating...";
            this.progressBar1.DisplayStyle = PixelAimbot.Classes.Misc.ProgressBarDisplayText.CustomText;
            this.progressBar1.ForeColor = System.Drawing.Color.Orange;
            this.progressBar1.Location = new System.Drawing.Point(171, 327);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(399, 40);
            this.progressBar1.TabIndex = 18;
            this.progressBar1.Visible = false;
            // 
            // nightButtonChangeLogs
            // 
            this.nightButtonChangeLogs.BackColor = System.Drawing.Color.Transparent;
            this.nightButtonChangeLogs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nightButtonChangeLogs.DialogResult = System.Windows.Forms.DialogResult.None;
            this.nightButtonChangeLogs.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nightButtonChangeLogs.ForeColor = System.Drawing.Color.Orange;
            this.nightButtonChangeLogs.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(242)))), ((int)(((byte)(93)))), ((int)(((byte)(89)))));
            this.nightButtonChangeLogs.HoverForeColor = System.Drawing.Color.Black;
            this.nightButtonChangeLogs.InterpolationType = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.nightButtonChangeLogs.Location = new System.Drawing.Point(586, 167);
            this.nightButtonChangeLogs.MinimumSize = new System.Drawing.Size(144, 37);
            this.nightButtonChangeLogs.Name = "nightButtonChangeLogs";
            this.nightButtonChangeLogs.NormalBackColor = System.Drawing.Color.Orange;
            this.nightButtonChangeLogs.PixelOffsetType = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            this.nightButtonChangeLogs.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(242)))), ((int)(((byte)(93)))), ((int)(((byte)(89)))));
            this.nightButtonChangeLogs.PressedForeColor = System.Drawing.Color.White;
            this.nightButtonChangeLogs.Radius = 5;
            this.nightButtonChangeLogs.Size = new System.Drawing.Size(144, 37);
            this.nightButtonChangeLogs.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.nightButtonChangeLogs.TabIndex = 29;
            this.nightButtonChangeLogs.Text = "Changelogs";
            this.toolTip1.SetToolTip(this.nightButtonChangeLogs, "Server is Offline!");
            this.nightButtonChangeLogs.Click += new System.EventHandler(this.nightButtonChangeLogs_Click);
            // 
            // nightButtonPurchase
            // 
            this.nightButtonPurchase.BackColor = System.Drawing.Color.Transparent;
            this.nightButtonPurchase.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nightButtonPurchase.DialogResult = System.Windows.Forms.DialogResult.None;
            this.nightButtonPurchase.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nightButtonPurchase.ForeColor = System.Drawing.Color.Orange;
            this.nightButtonPurchase.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(242)))), ((int)(((byte)(93)))), ((int)(((byte)(89)))));
            this.nightButtonPurchase.HoverForeColor = System.Drawing.Color.Black;
            this.nightButtonPurchase.InterpolationType = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.nightButtonPurchase.Location = new System.Drawing.Point(586, 206);
            this.nightButtonPurchase.MinimumSize = new System.Drawing.Size(144, 37);
            this.nightButtonPurchase.Name = "nightButtonPurchase";
            this.nightButtonPurchase.NormalBackColor = System.Drawing.Color.Orange;
            this.nightButtonPurchase.PixelOffsetType = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            this.nightButtonPurchase.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(242)))), ((int)(((byte)(93)))), ((int)(((byte)(89)))));
            this.nightButtonPurchase.PressedForeColor = System.Drawing.Color.White;
            this.nightButtonPurchase.Radius = 5;
            this.nightButtonPurchase.Size = new System.Drawing.Size(144, 37);
            this.nightButtonPurchase.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.nightButtonPurchase.TabIndex = 30;
            this.nightButtonPurchase.Text = "Purchase";
            this.toolTip1.SetToolTip(this.nightButtonPurchase, "Server is Offline!");
            this.nightButtonPurchase.Click += new System.EventHandler(this.nightButtonPurchase_Click);
            // 
            // nightButtonRegister
            // 
            this.nightButtonRegister.BackColor = System.Drawing.Color.Transparent;
            this.nightButtonRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nightButtonRegister.DialogResult = System.Windows.Forms.DialogResult.None;
            this.nightButtonRegister.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nightButtonRegister.ForeColor = System.Drawing.Color.Orange;
            this.nightButtonRegister.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(242)))), ((int)(((byte)(93)))), ((int)(((byte)(89)))));
            this.nightButtonRegister.HoverForeColor = System.Drawing.Color.Black;
            this.nightButtonRegister.InterpolationType = System.Drawing.Drawing2D.InterpolationMode.Low;
            this.nightButtonRegister.Location = new System.Drawing.Point(586, 245);
            this.nightButtonRegister.MinimumSize = new System.Drawing.Size(144, 37);
            this.nightButtonRegister.Name = "nightButtonRegister";
            this.nightButtonRegister.NormalBackColor = System.Drawing.Color.Orange;
            this.nightButtonRegister.PixelOffsetType = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            this.nightButtonRegister.PressedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(242)))), ((int)(((byte)(93)))), ((int)(((byte)(89)))));
            this.nightButtonRegister.PressedForeColor = System.Drawing.Color.White;
            this.nightButtonRegister.Radius = 5;
            this.nightButtonRegister.Size = new System.Drawing.Size(144, 37);
            this.nightButtonRegister.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.nightButtonRegister.TabIndex = 31;
            this.nightButtonRegister.Text = "Register Trial";
            this.toolTip1.SetToolTip(this.nightButtonRegister, "Server is Offline!");
            this.nightButtonRegister.Click += new System.EventHandler(this.nightButtonRegister_Click);
            // 
            // lbPreActive
            // 
            this.lbPreActive.AutoSize = true;
            this.lbPreActive.BackColor = System.Drawing.Color.Transparent;
            this.lbPreActive.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPreActive.ForeColor = System.Drawing.Color.Orange;
            this.lbPreActive.Location = new System.Drawing.Point(221, 330);
            this.lbPreActive.Name = "lbPreActive";
            this.lbPreActive.Size = new System.Drawing.Size(298, 32);
            this.lbPreActive.TabIndex = 32;
            this.lbPreActive.Text = "Account is Pre-Activated";
            this.lbPreActive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Peru;
            this.label4.Location = new System.Drawing.Point(12, 417);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(174, 21);
            this.label4.TabIndex = 33;
            this.label4.Text = "Discord: ShiiikK#1048";
            this.label4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Peru;
            this.label7.Location = new System.Drawing.Point(267, 370);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(205, 21);
            this.label7.TabIndex = 34;
            this.label7.Text = "The bot remains free for life.";
            this.label7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(741, 447);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbPreActive);
            this.Controls.Add(this.nightButtonRegister);
            this.Controls.Add(this.nightButtonPurchase);
            this.Controls.Add(this.nightButtonChangeLogs);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.chBoxShowPassword);
            this.Controls.Add(this.tbPass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbVersionOld);
            this.Controls.Add(this.chBoxRemember);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbClose);
            this.Controls.Add(this.label5);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLogin_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ToolTip toolTip1;

        private System.Windows.Forms.Label label7;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Label lbPreActive;

        private ReaLTaiizor.Controls.NightButton nightButtonRegister;

        private ReaLTaiizor.Controls.NightButton nightButtonPurchase;

        private ReaLTaiizor.Controls.NightButton nightButtonChangeLogs;

        private ReaLTaiizor.Controls.RibbonRadioButton radioButton2;
        private ReaLTaiizor.Controls.RibbonRadioButton radioButton3;

        private ReaLTaiizor.Controls.RibbonRadioButton radioButton1;

        #endregion

        private System.Windows.Forms.Label lbClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private ReaLTaiizor.Controls.RibbonCheckBox chBoxShowPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label15;
        private ReaLTaiizor.Controls.RibbonCheckBox chBoxRemember;
        private PixelAimbot.Classes.Misc.CustomProgressBar progressBar1;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Label lbVersionOld;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
    }
}