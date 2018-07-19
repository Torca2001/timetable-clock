using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SplashScreen
{

    static class Program
    {
        public static int curDay = 0;
        public static string Calltype = "lookup";
        public static int SynID = 000000;
        public static int APP_VERSION = 503; // Make sure for example 4.4 becomes 440 just get rid of each dot and make sure its 3 long 
        public static string CURRENT_DIRECTORY = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        public static string SETTINGS_DIRECTORY = "C:/Users/" + Environment.UserName + "/Documents/Timetable";
        public static settingstruct SettingsData = new settingstruct(new DateTime(2017,8,28,0,0,0), new DateTime(2017,1,1,0,0,0), Environment.UserName,false,false,0,true,1,0,0);
        public static List<Color> ColourTable = new List<Color>(new []{Color.Cyan, Color.DodgerBlue, Color.Orange, Color.Yellow, Color.Lime, Color.Green,Color.Red, Color.Tan, Color.Magenta,Color.Gray,Color.Teal,Color.Pink});
        public static Dictionary<string, Color> ColorRef = new Dictionary<string, Color>();
        public static Dictionary<string,period> TimetableList = new Dictionary<string, period>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                String thisExe = Assembly.GetExecutingAssembly().GetName().Name;
                AssemblyName embeddedAssembly = new AssemblyName(args.Name);
                String resourceName = thisExe + "." + embeddedAssembly.Name + ".dll";

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
            Application.Run(new frmSplash());
        }

        public static int Fetchday()
        {
            DateTime curdate = DateTime.Now;
            double calcschooldays = Math.Ceiling((curdate - SettingsData.Referencedayone).TotalDays+SettingsData.Dayoffset)%14;
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
                return SettingsData.Referencedayone;
            DateTime curdate = DateTime.Now;
            TimeSpan ts = new TimeSpan(curday-1,curdate.Hour,curdate.Minute,curdate.Second);
            curdate = curdate.Subtract(ts);
            if (curdate.DayOfWeek!=DayOfWeek.Monday)
                return SettingsData.Referencedayone;
            return curdate;
        }
    }
}
