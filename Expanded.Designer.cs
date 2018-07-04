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
            this.MissingLabel = new System.Windows.Forms.Label();
            this.Termlabel = new System.Windows.Forms.Label();
            this.Earlybutton = new System.Windows.Forms.Button();
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
            // Earlybutton
            // 
            this.Earlybutton.Location = new System.Drawing.Point(1097, 32);
            this.Earlybutton.Name = "Earlybutton";
            this.Earlybutton.Size = new System.Drawing.Size(75, 23);
            this.Earlybutton.TabIndex = 2;
            this.Earlybutton.Text = "Early finish";
            this.Earlybutton.UseVisualStyleBackColor = true;
            this.Earlybutton.Click += new System.EventHandler(this.Earlybutton_Click);
            // 
            // Expanded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 631);
            this.ControlBox = false;
            this.Controls.Add(this.Earlybutton);
            this.Controls.Add(this.Termlabel);
            this.Controls.Add(this.MissingLabel);
            this.MaximumSize = new System.Drawing.Size(1200, 670);
            this.MinimumSize = new System.Drawing.Size(1200, 670);
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
        private System.Windows.Forms.Button Earlybutton;
    }
}