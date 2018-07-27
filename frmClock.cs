using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Security.Authentication;
using System.Windows.Forms.VisualStyles;
using SchoolManager;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octokit;
using Application = System.Windows.Forms.Application;
using Timer = System.Windows.Forms.Timer;

namespace SplashScreen
{
    public partial class frmSplash : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(HandleRef hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);
        Settingsforms settingsForm = new Settingsforms();
        Expanded Expandedform = new Expanded();
        public int counter = 255;
        public int dayo = 0;
        float dx;
        private DayOfWeek lastDayOfWeek = DayOfWeek.Sunday;
        List<List<string>> timelayout = new List<List<string>>();

        public frmSplash()
        {
            timelayout.Add(new List<string> { "29700", "29700", "29700", "School Start" }); //Dayo = 1 is wednesday, dayo= 2 is Early finish
            timelayout.Add(new List<string> { "31500", "30300", "31500", "Form                                                                     0" });
            timelayout.Add(new List<string> { "31800", "30600", "31800", "Go to Period 1" });
            timelayout.Add(new List<string> { "34800", "33600", "34500", "Period 1" });
            timelayout.Add(new List<string> { "35100", "33900", "34800", "Go to Period 2" });
            timelayout.Add(new List<string> { "38100", "36900", "37500", "Period 2" });
            timelayout.Add(new List<string> { "39000", "37800", "38400", "Recess" });
            timelayout.Add(new List<string> { "39300", "38100", "38700", "Go to Period 3" });
            timelayout.Add(new List<string> { "42300", "41100", "41400", "Period 3" });
            timelayout.Add(new List<string> { "42600", "41400", "41700", "Go to Period 4" });
            timelayout.Add(new List<string> { "45600", "44100", "44400", "Period 4" });
            timelayout.Add(new List<string> { "48300", "48300", "46500", "Lunch" });
            timelayout.Add(new List<string> { "48600", "48600", "46800", "Go to Period 5" });
            timelayout.Add(new List<string> { "51600", "51600", "49500", "Period 5" });
            timelayout.Add(new List<string> { "51900", "51900", "49800", "Go to Period 6" });
            timelayout.Add(new List<string> { "54900", "54900", "52500", "Period 6" });
            bool timetableexist = File.Exists(Program.SETTINGS_DIRECTORY + "/Timetable.Json");
            
            //stuff for moving the window
            bool mouseDown;
            Point lastLocation;

           //lets you close the timetable by clicking back on the window
            bool ttIsOpen;

            //bool for stuff requiring mouse movement
            bool mouseMoved;
            
            try
            {
                using (StreamWriter writer =
                    new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\TimetableClock" +
                                     ".url"))
                {
                    string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    writer.WriteLine("[InternetShortcut]");
                    writer.WriteLine("URL=file:///" + app);
                    writer.WriteLine("IconIndex=0");
                    string icon = app.Replace('\\', '/');
                    writer.WriteLine("IconFile=" + icon);
                    writer.Flush();
                }
            }
            catch
            {
                Console.WriteLine("Shortcut creation failed");
            }

