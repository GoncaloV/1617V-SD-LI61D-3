using Interface;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ManagerClass
{
    public class ManagerClassImpl : MarshalByRefObject, IManagerServerSide, IManagerClientSide
    {
        LinkedList<IServer> servers;
        LinkedList<String> keys;

        public ManagerClassImpl()
        {
            servers = new LinkedList<IServer>();
            keys = new LinkedList<String>();

            //initRing(servers);

            Console.WriteLine("ManagerClass construtor");

        }

        private void initRing(LinkedList<IServer> servers)
        {
            List<string> keys = ConfigurationManager.AppSettings
                                .AllKeys.ToList()
                                .FindAll(s => !"serverNumber".Equals(s));


            Console.WriteLine();
            throw new NotImplementedException();
        }

        public bool checkIfKeyExists(string key)
        {
            throw new NotImplementedException();
        }

        public IServer getRing()
        {
            Console.WriteLine("Hello, darkness my old friend");
            return null;
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
