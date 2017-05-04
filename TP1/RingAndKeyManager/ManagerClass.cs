using Interface;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;

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
            List<String> serverListKey = ConfigurationManager.AppSettings.AllKeys.ToList();

            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            WellKnownClientTypeEntry entry = entries[0];

            if (entry == null)
                throw new RemotingException("Type not found");



            //Get the server URLS so we can deliver them to the client
            foreach (String key in serverListKey)
            {
                serverURLS.AddLast(ConfigurationManager.AppSettings.Get(key));

                servers.AddLast((IServer) Activator.GetObject(entry.ObjectType, ConfigurationManager.AppSettings.Get(key)));

            }


        }

        public bool checkIfKeyExists(string key)
        {
            throw new NotImplementedException();
        }

        public String getRing()
        {
            Console.WriteLine("Hello, darkness my old friend");
            try { 
                String a = servers.ElementAt(0).test();
            }catch(Exception e)
            {


            }
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
