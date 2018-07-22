using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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
            for (i=v1.Length; i < v2.Length; i++)
            {
                if (int.Parse(v2[i])>0)
                    return true;
            }
            return false;
        }

        public static Tuple<string, bool,bool> UpdateTimetable(string user,string pass)
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
                Match match = Regex.Match(html, "<input type=\"hidden\" value=\"(.*?)\" id=\"callType\">", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Program.Calltype = match.Groups[1].Value;
                }
                match = Regex.Match(html, "<input type=\"hidden\" value=\"(.*?)\" id=\"curDay\">", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    int.TryParse(match.Groups[1].Value, out var tempint);
                    Program.SettingsData.Referencedayone = Program.CalDayone(tempint);
                    Program.curDay = tempint;
                }
                match = Regex.Match(html, "<input type=\"hidden\" value=\"(.*?)\" id=\"synID\">", RegexOptions.IgnoreCase);
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
                        html = web.DownloadString("https://intranet.trinity.vic.edu.au/timetable/getTimetable1.asp?synID=" + Program.SynID + "&year=" + DateTime.Now.Year + "&term=" + i + sqlquery + "&callType=" + Program.Calltype);
                        if (html.Length > 10)
                        {
                            Program.SettingsData.Curterm = i;
                            break;
                        }
                    }
                }
                else
                {
                    html = web.DownloadString("https://intranet.trinity.vic.edu.au/timetable/getTimetable1.asp?synID=" + Program.SynID + "&year=" + DateTime.Now.Year + "&term=" + Program.SettingsData.Curterm + sqlquery + "&callType=" + Program.Calltype);
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
                }
                web.Dispose();
                //Analytics reporting
                TcpClient tcpclnt = new TcpClient();
                try
                {
                    if (tcpclnt.ConnectAsync("timetable.duckdns.org", 80).Wait(1500))
                    {
                        String str = "T" + Program.SynID + " " + Program.Calltype + " " + Program.APP_VERSION;
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
                    return new Tuple<string, bool,bool>("Unauthorised", false,false);
                }
                return new Tuple<string, bool,bool>(ee.Message, false,false);
            }
            catch (Exception ee)
            {
                return new Tuple<string, bool, bool>(ee.Message, false, false);
            }
        }
    }
}
