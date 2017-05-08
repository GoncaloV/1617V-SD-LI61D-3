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

            Console.WriteLine("Returned server number " + server);

            return serverURLS.ElementAt(server);
        }

        public bool ReplicateInformationBetweenServers(int id, string Key, String val)
        {
            if(id > servers.Count)
                return false;
            
            int count = 0;
            LinkedList<int> tmp = new LinkedList<int>();
            tmp.AddLast(id);

            int tmpServer = (id+ 1) % servers.Count;
            


            //Console.WriteLine("id " + id  + " f " + firstServer + " secon " + secondServer);
            while (count < 2)
            {
                try
                {
                    servers.ElementAt(tmpServer).storePairLocally(Key, val);
                    tmp.AddLast(tmpServer);
                    count++;
                }
                catch (Exception e)
                {
                    if(tmpServer == id)
                        return false;
                }
                tmpServer = (tmpServer + 1) % servers.Count;
            }
            keys.AddLast(new KeyWrapper(Key, tmp));
            return true;
        }

        public void deleteInformation(string key, int id)
        {
            Console.WriteLine("Attempting to delete information of key: " + key);
            KeyWrapper toRemove = null;

            foreach (KeyWrapper keyW in keys)
            {
                if (key.Equals(keyW.getKey())) {
                    toRemove = keyW;
                    try {
                        foreach(int serverID in keyW.getServers())
                        {
                            servers.ElementAt(serverID).deletePairLocally(key);
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            keys.Remove(toRemove);
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
                        foreach(int serverID in keyW.getServers())
                        {
                            String value = servers.ElementAt(serverID).readPairLocally(key);
                            if (value != null)
                                return value;
                        }
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
        public KeyWrapper(String key, LinkedList<int> ids)
        {
            this.key = key;
            this.servers = ids;
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
