using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using SplashScreen;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SchoolManager
{
    public partial class Settingsforms : Form
    {
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
                    Program.curDay = Convert.ToInt32(match.Groups[1].Value);
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
                myCache.Add(new Uri("https://intranet.trinity.vic.edu.au/timetable/getTimetable1.asp?synID=" + Program.SynID + "&year=" + DateTime.Now.Year + "&term=" + Program.curTerm + "&callType=" + Program.Calltype), "NTLM", new NetworkCredential(Userbox.Text, Passbox.Text));
                web.Credentials = myCache;
                html = web.DownloadString("https://intranet.trinity.vic.edu.au/timetable/getTimetable1.asp?synID=" + Program.SynID + "&year=" + DateTime.Now.Year + "&term=" + Program.curTerm + "&callType=" + Program.Calltype);
                List<period> timetableList = JsonConvert.DeserializeObject<List<period>>(html);
                foreach (var V in timetableList)
                {
                    Program.timetableList.Add(V.DayNumber.ToString() + V.PeriodNumber, V);
                }
                web.Dispose();
                Errormsg.Text = "Successfully extracted! " + Program.timetableList["106"].ClassCode;
            }
            catch (WebException ee)
            {
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
    }
}
