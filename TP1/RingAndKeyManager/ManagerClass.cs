using Interface;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
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

                //Init connection on the server with the Ring
                try
                {
                    servers.Last().init(servers.Count());
                }catch(Exception e)
                {
                    //If we cannot connect to the server we assume that isn´t a valid server, so we remove it from our list
                    servers.RemoveLast();
                    serverURLS.RemoveLast();
                    Console.WriteLine("Cannot init connection between Ring and Server" + key);
                }

            }

        }

        public bool checkIfKeyExists(string key, int originServer)
        {
            throw new NotImplementedException();
        }

        public String getRing()
        {
            Console.WriteLine("Get ring method called!");

            Random r = new Random();
            int server = r.Next(servers.Count);

            Console.WriteLine("Returned server number " + server);

            return serverURLS.ElementAt(server);
        }


        public bool ReplicateInformationBetweenServers(int id, string Key, String val)
        {
            throw new NotImplementedException();
        }

        public SerializableAttribute searchServersForObject(string key)
        {
            throw new NotImplementedException();
        }
    }
}
