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

        private void connectToRing()
        {
            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            WellKnownClientTypeEntry entry = entries[0];

            if (entry == null)
                throw new RemotingException("Type not found");

            ringManager = (IManagerClientSide)Activator.GetObject(entry.ObjectType, entry.ObjectUrl);
        }

        public void associateWithServer()
        {
            try { 
                String url = ringManager.getRing();

                WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
                WellKnownClientTypeEntry entry = entries[0];

                if (entry == null)
                    throw new RemotingException("Type not found");

                associatedServer = (IServer)Activator.GetObject(entry.ObjectType, url);

                Console.WriteLine();
            }catch(Exception e)
            {
                Console.WriteLine("Cannot talk to the Ring");
            }
        }

        public void deletePairFromServer(SerializableAttribute key)
        {
            throw new NotImplementedException();
        }

        public void readPairFromServer(SerializableAttribute key)
        {
            throw new NotImplementedException();
        }

        public void storePairOnServer(String key, String value)
        {
            try
            {
                String a = associatedServer.test();
                Console.WriteLine();
            }catch(Exception e)
            {
                Console.WriteLine("Deu merda");
            }
        }
       
    }
}
