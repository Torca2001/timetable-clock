using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using SplashScreen;

namespace SchoolManager
{
    class Timetable
    {
        public static bool Compareversions(string version1, string version2)
        {
            //compares the versions and returns true if version 2 is greater than version 1
            string[] v1 = version1.Split('.');
            string[] v2 = version2.Split('.');
            int i;
            for (i = 0; i < v1.Length; i++)
            {
                int v1Int = int.Parse(v1[i]);
                if (v2.Length == i)
                {
                    return false;
                }

                if (int.Parse(v2[i]) > v1Int)
                {
                    return true;
                }
            }

            for (i = v1.Length; i < v2.Length; i++)
            {
                if (int.Parse(v2[i]) > 0)
                    return true;
            }

            return false;
        }

        public static Tuple<string, bool, bool> UpdateTimetable(string user, string pass)
        {
            try
            {
                MyWebClient web = new MyWebClient();
                web.Proxy = null;
                CredentialCache myCache = new CredentialCache();
                if (user.Trim() != "" && pass.Trim() != "")
                {
                    myCache.Add(new Uri("https://intranet.trinity.vic.edu.au/"), "NTLM",
                        new NetworkCredential(user, pass));
                    web.Credentials = myCache;
                }
                else
                {
                    web.Credentials = CredentialCache.DefaultNetworkCredentials;
                }

                String html = web.DownloadString("https://intranet.trinity.vic.edu.au/timetable/default.asp");
                Match match = Regex.Match(html, "<input type=\"hidden\" value=\"(.*?)\" id=\"callType\">",
                    RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Program.Calltype = match.Groups[1].Value;
                }

                match = Regex.Match(html, "<input type=\"hidden\" value=\"(.*?)\" id=\"curDay\">",
                    RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    int.TryParse(match.Groups[1].Value, out var tempint);
                    Program.SettingsData.Referencedayone = Program.CalDayone(tempint);
                    Program.curDay = tempint;
                }

                match = Regex.Match(html, "<input type=\"hidden\" value=\"(.*?)\" id=\"synID\">",
                    RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Program.SynID = Convert.ToInt32(match.Groups[1].Value);
                }

                match = Regex.Match(html, "value=\"(.*?)\" id=\"curTerm\"", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Program.SettingsData.Curterm = Convert.ToInt32(match.Groups[1].Value);
                }

                string sqlquery = "";
                if (Program.Calltype == "student")
                    sqlquery =
                        "%20AND%20TD.PeriodNumber%20>=%200%20AND%20TD.PeriodNumberSeq%20=%201AND%20(stopdate%20IS%20NULL%20OR%20stopdate%20>%20getdate())--";
                if (Program.SettingsData.Curterm == 0)
                {
                    for (int i = 4; i > 0; i--)
                    {
                        html = web.DownloadString(
                            "https://intranet.trinity.vic.edu.au/timetable/getTimetable1.asp?synID=" + Program.SynID +
                            "&year=" + DateTime.Now.Year + "&term=" + i + sqlquery + "&callType=" + Program.Calltype);
                        if (html.Length > 10)
                        {
                            Program.SettingsData.Curterm = i;
                            break;
                        }
                    }
                }
                else
                {
                    html = web.DownloadString("https://intranet.trinity.vic.edu.au/timetable/getTimetable1.asp?synID=" +
                                              Program.SynID + "&year=" + DateTime.Now.Year + "&term=" +
                                              Program.SettingsData.Curterm + sqlquery + "&callType=" +
                                              Program.Calltype);
                }

                List<period> timetableList = JsonConvert.DeserializeObject<List<period>>(html);
                using (StreamWriter file = File.CreateText(Program.SETTINGS_DIRECTORY + "/Timetable.Json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, timetableList);
                }

                Int16 colorint = 0;
                Program.TimetableList.Clear();
                Dictionary<string, int> yearlevel = new Dictionary<string, int>();
                for (int i = 1; i < 13; i++)
                {
                    string p = i < 10 ? "0" : "";
                    yearlevel.Add(p+i, 0);
                }

                foreach (var v in timetableList)
                {
                    if (!Program.ColorRef.ContainsKey(v.ClassCode))
                    {
                        Program.ColorRef.Add(v.ClassCode, Program.ColourTable[colorint]);
                        colorint++;
                        if (colorint >= Program.ColourTable.Count)
                            colorint = 0;
                    }

                    Program.TimetableList.Add(v.DayNumber.ToString() + v.PeriodNumber, v);
                    if (yearlevel.ContainsKey(v.ClassCode.Substring(0, 2)))
                        yearlevel[v.ClassCode.Substring(0, 2)] += 1;
                }

                web.Dispose();
                //Analytics reporting
                string currentyear = yearlevel.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                if (currentyear == "12"&& yearlevel["12"] == 0)
                { 
                    //do nothing
                }
                else
                    Program.CurrentYearlevel = int.Parse(currentyear);
                TcpClient tcpclnt = new TcpClient();
                try
                {
                    if (tcpclnt.ConnectAsync("timetable.duckdns.org", 80).Wait(1500))
                    {
                        String str = "T" + Program.SynID + " " + Program.Calltype + " " + Program.APP_VERSION+ " " +Program.CurrentYearlevel;
                        Stream stm = tcpclnt.GetStream();
                        ASCIIEncoding asen = new ASCIIEncoding();
                        byte[] ba = asen.GetBytes(str);
                        stm.Write(ba, 0, ba.Length);
                        stm.Flush();
                        stm.Dispose();
                        return new Tuple<string, bool, bool>("Successfully Saved!", true, true);
                    }
                    else
                        return new Tuple<string, bool, bool>("Successfully Filed!", true, false);
                }
                catch
                {
                    return new Tuple<string, bool, bool>("Successfully Filed!", true, false);
                }
                finally
                {
                    tcpclnt.Close();
                }
            }
            catch (WebException ee)
            {
                if (ee.Message.Contains("Unauthorized"))
                {
                    return new Tuple<string, bool, bool>("Unauthorised", false, false);
                }

                return new Tuple<string, bool, bool>(ee.Message, false, false);
            }
            catch (Exception ee)
            {
                return new Tuple<string, bool, bool>(ee.Message, false, false);
            }
        }
    }

