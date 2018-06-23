using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SplashScreen;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SchoolManager
{
    public partial class Settingsforms : Form
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);
        public Settingsforms()
        {
            InitializeComponent();
        }

        private void Settingsforms_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }

        private void Loginbutton_Click(object sender, EventArgs e)
        {
            try
            {
                MyWebClient web = new MyWebClient();
                web.Proxy = null;
                CredentialCache myCache = new CredentialCache();
                myCache.Add(new Uri("https://intranet.trinity.vic.edu.au/timetable/default.asp"), "NTLM", new NetworkCredential(Userbox.Text, Passbox.Text));
                web.Credentials = myCache;
                String html = web.DownloadString("https://intranet.trinity.vic.edu.au/timetable/default.asp");
                Match match = Regex.Match(html, "<input type=\"hidden\" value=\"(.*?)\" id=\"callType\">", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Program.Calltype = match.Groups[1].Value;
                }
                match = Regex.Match(html, "<input type=\"hidden\" value=\"(.*?)\" id=\"curDay\">", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    int.TryParse(match.Groups[1].Value, out Program.curDay);
                }
                match = Regex.Match(html, "<input type=\"hidden\" value=\"(.*?)\" id=\"synID\">", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Program.SynID = Convert.ToInt32(match.Groups[1].Value);
                }
                match = Regex.Match(html, "value=\"(.*?)\" id=\"curTerm\"", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Program.curTerm = Convert.ToInt32(match.Groups[1].Value);
                }
                myCache.Add(new Uri("https://intranet.trinity.vic.edu.au/timetable/getTimetable1.asp?synID=" + Program.SynID + "&year=" + DateTime.Now.Year + "&term=" + Program.curTerm + @"%20AND%20TD.PeriodNumber%20>=%200%20AND%20TD.PeriodNumberSeq%20=%201--&callType=" + Program.Calltype), "NTLM", new NetworkCredential(Userbox.Text, Passbox.Text));
                web.Credentials = myCache;
                html = web.DownloadString("https://intranet.trinity.vic.edu.au/timetable/getTimetable1.asp?synID=" + Program.SynID + "&year=" + DateTime.Now.Year + "&term=" + Program.curTerm + @"%20AND%20TD.PeriodNumber%20>=%200%20AND%20TD.PeriodNumberSeq%20=%201--&callType=" + Program.Calltype);
                List<period> timetableList = JsonConvert.DeserializeObject<List<period>>(html);
                using (StreamWriter file = File.CreateText(Environment.CurrentDirectory + "/Timetable.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, timetableList);
                }
                Int16 colorint = 0;
                foreach (var V in timetableList)
                {
                    if (Program.timetableList.ContainsKey(V.DayNumber.ToString() + V.PeriodNumber))
                        Program.timetableList.Remove(V.DayNumber.ToString() + V.PeriodNumber);
                    if (!Program.Colorref.ContainsKey(V.ClassCode))
                    {
                        Program.Colorref.Add(V.ClassCode,Program.Colourtable[colorint]);
                        colorint++;
                        if (colorint >= Program.Colourtable.Count)
                            colorint = 0;
                    }
                    Program.timetableList.Add(V.DayNumber.ToString() + V.PeriodNumber, V);
                }
                web.Dispose();
                Errormsg.Text = "Successfully extracted! ";
            }
            catch (WebException ee)
            {
                Errormsg.Text = ee.Message;
                if (ee.Message.Contains("Unauthorized"))
                {
                    Errormsg.Text = "Authorization failed";
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }

            Userbox.Text = "Username-ID";
            Passbox.Text = "Password";

        }

        private void Settingsforms_Shown(object sender, EventArgs e)
        {
            SendMessage(Userbox.Handle, EM_SETCUEBANNER, 0, "Username");
            SendMessage(Passbox.Handle, EM_SETCUEBANNER, 0, "Password");
        }
    }
}
