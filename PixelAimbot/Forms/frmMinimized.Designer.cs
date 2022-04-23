namespace PixelAimbot
{
    partial class frmMinimized
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMinimized));
            this.labelMinimizedState = new System.Windows.Forms.Label();
            this.labelRuntimer = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.timerRuntimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // labelMinimizedState
            // 
            this.labelMinimizedState.AutoSize = false;
            this.labelMinimizedState.BackColor = System.Drawing.Color.Transparent;
            this.labelMinimizedState.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.labelMinimizedState.Location = new System.Drawing.Point(191, 1);
            this.labelMinimizedState.Name = "labelMinimizedState";
            this.labelMinimizedState.Size = new System.Drawing.Size(242, 28);
            this.labelMinimizedState.TabIndex = 0;
            this.labelMinimizedState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRuntimer
            // 
            this.labelRuntimer.AutoSize = false;
            this.labelRuntimer.BackColor = System.Drawing.Color.Transparent;
            this.labelRuntimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.labelRuntimer.Location = new System.Drawing.Point(464, 1);
            this.labelRuntimer.Name = "labelRuntimer";
            this.labelRuntimer.Size = new System.Drawing.Size(200, 28);
            this.labelRuntimer.TabIndex = 1;
            this.labelRuntimer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = false;
            this.labelTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTitle.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.Orange;
            this.labelTitle.Location = new System.Drawing.Point(12, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(185, 28);
            this.labelTitle.TabIndex = 12;
            this.labelTitle.Text = "ChaosDungeon Bot";
            // 
            // timerRuntimer
            // 
            this.timerRuntimer.Interval = 1000;
            this.timerRuntimer.Tick += new System.EventHandler(this.timerRuntimer_Tick);
            // 
            // frmMinimized
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(594, 28);
            this.ControlBox = false;
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelRuntimer);
            this.Controls.Add(this.labelMinimizedState);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMinimized";
            this.Size = new System.Drawing.Size(594, 28);
            this.Opacity = 0.8D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmMinimized";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label labelMinimizedState;
        public System.Windows.Forms.Label labelRuntimer;
        public System.Windows.Forms.Label labelTitle;
        public System.Windows.Forms.Timer timerRuntimer;
    }
}