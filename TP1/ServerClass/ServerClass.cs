using Interface;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading.Tasks;

namespace ServerClass
{
    class ServerClassImpl : MarshalByRefObject, IServer
    {
        private Dictionary<String, String> map;
        private int id;
        private IManagerServerSide ring;

        public ServerClassImpl()
        {
            Console.WriteLine("ServerClass construtor");
        }

        /**
         * Deletes an entry from the dictionary according to the key provided. Asks the ring to do the same for every server.
         * */
        public void deletePair(String key)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Console.WriteLine("Attempting to delete: " + key + " from the Ring");
                    map.Remove(key);
                    ring.deleteInformation(key, id);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        /**
         * Deletes an entry from the dictionary according to the key provided, but only locally.
         * Should only be called by the Ring while processing another server's deletePair.
         * */
        public void deletePairLocally(String key)
        {
            Console.WriteLine("Deleting " + key + " from Server");
            map.Remove(key);
        }

        /**
         * Initializes server and acquires an id.
         * */
        public void init(int serverID)
        {

            Console.WriteLine("Initializing Server with ID: " + serverID);

            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            WellKnownClientTypeEntry entry = entries[0];

            if (entry == null)
                throw new RemotingException("Type not found");

            ring = (IManagerServerSide)Activator.GetObject(entry.ObjectType, entry.ObjectUrl);

            id = serverID;
            map = new Dictionary<string, string>();
        }

        /**
         * Reads a pair locally. If the pair is not available locally it asks the ring to get it remotely.
         * */
        public String readPair(String key)
        {
            Console.WriteLine("Attempting to read key: " + key);
            String value;
            if (map.TryGetValue(key, out value))
                return value;
            return ring.searchServersForObject(key);
        }

        /**
         * Reads a pair locally. Returns null if not found and does not ask the ring server to find it, unlike readPair.
         * Should only be called by the Ring while processing another server's readPair.
         * */
        public String readPairLocally(String key)
        {
            Console.WriteLine("Attempting to read local key: " + key);
            String value = null;
            map.TryGetValue(key, out value);
            return value;
        }
        /**
         * Stores a pair locally and replicates it across two other servers.
         * */
        public void storePair(String key, String value)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {

                    Console.WriteLine("Attempting to store key: " + key + " value: " + value);
                    if (!map.ContainsKey(key) && !ring.checkIfKeyExists(key, id))
                    {
                        map.Add(key, value);
                        if (!ring.ReplicateInformationBetweenServers(id, key, value))
                            throw new Exception("Failed to replicate.");
                    }
                    else throw new Exception("Key does not exist.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        /**
         * Stores a pair locally but does not replicate it.
         * Should only be called by the Ring while processing another server's storePair.
         * */
        public void storePairLocally(String key, String value)
        {

            Console.WriteLine("Attempting to store local key: " + key + " value: " + value);
            map.Add(key, value);
            
        }
    }
}