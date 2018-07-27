<<<<<<< HEAD
﻿using System;
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
        private bool colorbox = false;
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
            Curdayhigh.SendToBack();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (colorbox)
                return;
            Hide();
            Termlabel.Text = "Term " + Program.SettingsData.Curterm;
            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i <= 6; i++)
                {
                    period tPeriod;
                    if (Program.TimetableList.ContainsKey((k + 1) + "" + i))
                        tPeriod = Program.TimetableList[(k + 1) + "" + i];
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
                    classroom.Location = new Point(62, 65);
                    classcod.Text = tPeriod.SchoolStaffCode+Environment.NewLine+tPeriod.ClassCode;
                    classcod.Location = new Point(0, 52);
                    classcod.AutoSize = true;
                    textLabel.ReadOnly = true;
                    textLabel.WordWrap = true;
                    textLabel.BorderStyle = BorderStyle.None;
                    textLabel.Width = 98;
                    textLabel.Height = 45;
                    textLabel.Multiline = true;
                    textLabel.Location = new Point(1, 3);
                    Panel eriod = (Panel)(Controls.Find((k + 1) + i.ToString(), false))[0];
                    textLabel.Click += (s, p) =>
                    {
                        colorbox = true;
                        colorDialog1.Color = eriod.BackColor;
                        DialogResult result = colorDialog1.ShowDialog();
                        if (result == DialogResult.OK&&Program.TimetableList.ContainsKey(eriod.Name))
                        {
                            Program.ColorRef[Program.TimetableList[eriod.Name].ClassCode] = colorDialog1.Color;
                            updatecolors();
                        }
                        colorbox = false;
                    };
                    textLabel.Text = tPeriod.ClassDescription;
                    eriod.BackColor = Program.ColorRef.ContainsKey(tPeriod.ClassCode) ? Program.ColorRef[tPeriod.ClassCode] : Color.White;
                    textLabel.BackColor = eriod.BackColor;
                    eriod.Controls.Clear();
                    eriod.Controls.Add(classcod);
                    eriod.Controls.Add(classroom);
                    eriod.Controls.Add(textLabel);
                }
            }

            if (Program.curDay == 0)
            {
                Curdayhigh.Visible = false;
            }
            else
            {
                Panel deriod = (Panel) (Controls.Find(Program.curDay + "0", false))[0];
                Curdayhigh.Visible = true;
                Curdayhigh.Location = new Point(deriod.Location.X-10, 0);
            }
        }

        private void Expanded_Activated(object sender, EventArgs e)
        {
            if (Program.TimetableList.Count == 0)
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
                Termlabel.Text = "Term " + Program.SettingsData.Curterm;
                started = true;
                for (int k = 0; k < 10; k++)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        period tPeriod;
                        if (Program.TimetableList.ContainsKey((k + 1) + "" + i))
                            tPeriod = Program.TimetableList[(k + 1) + "" + i];
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
                        classroom.Location = new Point(62, 65);
                        classcod.Text = tPeriod.SchoolStaffCode + Environment.NewLine + tPeriod.ClassCode;
                        classcod.Location = new Point(0, 52);
                        classcod.AutoSize = true;
                        textLabel.ReadOnly = true;
                        textLabel.WordWrap = true;
                        textLabel.BorderStyle = BorderStyle.None;
                        textLabel.Width = 98;
                        textLabel.Height = 45;
                        textLabel.Multiline = true;
                        textLabel.Location = new Point(1, 3);
                        Panel eriod = (Panel)(Controls.Find((k + 1) + i.ToString(), false))[0];
                        textLabel.Click += (s, p) =>
                        {
                            colorbox = true;
                            colorDialog1.Color = eriod.BackColor;
                            DialogResult result = colorDialog1.ShowDialog();
                            if (result == DialogResult.OK && Program.TimetableList.ContainsKey(eriod.Name))
                            {
                                Program.ColorRef[Program.TimetableList[eriod.Name].ClassCode] = colorDialog1.Color;
                                updatecolors();
                            }
                            colorbox = false;
                        };
                        textLabel.Text = tPeriod.ClassDescription;
                        eriod.BackColor = Program.ColorRef.ContainsKey(tPeriod.ClassCode) ? Program.ColorRef[tPeriod.ClassCode] : Color.White;
                        textLabel.BackColor = eriod.BackColor;
                        eriod.Controls.Clear();
                        eriod.Controls.Add(classcod);
                        eriod.Controls.Add(classroom);
                        eriod.Controls.Add(textLabel);
                    }
                }
            }

            if (Program.curDay == 0)
            {
                Curdayhigh.Visible = false;
            }
            else
            {
                Panel deriod = (Panel)(Controls.Find(Program.curDay + "0", false))[0];
                Curdayhigh.Visible = true;
                Curdayhigh.Location = new Point(deriod.Location.X-10, 0);
            }
            if (Program.SettingsData.EarlyDate.Date==DateTime.Now.Date)
                Earlybutt.BackColor = Color.GreenYellow;
        }

        private void Expanded_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = (e.CloseReason == CloseReason.UserClosing);  // this cancels the close event.
        }

        private void Earlybutt_MouseHover(object sender, EventArgs e)
        {
            Earlybutt.BackColor = Color.Red;
        }

        private void Earlybutt_MouseLeave(object sender, EventArgs e)
        {
            if (Program.SettingsData.EarlyDate.Date != DateTime.Now.Date)
                Earlybutt.BackColor = Color.DarkRed;
            else
                Earlybutt.BackColor = Color.GreenYellow;
        }

        private void Earlybutt_Click(object sender, EventArgs e)
        {
            if (Program.SettingsData.EarlyDate.Date != DateTime.Now.Date)
            {
                Program.SettingsData.EarlyDate = DateTime.Now;
                Earlybutt.BackColor = Color.GreenYellow;
            }
            else
            {
                Program.SettingsData.EarlyDate = new DateTime(2017,1,1);
                Earlybutt.BackColor = Color.DarkRed;
            }
        }

        private void updatecolors()
        {
            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i <= 6; i++)
                {
                    period tPeriod = new period();
                    if (Program.TimetableList.ContainsKey((k + 1) + "" + i))
                        tPeriod = Program.TimetableList[(k + 1) + "" + i];
                    else
                        continue;
                    Panel eriod = (Panel)(Controls.Find((k + 1) + i.ToString(), false))[0];
                    eriod.BackColor = Program.ColorRef[tPeriod.ClassCode];
                    eriod.Controls[2].BackColor = Program.ColorRef[tPeriod.ClassCode];
                }
            }
        }
    }
}
=======
﻿using System;
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
        private bool colorbox = false;
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
            Curdayhigh.SendToBack();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (colorbox)
                return;
            Hide();
            Termlabel.Text = "Term " + Program.SettingsData.Curterm;
            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i <= 6; i++)
                {
                    period tPeriod;
                    if (Program.TimetableList.ContainsKey((k + 1) + "" + i))
                        tPeriod = Program.TimetableList[(k + 1) + "" + i];
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
                    classroom.Location = new Point(62, 65);
                    classcod.Text = tPeriod.SchoolStaffCode+Environment.NewLine+tPeriod.ClassCode;
                    classcod.Location = new Point(0, 52);
                    classcod.AutoSize = true;
                    textLabel.ReadOnly = true;
                    textLabel.WordWrap = true;
                    textLabel.BorderStyle = BorderStyle.None;
                    textLabel.Width = 98;
                    textLabel.Height = 45;
                    textLabel.Multiline = true;
                    textLabel.Location = new Point(1, 3);
                    Panel eriod = (Panel)(Controls.Find((k + 1) + i.ToString(), false))[0];
                    textLabel.Click += (s, p) =>
                    {
                        colorbox = true;
                        colorDialog1.Color = eriod.BackColor;
                        DialogResult result = colorDialog1.ShowDialog();
                        if (result == DialogResult.OK&&Program.TimetableList.ContainsKey(eriod.Name))
                        {
                            Program.ColorRef[Program.TimetableList[eriod.Name].ClassCode] = colorDialog1.Color;
                            updatecolors();
                        }
                        colorbox = false;
                    };
                    textLabel.Text = tPeriod.ClassDescription;
                    eriod.BackColor = Program.ColorRef.ContainsKey(tPeriod.ClassCode) ? Program.ColorRef[tPeriod.ClassCode] : Color.White;
                    textLabel.BackColor = eriod.BackColor;
                    eriod.Controls.Clear();
                    eriod.Controls.Add(classcod);
                    eriod.Controls.Add(classroom);
                    eriod.Controls.Add(textLabel);
                }
            }

            if (Program.curDay == 0)
            {
                Curdayhigh.Visible = false;
            }
            else
            {
                Panel deriod = (Panel) (Controls.Find(Program.curDay + "0", false))[0];
                Curdayhigh.Visible = true;
                Curdayhigh.Location = new Point(deriod.Location.X-10, 0);
            }
        }

        private void Expanded_Activated(object sender, EventArgs e)
        {
            if (Program.TimetableList.Count == 0)
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
                Termlabel.Text = "Term " + Program.SettingsData.Curterm;
                started = true;
                for (int k = 0; k < 10; k++)
                {
                    for (int i = 0; i <= 6; i++)
                    {
                        period tPeriod;
                        if (Program.TimetableList.ContainsKey((k + 1) + "" + i))
                            tPeriod = Program.TimetableList[(k + 1) + "" + i];
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
                        classroom.Location = new Point(62, 65);
                        classcod.Text = tPeriod.SchoolStaffCode + Environment.NewLine + tPeriod.ClassCode;
                        classcod.Location = new Point(0, 52);
                        classcod.AutoSize = true;
                        textLabel.ReadOnly = true;
                        textLabel.WordWrap = true;
                        textLabel.BorderStyle = BorderStyle.None;
                        textLabel.Width = 98;
                        textLabel.Height = 45;
                        textLabel.Multiline = true;
                        textLabel.Location = new Point(1, 3);
                        Panel eriod = (Panel)(Controls.Find((k + 1) + i.ToString(), false))[0];
                        textLabel.Click += (s, p) =>
                        {
                            colorbox = true;
                            colorDialog1.Color = eriod.BackColor;
                            DialogResult result = colorDialog1.ShowDialog();
                            if (result == DialogResult.OK && Program.TimetableList.ContainsKey(eriod.Name))
                            {
                                Program.ColorRef[Program.TimetableList[eriod.Name].ClassCode] = colorDialog1.Color;
                                updatecolors();
                            }
                            colorbox = false;
                        };
                        textLabel.Text = tPeriod.ClassDescription;
                        eriod.BackColor = Program.ColorRef.ContainsKey(tPeriod.ClassCode) ? Program.ColorRef[tPeriod.ClassCode] : Color.White;
                        textLabel.BackColor = eriod.BackColor;
                        eriod.Controls.Clear();
                        eriod.Controls.Add(classcod);
                        eriod.Controls.Add(classroom);
                        eriod.Controls.Add(textLabel);
                    }
                }
            }

            if (Program.curDay == 0)
            {
                Curdayhigh.Visible = false;
            }
            else
            {
                Panel deriod = (Panel)(Controls.Find(Program.curDay + "0", false))[0];
                Curdayhigh.Visible = true;
                Curdayhigh.Location = new Point(deriod.Location.X-10, 0);
            }
            if (Program.SettingsData.EarlyDate.Date==DateTime.Now.Date)
                Earlybutt.BackColor = Color.GreenYellow;
        }

        private void Expanded_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = (e.CloseReason == CloseReason.UserClosing);  // this cancels the close event.
        }

        private void Earlybutt_MouseHover(object sender, EventArgs e)
        {
            Earlybutt.BackColor = Color.Red;
        }

        private void Earlybutt_MouseLeave(object sender, EventArgs e)
        {
            if (Program.SettingsData.EarlyDate.Date != DateTime.Now.Date)
                Earlybutt.BackColor = Color.DarkRed;
            else
                Earlybutt.BackColor = Color.GreenYellow;
        }

        private void Earlybutt_Click(object sender, EventArgs e)
        {
            if (Program.SettingsData.EarlyDate.Date != DateTime.Now.Date)
            {
                Program.SettingsData.EarlyDate = DateTime.Now;
                Earlybutt.BackColor = Color.GreenYellow;
            }
            else
            {
                Program.SettingsData.EarlyDate = new DateTime(2017,1,1);
                Earlybutt.BackColor = Color.DarkRed;
            }
        }

        private void updatecolors()
        {
            for (int k = 0; k < 10; k++)
            {
                for (int i = 0; i <= 6; i++)
                {
                    period tPeriod = new period();
                    if (Program.TimetableList.ContainsKey((k + 1) + "" + i))
                        tPeriod = Program.TimetableList[(k + 1) + "" + i];
                    else
                        continue;
                    Panel eriod = (Panel)(Controls.Find((k + 1) + i.ToString(), false))[0];
                    eriod.BackColor = Program.ColorRef[tPeriod.ClassCode];
                    eriod.Controls[2].BackColor = Program.ColorRef[tPeriod.ClassCode];
                }
            }
        }
    }
}
>>>>>>> 5bb2eea2085facea4119796ca3466cd8a6084482
