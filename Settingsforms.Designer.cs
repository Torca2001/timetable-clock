using System;

namespace SchoolManager
{
    partial class Settingsforms
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
            this.label1 = new System.Windows.Forms.Label();
            this.Userbox = new System.Windows.Forms.TextBox();
            this.Errormsg = new System.Windows.Forms.Label();
            this.Passbox = new System.Windows.Forms.TextBox();
            this.Loginbutton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Week Override";
            // 
            // Userbox
            // 
            this.Userbox.Location = new System.Drawing.Point(3, 348);
            this.Userbox.Name = "Userbox";
            this.Userbox.Size = new System.Drawing.Size(100, 20);
            this.Userbox.TabIndex = 1;
            // 
            // Errormsg
            // 
            this.Errormsg.AutoSize = true;
            this.Errormsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Errormsg.Location = new System.Drawing.Point(0, 371);
            this.Errormsg.Name = "Errormsg";
            this.Errormsg.Size = new System.Drawing.Size(0, 13);
            this.Errormsg.TabIndex = 2;
            // 
            // Passbox
            // 
            this.Passbox.Location = new System.Drawing.Point(109, 348);
            this.Passbox.Name = "Passbox";
            this.Passbox.PasswordChar = '*';
            this.Passbox.Size = new System.Drawing.Size(100, 20);
            this.Passbox.TabIndex = 3;
            this.Passbox.UseSystemPasswordChar = true;
            // 
            // Loginbutton
            // 
            this.Loginbutton.Location = new System.Drawing.Point(215, 348);
            this.Loginbutton.Name = "Loginbutton";
            this.Loginbutton.Size = new System.Drawing.Size(75, 23);
            this.Loginbutton.TabIndex = 4;
            this.Loginbutton.Text = "Fetch";
            this.Loginbutton.UseVisualStyleBackColor = true;
            this.Loginbutton.Click += new System.EventHandler(this.Loginbutton_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Loginbutton);
            this.panel1.Controls.Add(this.Passbox);
            this.panel1.Controls.Add(this.Errormsg);
            this.panel1.Controls.Add(this.Userbox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(641, 384);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(469, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 52);
            this.label2.TabIndex = 5;
            this.label2.Text = "About\r\nTimetable Clock made by\r\nWilliam C\r\nDistributed by\r\nJosh Harper";
            // 
            // Settingsforms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 384);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Settingsforms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settingsforms";
            this.Deactivate += new System.EventHandler(this.Settingsforms_Deactivate);
            this.Shown += new System.EventHandler(this.Settingsforms_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Userbox;
        private System.Windows.Forms.Label Errormsg;
        private System.Windows.Forms.TextBox Passbox;
        private System.Windows.Forms.Button Loginbutton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
    }
}