            try
            {
                Program.ColorRef = JsonConvert.DeserializeObject<Dictionary<string, Color>>(
                    File.ReadAllText(Program.SETTINGS_DIRECTORY + "/Colours.Json"),
                    new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.Populate});
            }
            catch
            {

            }
            if (File.Exists(Program.SETTINGS_DIRECTORY + "/Settings.Json"))
            {
                try
                {
                    Program.SettingsData = JsonConvert.DeserializeObject<Settingstruct>(File.ReadAllText(Program.SETTINGS_DIRECTORY + "/Settings.Json"), new JsonSerializerSettings{ DefaultValueHandling = DefaultValueHandling.Populate });
                }
                catch
                {
                    //Disregard corrupted settings
                }
                if (Program.SettingsData.User != Environment.UserName)
                {
                    Console.WriteLine("The settings were for the wrong user.");
                    Program.SettingsData = new Settingstruct();
                    MessageBox.Show("Welcome " + Environment.UserName + @"!  Thanks for using the program!", "Welcome!");
                    if (timetableexist)
                    {
                        File.Delete(Program.SETTINGS_DIRECTORY + "/Timetable.Json");
                        timetableexist = false;
                    }
                } 

            }
            else
            {
                MessageBox.Show("Welcome "+Environment.UserName+@"!  Thanks for using the program!", "Welcome!");
                if (timetableexist)
                {
                    File.Delete(Program.SETTINGS_DIRECTORY + "/Timetable.Json");
                    timetableexist = false;
                }
            }
            if (timetableexist)
            {
                try
                {
                    List<period> TimetableListtemp =
                        JsonConvert.DeserializeObject<List<period>>(
                            File.ReadAllText(Program.SETTINGS_DIRECTORY + "/Timetable.Json"));
                    Int16 colorint = 0;
                    Program.TimetableList.Clear();
                    foreach (var v in TimetableListtemp)
                    {
                        if (!Program.ColorRef.ContainsKey(v.ClassCode))
                        {
                            Program.ColorRef.Add(v.ClassCode, Program.ColourTable[colorint]);
                            colorint++;
                            if (colorint >= Program.ColourTable.Count)
                                colorint = 0;
                        }

                        if (Program.TimetableList.ContainsKey(v.DayNumber.ToString() + v.PeriodNumber))
                            Program.TimetableList.Remove(v.DayNumber.ToString() + v.PeriodNumber);
                        Program.TimetableList.Add(v.DayNumber.ToString() + v.PeriodNumber, v);
                    }
                }
                catch
                {
                    //Disregard corrupted file
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(Program.SETTINGS_DIRECTORY);
                }
                catch
                {
                    //do nothing
                }
            }
            InitializeComponent();
            ttIsOpen = false; //setting the variable for whether the timetable window is opened to be false by default
            /*
            WebBrowser mytgs = new WebBrowser();
            mytgs.Navigate(new Uri("http://mytgs.trinity.vic.edu.au/dashboard#section-id-29"));
            while (mytgs.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            Console.WriteLine(mytgs.DocumentText);
            */
            if (!Program.SettingsData.Alwaystop)
                toolStripMenuItem1.Checked = false;
            switch (Program.SettingsData.Hideset)
            {
                case 1:
                    autoToolStripMenuItem.PerformClick();
                    break;
                case 2:
                    dontHideToolStripMenuItem.PerformClick();
                    break;
                case 3:
                    hideToolStripMenuItem.PerformClick();
                    break;
            }
            MouseClick += mouseClick;
            InitTimer();
            ShowInTaskbar = false;
            AutoScaleDimensions = new SizeF(6F, 13F);
            Graphics g = CreateGraphics();
            try
            {
                dx = 96/g.DpiX;
            }
            finally
            {
                g.Dispose();
            }
            new Task(() =>
            {
                Timetable.UpdateTimetable("", "");
                if (Program.TimetableList.Count == 0)
                {
                    if (File.Exists(Program.SETTINGS_DIRECTORY + "/Timetable.Json"))
                        notifyIcon1.BalloonTipText = "Unable to fetch timetable";
                    notifyIcon1.ShowBalloonTip(1000);
                }
            }).Start();
            Githubupdate("Mrmeguyme", "timetable-clock", false);
        }

        public List<string> Currentcountdown()
        {
            int offset = Program.SettingsData.TimeOffset;
            int i = 0;
            DateTime timenow = DateTime.Now;
            int tleft = 0;
            string outt;
            if (lastDayOfWeek != timenow.DayOfWeek)
            {
                lastDayOfWeek = timenow.DayOfWeek;
                Program.curDay = Program.Fetchday();
            }
            if (Program.SettingsData.EarlyDate.Date == timenow.Date)
                dayo = 2;
            else if (timenow.DayOfWeek == DayOfWeek.Wednesday)
                dayo = 1;
            else
                dayo = 0;
            for (; i < timelayout.Count; i++)
            {
                tleft = Int32.Parse(timelayout[i][0 + dayo]) - (timenow.Hour * 3600 + timenow.Minute * 60 + timenow.Second - offset);
                if (tleft > 0)
                {
                    if (!Program.SettingsData.Doubles)
                        break;
                    switch (i)
                        {
                            case 3:
                            case 4:
                                if (Program.TimetableList.ContainsKey(Program.curDay + "1") &&
                                    Program.TimetableList.ContainsKey(Program.curDay + "2") &&
                                    Program.TimetableList[Program.curDay + "1"].ClassCode ==
                                    Program.TimetableList[Program.curDay + "2"].ClassCode)
                                {
                                    tleft = Int32.Parse(timelayout[5][0 + dayo]) -
                                            (timenow.Hour * 3600 + timenow.Minute * 60 + timenow.Second - offset);
                                    i = 5;
                                }

                                break;
                            case 8:
                            case 9:
                                if (Program.TimetableList.ContainsKey(Program.curDay + "3") &&
                                    Program.TimetableList.ContainsKey(Program.curDay + "4") &&
                                    Program.TimetableList[Program.curDay + "3"].ClassCode ==
                                    Program.TimetableList[Program.curDay + "4"].ClassCode)
                                {
                                    tleft = Int32.Parse(timelayout[10][0 + dayo]) -
                                            (timenow.Hour * 3600 + timenow.Minute * 60 + timenow.Second - offset);
                                    i = 10;
                                }
                                break;
                            case 13:
                            case 14:
                                if (Program.TimetableList.ContainsKey(Program.curDay + "5") &&
                                    Program.TimetableList.ContainsKey(Program.curDay + "6") &&
                                    Program.TimetableList[Program.curDay + "5"].ClassCode ==
                                    Program.TimetableList[Program.curDay + "6"].ClassCode)
                                {
                                    tleft = Int32.Parse(timelayout[15][0 + dayo]) -
                                            (timenow.Hour * 3600 + timenow.Minute * 60 + timenow.Second - offset);
                                    i = 15;
                                }

                                break;
                        }
                    break;
                }
            }
            if (i==16)
            {
                tleft = 0;
                outt = "End";
            }
            else
            {
                outt = timelayout[i][3];
            }
            List<string> result = new List<string> { tleft.ToString(), i.ToString(), outt};
            return result;


        }
        /*
        struct ppands
        {
            public int p;
            public int pp;
            public string s;
        }
        */

        #region CUSTOM PAINT METHODS ----------------------------------------------
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00080000; // Required: set WS_EX_LAYERED extended style
                return cp;
            }
        }

        //Updates the Form's display using API calls
        public void UpdateFormDisplay(Image backgroundImage)
        {
            IntPtr screenDc = API.GetDC(IntPtr.Zero);
            IntPtr memDc = API.CreateCompatibleDC(screenDc);
            IntPtr hBitmap;
            IntPtr oldBitmap;

            try
            {
                //Display-image
                Bitmap bmp = new Bitmap(backgroundImage);
                Graphics g = Graphics.FromImage(bmp);
                
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                var brushc = new System.Drawing.SolidBrush(Color.FromArgb(255, 255, 255, 255));
                if (counter != 0)
                {
                    List<string> temp = Currentcountdown();
                    double timeleft = Int32.Parse(temp[0]);
                    if (temp[2] == "End")
                        counter = 0;

                    string hours;
                    string minutes;
                    string seconds;
                    if (Math.Floor(timeleft / 3600) < 10)
                    {
                        hours = "0" + (Math.Floor(timeleft / 3600));
                        timeleft %= 3600;
                    }
                    else
                    {
                        hours = (Math.Floor(timeleft / 3600)).ToString();
                        timeleft %= 3600;
                    }

                    if (Math.Floor(timeleft / 60) < 10)
                    {
                        minutes = "0" + (Math.Floor(timeleft / 60));
                    }
                    else
                    {
                        minutes = (Math.Floor(timeleft / 60)).ToString();
                    }

                    if (Math.Floor(timeleft % 60) < 10)
                    {
                        seconds = "0" + (timeleft % 60);
                    }
                    else
                    {
                        seconds = (timeleft % 60).ToString();
                    }

                    g.DrawString(hours + ":" + minutes + ":" + seconds, new Font("Trebuchet MS", 18 * dx), brushc, 10,
                        40);
                    g.DrawString(
                        Program.TimetableList.ContainsKey(Program.curDay + "" + temp[2].Substring(temp[2].Length - 1))
                            ? Program.TimetableList[Program.curDay + "" + temp[2].Substring(temp[2].Length - 1)].Room
                            : "", new Font("Trebuchet MS", 18 * dx), brushc, 140, 40);
                    string label = temp[2];
                    if ((temp[2].StartsWith("Period") || temp[2].StartsWith("Form")) &&
                        Program.TimetableList.ContainsKey(Program.curDay + "" + temp[2].Substring(temp[2].Length - 1)))
                        label =
                            Program.TimetableList[Program.curDay + "" + temp[2].Substring(temp[2].Length - 1)]
                                .ClassDescription.Length > 12
                                ? Program.TimetableList[Program.curDay + "" + temp[2].Substring(temp[2].Length - 1)]
                                    .ClassCode
                                : Program.TimetableList[Program.curDay + "" + temp[2].Substring(temp[2].Length - 1)]
                                    .ClassDescription;
                    if (temp[2].StartsWith("Go to") &&
                        Program.TimetableList.ContainsKey(Program.curDay + "" + temp[2].Substring(temp[2].Length - 1)))
                        label = "Go to " + Program
                                    .TimetableList[Program.curDay + "" + temp[2].Substring(temp[2].Length - 1)]
                                    .ClassCode;
                    g.DrawString(label, new Font("Trebuchet MS", 14 * dx), brushc, 10, 2);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                    g.RotateTransform(-90);
                    string k = "Day " + Program.curDay;
                    g.DrawString(k, new Font("Trebuchet MS", 8 * dx), brushc, -63, 2);
                    stringFormat.Dispose();
                    
                }

                hBitmap = bmp.GetHbitmap(Color.FromArgb(0));  //Set the fact that background is transparent
                oldBitmap = API.SelectObject(memDc, hBitmap);

                //Display-rectangle
                Size size = bmp.Size;
                Point pointSource = new Point(0, 0);
                Point topPos = new Point(Left, Top);

                //Set up blending options
                API.BLENDFUNCTION blend = new API.BLENDFUNCTION();
                blend.BlendOp = API.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = Convert.ToByte(counter);
                blend.AlphaFormat = API.AC_SRC_ALPHA;

                API.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, API.ULW_ALPHA);

                //Clean-up
                brushc.Dispose();
                g.Dispose();
                bmp.Dispose();
                API.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    API.SelectObject(memDc, oldBitmap);
                    API.DeleteObject(hBitmap);
                }
                API.DeleteDC(memDc);
            }
            catch (Exception)
            {
                // ignored
            }
        } 
        #endregion

        #region MENU EVENTS -------------------------------------------------------
        private void mHomepage_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://mrmeguyme.github.io/timetable-clock/#controls--tips");
        }

        private void mExit_Click(object sender, EventArgs e)
        {
            Close(); //no extra commands are required
        }

        #endregion

        #region FORM EVENTS -------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateFormDisplay(BackgroundImage);
            SetDesktopLocation(Screen.PrimaryScreen.Bounds.Width - 208, Screen.PrimaryScreen.WorkingArea.Height-66);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            //Call our drawing function
            UpdateFormDisplay(BackgroundImage);
        }
        #endregion

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if(ttIsOpen == false) //only opens if the window isn't already open
            {
                if (mouseMoved == false)
                {
                    Expandedform.Show();
                    Expandedform.Activate();
                    ttIsOpen = true;
                }
            }
            else //closes the window if it's already open instead of opening a new window
            {
                Expandedform.Close();
                ttIsOpen = false;
            }
        }

        private Timer _timer1;
        private Timer _timer2;
        public void InitTimer()
        {
            
            _timer1 = new Timer();
            _timer1.Tick += timer1_Tick;
            _timer1.Interval = 10; // in miliseconds
            _timer1.Start();
            _timer2 = new Timer();
            _timer2.Tick += timer2_Tick;
            _timer2.Interval = 600000;
            _timer2.Start();

        }

            private void timer1_Tick(object sender, EventArgs e)
        {
            if (Program.curDay == 0)
            {
                counter = 0;
                UpdateFormDisplay(BackgroundImage);
                return;
            }
            if (Left < Cursor.Position.X - 12 && Left + Width > System.Windows.Forms.Cursor.Position.X && Top < System.Windows.Forms.Cursor.Position.Y && Top + Height > System.Windows.Forms.Cursor.Position.Y&&dontHideToolStripMenuItem.Checked==false)
            {
                if (autoToolStripMenuItem.Checked)
                {
                    counter = 0;
                }
                if (counter > 0)
                {
                    counter -= 25;
                }
                else
                {
                    counter = 0;
                }

                //this.Visible = false;
            }
            else if (dontHideToolStripMenuItem.Checked)
                {
                counter = 255;
                }
            else
            {   
                if (autoToolStripMenuItem.Checked)
                {
                    counter = 255;
                }
                if (counter < 255)
                {
                    counter += 25;
                }
                else
                {
                    counter = 255;
                }

                //this.Visible = true;
            }
            if (counter > 255)
            {
                counter = 255;
            }
            if (counter < 0)
            {
                counter = 0;
            }
            if (hideToolStripMenuItem.Checked)
            {
                Visible = false;
            }
            else
            {
                Visible = true;
            }
            int capacity = GetWindowTextLength(new HandleRef(this, GetForegroundWindow())) * 2;
            StringBuilder stringBuilder = new StringBuilder(capacity);
            GetWindowText(new HandleRef(this, GetForegroundWindow()), stringBuilder, stringBuilder.Capacity);
            Rectangle rect;
            GetWindowRect(GetForegroundWindow(), out rect);
            if (toolStripMenuItem1.Checked&&!settingsForm.Visible && !Expandedform.Visible&&!contextMenu1.Visible&&(((rect.Height - rect.Y - Screen.FromHandle(Handle).Bounds.Height) !=0&& (rect.Width - rect.X- Screen.FromHandle(Handle).Bounds.Width) != 0)||Screen.FromHandle(GetForegroundWindow()).DeviceName!=Screen.FromHandle(Handle).DeviceName||stringBuilder.Length==0))
                TopMost = true;
            else
            {
                if (!settingsForm.Visible && !Expandedform.Visible && !contextMenu1.Visible&&stringBuilder.Length!=0&&TopMost)
                {
                    TopMost = false;
                    SendToBack();
                }
            }
            UpdateFormDisplay(BackgroundImage);
            //this.SetDesktopLocation(System.Windows.Forms.Cursor.Position.X-100, System.Windows.Forms.Cursor.Position.Y-30);
  
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Githubupdate("Mrmeguyme", "timetable-clock", false);
        }

        private void mMove_Click_1(object sender, EventArgs e)
        {
            //this.SetDesktopLocation(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);

            // Show the settings form           
            //settingsForm.Show();
            
        }

        private void dontHideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dontHideToolStripMenuItem.Checked = true;
            autoToolStripMenuItem.Checked = false;
            autoAnimatedToolStripMenuItem.Checked = false;
        }

        private void autoAnimatedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoAnimatedToolStripMenuItem.Checked = true;
            dontHideToolStripMenuItem.Checked = false;
            autoToolStripMenuItem.Checked = false;
        }

        private void autoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoToolStripMenuItem.Checked = true;
            autoAnimatedToolStripMenuItem.Checked = false;
            dontHideToolStripMenuItem.Checked = false;
        }

        private void frmSplash_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Saving of settings to local directory
                _timer1.Dispose();
                notifyIcon1.Visible = false;
                Program.SettingsData.Alwaystop = toolStripMenuItem1.Checked;
                if (dontHideToolStripMenuItem.Checked)
                    Program.SettingsData.Hideset = 2;
                else if (autoToolStripMenuItem.Checked)
                    Program.SettingsData.Hideset = 1;
                else if (hideToolStripMenuItem.Checked)
                    Program.SettingsData.Hideset = 3;
                else
                    Program.SettingsData.Hideset = 0;
                notifyIcon1.Dispose();
                using (StreamWriter file = File.CreateText(Program.SETTINGS_DIRECTORY + "/Settings.Json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, Program.SettingsData);
                }
                using (StreamWriter file = File.CreateText(Program.SETTINGS_DIRECTORY + "/Colours.Json"))
                {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, Program.ColorRef);
                }
                settingsForm.Dispose();
                Expandedform.Dispose();
                Dispose();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm.Show();
            settingsForm.Activate();
        }

        private void frmSplash_Activated(object sender, EventArgs e)
        {
            int capacity = GetWindowTextLength(new HandleRef(this, GetForegroundWindow())) * 2;
            StringBuilder stringBuilder = new StringBuilder(capacity);
            GetWindowText(new HandleRef(this, GetForegroundWindow()), stringBuilder, stringBuilder.Capacity);
            if (!toolStripMenuItem1.Checked&&stringBuilder.Length!=0)
                SendToBack();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1.Checked = !toolStripMenuItem1.Checked;
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            settingsForm.Show();
            settingsForm.Activate();
        }

        public string Githubupdate(string owner, string name,bool check)
        {
            try
            {
                var header = new ProductHeaderValue("TimeteAuto");
                GitHubClient client = new GitHubClient(header);
                var release = client.Repository.Release.GetAll(owner, name);
                Release latest = new Release();
                if (!Program.SettingsData.Dev)
                {
                    foreach (var v in release.Result)
                    {
                        if (!v.Prerelease)
                        {
                            latest = v;
                            break;
                        }
                    }
                }
                else
                {
                    latest = release.Result[0];
                }

                if (latest.TagName == null || !Timetable.Compareversions(Program.APP_VERSION, latest.TagName))
                    return "Up to date";
                WebClient downloader = new WebClient();
                Console.WriteLine("Downloading update");
                downloader.DownloadFileAsync(new Uri(latest.Assets[0].BrowserDownloadUrl),
                    Program.SETTINGS_DIRECTORY + "/NewTimetableclock.exe");
                downloader.DownloadProgressChanged += (s, e) =>
                {
                    settingsForm.Download = e.ProgressPercentage;
                    settingsForm.Bytesneeded = Decimal.Divide(e.TotalBytesToReceive, 1048576);
                    settingsForm.bytesreceived = Decimal.Divide(e.BytesReceived, 1048576);
                };
                downloader.DownloadFileCompleted += delegate
                {
                    if (File.Exists(Program.SETTINGS_DIRECTORY + "/delete.exe"))
                        File.Delete(Program.SETTINGS_DIRECTORY + "/delete.exe");
                    Console.WriteLine(Program.CURRENT_DIRECTORY + "/SchoolManager.exe");
                    File.Move(Program.CURRENT_DIRECTORY + "/SchoolManager.exe", Program.CURRENT_DIRECTORY+"/delete.exe");
                    File.Move(Program.SETTINGS_DIRECTORY+"/NewTimetableclock.exe", Program.CURRENT_DIRECTORY+"/SchoolManager.exe");
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = "SchoolManager.exe";
                    processInfo.WorkingDirectory = Program.CURRENT_DIRECTORY;
                    Process.Start(processInfo);
                    ProcessStartInfo Info = new ProcessStartInfo();
                    Console.WriteLine(Program.CURRENT_DIRECTORY);
                    Info.Arguments = "/C timeout /t 3 & Del \"" + Program.CURRENT_DIRECTORY.Replace("/","\\")+"\"\\delete.exe";
                    Info.WindowStyle = ProcessWindowStyle.Hidden;
                    Info.CreateNoWindow = true;
                    Info.FileName = "cmd.exe";
                    Process.Start(Info);
                    Close();
                };
                return "Updating";
            }
            catch
            {
                return "Failed";
            }
        }

        private void frmSplash_Shown(object sender, EventArgs e)
        {
            Left += 1;
        }

        private void frmSplash_MouseDown(object sender, MouseEventArgs e)
        {
            mouseMoved = false;
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void frmSplash_MouseMove(object sender, MouseEventArgs e)
        {
            mouseMoved = true;
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }   
        }

        private void frmSplash_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        }
    }


    internal class API
    {
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;
        public const Int32 ULW_ALPHA = 0x00000002;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);


        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
