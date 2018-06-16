using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SplashScreen;

namespace SchoolManager
{
    public partial class Expanded : Form
    {
        public Expanded()
        {
            InitializeComponent();
            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i <= 6; i++)
                {
                    Panel Pperiod = new Panel();
                    Pperiod.Name = (k+1)+""+i;
                    Pperiod.Width = 100;
                    Pperiod.Height = 80;
                    Pperiod.BackColor = Color.Aqua;
                    Pperiod.BorderStyle = BorderStyle.FixedSingle;
                    Pperiod.Location = new Point(110*k+10, 82 * i + 10);
                    Controls.Add(Pperiod);
                }
            }            
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }

        private void Expanded_Activated(object sender, EventArgs e)
        {
            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i <= 6; i++)
                {
                    TextBox textLabel = new TextBox();
                    textLabel.ReadOnly = true;
                    textLabel.WordWrap = true;
                    textLabel.BorderStyle = BorderStyle.None;
                    textLabel.Width = 98;
                    textLabel.Height = 30;
                    textLabel.Multiline = true;
                    textLabel.Location = new Point(1, 3);
                    Panel eriod = (Panel)(Controls.Find((k + 1) + i.ToString(), false))[0];
                    textLabel.BackColor = eriod.BackColor;
                    textLabel.Text = Program.timetableList.ContainsKey((k + 1) + "" + i) ? Program.timetableList[(k + 1) + "" + i].ClassDescription : "Period " + i;
                    eriod.Controls.Clear();
                    eriod.Controls.Add(textLabel);
                }
            }
        }
    }
}
