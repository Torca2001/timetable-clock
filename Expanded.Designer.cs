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
            this.Termlabel = new System.Windows.Forms.Label();
            this.Curdayhigh = new System.Windows.Forms.Panel();
            this.Earlybutt = new System.Windows.Forms.Label();
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
            // Termlabel
            // 
            this.Termlabel.AutoSize = true;
            this.Termlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Termlabel.Location = new System.Drawing.Point(1114, 9);
            this.Termlabel.Name = "Termlabel";
            this.Termlabel.Size = new System.Drawing.Size(58, 20);
            this.Termlabel.TabIndex = 1;
            this.Termlabel.Text = "Term 0";
            // 
            // Curdayhigh
            // 
            this.Curdayhigh.BackColor = System.Drawing.Color.Red;
            this.Curdayhigh.Location = new System.Drawing.Point(66, 0);
            this.Curdayhigh.Name = "Curdayhigh";
            this.Curdayhigh.Size = new System.Drawing.Size(120, 593);
            this.Curdayhigh.TabIndex = 3;
            this.Curdayhigh.Visible = false;
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
            // Expanded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 591);
            this.ControlBox = false;
            this.Controls.Add(this.Earlybutt);
            this.Controls.Add(this.Curdayhigh);
            this.Controls.Add(this.Termlabel);
            this.Controls.Add(this.MissingLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1200, 630);
            this.MinimumSize = new System.Drawing.Size(1200, 630);
            this.Name = "Expanded";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Expanded";
            this.Activated += new System.EventHandler(this.Expanded_Activated);
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Expanded_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MissingLabel;
        private System.Windows.Forms.Label Termlabel;
        private System.Windows.Forms.Panel Curdayhigh;
        private System.Windows.Forms.Label Earlybutt;
    }
}