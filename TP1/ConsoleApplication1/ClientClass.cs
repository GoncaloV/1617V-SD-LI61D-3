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
            throw new NotImplementedException();
        }

        public String readPairFromServer(String key)
        {
            throw new NotImplementedException();
        }

        public String storePairOnServer(String key, String value)
        {
            try
            {
                String a = associatedServer.test();
                Console.WriteLine();

                return "Pushed " + key + " to Server";
            }catch(Exception e)
            {
                return "Cannot communicate with Server";
            }
        }
       
    }
}
