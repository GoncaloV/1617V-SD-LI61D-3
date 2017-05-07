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
                    servers.Last().init(servers.Count() -1);
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
            Console.WriteLine("Checking if key: " + key + " exist");
            foreach (KeyWrapper keyW in keys)
            {
                if (key.Equals(keyW.getKey()))
                {
                    return true;
                }
            }
            return false;
        }

        public String getRing()
        {
            Console.WriteLine("Get ring method called!");

            Random r = new Random();
            int server = r.Next(servers.Count);

            Console.WriteLine("Returned server number " + server +1);

            return serverURLS.ElementAt(server);
        }

        public bool ReplicateInformationBetweenServers(int id, string Key, String val)
        {
            if(id > servers.Count)
                return false;

            int repId = servers.Count - id;

            try
            {
                if (repId == 1)
                {
                    Console.WriteLine("Replicating between server " + (id + 1) + " and 0");
                    servers.ElementAt(id + 1).storePairLocally(Key, val);
                    servers.ElementAt(0).storePairLocally(Key, val);
                    return true;
                }
                if (repId == 0)
                {

                    Console.WriteLine("Replicating between server 0 and 1");
                    servers.ElementAt(0).storePairLocally(Key, val);
                    servers.ElementAt(1).storePair(Key, val);
                    return true;
                }

                Console.WriteLine("Replicating between server " + (id + 1) + " and " + (id + 2));
                servers.ElementAt(id + 1).storePairLocally(Key, val);
                servers.ElementAt(id + 2).storePairLocally(Key, val);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }            
        }

        public void deleteInformation(string key, int id)
        {
            Console.WriteLine("Attempting to delete information of key: " + key);
            foreach (KeyWrapper keyW in keys)
            {
                if (key.Equals(keyW.getKey())) { 
                    keyW.removeServerFromKey(id);
                    try {
                        servers.ElementAt(id).deletePairLocally(key);
                    }catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        public string searchServersForObject(string key)
        {
            Console.WriteLine("Searching for key: " + key);
            foreach (KeyWrapper keyW in keys)
            {
                if (keyW.getKey().Equals(key))
                {
                    try
                    {
                        return servers.ElementAt(keyW.getServers().ElementAt(0)).readPair(key);
                    }catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
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
