namespace PixelAimbot
{
    partial class frmGuide
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGuide));
            this.lbClose = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.imageBox5 = new Emgu.CV.UI.ImageBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.imageBox4 = new Emgu.CV.UI.ImageBox();
            this.imageBox3 = new Emgu.CV.UI.ImageBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // lbClose
            // 
            this.lbClose.AutoSize = true;
            this.lbClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbClose.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbClose.ForeColor = System.Drawing.Color.White;
            this.lbClose.Location = new System.Drawing.Point(685, 9);
            this.lbClose.Name = "lbClose";
            this.lbClose.Size = new System.Drawing.Size(57, 21);
            this.lbClose.TabIndex = 4;
            this.lbClose.Text = "CLOSE";
            this.lbClose.Click += new System.EventHandler(this.lbClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Orange;
            this.label1.Location = new System.Drawing.Point(7, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "ChaosDungeon Bot";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.imageBox5);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.imageBox4);
            this.groupBox1.Controls.Add(this.imageBox3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label36);
            this.groupBox1.Font = new System.Drawing.Font("Nirmala UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.groupBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Location = new System.Drawing.Point(12, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(729, 569);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Instructions";
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Peru;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLogin.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnLogin.Location = new System.Drawing.Point(505, 403);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(203, 31);
            this.btnLogin.TabIndex = 70;
            this.btnLogin.Text = "<CLICK FOR BOT SETTINGS>";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // imageBox5
            // 
            this.imageBox5.BackgroundImage = global::PixelAimbot.Properties.Resources.scale;
            this.imageBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imageBox5.Location = new System.Drawing.Point(13, 69);
            this.imageBox5.Name = "imageBox5";
            this.imageBox5.Size = new System.Drawing.Size(710, 222);
            this.imageBox5.TabIndex = 79;
            this.imageBox5.TabStop = false;
            this.imageBox5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold);
            this.label15.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label15.Location = new System.Drawing.Point(52, 308);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(623, 21);
            this.label15.TabIndex = 78;
            this.label15.Text = "\"Game\" Recommend Resolution to 1920x1080. BORDERLESS and 21:9 = disabled!";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label15.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Coral;
            this.label16.Location = new System.Drawing.Point(24, 305);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(28, 25);
            this.label16.TabIndex = 77;
            this.label16.Text = "2.";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label16.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label13.Location = new System.Drawing.Point(511, 366);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(206, 34);
            this.label13.TabIndex = 76;
            this.label13.Text = "Stand near the Dungeon Statue!\r\nInteract \"G\" must be visible.";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label13.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Coral;
            this.label14.Location = new System.Drawing.Point(489, 360);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(28, 25);
            this.label14.TabIndex = 75;
            this.label14.Text = "4.";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label14.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // imageBox4
            // 
            this.imageBox4.BackgroundImage = global::PixelAimbot.Properties.Resources.hud;
            this.imageBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imageBox4.Location = new System.Drawing.Point(10, 506);
            this.imageBox4.Name = "imageBox4";
            this.imageBox4.Size = new System.Drawing.Size(707, 46);
            this.imageBox4.TabIndex = 74;
            this.imageBox4.TabStop = false;
            this.imageBox4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // imageBox3
            // 
            this.imageBox3.BackgroundImage = global::PixelAimbot.Properties.Resources.resoulution;
            this.imageBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imageBox3.Location = new System.Drawing.Point(15, 338);
            this.imageBox3.Name = "imageBox3";
            this.imageBox3.Size = new System.Drawing.Size(468, 130);
            this.imageBox3.TabIndex = 73;
            this.imageBox3.TabStop = false;
            this.imageBox3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label7.Location = new System.Drawing.Point(53, 526);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 17);
            this.label7.TabIndex = 71;
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label8.Location = new System.Drawing.Point(28, 509);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 17);
            this.label8.TabIndex = 70;
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Location = new System.Drawing.Point(26, 483);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(161, 21);
            this.label9.TabIndex = 69;
            this.label9.Text = "HUD Resizing/Scale";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.Location = new System.Drawing.Point(52, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(623, 21);
            this.label12.TabIndex = 66;
            this.label12.Text = "\"Windows-System\" Resolution to 1920x1080.  \"Windows-System\" Scale to 100%!";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Coral;
            this.label4.Location = new System.Drawing.Point(24, 526);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 25);
            this.label4.TabIndex = 65;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Coral;
            this.label2.Location = new System.Drawing.Point(7, 475);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 25);
            this.label2.TabIndex = 61;
            this.label2.Text = "3.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.Coral;
            this.label36.Location = new System.Drawing.Point(24, 32);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(28, 25);
            this.label36.TabIndex = 60;
            this.label36.Text = "1.";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label36.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Coral;
            this.label19.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label19.Location = new System.Drawing.Point(185, -2);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(389, 42);
            this.label19.TabIndex = 67;
            this.label19.Text = "READ SLOWLY AND CAREFULLY!\r\nNo Support before Checking all Settings are right.";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label19.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Peru;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button1.Location = new System.Drawing.Point(517, 480);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(203, 31);
            this.button1.TabIndex = 80;
            this.button1.Text = "<CLICK FOR STATISTIC>";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmGuide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.ClientSize = new System.Drawing.Size(754, 610);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGuide";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmGuide";
            this.Load += new System.EventHandler(this.frmGuide_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGuide_MouseDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label36;
        private Emgu.CV.UI.ImageBox imageBox3;
        private Emgu.CV.UI.ImageBox imageBox4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private Emgu.CV.UI.ImageBox imageBox5;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button button1;
    }
}