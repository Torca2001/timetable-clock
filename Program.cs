using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace SplashScreen
{
    static class Program
    {
        public static int curDay = 0;
        public static string Calltype = "lookup";
        public static int SynID = 000000;
        public static int curTerm = 0;
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
    }
}