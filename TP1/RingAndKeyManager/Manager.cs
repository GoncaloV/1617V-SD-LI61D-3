using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RingAndKeyManager
{
    public class Manager : MarshalByRefObject, IManager
    {

        List<IServer> servers;
        List<String> keys;

        public Manager()
        {
            servers = new List<IServer>();
            keys = new List<string>();
        }
        
        public IServer getRing()
        {
            
        }

        public bool checkIfKeyExists(string key)
        {
            throw new NotImplementedException();
        }

        public SerializableAttribute searchServersForObject(string key)
        {
            throw new NotImplementedException();
        }

        public bool ReplicateInformationBetweenServers(int id, string Key, SerializableAttribute val)
        {
            throw new NotImplementedException();
        }

        public int getServerId()
        {
            throw new NotImplementedException();
        }

        static void Main(string[] args)
        {

        }
    }
}
