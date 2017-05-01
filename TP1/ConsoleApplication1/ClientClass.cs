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
            ringManager = connectToRing();

        }

        private IManagerClientSide connectToRing()
        {
            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            WellKnownClientTypeEntry entry = entries[0];

            if (entry == null)
                throw new RemotingException("Type not found");

            IManagerClientSide ret = (IManagerClientSide)Activator.GetObject(entry.ObjectType, entry.ObjectUrl);

            return ret;
        }

        public void associateWithServer()
        {
            try { 
                ringManager.getRing();
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

        public void storePairOnServer(SerializableAttribute key, SerializableAttribute value)
        {
            throw new NotImplementedException();
        }
       
    }
}
