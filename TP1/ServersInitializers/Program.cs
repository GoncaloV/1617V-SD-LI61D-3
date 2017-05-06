using Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServersInitializers
{

    //Starts X servers based on the config file.
    //Note this Project is only to helps us launched X servers.
    //It requires a very specific folders structure, so dont use it in production, unless you have a compatible
    //file structure

    class Program
    {
        static void Main(string[] args)
        {
            List<String> serverListKey = ConfigurationManager.AppSettings.AllKeys.ToList();

            int number_servers = 0;

            Int32.TryParse(ConfigurationManager.AppSettings.Get(serverListKey.ElementAt(0)), out number_servers);

            Helper helper = new Helper();
            String path = helper.runningPath();
        
            //Black Magic.We dont care this is just so we dont have to initialize X servers by hand
            path = path.Substring(0, path.Length - 29);

            path += "Server\\bin\\Debug";

            for (int i = 1; i <= number_servers; i++)
            {

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = true;
                psi.LoadUserProfile = true;
                psi.WorkingDirectory = path;
                psi.FileName = "Server.exe" ;
                psi.Arguments = "Server"+i;
                Process.Start(psi);
            }
            Console.ReadLine();
        }
    }
}
