namespace SchoolManager
{
    partial class Expanded
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Expanded));
            this.MissingLabel = new System.Windows.Forms.Label();
            this.Earlybutt = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // MissingLabel
            // 
            this.MissingLabel.AutoSize = true;
            this.MissingLabel.BackColor = System.Drawing.SystemColors.Window;
            this.MissingLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MissingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F);
            this.MissingLabel.ForeColor = System.Drawing.Color.Red;
            this.MissingLabel.Location = new System.Drawing.Point(394, 285);
            this.MissingLabel.Name = "MissingLabel";
            this.MissingLabel.Size = new System.Drawing.Size(351, 65);
            this.MissingLabel.TabIndex = 0;
            this.MissingLabel.Text = "No Timetable";
            // 
            // Earlybutt
            // 
            this.Earlybutt.AutoSize = true;
            this.Earlybutt.BackColor = System.Drawing.Color.DarkRed;
            this.Earlybutt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Earlybutt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Earlybutt.Location = new System.Drawing.Point(1118, 33);
            this.Earlybutt.Name = "Earlybutt";
            this.Earlybutt.Size = new System.Drawing.Size(53, 42);
            this.Earlybutt.TabIndex = 4;
            this.Earlybutt.Text = "Early\r\nFinish";
            this.Earlybutt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Earlybutt.Click += new System.EventHandler(this.Earlybutt_Click);
            this.Earlybutt.MouseLeave += new System.EventHandler(this.Earlybutt_MouseLeave);
            this.Earlybutt.MouseHover += new System.EventHandler(this.Earlybutt_MouseHover);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            // 
            // Expanded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1200, 630);
            this.ControlBox = false;
            this.Controls.Add(this.Earlybutt);
            this.Controls.Add(this.MissingLabel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1200, 630);
            this.MinimumSize = new System.Drawing.Size(1200, 630);
            this.Name = "Expanded";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Two-Week Timetable";
            this.Activated += new System.EventHandler(this.Expanded_Activated);
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Expanded_FormClosing);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Expanded_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Expanded_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Expanded_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Expanded_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MissingLabel;
        private System.Windows.Forms.Label Earlybutt;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}