namespace PixelAimbot
{
    partial class GatheringBot
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GatheringBot));
            this.lbClose = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnInstructions = new System.Windows.Forms.Button();
            this.chBoxChannelSwap = new System.Windows.Forms.CheckBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.txtLOGOUT = new System.Windows.Forms.TextBox();
            this.chBoxLOGOUT = new System.Windows.Forms.CheckBox();
            this.label28 = new System.Windows.Forms.Label();
            this.chBoxAutoRepair = new System.Windows.Forms.CheckBox();
            this.label27 = new System.Windows.Forms.Label();
            this.chBoxAutoBuff = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label49 = new System.Windows.Forms.Label();
            this.chBoxDoubleF = new System.Windows.Forms.CheckBox();
            this.chBoxDoubleD = new System.Windows.Forms.CheckBox();
            this.chBoxDoubleS = new System.Windows.Forms.CheckBox();
            this.chBoxDoubleA = new System.Windows.Forms.CheckBox();
            this.chBoxDoubleR = new System.Windows.Forms.CheckBox();
            this.chBoxDoubleE = new System.Windows.Forms.CheckBox();
            this.chBoxDoubleW = new System.Windows.Forms.CheckBox();
            this.chBoxDoubleQ = new System.Windows.Forms.CheckBox();
            this.lbPF = new System.Windows.Forms.Label();
            this.lbPD = new System.Windows.Forms.Label();
            this.lbPS = new System.Windows.Forms.Label();
            this.lbPA = new System.Windows.Forms.Label();
            this.lbPR = new System.Windows.Forms.Label();
            this.lbPE = new System.Windows.Forms.Label();
            this.lbPW = new System.Windows.Forms.Label();
            this.lbPQ = new System.Windows.Forms.Label();
            this.txPA = new System.Windows.Forms.TextBox();
            this.txPS = new System.Windows.Forms.TextBox();
            this.txPD = new System.Windows.Forms.TextBox();
            this.txPF = new System.Windows.Forms.TextBox();
            this.txPW = new System.Windows.Forms.TextBox();
            this.txPQ = new System.Windows.Forms.TextBox();
            this.txPE = new System.Windows.Forms.TextBox();
            this.txPR = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.STARTEXIT = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonSetup = new System.Windows.Forms.Button();
            this.buttonSelectArea = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.SETTINGS = new System.Windows.Forms.TabPage();
            this.groupBoxTelegram = new System.Windows.Forms.GroupBox();
            this.buttonConnectTelegram = new System.Windows.Forms.Button();
            this.buttonTestTelegram = new System.Windows.Forms.Button();
            this.labelTelegramState = new System.Windows.Forms.Label();
            this.labelApiTelegram = new System.Windows.Forms.Label();
            this.textBoxTelegramAPI = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.labelSwap = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.STARTEXIT.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SETTINGS.SuspendLayout();
            this.groupBoxTelegram.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbClose
            // 
            this.lbClose.AutoSize = true;
            this.lbClose.BackColor = System.Drawing.Color.Transparent;
            this.lbClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbClose.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lbClose.ForeColor = System.Drawing.Color.White;
            this.lbClose.Location = new System.Drawing.Point(381, 9);
            this.lbClose.Name = "lbClose";
            this.lbClose.Size = new System.Drawing.Size(57, 21);
            this.lbClose.TabIndex = 3;
            this.lbClose.Text = "CLOSE";
            this.lbClose.Click += new System.EventHandler(this.lbClose_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbStatus.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lbStatus.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbStatus.Location = new System.Drawing.Point(12, 210);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(62, 21);
            this.lbStatus.TabIndex = 8;
            this.lbStatus.Text = "READY";
            this.lbStatus.TextChanged += new System.EventHandler(this.lbStatus_TextChanged);
            this.lbStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FishBot_MouseDown);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label15.ForeColor = System.Drawing.Color.Peru;
            this.label15.Location = new System.Drawing.Point(396, 210);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 17);
            this.label15.TabIndex = 11;
            this.label15.Text = "version";
            this.label15.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FishBot_MouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnInstructions);
            this.groupBox1.Controls.Add(this.chBoxChannelSwap);
            this.groupBox1.Controls.Add(this.label34);
            this.groupBox1.Controls.Add(this.label33);
            this.groupBox1.Controls.Add(this.txtLOGOUT);
            this.groupBox1.Controls.Add(this.chBoxLOGOUT);
            this.groupBox1.Controls.Add(this.label28);
            this.groupBox1.Controls.Add(this.chBoxAutoRepair);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.chBoxAutoBuff);
            this.groupBox1.Font = new System.Drawing.Font("Nirmala UI", 12F, ((System.Drawing.FontStyle) ((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.groupBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Location = new System.Drawing.Point(172, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 139);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Action";
            this.toolTip1.SetToolTip(this.groupBox1, "To activate the functions, you have \r\nto Click on the Checkboxes!");
            // 
            // btnInstructions
            // 
            this.btnInstructions.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.btnInstructions.FlatAppearance.BorderSize = 0;
            this.btnInstructions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstructions.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnInstructions.ForeColor = System.Drawing.Color.Peru;
            this.btnInstructions.Location = new System.Drawing.Point(6, 111);
            this.btnInstructions.Name = "btnInstructions";
            this.btnInstructions.Size = new System.Drawing.Size(59, 22);
            this.btnInstructions.TabIndex = 62;
            this.btnInstructions.Text = "GUIDE";
            this.toolTip1.SetToolTip(this.btnInstructions, "Thats resets everything except gamesettings.");
            this.btnInstructions.UseVisualStyleBackColor = false;
            this.btnInstructions.Click += new System.EventHandler(this.btnInstructions_Click_1);
            // 
            // chBoxChannelSwap
            // 
            this.chBoxChannelSwap.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (28)))), ((int) (((byte) (28)))), ((int) (((byte) (28)))));
            this.chBoxChannelSwap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chBoxChannelSwap.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chBoxChannelSwap.Cursor = System.Windows.Forms.Cursors.Help;
            this.chBoxChannelSwap.FlatAppearance.BorderSize = 0;
            this.chBoxChannelSwap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chBoxChannelSwap.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.chBoxChannelSwap.Location = new System.Drawing.Point(4, 89);
            this.chBoxChannelSwap.Name = "chBoxChannelSwap";
            this.chBoxChannelSwap.Size = new System.Drawing.Size(154, 22);
            this.chBoxChannelSwap.TabIndex = 61;
            this.chBoxChannelSwap.Text = "Channel-Swap";
            this.toolTip1.SetToolTip(this.chBoxChannelSwap, "Automaticly Channelswap after 15, 30, 45 Attempts");
            this.chBoxChannelSwap.UseMnemonic = false;
            this.chBoxChannelSwap.UseVisualStyleBackColor = false;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Nirmala UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label34.ForeColor = System.Drawing.Color.Coral;
            this.label34.Location = new System.Drawing.Point(68, -2);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(171, 12);
            this.label34.TabIndex = 48;
            this.label34.Text = "Hover over text for more information!";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Cursor = System.Windows.Forms.Cursors.Help;
            this.label33.Font = new System.Drawing.Font("Nirmala UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label33.Location = new System.Drawing.Point(86, 96);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(0, 12);
            this.label33.TabIndex = 47;
            this.label33.Visible = false;
            // 
            // txtLOGOUT
            // 
            this.txtLOGOUT.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.txtLOGOUT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLOGOUT.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txtLOGOUT.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtLOGOUT.Location = new System.Drawing.Point(166, 64);
            this.txtLOGOUT.Name = "txtLOGOUT";
            this.txtLOGOUT.ReadOnly = true;
            this.txtLOGOUT.Size = new System.Drawing.Size(35, 22);
            this.txtLOGOUT.TabIndex = 45;
            this.txtLOGOUT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLOGOUT.WordWrap = false;
            this.txtLOGOUT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckIsDigit);
            // 
            // chBoxLOGOUT
            // 
            this.chBoxLOGOUT.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (28)))), ((int) (((byte) (28)))), ((int) (((byte) (28)))));
            this.chBoxLOGOUT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chBoxLOGOUT.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chBoxLOGOUT.Cursor = System.Windows.Forms.Cursors.Help;
            this.chBoxLOGOUT.FlatAppearance.BorderSize = 0;
            this.chBoxLOGOUT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chBoxLOGOUT.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.chBoxLOGOUT.Location = new System.Drawing.Point(4, 65);
            this.chBoxLOGOUT.Name = "chBoxLOGOUT";
            this.chBoxLOGOUT.Size = new System.Drawing.Size(154, 22);
            this.chBoxLOGOUT.TabIndex = 44;
            this.chBoxLOGOUT.Text = "Auto-Logout";
            this.toolTip1.SetToolTip(this.chBoxLOGOUT, "adjust your time from when to activate Auto-Logout, \r\nspecify in minutes. Do not " + "enter the same time as for Auto-Repair!\r\nLogout means = character selection.\r\n");
            this.chBoxLOGOUT.UseMnemonic = false;
            this.chBoxLOGOUT.UseVisualStyleBackColor = false;
            this.chBoxLOGOUT.CheckedChanged += new System.EventHandler(this.chBoxLOGOUT_CheckedChanged);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label28.Location = new System.Drawing.Point(204, 67);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(32, 17);
            this.label28.TabIndex = 38;
            this.label28.Text = "min.";
            // 
            // chBoxAutoRepair
            // 
            this.chBoxAutoRepair.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (28)))), ((int) (((byte) (28)))), ((int) (((byte) (28)))));
            this.chBoxAutoRepair.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chBoxAutoRepair.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chBoxAutoRepair.Cursor = System.Windows.Forms.Cursors.Help;
            this.chBoxAutoRepair.FlatAppearance.BorderSize = 0;
            this.chBoxAutoRepair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chBoxAutoRepair.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.chBoxAutoRepair.Location = new System.Drawing.Point(4, 40);
            this.chBoxAutoRepair.Name = "chBoxAutoRepair";
            this.chBoxAutoRepair.Size = new System.Drawing.Size(154, 24);
            this.chBoxAutoRepair.TabIndex = 36;
            this.chBoxAutoRepair.Text = "Auto-Repair";
            this.toolTip1.SetToolTip(this.chBoxAutoRepair, "Automaticly repair Fishingrod");
            this.chBoxAutoRepair.UseMnemonic = false;
            this.chBoxAutoRepair.UseVisualStyleBackColor = false;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label27.ForeColor = System.Drawing.Color.Peru;
            this.label27.Location = new System.Drawing.Point(162, 20);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(73, 15);
            this.label27.TabIndex = 35;
            this.label27.Text = "Fishing Buff";
            // 
            // chBoxAutoBuff
            // 
            this.chBoxAutoBuff.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (28)))), ((int) (((byte) (28)))), ((int) (((byte) (28)))));
            this.chBoxAutoBuff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chBoxAutoBuff.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chBoxAutoBuff.Cursor = System.Windows.Forms.Cursors.Help;
            this.chBoxAutoBuff.FlatAppearance.BorderSize = 0;
            this.chBoxAutoBuff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chBoxAutoBuff.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.chBoxAutoBuff.Location = new System.Drawing.Point(4, 17);
            this.chBoxAutoBuff.Name = "chBoxAutoBuff";
            this.chBoxAutoBuff.Size = new System.Drawing.Size(154, 22);
            this.chBoxAutoBuff.TabIndex = 34;
            this.chBoxAutoBuff.Text = "Auto Buff";
            this.chBoxAutoBuff.UseMnemonic = false;
            this.chBoxAutoBuff.UseVisualStyleBackColor = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(124, 57);
            this.groupBox6.TabIndex = 41;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Mouse";
            this.toolTip1.SetToolTip(this.groupBox6, "Only change if your ingame settings \r\nare different than those given here!");
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label17.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label17.Location = new System.Drawing.Point(14, 22);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(95, 20);
            this.label17.TabIndex = 21;
            this.label17.Text = "Walk = LEFT";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label49);
            this.groupBox7.Controls.Add(this.chBoxDoubleF);
            this.groupBox7.Controls.Add(this.chBoxDoubleD);
            this.groupBox7.Controls.Add(this.chBoxDoubleS);
            this.groupBox7.Controls.Add(this.chBoxDoubleA);
            this.groupBox7.Controls.Add(this.chBoxDoubleR);
            this.groupBox7.Controls.Add(this.chBoxDoubleE);
            this.groupBox7.Controls.Add(this.chBoxDoubleW);
            this.groupBox7.Controls.Add(this.chBoxDoubleQ);
            this.groupBox7.Controls.Add(this.lbPF);
            this.groupBox7.Controls.Add(this.lbPD);
            this.groupBox7.Controls.Add(this.lbPS);
            this.groupBox7.Controls.Add(this.lbPA);
            this.groupBox7.Controls.Add(this.lbPR);
            this.groupBox7.Controls.Add(this.lbPE);
            this.groupBox7.Controls.Add(this.lbPW);
            this.groupBox7.Controls.Add(this.lbPQ);
            this.groupBox7.Controls.Add(this.txPA);
            this.groupBox7.Controls.Add(this.txPS);
            this.groupBox7.Controls.Add(this.txPD);
            this.groupBox7.Controls.Add(this.txPF);
            this.groupBox7.Controls.Add(this.txPW);
            this.groupBox7.Controls.Add(this.txPQ);
            this.groupBox7.Controls.Add(this.txPE);
            this.groupBox7.Controls.Add(this.txPR);
            this.groupBox7.Font = new System.Drawing.Font("Nirmala UI", 9.75F, ((System.Drawing.FontStyle) ((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.groupBox7.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox7.Location = new System.Drawing.Point(137, 1);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(270, 119);
            this.groupBox7.TabIndex = 44;
            this.groupBox7.TabStop = false;
            this.groupBox7.Tag = "5";
            this.groupBox7.Text = "Rotation";
            this.toolTip1.SetToolTip(this.groupBox7, "Only change if your ingame settings \r\nare different than those given here!");
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label49.ForeColor = System.Drawing.Color.Coral;
            this.label49.Location = new System.Drawing.Point(32, 107);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(207, 15);
            this.label49.TabIndex = 50;
            this.label49.Text = " Activate Checkboxes = DoubleClick";
            // 
            // chBoxDoubleF
            // 
            this.chBoxDoubleF.AutoSize = true;
            this.chBoxDoubleF.Location = new System.Drawing.Point(209, 86);
            this.chBoxDoubleF.Name = "chBoxDoubleF";
            this.chBoxDoubleF.Size = new System.Drawing.Size(15, 14);
            this.chBoxDoubleF.TabIndex = 47;
            this.chBoxDoubleF.UseVisualStyleBackColor = true;
            // 
            // chBoxDoubleD
            // 
            this.chBoxDoubleD.AutoSize = true;
            this.chBoxDoubleD.Location = new System.Drawing.Point(147, 86);
            this.chBoxDoubleD.Name = "chBoxDoubleD";
            this.chBoxDoubleD.Size = new System.Drawing.Size(15, 14);
            this.chBoxDoubleD.TabIndex = 46;
            this.chBoxDoubleD.UseVisualStyleBackColor = true;
            // 
            // chBoxDoubleS
            // 
            this.chBoxDoubleS.AutoSize = true;
            this.chBoxDoubleS.Location = new System.Drawing.Point(85, 86);
            this.chBoxDoubleS.Name = "chBoxDoubleS";
            this.chBoxDoubleS.Size = new System.Drawing.Size(15, 14);
            this.chBoxDoubleS.TabIndex = 45;
            this.chBoxDoubleS.UseVisualStyleBackColor = true;
            // 
            // chBoxDoubleA
            // 
            this.chBoxDoubleA.AutoSize = true;
            this.chBoxDoubleA.Location = new System.Drawing.Point(23, 86);
            this.chBoxDoubleA.Name = "chBoxDoubleA";
            this.chBoxDoubleA.Size = new System.Drawing.Size(15, 14);
            this.chBoxDoubleA.TabIndex = 44;
            this.chBoxDoubleA.UseVisualStyleBackColor = true;
            // 
            // chBoxDoubleR
            // 
            this.chBoxDoubleR.AutoSize = true;
            this.chBoxDoubleR.Location = new System.Drawing.Point(209, 41);
            this.chBoxDoubleR.Name = "chBoxDoubleR";
            this.chBoxDoubleR.Size = new System.Drawing.Size(15, 14);
            this.chBoxDoubleR.TabIndex = 43;
            this.chBoxDoubleR.UseVisualStyleBackColor = true;
            // 
            // chBoxDoubleE
            // 
            this.chBoxDoubleE.AutoSize = true;
            this.chBoxDoubleE.Location = new System.Drawing.Point(147, 41);
            this.chBoxDoubleE.Name = "chBoxDoubleE";
            this.chBoxDoubleE.Size = new System.Drawing.Size(15, 14);
            this.chBoxDoubleE.TabIndex = 42;
            this.chBoxDoubleE.UseVisualStyleBackColor = true;
            // 
            // chBoxDoubleW
            // 
            this.chBoxDoubleW.AutoSize = true;
            this.chBoxDoubleW.Location = new System.Drawing.Point(84, 42);
            this.chBoxDoubleW.Name = "chBoxDoubleW";
            this.chBoxDoubleW.Size = new System.Drawing.Size(15, 14);
            this.chBoxDoubleW.TabIndex = 41;
            this.chBoxDoubleW.UseVisualStyleBackColor = true;
            // 
            // chBoxDoubleQ
            // 
            this.chBoxDoubleQ.AutoSize = true;
            this.chBoxDoubleQ.Location = new System.Drawing.Point(23, 42);
            this.chBoxDoubleQ.Name = "chBoxDoubleQ";
            this.chBoxDoubleQ.Size = new System.Drawing.Size(15, 14);
            this.chBoxDoubleQ.TabIndex = 40;
            this.chBoxDoubleQ.UseVisualStyleBackColor = true;
            // 
            // lbPF
            // 
            this.lbPF.AutoSize = true;
            this.lbPF.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lbPF.ForeColor = System.Drawing.Color.Orange;
            this.lbPF.Location = new System.Drawing.Point(236, 61);
            this.lbPF.Name = "lbPF";
            this.lbPF.Size = new System.Drawing.Size(15, 17);
            this.lbPF.TabIndex = 39;
            this.lbPF.Text = "F";
            this.lbPF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPD
            // 
            this.lbPD.AutoSize = true;
            this.lbPD.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lbPD.ForeColor = System.Drawing.Color.Orange;
            this.lbPD.Location = new System.Drawing.Point(172, 61);
            this.lbPD.Name = "lbPD";
            this.lbPD.Size = new System.Drawing.Size(18, 17);
            this.lbPD.TabIndex = 38;
            this.lbPD.Text = "D";
            this.lbPD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPS
            // 
            this.lbPS.AutoSize = true;
            this.lbPS.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lbPS.ForeColor = System.Drawing.Color.Orange;
            this.lbPS.Location = new System.Drawing.Point(112, 61);
            this.lbPS.Name = "lbPS";
            this.lbPS.Size = new System.Drawing.Size(15, 17);
            this.lbPS.TabIndex = 37;
            this.lbPS.Text = "S";
            this.lbPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPA
            // 
            this.lbPA.AutoSize = true;
            this.lbPA.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lbPA.ForeColor = System.Drawing.Color.Orange;
            this.lbPA.Location = new System.Drawing.Point(50, 61);
            this.lbPA.Name = "lbPA";
            this.lbPA.Size = new System.Drawing.Size(17, 17);
            this.lbPA.TabIndex = 36;
            this.lbPA.Text = "A";
            this.lbPA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPR
            // 
            this.lbPR.AutoSize = true;
            this.lbPR.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lbPR.ForeColor = System.Drawing.Color.Orange;
            this.lbPR.Location = new System.Drawing.Point(235, 16);
            this.lbPR.Name = "lbPR";
            this.lbPR.Size = new System.Drawing.Size(16, 17);
            this.lbPR.TabIndex = 35;
            this.lbPR.Text = "R";
            this.lbPR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPE
            // 
            this.lbPE.AutoSize = true;
            this.lbPE.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lbPE.ForeColor = System.Drawing.Color.Orange;
            this.lbPE.Location = new System.Drawing.Point(173, 16);
            this.lbPE.Name = "lbPE";
            this.lbPE.Size = new System.Drawing.Size(15, 17);
            this.lbPE.TabIndex = 34;
            this.lbPE.Text = "E";
            this.lbPE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPW
            // 
            this.lbPW.AutoSize = true;
            this.lbPW.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lbPW.ForeColor = System.Drawing.Color.Orange;
            this.lbPW.Location = new System.Drawing.Point(109, 16);
            this.lbPW.Name = "lbPW";
            this.lbPW.Size = new System.Drawing.Size(21, 17);
            this.lbPW.TabIndex = 33;
            this.lbPW.Text = "W";
            this.lbPW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPQ
            // 
            this.lbPQ.AutoSize = true;
            this.lbPQ.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lbPQ.ForeColor = System.Drawing.Color.Orange;
            this.lbPQ.Location = new System.Drawing.Point(50, 14);
            this.lbPQ.Name = "lbPQ";
            this.lbPQ.Size = new System.Drawing.Size(18, 17);
            this.lbPQ.TabIndex = 32;
            this.lbPQ.Text = "Q";
            this.lbPQ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txPA
            // 
            this.txPA.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.txPA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txPA.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txPA.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txPA.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txPA.Location = new System.Drawing.Point(41, 81);
            this.txPA.Name = "txPA";
            this.txPA.Size = new System.Drawing.Size(32, 22);
            this.txPA.TabIndex = 28;
            this.txPA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txPA.WordWrap = false;
            this.txPA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckIsDigit);
            // 
            // txPS
            // 
            this.txPS.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.txPS.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txPS.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txPS.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txPS.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txPS.Location = new System.Drawing.Point(103, 81);
            this.txPS.Name = "txPS";
            this.txPS.Size = new System.Drawing.Size(32, 22);
            this.txPS.TabIndex = 29;
            this.txPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txPS.WordWrap = false;
            this.txPS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckIsDigit);
            // 
            // txPD
            // 
            this.txPD.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.txPD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txPD.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txPD.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txPD.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txPD.Location = new System.Drawing.Point(165, 81);
            this.txPD.Name = "txPD";
            this.txPD.Size = new System.Drawing.Size(32, 22);
            this.txPD.TabIndex = 30;
            this.txPD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txPD.WordWrap = false;
            this.txPD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckIsDigit);
            // 
            // txPF
            // 
            this.txPF.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.txPF.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txPF.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txPF.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txPF.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txPF.Location = new System.Drawing.Point(227, 81);
            this.txPF.Name = "txPF";
            this.txPF.Size = new System.Drawing.Size(32, 22);
            this.txPF.TabIndex = 31;
            this.txPF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txPF.WordWrap = false;
            this.txPF.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckIsDigit);
            // 
            // txPW
            // 
            this.txPW.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.txPW.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txPW.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txPW.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txPW.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txPW.Location = new System.Drawing.Point(102, 37);
            this.txPW.Name = "txPW";
            this.txPW.Size = new System.Drawing.Size(32, 22);
            this.txPW.TabIndex = 24;
            this.txPW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txPW.WordWrap = false;
            this.txPW.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckIsDigit);
            // 
            // txPQ
            // 
            this.txPQ.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.txPQ.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txPQ.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txPQ.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txPQ.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txPQ.Location = new System.Drawing.Point(41, 37);
            this.txPQ.Name = "txPQ";
            this.txPQ.Size = new System.Drawing.Size(32, 22);
            this.txPQ.TabIndex = 25;
            this.txPQ.Tag = "";
            this.txPQ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txPQ.WordWrap = false;
            this.txPQ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckIsDigit);
            // 
            // txPE
            // 
            this.txPE.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.txPE.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txPE.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txPE.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txPE.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txPE.Location = new System.Drawing.Point(165, 36);
            this.txPE.Name = "txPE";
            this.txPE.Size = new System.Drawing.Size(32, 22);
            this.txPE.TabIndex = 26;
            this.txPE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txPE.WordWrap = false;
            this.txPE.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckIsDigit);
            // 
            // txPR
            // 
            this.txPR.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.txPR.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txPR.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txPR.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.txPR.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txPR.Location = new System.Drawing.Point(227, 36);
            this.txPR.Name = "txPR";
            this.txPR.Size = new System.Drawing.Size(32, 22);
            this.txPR.TabIndex = 27;
            this.txPR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txPR.WordWrap = false;
            this.txPR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckIsDigit);
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Controls.Add(this.STARTEXIT);
            this.tabControl1.Controls.Add(this.SETTINGS);
            this.tabControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabControl1.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(240, 16);
            this.tabControl1.Location = new System.Drawing.Point(12, 33);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(3, 3);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(426, 174);
            this.tabControl1.TabIndex = 9;
            // 
            // STARTEXIT
            // 
            this.STARTEXIT.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (28)))), ((int) (((byte) (28)))), ((int) (((byte) (28)))));
            this.STARTEXIT.Controls.Add(this.groupBox1);
            this.STARTEXIT.Controls.Add(this.groupBox2);
            this.STARTEXIT.Location = new System.Drawing.Point(4, 20);
            this.STARTEXIT.Name = "STARTEXIT";
            this.STARTEXIT.Padding = new System.Windows.Forms.Padding(3);
            this.STARTEXIT.Size = new System.Drawing.Size(418, 150);
            this.STARTEXIT.TabIndex = 0;
            this.STARTEXIT.Text = "Fishing";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonSetup);
            this.groupBox2.Controls.Add(this.buttonSelectArea);
            this.groupBox2.Controls.Add(this.btnPause);
            this.groupBox2.Controls.Add(this.btnStart);
            this.groupBox2.Font = new System.Drawing.Font("Nirmala UI", 12F, ((System.Drawing.FontStyle) ((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.groupBox2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Location = new System.Drawing.Point(6, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(160, 139);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Main";
            // 
            // buttonSetup
            // 
            this.buttonSetup.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.buttonSetup.FlatAppearance.BorderSize = 0;
            this.buttonSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetup.Font = new System.Drawing.Font("Nirmala UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.buttonSetup.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonSetup.Location = new System.Drawing.Point(10, 98);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(144, 30);
            this.buttonSetup.TabIndex = 12;
            this.buttonSetup.Text = "Setup Fishing Bot";
            this.buttonSetup.UseVisualStyleBackColor = false;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // buttonSelectArea
            // 
            this.buttonSelectArea.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.buttonSelectArea.FlatAppearance.BorderSize = 0;
            this.buttonSelectArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectArea.Font = new System.Drawing.Font("Nirmala UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.buttonSelectArea.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonSelectArea.Location = new System.Drawing.Point(9, 62);
            this.buttonSelectArea.Name = "buttonSelectArea";
            this.buttonSelectArea.Size = new System.Drawing.Size(144, 30);
            this.buttonSelectArea.TabIndex = 11;
            this.buttonSelectArea.Text = "Select Fishing Area";
            this.buttonSelectArea.UseVisualStyleBackColor = false;
            this.buttonSelectArea.Click += new System.EventHandler(this.buttonSelectArea_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.btnPause.Enabled = false;
            this.btnPause.FlatAppearance.BorderSize = 0;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Font = new System.Drawing.Font("Nirmala UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.btnPause.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnPause.Location = new System.Drawing.Point(83, 21);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(70, 30);
            this.btnPause.TabIndex = 10;
            this.btnPause.Text = "STOP (F10)";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.btnStart.Enabled = false;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Nirmala UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.btnStart.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnStart.Location = new System.Drawing.Point(9, 21);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(70, 30);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "START (F9)";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // SETTINGS
            // 
            this.SETTINGS.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (28)))), ((int) (((byte) (28)))), ((int) (((byte) (28)))));
            this.SETTINGS.Controls.Add(this.groupBoxTelegram);
            this.SETTINGS.Controls.Add(this.groupBox6);
            this.SETTINGS.Location = new System.Drawing.Point(4, 20);
            this.SETTINGS.Name = "SETTINGS";
            this.SETTINGS.Size = new System.Drawing.Size(418, 150);
            this.SETTINGS.TabIndex = 4;
            this.SETTINGS.Text = "GameSettings";
            // 
            // groupBoxTelegram
            // 
            this.groupBoxTelegram.Controls.Add(this.buttonConnectTelegram);
            this.groupBoxTelegram.Controls.Add(this.buttonTestTelegram);
            this.groupBoxTelegram.Controls.Add(this.labelTelegramState);
            this.groupBoxTelegram.Controls.Add(this.labelApiTelegram);
            this.groupBoxTelegram.Controls.Add(this.textBoxTelegramAPI);
            this.groupBoxTelegram.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBoxTelegram.Location = new System.Drawing.Point(133, 3);
            this.groupBoxTelegram.Name = "groupBoxTelegram";
            this.groupBoxTelegram.Size = new System.Drawing.Size(281, 114);
            this.groupBoxTelegram.TabIndex = 43;
            this.groupBoxTelegram.TabStop = false;
            this.groupBoxTelegram.Text = "Telegram Settings";
            // 
            // buttonConnectTelegram
            // 
            this.buttonConnectTelegram.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.buttonConnectTelegram.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.buttonConnectTelegram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnectTelegram.ForeColor = System.Drawing.Color.Orange;
            this.buttonConnectTelegram.Location = new System.Drawing.Point(6, 83);
            this.buttonConnectTelegram.Name = "buttonConnectTelegram";
            this.buttonConnectTelegram.Size = new System.Drawing.Size(269, 23);
            this.buttonConnectTelegram.TabIndex = 43;
            this.buttonConnectTelegram.Text = "connect";
            this.buttonConnectTelegram.UseVisualStyleBackColor = true;
            this.buttonConnectTelegram.Click += new System.EventHandler(this.buttonConnectTelegram_Click);
            // 
            // buttonTestTelegram
            // 
            this.buttonTestTelegram.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.buttonTestTelegram.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.buttonTestTelegram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTestTelegram.ForeColor = System.Drawing.Color.Orange;
            this.buttonTestTelegram.Location = new System.Drawing.Point(128, 48);
            this.buttonTestTelegram.Name = "buttonTestTelegram";
            this.buttonTestTelegram.Size = new System.Drawing.Size(147, 23);
            this.buttonTestTelegram.TabIndex = 42;
            this.buttonTestTelegram.Text = "test connection";
            this.buttonTestTelegram.UseVisualStyleBackColor = true;
            this.buttonTestTelegram.Click += new System.EventHandler(this.buttonTestTelegram_Click_1);
            // 
            // labelTelegramState
            // 
            this.labelTelegramState.Location = new System.Drawing.Point(6, 48);
            this.labelTelegramState.Name = "labelTelegramState";
            this.labelTelegramState.Size = new System.Drawing.Size(269, 23);
            this.labelTelegramState.TabIndex = 41;
            this.labelTelegramState.Text = "Status = Unbekannt";
            this.labelTelegramState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelApiTelegram
            // 
            this.labelApiTelegram.AutoSize = true;
            this.labelApiTelegram.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelApiTelegram.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelApiTelegram.Location = new System.Drawing.Point(5, 23);
            this.labelApiTelegram.Name = "labelApiTelegram";
            this.labelApiTelegram.Size = new System.Drawing.Size(75, 20);
            this.labelApiTelegram.TabIndex = 40;
            this.labelApiTelegram.Text = "APIKey =";
            // 
            // textBoxTelegramAPI
            // 
            this.textBoxTelegramAPI.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (60)))), ((int) (((byte) (60)))), ((int) (((byte) (60)))));
            this.textBoxTelegramAPI.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTelegramAPI.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.textBoxTelegramAPI.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxTelegramAPI.Location = new System.Drawing.Point(82, 20);
            this.textBoxTelegramAPI.Name = "textBoxTelegramAPI";
            this.textBoxTelegramAPI.Size = new System.Drawing.Size(193, 22);
            this.textBoxTelegramAPI.TabIndex = 40;
            this.textBoxTelegramAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxTelegramAPI.WordWrap = false;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Nirmala UI", 9.75F, ((System.Drawing.FontStyle) ((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label46.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label46.Location = new System.Drawing.Point(3, 15);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(131, 17);
            this.label46.TabIndex = 48;
            this.label46.Text = "PRIORITIZED SKILLS";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label48.ForeColor = System.Drawing.Color.Orange;
            this.label48.Location = new System.Drawing.Point(5, 47);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(126, 75);
            this.label48.TabIndex = 49;
            this.label48.Text = "Set your own \r\nSkill Rotation!\r\nprioritize by Nr\'s...\r\n\r\n1 , 2 , 3 , 4 , 5 , 6 , " + "7 , 8";
            // 
            // labelSwap
            // 
            this.labelSwap.AutoSize = true;
            this.labelSwap.BackColor = System.Drawing.Color.Transparent;
            this.labelSwap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelSwap.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSwap.ForeColor = System.Drawing.Color.White;
            this.labelSwap.Location = new System.Drawing.Point(325, 9);
            this.labelSwap.Name = "labelSwap";
            this.labelSwap.Size = new System.Drawing.Size(50, 21);
            this.labelSwap.TabIndex = 12;
            this.labelSwap.Text = "CBOT";
            this.labelSwap.Click += new System.EventHandler(this.labelSwap_Click);
            // 
            // GatheringBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (37)))), ((int) (((byte) (37)))), ((int) (((byte) (37)))));
            this.BackgroundImage = ((System.Drawing.Image) (resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(450, 237);
            this.Controls.Add(this.labelSwap);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbClose);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GatheringBot";
            this.Text = "FishBot";
            this.Load += new System.EventHandler(this.FishBot_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FishBot_MouseDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.STARTEXIT.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.SETTINGS.ResumeLayout(false);
            this.groupBoxTelegram.ResumeLayout(false);
            this.groupBoxTelegram.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label labelSwap;

        private System.Windows.Forms.Button btnInstructions;

        private System.Windows.Forms.Button buttonSetup;

        #endregion
        private System.Windows.Forms.Label lbClose;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TabPage STARTEXIT;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TabPage SETTINGS;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.CheckBox chBoxLOGOUT;
        private System.Windows.Forms.TextBox txtLOGOUT;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.CheckBox chBoxAutoBuff;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.CheckBox chBoxChannelSwap;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.CheckBox chBoxDoubleF;
        private System.Windows.Forms.CheckBox chBoxDoubleD;
        private System.Windows.Forms.CheckBox chBoxDoubleS;
        private System.Windows.Forms.CheckBox chBoxDoubleA;
        private System.Windows.Forms.CheckBox chBoxDoubleR;
        private System.Windows.Forms.CheckBox chBoxDoubleE;
        private System.Windows.Forms.CheckBox chBoxDoubleW;
        private System.Windows.Forms.CheckBox chBoxDoubleQ;
        private System.Windows.Forms.Label lbPF;
        private System.Windows.Forms.Label lbPD;
        private System.Windows.Forms.Label lbPS;
        private System.Windows.Forms.Label lbPA;
        private System.Windows.Forms.Label lbPR;
        private System.Windows.Forms.Label lbPE;
        private System.Windows.Forms.Label lbPW;
        private System.Windows.Forms.Label lbPQ;
        private System.Windows.Forms.TextBox txPA;
        private System.Windows.Forms.TextBox txPS;
        private System.Windows.Forms.TextBox txPD;
        private System.Windows.Forms.TextBox txPF;
        private System.Windows.Forms.TextBox txPW;
        private System.Windows.Forms.TextBox txPQ;
        private System.Windows.Forms.TextBox txPE;
        private System.Windows.Forms.TextBox txPR;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.CheckBox chBoxAutoRepair;
        private System.Windows.Forms.Button buttonSelectArea;
        private System.Windows.Forms.GroupBox groupBoxTelegram;
        private System.Windows.Forms.Button buttonConnectTelegram;
        private System.Windows.Forms.Button buttonTestTelegram;
        private System.Windows.Forms.Label labelTelegramState;
        private System.Windows.Forms.Label labelApiTelegram;
        private System.Windows.Forms.TextBox textBoxTelegramAPI;
    }
}

