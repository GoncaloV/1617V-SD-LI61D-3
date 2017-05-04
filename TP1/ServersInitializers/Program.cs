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

    //It starts 5 servers.The problem is that it appears as if the servers start but windows forgets to init the servers on the machine.
    //Find a way to exec 5 servers by hand.Good luck...Bye!
    class Program
    {
        static void Main(string[] args)
        {
            List<String> serverListKey = ConfigurationManager.AppSettings.AllKeys.ToList();

            int number_servers = 5;

            Helper helper = new Helper();
            String path = helper.runningPath();


            //Black Magic.We dont care this is just so we dont have to initialize 5 servers by hand
            path = path.Substring(0, path.Length - 29);

            path += "Server\\bin\\Debug";

            for (int i = 1; i <= number_servers; i++)
            {
                Process.Start(helper.runningPath() + "\\Server.exe", "Server"+i);
            }
            Console.ReadLine();
        }
    }
}
