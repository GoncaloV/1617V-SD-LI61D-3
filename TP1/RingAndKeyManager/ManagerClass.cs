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
        LinkedList<KeyWrapper> keys;
        LinkedList<String> serverURLS;

        public ManagerClassImpl()
        {
            Console.WriteLine("ManagerClass construtor");
            servers = new LinkedList<IServer>();
            keys = new LinkedList<KeyWrapper>();
            serverURLS = new LinkedList<String>();

            initRing();
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
            foreach (KeyWrapper keyW in keys)
            {
                if (keyW.getKey().Equals(key) && keyW.getServers().Contains(originServer))
                {
                    return true;
                }
                return false;
            }
            return false;
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
            if(id > servers.Count)
                return false;
            int repId = servers.Count - id;
            if (repId == 1)
            {
                servers.ElementAt(id + 1).storePairLocally(Key, val);
                servers.ElementAt(0).storePairLocally(Key, val);
                return true;
            }
            if (repId == 0)
            {
                servers.ElementAt(0).storePairLocally(Key, val);
                servers.ElementAt(1).storePair(Key, val);
                return true;
            }
            servers.ElementAt(id + 1).storePairLocally(Key, val);
            servers.ElementAt(id + 2).storePairLocally(Key, val);
            return true;
            
        }

        public void deleteInformation(string key, int id)
        {
            foreach (KeyWrapper keyW in keys)
            {
                if (keyW.getKey().Equals(key))
                    keyW.removeServerFromKey(id);
            }
        }

        public string searchServersForObject(string key)
        {
            foreach (KeyWrapper keyW in keys)
            {
                if (keyW.getKey().Equals(key))
                {
                    return servers.ElementAt(keyW.getServers().ElementAt(0)).readPair(key);
                }
                return null;
            }
            return null;
        }
    }

    internal class KeyWrapper
    {
        private String key;
        private LinkedList<int> servers;
        public KeyWrapper(String key, int id)
        {
            this.key = key;
            this.servers = new LinkedList<int>();
            servers.AddLast(id);
        }
        public Boolean addServerToKey(int id)
        {
            if (servers.Contains(id))
                return false;
            else
            {
                servers.AddLast(id);
                return true;
            }
        }
        public Boolean removeServerFromKey(int id)
        {
            if (servers.Contains(id))
            {
                servers.Remove(id);
                return true;
            }
            return false;
        }
        public String getKey()
        {
            return key;
        }
        public LinkedList<int> getServers()
        {
            return servers;
        }
    }
}
