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
        private bool started=false;
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
            Termlabel.Text = "Term " + Program.Settingsdata.Curterm;
            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i <= 6; i++)
                {
                    period tPeriod;
                    if (Program.timetableList.ContainsKey((k + 1) + "" + i))
                        tPeriod = Program.timetableList[(k + 1) + "" + i];
                    else
                    {
                        tPeriod = new period();
                        tPeriod.ClassCode = "";
                        tPeriod.DayNumber = -1;
                        tPeriod.PeriodNumber = -1;
                    }
                    TextBox textLabel = new TextBox();
                    Label classroom = new Label();
                    Label classcod = new Label();
                    classroom.Text = tPeriod.Room;
                    classroom.Location = new Point(65, 65);
                    classcod.Text = tPeriod.ClassCode;
                    classcod.Location = new Point(0, 65);
                    classcod.AutoSize = true;
                    textLabel.ReadOnly = true;
                    textLabel.WordWrap = true;
                    textLabel.BorderStyle = BorderStyle.None;
                    textLabel.Width = 98;
                    textLabel.Height = 45;
                    textLabel.Multiline = true;
                    textLabel.Location = new Point(1, 3);
                    Panel eriod = (Panel)(Controls.Find((k + 1) + i.ToString(), false))[0];
                    textLabel.Text = tPeriod.ClassDescription;
                    eriod.BackColor = Program.Colorref.ContainsKey(tPeriod.ClassCode) ? Program.Colorref[tPeriod.ClassCode] : Color.White;
                    textLabel.BackColor = eriod.BackColor;
                    eriod.Controls.Clear();
                    eriod.Controls.Add(classcod);
                    eriod.Controls.Add(classroom);
                    eriod.Controls.Add(textLabel);
                }
            }
        }

        private void Expanded_Activated(object sender, EventArgs e)
        {
            if (Program.timetableList.Count == 0)
            {
                MissingLabel.BringToFront();
                MissingLabel.Show();
            }
            else
            {
                MissingLabel.Hide();
            }
            if (!started)
            {
                Termlabel.Text = "Term " + Program.Settingsdata.Curterm;
                started = true;
                Console.WriteLine("Loaded");
                for (int k = 0; k < 10; k++)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        period tPeriod;
                        if (Program.timetableList.ContainsKey((k + 1) + "" + i))
                            tPeriod = Program.timetableList[(k + 1) + "" + i];
                        else
                        {
                            tPeriod = new period();
                            tPeriod.ClassCode = "";
                            tPeriod.DayNumber = -1;
                            tPeriod.PeriodNumber = -1;
                        }
                        TextBox textLabel = new TextBox();
                        Label classroom = new Label();
                        Label classcod = new Label();
                        classroom.Text = tPeriod.Room;
                        classroom.Location = new Point(65, 65);
                        classcod.Text = tPeriod.ClassCode;
                        classcod.Location = new Point(0, 65);
                        classcod.AutoSize = true;
                        textLabel.ReadOnly = true;
                        textLabel.WordWrap = true;
                        textLabel.BorderStyle = BorderStyle.None;
                        textLabel.Width = 98;
                        textLabel.Height = 45;
                        textLabel.Multiline = true;
                        textLabel.Location = new Point(1, 3);
                        Panel eriod = (Panel)(Controls.Find((k + 1) + i.ToString(), false))[0];
                        textLabel.Text = tPeriod.ClassDescription;
                        eriod.BackColor = Program.Colorref.ContainsKey(tPeriod.ClassCode) ? Program.Colorref[tPeriod.ClassCode] : Color.White;
                        textLabel.BackColor = eriod.BackColor;
                        eriod.Controls.Clear();
                        eriod.Controls.Add(classcod);
                        eriod.Controls.Add(classroom);
                        eriod.Controls.Add(textLabel);
                    }
                }
            }
        }

        private void Expanded_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true; // this cancels the close event.
        }
    }
}
