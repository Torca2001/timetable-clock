using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SplashScreen
{
    static class Program
    {
        public static int curDay = 0;
        public static string Calltype = "lookup";
        public static int SynID = 000000;
        public static int curTerm = 0;
        public static double AppVersion = 4.4;
        public static settingstruct Settingsdata = new settingstruct(new DateTime(2017,8,28,0,0,0), new DateTime(2017,1,1,0,0,0), Environment.UserName,false );
        public static List<Color> Colourtable = new List<Color>(new Color[]{Color.Cyan, Color.DodgerBlue, Color.Orange, Color.Yellow, Color.Lime, Color.Green,Color.Red, Color.Purple, Color.Magenta,Color.Gray,Color.Teal,Color.Pink});
        public static Dictionary<string, Color> Colorref = new Dictionary<string, Color>();
        public static Dictionary<string,period> timetableList = new Dictionary<string, period>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmSplash());
        }

        public static int Fetchday()
        {
            DateTime curdate = DateTime.Now;
            double calcschooldays = Math.Ceiling((curdate - Settingsdata.Referencedayone).TotalDays)%14;
            switch (calcschooldays)
            {
                case 6:
                case 7:
                case 13:
                    calcschooldays = 0;
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    calcschooldays -= 2;
                    break;
            }
            return (int)calcschooldays;
        }

        public static DateTime CalDayone(int curday)
        {
            if (curday >= 6)
                curday += 2;
            if (curday == 0)
                return Settingsdata.Referencedayone;
            DateTime curdate = DateTime.Now;
            TimeSpan ts = new TimeSpan(curday-1,curdate.Hour,curdate.Minute,curdate.Second);
            curdate = curdate.Subtract(ts);
            if (curdate.DayOfWeek!=DayOfWeek.Monday)
                return Settingsdata.Referencedayone;
            return curdate;
        }
    }
}