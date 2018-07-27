<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SplashScreen;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SchoolManager
{
    public partial class Settingsforms : Form
    {
        public int Download
        {
            get { return _Download; }
            set
            {
                if (100 > value && value > 0)
                {
                    progresslabel.Text = bytesreceived.ToString("0.###") + "/" + Bytesneeded.ToString("0.###") + " MB";
                    progressBar1.Visible = true;
                }
                else if (progressBar1.Visible)
                {
                    progressBar1.Visible = false;
                    if (value==100)
                        progresslabel.Text = "Downloaded";
                }

                _Download = value;
                progressBar1.Value = value;
            }
        }

        public decimal bytesreceived;
        public decimal Bytesneeded;

        private int _Download;
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);
        public Settingsforms()
        {
            InitializeComponent();
            label2.Text += Program.APP_VERSION;
        }

        private void Settingsforms_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }

        private void Loginbutton_Click(object sender, EventArgs e)
        {
            Errormsg.Text = "Attempting fetch...";
            string usertxt = Userbox.Text;
            string passtxt = Passbox.Text;
            Task.Run(() =>
            {
                SetLabel1TextSafe(Timetable.UpdateTimetable(usertxt, passtxt).Item1);
                usertxt = "";
                passtxt = "";
            });
            Userbox.Clear();
            Passbox.Clear();
        }

        private void SetLabel1TextSafe(string txt)
        {
            if (Errormsg.InvokeRequired)
            {
                Errormsg.Invoke(new Action(() => Errormsg.Text = txt));
                return;
            }
            Errormsg.Text = txt;
        }

        private void Settingsforms_Shown(object sender, EventArgs e)
        {
            Doublescheckbox.Checked = Program.SettingsData.Doubles;
            numericUpDown1.Value = Program.SettingsData.TimeOffset;
            SendMessage(Userbox.Handle, EM_SETCUEBANNER, 0, "Username");
            SendMessage(Passbox.Handle, EM_SETCUEBANNER, 0, "Password");
        }

        private void Passbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Loginbutton.PerformClick();
        }

        private void Settingsforms_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = (e.CloseReason == CloseReason.UserClosing);  // this cancels the close event.
        }

        private void Weekoverride_Click(object sender, EventArgs e)
        {
            if (Program.SettingsData.Dayoffset == 0)
            {
                Program.SettingsData.Dayoffset = 7;
            }
            else
            {
                Program.SettingsData.Dayoffset = 0;
            }

            Program.curDay = Program.Fetchday();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Program.SettingsData.TimeOffset = int.Parse(numericUpDown1.Value.ToString());
        }

        private void Doublescheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Program.SettingsData.Doubles = Doublescheckbox.Checked;
        }
    }
}
=======
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SplashScreen;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SchoolManager
{
    public partial class Settingsforms : Form
    {
        public int Download
        {
            get { return _Download; }
            set
            {
                if (100 > value && value > 0)
                {
                    progresslabel.Text = bytesreceived.ToString("0.###") + "/" + Bytesneeded.ToString("0.###") + " MB";
                    progressBar1.Visible = true;
                }
                else if (progressBar1.Visible)
                {
                    progressBar1.Visible = false;
                    if (value==100)
                        progresslabel.Text = "Downloaded";
                }

                _Download = value;
                progressBar1.Value = value;
            }
        }

        public decimal bytesreceived;
        public decimal Bytesneeded;

        private int _Download;
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);
        public Settingsforms()
        {
            InitializeComponent();
            label2.Text += Program.APP_VERSION;
        }

        private void Settingsforms_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }

        private void Loginbutton_Click(object sender, EventArgs e)
        {
            Errormsg.Text = "Attempting fetch...";
            string usertxt = Userbox.Text;
            string passtxt = Passbox.Text;
            Task.Run(() =>
            {
                SetLabel1TextSafe(Timetable.UpdateTimetable(usertxt, passtxt).Item1);
                usertxt = "";
                passtxt = "";
            });
            Userbox.Clear();
            Passbox.Clear();
        }

        private void SetLabel1TextSafe(string txt)
        {
            if (Errormsg.InvokeRequired)
            {
                Errormsg.Invoke(new Action(() => Errormsg.Text = txt));
                return;
            }
            Errormsg.Text = txt;
        }

        private void Settingsforms_Shown(object sender, EventArgs e)
        {
            Doublescheckbox.Checked = Program.SettingsData.Doubles;
            numericUpDown1.Value = Program.SettingsData.TimeOffset;
            SendMessage(Userbox.Handle, EM_SETCUEBANNER, 0, "Username");
            SendMessage(Passbox.Handle, EM_SETCUEBANNER, 0, "Password");
        }

        private void Passbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Loginbutton.PerformClick();
        }

        private void Settingsforms_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = (e.CloseReason == CloseReason.UserClosing);  // this cancels the close event.
        }

        private void Weekoverride_Click(object sender, EventArgs e)
        {
            if (Program.SettingsData.Dayoffset == 0)
            {
                Program.SettingsData.Dayoffset = 7;
            }
            else
            {
                Program.SettingsData.Dayoffset = 0;
            }

            Program.curDay = Program.Fetchday();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Program.SettingsData.TimeOffset = int.Parse(numericUpDown1.Value.ToString());
        }

        private void Doublescheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Program.SettingsData.Doubles = Doublescheckbox.Checked;
        }
    }
}
>>>>>>> 5bb2eea2085facea4119796ca3466cd8a6084482
