using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Ring
{
    class Program
    {
        static void Main(string[] args)
        {
            string configfile = "Ring.exe.config";
            RemotingConfiguration.Configure(configfile, false);

            Console.WriteLine("Ring Manager intialized\nWaiting for requests...");
            Console.ReadLine();
            Console.WriteLine("Ring Manager finished");

        }
    }
}