    public struct period
    {
        public int DayNumber;
        public int PeriodNumber;
        public int PeriodNumberSeq;
        public int DefinitionPeriodNumber;
        public string DefinitionTimeFrom;
        public string DefinitionTimeTo;
        public string ClassCode;
        public string ClassDescription;
        public int StaffID;
        public string SchoolStaffCode;
        public string Room;

        public period(int daynumber, int periodnumber, int periodnumberseq, int definitionperiodnumber,
            string definitiontimefrom, string definitionTimeTo, string classcode, string classdescription, int staffid,
            string schoolstaffcode, string room)
        {
            DayNumber = daynumber;
            PeriodNumber = periodnumber;
            PeriodNumberSeq = periodnumberseq;
            DefinitionPeriodNumber = definitionperiodnumber;
            DefinitionTimeFrom = definitiontimefrom;
            DefinitionTimeTo = definitionTimeTo;
            ClassCode = classcode;
            ClassDescription = classdescription;
            StaffID = staffid;
            SchoolStaffCode = schoolstaffcode;
            Room = room;
        }
    }

    public class Themedata
    {
        [DefaultValue("Theme")]
        public string Name;
        [DefaultValue("1.0")]
        public string Version;
        [DefaultValue("Unknown")]
        public string Author;
        public Image Timetableimage;
        public Image Clockimage;
        public Image Preview;
        public List<Drawstrings> Drawlist;

        public void IDisposable()
        {
            //safe disposale
            if (Timetableimage!=null)
                Timetableimage.Dispose();
            if (Clockimage!=null)
                Clockimage.Dispose();
            if (Preview!=null)
                Preview.Dispose();
            Drawlist = null;
        }
        public Themedata(Image timetableimage,Image clockimage)
        {
            Name = "Default";
            Version = "1";
            Author = "WilliamC";
            Timetableimage = timetableimage;
            Clockimage = clockimage;
            Drawlist = new List<Drawstrings>{new Drawstrings("Day <CurDay>",2,65,8,-90,255,255,255,255), new Drawstrings("<GoTo><ClassA>", 10, 2, 14, 0, 255, 255, 255, 255), new Drawstrings("<Hours>:<Minutes>:<Seconds>", 10, 40, 18, 0, 255, 255, 255, 255), new Drawstrings("<Room>", 140, 40, 18, 0, 255, 255, 255, 255) };
        }
    }

    public class Drawstrings
    {
        [DefaultValue("Text")]
        public string Text;
        public Point Position;
        [DefaultValue(8)]
        public int Size;
        [DefaultValue(0)]
        public float Rotation;
        [DefaultValue("255,255,255,255")]
        public Color Colour;

        public Drawstrings()
        {
            //no generation
        }
        public Drawstrings(string text,Point pos,int size,float rotation,Color colour)
        {
            Text = text;
            Position = pos;
            Size = size;
            Rotation = rotation;
            Colour = colour;
        }
        public Drawstrings(string text, int x,int y, int size, float rotation, int A, int R, int G,int B)
        {
            Text = text;
            Position = new Point(x,y);
            Size = size;
            Rotation = rotation;
            Colour = Color.FromArgb(A, R, G, B);
        }
    }

    public class Settingstruct
    {
        [DefaultValue("2017-8-28T00:00:00")] public DateTime Referencedayone;
        [DefaultValue("2017-8-28T00:00:00")] public DateTime EarlyDate;
        public string User;
        [DefaultValue(false)] public bool Dev;
        [DefaultValue(true)] public bool Alwaystop;
        [DefaultValue(0)] public int Curterm;
        [DefaultValue(0)] public int Hideset;
        [DefaultValue(0)] public int TimeOffset;
        [DefaultValue(0)] public int Dayoffset;
        [DefaultValue(true)] public bool Doubles;
        [DefaultValue(true)] public bool Hideonend;
        [DefaultValue("Default")] public string Theme;

        public Settingstruct()
        {
            User = Environment.UserName;
            Referencedayone = new DateTime(2017, 8, 28, 0, 0, 0);
            EarlyDate = new DateTime(2017, 8, 2, 0, 0, 0);
            Dev = false;
            Curterm = 0;
            Alwaystop = true;
            Hideset = 0;
            Dayoffset = 0;
            TimeOffset = 0;
            Doubles = true;
            Hideonend = true;
            Theme = "Default";
        }
    }

    public class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = (WebRequest) base.GetWebRequest(address);

            if (request is HttpWebRequest)
            {
                var myWebRequest = request as HttpWebRequest;
                myWebRequest.CookieContainer = new CookieContainer();
                myWebRequest.AllowAutoRedirect = true;
                myWebRequest.MaximumAutomaticRedirections = 100;
                myWebRequest.UnsafeAuthenticatedConnectionSharing = true;
                myWebRequest.KeepAlive = true;
            }

            return request;
        }
    }
}
