using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RingAndKeyManager
{
    public class ManagerClass : MarshalByRefObject, IManagerServerSide, IManagerClientSide
    {
        LinkedList<IServer> servers;
        LinkedList<String> keys;

        public ManagerClass()
        {
            Console.WriteLine("ManagerClass construtor");

        }

        public bool checkIfKeyExists(string key)
        {
            throw new NotImplementedException();
        }

        public IServer getRing()
        {
            throw new NotImplementedException();
        }

        public int getServerId()
        {
            throw new NotImplementedException();
        }

        public bool ReplicateInformationBetweenServers(int id, string Key, SerializableAttribute val)
        {
            throw new NotImplementedException();
        }

        public SerializableAttribute searchServersForObject(string key)
        {
            throw new NotImplementedException();
        }
    }
}
