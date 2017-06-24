using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace UserInitializer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<String> serverListKey = ConfigurationManager.AppSettings.AllKeys.ToList();

            String[] users = new String[9];

            users[0] = "Andre";
            users[1] = "pt";
            users[2] = "http://localhost:8765/ServiceChat";

            users[3] = "Goncalo";
            users[4] = "en";
            users[4] = "http://localhost:8764/ServiceChat";


            users[6] = "Ruben";
            users[7] = "fr";
            users[8] = "http://localhost:8763/ServiceChat";


            int number = 0;

            Int32.TryParse(ConfigurationManager.AppSettings.Get(serverListKey.ElementAt(0)), out number);

            Helper helper = new Helper();
            String path = helper.runningPath();

            //Black Magic.Same as TP1
            path = path.Substring(0, path.Length - 29);

            path += "TP3\\User\\bin\\Debug";

            for (int i = 0; i < number*3; i = i+3)
            {

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = true;
                psi.LoadUserProfile = true;
                psi.WorkingDirectory = path;
                psi.FileName = "User.exe";
                psi.Arguments = users[i] + " " + users[i+1] + " " + users[i+2];
                Process.Start(psi);
            }
            Console.ReadLine();

        }
    }
}
