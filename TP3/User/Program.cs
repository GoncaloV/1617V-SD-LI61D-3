using System;
using System.Configuration;
using System.Windows.Forms;

namespace User
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Main(args.Length > 0 ? args : new string[1] { ConfigurationManager.AppSettings["URI"] }, args.Length > 0));

        }
    }

    public class Helper
    {

        //Only used by the serversInitializer Project
        public String runningPath()
        {
            return Environment.CurrentDirectory;
        }

    }
}
