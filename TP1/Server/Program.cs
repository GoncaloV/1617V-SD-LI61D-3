using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            String config = "Server1";

            if (args.Length != 0)
                config = args[0];

            RemotingConfiguration.Configure(config + ".config" , false);

            Console.WriteLine("Server for config: " + config + " intialized\nWaiting for requests...");
            Console.ReadLine();
            Console.WriteLine("Server for config " + config + " finished");
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
