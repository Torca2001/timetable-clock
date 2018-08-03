using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
        private bool Savebox = false;
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
            if (!Savebox)
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
            Transparencyupdown.Value = Program.SettingsData.Transparency;
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

        private void Hideonend_CheckedChanged(object sender, EventArgs e)
        {
            Program.SettingsData.Hideonend = Hideonend.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Savebox = true;
            DialogResult results = openFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                if (Program.BackImage != null)
                {
                    Program.BackImage.Dispose();
                    Program.BackImage = null;
                }
                if (File.Exists(Program.SETTINGS_DIRECTORY+"/Backimage.png"))
                    File.Delete(Program.SETTINGS_DIRECTORY+"/Backimage.png");
                File.Copy(openFileDialog1.FileName,Program.SETTINGS_DIRECTORY+"/Backimage.png");
                try
                {
                    Program.BackImage = ResizeImage(Image.FromFile(Program.SETTINGS_DIRECTORY + "/Backimage.png"), 1180,
                        590);

                }
                catch
                {
                    MessageBox.Show("Unable to read image file!");
                }
            }

            Savebox = false;
        }

        private void Transparencyupdown_ValueChanged(object sender, EventArgs e)
        {
            Program.SettingsData.Transparency = (int)Transparencyupdown.Value;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("dispose!");
            if (Program.BackImage != null)
                Program.BackImage.Dispose();
                Program.BackImage = null;
            if (File.Exists(Program.SETTINGS_DIRECTORY + "/Backimage.png"))
                File.Delete(Program.SETTINGS_DIRECTORY + "/Backimage.png");
        }
    }

}