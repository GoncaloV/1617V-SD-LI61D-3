using System;
using Interface;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace ClientClass
{
    public class ClientClassImpl : MarshalByRefObject, IClientInterface
    {

        private IServer associatedServer;
        private IManagerClientSide ringManager;

        public ClientClassImpl()
        {
            Console.WriteLine("ClientClass construtor");
            connectToRing();

        }

        private String connectToRing()
        {
            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            WellKnownClientTypeEntry entry = entries[0];

            if (entry == null)
                throw new RemotingException("Type not found");

            ringManager = (IManagerClientSide)Activator.GetObject(entry.ObjectType, entry.ObjectUrl);

            return "Connected!";
        }

        public String associateWithServer()
        {
            try { 
                String url = ringManager.getRing();

                WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
                WellKnownClientTypeEntry entry = entries[0];

                if (entry == null)
                    throw new RemotingException("Type not found");

                associatedServer = (IServer)Activator.GetObject(entry.ObjectType, url);

                return "Associated with server: " + url;
            }catch(Exception e)
            {
                return "Cannot connect to Ring";
            }
        }

        public String deletePairFromServer(String key)
        {
            try
            {
                associatedServer.deletePair(key);
                return "Deleted value for key: " + key;
            }catch(Exception e)
            {
                return "Cannot communicate with Server";
            }
        }

        public String readPairFromServer(String key)
        {
            try
            {
                return "Value for key: " + key + " is " + associatedServer.readPair(key);
            }
            catch (Exception e)
            {
                return "Cannot communicate with Server";
            }
        }

        public String storePairOnServer(String key, String value)
        {
            try
            {
                associatedServer.storePair(key, value);

                return "Pushed " + key + " to Server";
            }catch(Exception e)
            {
                return "Cannot communicate with Server";
            }
        }
       
    }
}
