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
        LinkedList<String> serverURLS;

        public ManagerClassImpl()
        {
            servers = new LinkedList<IServer>();
            keys = new LinkedList<String>();
            serverURLS = new LinkedList<String>();

            initRing();

            Console.WriteLine("ManagerClass construtor");

        }

        private void initRing()
        {
            ConfigurationManager.AppSettings.AllKeys
                                .ToList()
                                .ForEach(s => serverURLS.AddLast(ConfigurationManager.AppSettings.Get(s)));

            foreach(String url in serverURLS)
            {
                servers.AddLast((IServer)Activator.GetObject(typeof(IServer), url));
            }
        }

        public bool checkIfKeyExists(string key)
        {
            throw new NotImplementedException();
        }

        public String getRing()
        {
            Console.WriteLine("Hello, darkness my old friend");

            return serverURLS.ElementAt(0);
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
